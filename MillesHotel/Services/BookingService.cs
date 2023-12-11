using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MillesHotel.Services
{
    public class BookingService
    {
        private readonly HotelDbContext _dbContext;

        public BookingService(DbContextOptionsBuilder<HotelDbContext> options)
        {
            _dbContext = new HotelDbContext(options.Options);
        }

        public void CreateBooking()
        {
        }

        public void GetBookingByID()
        {
        }

        public void UpdateBooking()
        {

        }

        public void DeleteBooking()
        {

        }

    }
}
