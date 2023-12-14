using Microsoft.EntityFrameworkCore;
using MillesHotelLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MillesHotelLibrary.Data
{
    public class DbSeeding
    {
        public void SeedData(HotelDbContext dbContext)
        {
            dbContext.Database.Migrate();
            SeedRooms(dbContext);
            dbContext.SaveChanges();
        }

        private void SeedRooms(HotelDbContext dbContext)
        {
            if (!dbContext.Rooms.Any())
            {
                var customers = new List<Customer>
                {
                    new Customer { CustomerFirstName = "Stefan", CustomerLastName = "Löfven", CustomerAge = 64,
                CustomerEmail = "steffe@riksdagen.se", CustomerPhone = "08-2353123", CustomerCountry = "Sweden", IsActive = true },
                    new Customer { CustomerFirstName = "Mille", CustomerLastName = "Elfver", CustomerAge = 25,
                CustomerEmail = "mille@kyh.se", CustomerPhone = "070-23221532", CustomerCountry = "Sweden", IsActive = true },
                    new Customer { CustomerFirstName = "Richard", CustomerLastName = "Chalk", CustomerAge = 35,
                CustomerEmail = "richard@systementor.se", CustomerPhone = "08-423213", CustomerCountry = "Sweden", IsActive = true },
                    new Customer { CustomerFirstName = "Bill", CustomerLastName = "Gates", CustomerAge = 75,
                CustomerEmail = "billgates@microsoft.com", CustomerPhone = "421555234", CustomerCountry = "USA", IsActive = true },
                    new Customer { CustomerFirstName = "Pamela", CustomerLastName = "Andersson", CustomerAge = 30,
                CustomerEmail = "pamela@andersson.com", CustomerPhone = "69696969", CustomerCountry = "USA", IsActive = true },
                    new Customer { CustomerFirstName = "David", CustomerLastName = "Hasselhoff", CustomerAge = 60,
                CustomerEmail = "david@hasselhoff.com", CustomerPhone = "452353572", CustomerCountry = "USA", IsActive = true }
                };

                dbContext.Customers.AddRange(customers);
                dbContext.SaveChanges();

                var rooms = new List<Room>
                {
                    new Room { RoomSize = 25, RoomType = RoomType.SingleRoom, RoomName = "Princess Suite", ExtraBeds = false },
                    new Room { RoomSize = 75, RoomType = RoomType.DoubleRoom, RoomName = "King Suite", ExtraBeds = true, ExtraBedsCount = 2  },
                    new Room { RoomSize = 45, RoomType = RoomType.SingleRoom, RoomName = "Prince Suite", ExtraBeds = false  },
                    new Room { RoomSize = 145, RoomType = RoomType.DoubleRoom, RoomName = "Presidential Suite", ExtraBeds = true, ExtraBedsCount = 2  },
                    new Room { RoomSize = 1450, RoomType = RoomType.DoubleRoom, RoomName = "Haunted Room", ExtraBeds = true, ExtraBedsCount = 1  },
                    new Room { RoomSize = 450, RoomType = RoomType.SingleRoom, RoomName = "Disney Room", ExtraBeds = false  }
                };

                dbContext.Rooms.AddRange(rooms);
                dbContext.SaveChanges();

                var bookings = new List<Booking>
                {
                    new Booking { BookingStartDate = DateTime.Now.AddDays(7).Date, BookingEndDate = DateTime.Now.AddDays(14).Date,
                IsActive = true, CustomerID = customers[1].CustomerID, RoomID = rooms[1].RoomID },
                    new Booking { BookingStartDate = DateTime.Now.AddDays(7).Date, BookingEndDate = DateTime.Now.AddDays(14).Date,
                IsActive = true, CustomerID = customers[3].CustomerID, RoomID = rooms[0].RoomID },
                    new Booking { BookingStartDate = DateTime.Now.AddDays(7).Date, BookingEndDate = DateTime.Now.AddDays(14).Date,
                IsActive = true, CustomerID = customers[2].CustomerID, RoomID = rooms[3].RoomID },
                    new Booking { BookingStartDate = DateTime.Now.AddDays(7).Date, BookingEndDate = DateTime.Now.AddDays(14).Date,
                IsActive = true, CustomerID = customers[0].CustomerID, RoomID = rooms[2].RoomID }
                };

                dbContext.Bookings.AddRange(bookings);
                dbContext.SaveChanges();

                var invoices = new List<Invoice>();

                foreach (var booking in bookings)
                {
                    var invoiceAmount = 1000 * (booking.BookingEndDate - booking.BookingStartDate).TotalDays;

                    var invoice = new Invoice
                    {
                        InvoiceAmount = invoiceAmount,
                        InvoiceDue = booking.BookingEndDate,
                        IsActive = invoiceAmount > 0,
                        CustomerID = booking.CustomerID,
                        Customer = booking.Customer,
                        //BookingID = booking.BookingID,
                    };

                    invoices.Add(invoice);
                }

                dbContext.Invoices.AddRange(invoices);
                dbContext.SaveChanges();
            }
        }
    }
}
