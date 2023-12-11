using MillesHotel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MillesHotel.Data
{
    public static class DbSeeding
    {
        public static void SeedData(HotelDbContext dbContext)
        {
            SeedRooms(dbContext);
        }

        private static void SeedRooms(HotelDbContext dbContext)
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
                CustomerEmail = "billgates@microsoft.com", CustomerPhone = "421555234", CustomerCountry = "USA", IsActive = true }
                };

                dbContext.Customers.AddRange(customers);
                dbContext.SaveChanges();

                var bookings = new List<Booking>
                {
                    new Booking { BookingDate = DateTime.Now, IsActive = true, CustomerID = customers[0].CustomerID },
                    new Booking { BookingDate = DateTime.Now, IsActive = true, CustomerID = customers[1].CustomerID },
                    new Booking { BookingDate = DateTime.Now, IsActive = true, CustomerID = customers[2].CustomerID },
                    new Booking { BookingDate = DateTime.Now, IsActive = true, CustomerID = customers[3].CustomerID }
                };

                dbContext.Bookings.AddRange(bookings);
                dbContext.SaveChanges();

                var rooms = new List<Room>
                {
                    new Room { RoomSize = 2, RoomType = true, ExtraBeds = false, IsActive = true, BookingID = bookings[0].BookingID },
                    new Room { RoomSize = 1, RoomType = false, ExtraBeds = true, IsActive = true, BookingID = bookings[1].BookingID },
                    new Room { RoomSize = 2, RoomType = false, ExtraBeds = true, IsActive = true, BookingID = bookings[2].BookingID },
                    new Room { RoomSize = 1, RoomType = true, ExtraBeds = false, IsActive = true, BookingID = bookings[3].BookingID }
                };

                dbContext.Rooms.AddRange(rooms);
                dbContext.SaveChanges();
            }
        }
    }
}
