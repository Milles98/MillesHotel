using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MillesHotel.Interfaces
{
    public interface IBookingService
    {
        void CreateBooking();
        void GetBookingByID();
        void UpdateBooking();
        void SoftDeleteBooking();
    }
}
