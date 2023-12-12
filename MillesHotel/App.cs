using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MillesHotelLibrary.Data;
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
        public void Build(HotelDbContext options)
        {
            UpdateBookingStatus(options);

            bool programRunning = true;
            do
            {
                MainMenu.ShowMenu(options);
            } while (programRunning);

        }

        private static void UpdateBookingStatus(HotelDbContext dbContext)
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
