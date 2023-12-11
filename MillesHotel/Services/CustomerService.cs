using Microsoft.EntityFrameworkCore;
using MillesHotel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MillesHotel.Services
{
    public class CustomerService
    {
        private readonly HotelDbContext _dbContext;

        public CustomerService(DbContextOptionsBuilder<HotelDbContext> options)
        {
            _dbContext = new HotelDbContext(options.Options);
        }

        public void CreateCustomer()
        {
            Console.Write("Enter new customer first name:");
            string firstName = Console.ReadLine();

            Console.Write("Enter customer last name: ");
            string lastName = Console.ReadLine();

            Console.Write("Enter customer age: ");
            if (int.TryParse(Console.ReadLine(), out int age))
            {
                Console.Write("Enter customer email: ");
                string email = Console.ReadLine();

                Console.Write("Enter customer phone: ");
                string phone = Console.ReadLine();

                Console.Write("Enter customer country: ");
                string country = Console.ReadLine();

                var newCustomer = new Customer()
                {
                    CustomerFirstName = firstName,
                    CustomerLastName = lastName,
                    CustomerAge = age,
                    CustomerEmail = email,
                    CustomerPhone = phone,
                    CustomerCountry = country,
                    IsActive = true
                };

                _dbContext.Customers.Add(newCustomer);
                _dbContext.SaveChanges();
            }
            else
            {
                Console.WriteLine("Invalid age input. Please enter a valid number.");
            }
        }

        public void GetCustomerByID()
        {
        }

        public void UpdateCustomer()
        {

        }

        public void DeleteCustomer()
        {

        }

    }
}
