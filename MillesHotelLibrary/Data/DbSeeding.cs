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
                    new Customer { CustomerFirstName = "Penélope", CustomerLastName = "Cruz", CustomerAge = 49,
                CustomerEmail = "pene@cruz.com", CustomerPhone = "5432678643", CustomerCountry = "Spain", IsActive = true },
                    new Customer { CustomerFirstName = "Mille", CustomerLastName = "Elfver", CustomerAge = 25,
                CustomerEmail = "mille@kyh.se", CustomerPhone = "070-23221532", CustomerCountry = "Sweden", IsActive = true },
                    new Customer { CustomerFirstName = "Richard", CustomerLastName = "Chalk", CustomerAge = 35,
                CustomerEmail = "richard@systementor.se", CustomerPhone = "08-423213", CustomerCountry = "Sweden", IsActive = true },
                    new Customer { CustomerFirstName = "Bill", CustomerLastName = "Gates", CustomerAge = 75,
                CustomerEmail = "billgates@microsoft.com", CustomerPhone = "421555234", CustomerCountry = "USA", IsActive = true },
                    new Customer { CustomerFirstName = "Pamela", CustomerLastName = "Andersson", CustomerAge = 30,
                CustomerEmail = "pamela@andersson.com", CustomerPhone = "69696969", CustomerCountry = "USA", IsActive = true },
                    new Customer { CustomerFirstName = "David", CustomerLastName = "Hasselhoff", CustomerAge = 60,
                CustomerEmail = "david@hasselhoff.com", CustomerPhone = "452353572", CustomerCountry = "USA", IsActive = true },
                    new Customer { CustomerFirstName = "Christopher", CustomerLastName = "Waltz", CustomerAge = 67,
                CustomerEmail = "chris@waltz.com", CustomerPhone = "474232673", CustomerCountry = "Germany", IsActive = true },
                    new Customer { CustomerFirstName = "Jeff", CustomerLastName = "Bezos", CustomerAge = 58,
                CustomerEmail = "jeff@amazon.com", CustomerPhone = "424467683", CustomerCountry = "USA", IsActive = true },
                    new Customer { CustomerFirstName = "Mark", CustomerLastName = "Zuckerberg", CustomerAge = 60,
                CustomerEmail = "zucker@facebook.com", CustomerPhone = "493233545", CustomerCountry = "USA", IsActive = true },
                    new Customer { CustomerFirstName = "Mahatma", CustomerLastName = "Ghandi", CustomerAge = 90,
                CustomerEmail = "mahatma@ghandi.com", CustomerPhone = "493233545", CustomerCountry = "India", IsActive = true },
                    new Customer { CustomerFirstName = "Kristofer", CustomerLastName = "Hivju", CustomerAge = 45,
                CustomerEmail = "kris@hivju.com", CustomerPhone = "493233545", CustomerCountry = "Norway", IsActive = true },
                    new Customer { CustomerFirstName = "Mads", CustomerLastName = "Mikkelsen", CustomerAge = 58,
                CustomerEmail = "mads@mik.com", CustomerPhone = "493233545", CustomerCountry = "Denmark", IsActive = true }
                };

                dbContext.Customers.AddRange(customers);
                dbContext.SaveChanges();

                var rooms = new List<Room>
                {
                    new Room { RoomSize = 25, RoomType = RoomType.SingleRoom, RoomName =
                    "Princess Suite", ExtraBeds = false, RoomPrice = 325 },
                    new Room { RoomSize = 75, RoomType = RoomType.DoubleRoom, RoomName =
                    "King Suite", ExtraBeds = true, ExtraBedsCount = 2, RoomPrice = 990 },
                    new Room { RoomSize = 45, RoomType = RoomType.SingleRoom, RoomName =
                    "Prince Suite", ExtraBeds = false, RoomPrice = 450 },
                    new Room { RoomSize = 145, RoomType = RoomType.DoubleRoom, RoomName =
                    "Presidential Suite", ExtraBeds = true, ExtraBedsCount = 2, RoomPrice = 1750 },
                    new Room { RoomSize = 1450, RoomType = RoomType.DoubleRoom, RoomName =
                    "Haunted Room", ExtraBeds = true, ExtraBedsCount = 1, RoomPrice = 1950 },
                    new Room { RoomSize = 450, RoomType = RoomType.SingleRoom, RoomName =
                    "Disney Room", ExtraBeds = false, RoomPrice = 2250 },
                    new Room { RoomSize = 55, RoomType = RoomType.SingleRoom, RoomName =
                    "Milles Room", ExtraBeds = false, RoomPrice = 250 },
                    new Room { RoomSize = 1750, RoomType = RoomType.DoubleRoom, RoomName =
                    "KYH Room", ExtraBeds = true, ExtraBedsCount = 2, RoomPrice = 3250 },
                    new Room { RoomSize = 450, RoomType = RoomType.SingleRoom, RoomName =
                    "Student Room", ExtraBeds = false, RoomPrice = 3500 }
                };

                dbContext.Rooms.AddRange(rooms);
                dbContext.SaveChanges();

                var bookings = new List<Booking>
                {
                    new Booking
                {
                    BookingStartDate = DateTime.Now.AddDays(7).Date,
                    BookingEndDate = DateTime.Now.AddDays(14).Date,
                    IsBooked = true,
                    Customer = customers[1],
                    RoomID = rooms[1].RoomID
                },
                    new Booking
                {
                    BookingStartDate = DateTime.Now.Date,
                    BookingEndDate = DateTime.Now.AddDays(10).Date,
                    IsBooked = true,
                    Customer = customers[3],
                    RoomID = rooms[0].RoomID
                },
                    new Booking
                {
                    BookingStartDate = DateTime.Now.AddDays(5).Date,
                    BookingEndDate = DateTime.Now.AddDays(9).Date,
                    IsBooked = true,
                    Customer = customers[2],
                    RoomID = rooms[3].RoomID
                },
                    new Booking
                {
                    BookingStartDate = DateTime.Now.AddDays(2).Date,
                    BookingEndDate = DateTime.Now.AddDays(7).Date,
                    IsBooked = true,
                    Customer = customers[0],
                    RoomID = rooms[2].RoomID
                }
                };

                dbContext.Bookings.AddRange(bookings);
                dbContext.SaveChanges();

                var invoices = new List<Invoice>();

                foreach (var booking in bookings)
                {
                    var room = dbContext.Rooms.Find(booking.RoomID);

                    var invoiceAmount = room.RoomPrice * (booking.BookingEndDate - booking.BookingStartDate).TotalDays;

                    var invoice = new Invoice
                    {
                        InvoiceAmount = invoiceAmount,
                        InvoiceDue = booking.BookingEndDate,
                        IsPaid = false,
                    };

                    invoices.Add(invoice);
                }

                dbContext.Invoices.AddRange(invoices);
                dbContext.SaveChanges();

                for (int i = 0; i < bookings.Count; i++)
                {
                    bookings[i].InvoiceID = invoices[i].InvoiceID;
                }
            }
        }
    }
}
