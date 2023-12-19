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
    public class AppStart
    {
        private readonly HotelDbContext _dbContext;

        public AppStart(HotelDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public void Build()
        {
            UpdateBookingStatus(_dbContext);

            bool programRunning = true;
            do
            {
                MainMenu.ShowMenu(_dbContext);
            } while (programRunning);

        }

        private void UpdateBookingStatus(HotelDbContext dbContext)
        {
            var bookings = dbContext.Bookings.ToList();

            foreach (var booking in bookings)
            {
                // Om BookingEndDate har passerat dagens datum
                if (booking.BookingEndDate < DateTime.Now)
                {
                    booking.IsBooked = false;
                }
            }

            dbContext.SaveChanges();
        }
    }
}
