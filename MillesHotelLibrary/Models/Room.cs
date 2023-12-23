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

        [StringLength(19)]
        public string RoomName { get; set; }

        [Required]
        [Range(10, 100)]
        public int RoomSize { get; set; }

        [Required]
        public RoomType RoomType { get; set; }

        public bool ExtraBeds { get; set; }

        [Range(0, 3)]
        public int ExtraBedsCount { get; set; }

        [Range(250, 3500)]
        public decimal RoomPrice { get; set; }

        public bool RoomBooked
        {
            get
            {
                return Bookings?.Any(b => b.Occupied && BookingDatesOverlap(b, DateTime.Now, DateTime.Now.AddDays(7))) ?? false;
            }
            set
            {
                if (Bookings != null)
                {
                    foreach (var booking in Bookings)
                    {
                        booking.Occupied = value;
                    }
                }
            }
        }
        public bool IsActive { get; set; } = true;

        public List<Booking>? Bookings { get; set; }

        public static bool BookingDatesOverlap(Booking booking, DateTime start, DateTime end)
        {
            return !(booking.BookingEndDate <= start || booking.BookingStartDate >= end);
        }
    }
    public enum RoomType
    {
        SingleRoom,
        DoubleRoom
    }
}
