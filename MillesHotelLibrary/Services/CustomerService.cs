using MillesHotelLibrary.Interfaces;
using MillesHotelLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MillesHotelLibrary.Services
{
    public class CustomerService : ICustomerService
    {
        private List<Customer> customers = new List<Customer>();

        public Customer CreateCustomer(Customer newCustomer)
        {
            return newCustomer;
        }

        public Customer GetCustomerByID(int customerID)
        {
            return customers[customerID];
        }

        public void UpdateCustomer(Customer updatedCustomer)
        {

        }

        public void DeleteCustomer(int customerID)
        {

        }

    }
}
