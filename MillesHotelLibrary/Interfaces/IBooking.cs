using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MillesHotelLibrary.Interfaces
{
    public interface IBooking
    {
        public int BookingID { get; set; }
        public DateTime BookingDate { get; set; }
        public int CustomerID { get; set; }
        public int RoomID { get; set; }
    }
}
