using MillesHotelLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MillesHotelLibrary.Interfaces
{
    public interface IInvoiceService
    {
        Invoice CreateInvoice(Invoice newInvoice);
        Invoice GetInvoiceByID(int invoiceID);
        void UpdateInvoice(Invoice updatedInvoice);
        void DeleteInvoice(int invoiceID);
    }
}
