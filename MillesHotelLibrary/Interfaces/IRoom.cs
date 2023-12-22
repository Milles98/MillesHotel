using MillesHotelLibrary.Models;
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
        [Key]
        public int RoomID { get; set; }

        [StringLength(19)]
        public string RoomName { get; set; }

        [Required]
        [Range(20, 3000)]
        public int RoomSize { get; set; }

        [Required]
        public bool RoomBooked { get; set; }

        public bool ExtraBeds { get; set; }

        [Range(0, 3)]
        public int ExtraBedsCount { get; set; }

        [Range(250, 3500)]
        public decimal RoomPrice { get; set; }

        public List<Booking>? Bookings { get; set; }

        public bool BookingDatesOverlap(Booking booking, DateTime start, DateTime end);

    }
}
