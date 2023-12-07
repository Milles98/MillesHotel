using MillesHotelLibrary.Interfaces;
using MillesHotelLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MillesHotelLibrary.Services
{
    public class BookingService : IBookingService
    {
        private List<Booking> bookings = new List<Booking>();

        public Booking CreateBooking(Booking newBooking)
        {
            return newBooking;
        }

        public Booking GetBookingByID(int bookingID)
        {
            return bookings[bookingID];
        }

        public void UpdateBooking(Booking newBooking)
        {

        }

        public void DeleteBooking(int bookingID)
        {

        }

    }
}
