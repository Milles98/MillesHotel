using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MillesHotelLibrary.Interfaces
{
    public interface IAdminService
    {
        void DeleteRoom();
        void DeleteCustomer();
        void DeleteBooking();
        void DeleteInvoice();
        void GetNumberOfCustomers();
        void GetTop10Customers();
        void GetTop10CustomersByBooking();
        void GetTop10CustomersByCountry();
    }
}
