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
        public RoomType RoomType { get; set; }

        public bool ExtraBeds => RoomType == RoomType.DoubleRoom;
        public bool IsActive
        {
            get
            {
                // Check if there are any active bookings that overlap with the specified date range
                return Bookings?.Any(b => b.IsActive && BookingDatesOverlap(b, DateTime.Now, DateTime.Now.AddDays(7))) ?? false;
            }
        }

        // Foreign key för att koppla till Booking
        public int? BookingID { get; set; }
        public Booking Booking { get; set; }
        public ICollection<Booking> Bookings { get; set; }

        public bool BookingDatesOverlap(Booking booking, DateTime start, DateTime end)
        {
            // Check if the booking overlaps with the specified date range
            return !(booking.BookingEndDate <= start || booking.BookingStartDate >= end);
        }

    }

    public enum RoomType
    {
        SingleRoom,
        DoubleRoom
    }
}
