using MillesHotelLibrary.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MillesHotelLibrary.Models
{
    public class Room : IRoom
    {
        [Key]
        public int RoomID { get; set; }

        public string RoomName { get; set; }

        [Required]
        public int RoomSize { get; set; }

        [Required]
        public RoomType RoomType { get; set; }

        public bool ExtraBeds { get; set; }/*=> RoomType == RoomType.DoubleRoom;*/
        public bool IsActive
        {
            get
            {
                // Check if there are any active bookings that overlap with the specified date range
                return Bookings?.Any(b => b.IsActive && BookingDatesOverlap(b, DateTime.Now, DateTime.Now.AddDays(7))) ?? false;
            }
            set
            {
                // Set IsActive for all bookings related to this room
                if (Bookings != null)
                {
                    foreach (var booking in Bookings)
                    {
                        booking.IsActive = value;
                    }
                }
            }
        }

        public ICollection<Booking>? Bookings { get; set; }

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
