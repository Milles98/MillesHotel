using MillesHotelLibrary.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MillesHotelLibrary.Models
{
    public class Invoice : IInvoice
    {
        [Key]
        public int InvoiceID { get; set; }

        [Required]
        public double InvoiceAmount { get; set; }

        [Required]
        public DateTime InvoiceDue { get; set; }
        public bool IsPaid
        {
            get
            {
                return !IsActive && DateTime.Now <= InvoiceDue;
            }
            set
            {

            }
        }
        public bool IsActive { get; set; } = true;

        public List<Booking>? Bookings { get; set; }
    }
}
