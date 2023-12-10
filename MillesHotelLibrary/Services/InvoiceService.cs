using MillesHotelLibrary.Interfaces;
using MillesHotel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MillesHotelLibrary.Services
{
    public class InvoiceService : IInvoiceService
    {
        private List<Invoice> invoices = new List<Invoice>();

        public Invoice CreateInvoice(Invoice newInvoice)
        {
            return newInvoice;
        }

        public Invoice GetInvoiceByID(int invoiceID)
        {
            return invoices[invoiceID];
        }

        public void UpdateInvoice(Invoice updatedInvoice)
        {

        }

        public void DeleteInvoice(int invoiceID)
        {

        }
    }
}
