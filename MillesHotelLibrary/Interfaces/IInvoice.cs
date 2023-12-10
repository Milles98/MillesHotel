using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MillesHotelLibrary.Interfaces
{
    public interface IInvoice
    {
        public int InvoiceID { get; set; }

        public double InvoiceAmount { get; set; }

        public DateTime InvoiceDue { get; set; }
        public bool IsActive { get; set; }

        // Foreign key för att koppla till Customer
        public int CustomerID { get; set; }
    }
}
