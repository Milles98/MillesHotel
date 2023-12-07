using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MillesHotelLibrary.Interfaces
{
    public interface IBookingService
    {
        public void AddBooking();
        public void ReadBooking();
        public void UpdateBooking();
        public void RemoveBooking();
    }
}
