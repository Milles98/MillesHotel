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

        [Required]
        public int RoomSize { get; set; }

        [Required]

        public bool IsActive { get; set; }

        // Foreign key för att koppla till Booking
        //public int? BookingID { get; set; }
        //public Booking? Booking { get; set; }
        public ICollection<Booking>? Bookings { get; set; }

        public bool BookingDatesOverlap(Booking booking, DateTime start, DateTime end);

    }
}
