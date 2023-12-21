using Microsoft.EntityFrameworkCore;
using MillesHotelLibrary.Interfaces;
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
            Seeding(dbContext);
            dbContext.SaveChanges();
        }

        private void Seeding(HotelDbContext dbContext)
        {
            if (!dbContext.Room.Any())
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

                dbContext.Customer.AddRange(customers);
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

                dbContext.Room.AddRange(rooms);
                dbContext.SaveChanges();

                var invoices = new List<Invoice>();

                foreach (var room in rooms)
                {
                    var invoiceAmount = room.RoomPrice;

                    var invoice = new Invoice
                    {
                        InvoiceAmount = invoiceAmount,
                        IsPaid = false,
                    };

                    invoices.Add(invoice);
                }

                dbContext.Invoice.AddRange(invoices);
                dbContext.SaveChanges();

                var bookings = new List<Booking>();

                for (int i = 0; i < rooms.Count; i++)
                {
                    var room = rooms[i];
                    var invoice = invoices[i];

                    var booking = new Booking
                    {
                        BookingStartDate = DateTime.Now.AddDays(i).Date,
                        BookingEndDate = DateTime.Now.AddDays(i + 7).Date,
                        IsBooked = true,
                        Customer = customers[i],
                        RoomID = room.RoomID,
                        InvoiceID = invoice.InvoiceID
                    };

                    booking.Invoice = invoice;

                    booking.Invoice.InvoiceAmount = room.RoomPrice * (booking.BookingEndDate - booking.BookingStartDate).TotalDays;

                    booking.Invoice.InvoiceDue = booking.BookingStartDate.AddDays(10);

                    bookings.Add(booking);
                }

                dbContext.Booking.AddRange(bookings);
                dbContext.SaveChanges();


            }
        }
    }
}
