using MillesHotelLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MillesHotelLibrary.Interfaces
{
    public interface IBookingService
    {
        Booking CreateBooking(Booking newBooking);
        Booking GetBookingByID(int bookingID);
        void UpdateBooking(Booking updatedBooking);
        void DeleteBooking(int bookingID);
    }
}
