using MillesHotelLibrary.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MillesHotelLibrary.Models
{
    public class Booking : IBooking
    {
        [Key]
        public int BookingID { get; set; }

        [Required]
        public DateTime BookingStartDate { get; set; }
        [Required]
        public DateTime BookingEndDate { get; set; }

        public bool IsActive { get; set; } = true;

        public int CustomerID { get; set; }
        public int RoomID { get; set; }
        public int InvoiceID { get; set; }

        public Customer Customer { get; set; }
        public Room Room { get; set; }
        public Invoice Invoice { get; set; }

        public bool IsOccupied()
        {
            return BookingStartDate <= DateTime.UtcNow && DateTime.UtcNow <= BookingEndDate;
        }
        public override string ToString()
        {
            return $"BookingID: {BookingID}, StartDate: {BookingStartDate:yyyy-MM-dd}, EndDate " +
                $"{BookingEndDate:yyyy-MM-dd}, RoomID: {RoomID}, InvoiceID: {InvoiceID}";
        }
    }
}
