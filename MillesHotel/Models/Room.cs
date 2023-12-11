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
        public RoomType RoomType { get; set; } // Antag att detta representerar Double Room (true) eller Single Room (false)

        public bool ExtraBeds { get; set; }
        public bool IsActive
        {
            get
            {
                // Returnera true om det finns någon pågående bokning
                return Bookings?.Any(b => b.IsActive) ?? false;
            }
        }

        // Foreign key för att koppla till Booking
        public int? BookingID { get; set; }
        public Booking Booking { get; set; }
        public ICollection<Booking> Bookings { get; set; }

    }

    public enum RoomType
    {
        SingleRoom,
        DoubleRoom
    }
}
