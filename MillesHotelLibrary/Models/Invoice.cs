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
        public bool IsActive
        {
            get
            {
                return InvoiceAmount > 0 && DateTime.Now <= InvoiceDue;
            }
            set
            {

            }
        }

        // Foreign key för att koppla till Customer
        public int CustomerID { get; set; }
        public Customer? Customer { get; set; }
        // Foreign key för att koppla till Booking
        //public int? BookingID { get; set; }  // Nullable because it's optional
        public Booking? Booking { get; set; }

        public override string ToString()
        {
            return $"Amount: {InvoiceAmount}kr, Due Date: {InvoiceDue.ToString("yyyy-MM-dd")}, CustomerID: {CustomerID}";
        }

    }
}
