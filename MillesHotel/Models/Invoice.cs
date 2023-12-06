using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MillesHotel.Entities
{
    public class Invoice
    {
        private List<Invoice> _invoice;
        public Invoice()
        {
            _invoice = new List<Invoice>();
        }
        [Key]
        public int InvoiceID { get; set; }
        public double InvoiceAmount { get; set; }
        public DateTime InvoiceDue { get; set; }
        //public int CustomerID (FK)

    }
}
