using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MillesHotelLibrary.Models
{
    public class Invoice
    {
        [Key]
        public int InvoiceID { get; set; }

        [Required]
        public double InvoiceAmount { get; set; }

        [Required]
        public DateTime InvoiceDue { get; set; }
        public bool IsActive { get; set; }

        // Foreign key för att koppla till Customer
        public int CustomerID { get; set; }
        public Customer Customer { get; set; }

    }
}
