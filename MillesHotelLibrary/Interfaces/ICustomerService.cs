using MillesHotel.Models;
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
        Customer CreateCustomer(Customer newCustomer);
        Customer GetCustomerByID(int customerID);
        void UpdateCustomer(Customer updatedCustomer);
        void DeleteCustomer(int customerID);
    }
}
