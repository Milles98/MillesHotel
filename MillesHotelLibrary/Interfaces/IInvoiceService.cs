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
        void GetInvoiceByID();
        void UpdateInvoice();
        void GetAllInvoices();
        void SoftDeleteInvoice();
        void RegisterPayment();
        void CheckAndDeactivateOverdueBookings();
    }
}
