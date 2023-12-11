using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MillesHotel.Data;
using MillesHotel.Menus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MillesHotel
{
    public class App
    {
        public void Build(DbContextOptionsBuilder<HotelDbContext> options)
        {
            UpdateBookingStatus(options);

            bool programRunning = true;
            do
            {
                MainMenu.ShowMenu(options);
            } while (programRunning);

        }

        private static void UpdateBookingStatus(DbContextOptionsBuilder<HotelDbContext> options)
        {
            using (var dbContext = new HotelDbContext(options.Options))
            {
                var bookings = dbContext.Bookings.ToList();

                foreach (var booking in bookings)
                {
                    // Om BookingEndDate har passerat dagens datum
                    if (booking.BookingEndDate < DateTime.Now)
                    {
                        booking.IsActive = false;
                    }
                }

                dbContext.SaveChanges();
            }
        }
    }
}
