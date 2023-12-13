using MillesHotelLibrary.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MillesHotelLibrary.Interfaces
{
    public interface ICustomerService
    {
        void CreateCustomer();
        void GetCustomerByID();
        void SoftDeleteCustomer();
        void UpdateCustomerFirstName();
        void UpdateCustomerLastName();
        void UpdateCustomerAge();
        void UpdateCustomerEmail();
        void UpdateCustomerPhone();
        void UpdateCustomerCountry();
    }
}
