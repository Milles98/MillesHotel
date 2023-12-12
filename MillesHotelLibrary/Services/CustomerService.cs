using Microsoft.EntityFrameworkCore;
using MillesHotelLibrary.Data;
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

            Console.WriteLine("Press any button to continue...");
            Console.ReadKey();
        }

        public void GetCustomerByID()
        {
            foreach (var showCustomer in _dbContext.Customers)
            {
                Console.WriteLine($"CustomerID: {showCustomer.CustomerID}");
            }

            Console.Write("Input Customer ID: ");
            int customerId = Convert.ToInt32(Console.ReadLine());

            var customer = _dbContext.Customers.Find(customerId);

            if (customer != null)
            {
                Console.WriteLine($"Customer ID: {customer.CustomerID}");
                Console.WriteLine($"First Name: {customer.CustomerFirstName}");
                Console.WriteLine($"Last Name: {customer.CustomerLastName}");
                Console.WriteLine($"Age: {customer.CustomerAge}");
                Console.WriteLine($"Email: {customer.CustomerEmail}");
                Console.WriteLine($"Phone: {customer.CustomerPhone}");
                Console.WriteLine($"Country: {customer.CustomerCountry}");
                Console.WriteLine($"Is Active: {customer.IsActive}");
            }
            else
            {
                Console.WriteLine("Customer not found.");
            }

            Console.WriteLine("Press any button to continue...");
            Console.ReadKey();
        }

        public void UpdateCustomer()
        {
            foreach (var showCustomer in _dbContext.Customers)
            {
                Console.WriteLine($"CustomerID: {showCustomer.CustomerID}");
            }

            Console.Write("Input Customer ID: ");
            int customerId = Convert.ToInt32(Console.ReadLine());

            var customer = _dbContext.Customers.Find(customerId);

            if (customer != null)
            {
                Console.Write("Enter new customer first name: ");
                customer.CustomerFirstName = Console.ReadLine();

                Console.Write("Enter new customer last name: ");
                customer.CustomerLastName = Console.ReadLine();

                Console.Write("Enter new customer age: ");
                if (int.TryParse(Console.ReadLine(), out int age))
                {
                    customer.CustomerAge = age;
                }
                else
                {
                    Console.WriteLine("Invalid age input. Age not updated.");
                }

                Console.Write("Enter new customer email: ");
                customer.CustomerEmail = Console.ReadLine();

                Console.Write("Enter new customer phone: ");
                customer.CustomerPhone = Console.ReadLine();

                Console.Write("Enter new customer country: ");
                customer.CustomerCountry = Console.ReadLine();

                _dbContext.SaveChanges();
                Console.WriteLine("Customer information updated successfully.");
            }
            else
            {
                Console.WriteLine("Customer not found.");
            }

            Console.WriteLine("Press any button to continue...");
            Console.ReadKey();
        }

        public void SoftDeleteCustomer()
        {
            foreach (var showCustomer in _dbContext.Customers)
            {
                Console.WriteLine($"CustomerID: {showCustomer.CustomerID}");
            }

            Console.Write("Input Customer ID: ");
            int customerId = Convert.ToInt32(Console.ReadLine());

            var customer = _dbContext.Customers.Find(customerId);

            if (customer != null)
            {
                customer.IsActive = false;
                _dbContext.SaveChanges();
                Console.WriteLine("Customer soft deleted successfully.");
            }
            else
            {
                Console.WriteLine("Customer not found.");
            }

            Console.WriteLine("Press any button to continue...");
            Console.ReadKey();
        }

    }
}
