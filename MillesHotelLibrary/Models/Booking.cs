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
        public bool IsBooked
        {
            get
            {
                return BookingStartDate <= DateTime.Now && DateTime.Now <= BookingEndDate;
            }
            set
            {
            }
        }

        public bool IsActive { get; set; } = true;

        public int CustomerID { get; set; }
        public int RoomID { get; set; }
        public int InvoiceID { get; set; }

        public Customer Customer { get; set; }
        public Room Room { get; set; }
        public Invoice Invoice { get; set; }

        public override string ToString()
        {
            return $"BookingID: {BookingID}, BookingStartDate: {BookingStartDate:yyyy-MM-dd}, RoomID: {RoomID}, InvoiceID: {InvoiceID}";
        }
    }
}
