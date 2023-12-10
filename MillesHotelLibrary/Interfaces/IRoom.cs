using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MillesHotelLibrary.Interfaces
{
    public interface IRoom
    {
        public int RoomID { get; set; }

        public int RoomSize { get; set; }

        public bool RoomType { get; set; } // Antag att detta representerar Double Room (true) eller Single Room (false)

        public bool ExtraBeds { get; set; }
        public bool IsActive { get; set; }

        // Foreign key för att koppla till Booking
        public int BookingID { get; set; }
    }
}
