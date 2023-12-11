using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MillesHotel.Models
{
    public class Room
    {
        [Key]
        public int RoomID { get; set; }

        [Required]
        public int RoomSize { get; set; }

        [Required]
        public bool RoomType { get; set; } // Antag att detta representerar Double Room (true) eller Single Room (false)

        public bool ExtraBeds { get; set; }
        public bool IsActive { get; set; }

        // Foreign key för att koppla till Booking
        public int? BookingID { get; set; }
        public Booking Booking { get; set; }

    }
}
