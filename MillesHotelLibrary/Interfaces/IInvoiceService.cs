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
        void CreateInvoice();
        void AssignInvoiceToBooking(Invoice newInvoice);
        void GetInvoiceByID();
        void GetAllInvoices();
        void GetAllPaidInvoices();
        void GetAllUnpaidInvoices();
        void UpdateInvoiceAmount();
        void UpdateInvoiceDue();
        void SoftDeleteInvoice();
        void RegisterPayment();
        void CheckAndDeactivateOverdueBookings();
    }
}
