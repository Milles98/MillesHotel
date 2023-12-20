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
        void GetAllCustomers();
        void SoftDeleteCustomer();
        void UpdateCustomerFirstName();
        void UpdateCustomerLastName();
        void UpdateCustomerAge();
        void UpdateCustomerEmail();
        bool IsValidEmail(string email);
        void UpdateCustomerPhone();
        bool IsValidPhoneNumber(string phoneNumber);
        void UpdateCustomerCountry();
        void ReactivateCustomer();
    }
}
