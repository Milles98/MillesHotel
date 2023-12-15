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

        // Foreign key för att koppla till Customer
        public int CustomerID { get; set; }
        public Customer? Customer { get; set; }

        // Foreign key för att koppla till Room
        public int? RoomID { get; set; }
        public Room? Room { get; set; }
        // Foreign key för att koppla till Invoice
        //Till varje bokning skall det kopplas en betalning dvs en faktura.
        public int? InvoiceID { get; set; }
        public Invoice? Invoice { get; set; }

        public override string ToString()
        {
            return $"Start Date: {BookingStartDate.ToString("yyyy-MM-dd")}, End Date: {BookingEndDate.ToString("yyyy-MM-dd")}, CustomerID: {CustomerID}, RoomID: {RoomID}";
        }

    }
}
