using MillesHotelLibrary.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MillesHotelLibrary.Interfaces
{
    public interface IBooking
    {
        [Key]
        public int BookingID { get; set; }

        [Required]
        public DateTime BookingStartDate { get; set; }
        [Required]
        public DateTime BookingEndDate { get; set; }
        public bool IsBooked { get; set; }
        public bool IsActive { get; set; }

        public int CustomerID { get; set; }
        public int RoomID { get; set; }
        public int InvoiceID { get; set; }

        public Customer Customer { get; set; }
        public Room Room { get; set; }
        public Invoice Invoice { get; set; }
    }
}
