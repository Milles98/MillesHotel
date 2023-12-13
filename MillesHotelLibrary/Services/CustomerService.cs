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

        public CustomerService(HotelDbContext dbContext)
        {
            _dbContext = dbContext;
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
                Console.WriteLine();
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

        public void GetAllBookings()
        {
            var customers = _dbContext.Customers.ToList();

            Console.WriteLine("╭─────────────╮─────────────╮─────────────╮─────╮─────────────────────────╮───────────────╮─────────╮───────────╮");
            Console.WriteLine("│Customer ID  │ First Name  │ Last Name   │ Age │ Email                   │ Phone         │ Country │ Is Active │");
            Console.WriteLine("├─────────────┼─────────────┼─────────────┼─────┼─────────────────────────┼───────────────┼─────────┼───────────┤");

            foreach (var customer in customers)
            {
                Console.WriteLine($"│{customer.CustomerID,-13}│{customer.CustomerFirstName,-13}│{customer.CustomerLastName,-13}│{customer.CustomerAge,-5}│{customer.CustomerEmail,-25}│{customer.CustomerPhone,-15}│{customer.CustomerCountry,-9}│{customer.IsActive,-11}│");
                Console.WriteLine("├─────────────┼─────────────┼─────────────┼─────┼─────────────────────────┼───────────────┼─────────┼───────────┤");
            }

            Console.WriteLine("╰─────────────╯─────────────╯─────────────╯─────╯─────────────────────────╯───────────────╯─────────╯───────────╯");
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

        public void UpdateCustomerFirstName()
        {
            Console.WriteLine("Available CustomerIDs:");
            foreach (var customersID in _dbContext.Customers)
            {
                Console.WriteLine($"CustomerID: {customersID.CustomerID}, Name: {customersID.CustomerFirstName} {customersID.CustomerLastName}");
            }

            Console.Write("Input Customer ID: ");
            int customerId = Convert.ToInt32(Console.ReadLine());

            var customer = _dbContext.Customers.Find(customerId);

            Console.Write("Input New First Name: ");
            string newName = Console.ReadLine();

            if (customer != null)
            {
                customer.CustomerFirstName = newName;
                _dbContext.SaveChanges();
                Console.WriteLine("Customer name updated successfully.");
            }
            else
            {
                Console.WriteLine("Customer not found.");
            }
            Console.WriteLine("Press any button to continue...");
            Console.ReadKey();
        }

        public void UpdateCustomerLastName()
        {
            Console.WriteLine("Available CustomerIDs:");
            foreach (var customersID in _dbContext.Customers)
            {
                Console.WriteLine($"CustomerID: {customersID.CustomerID}, Name: {customersID.CustomerFirstName} {customersID.CustomerLastName}");
            }

            Console.Write("Input Customer ID: ");
            int customerId = Convert.ToInt32(Console.ReadLine());

            var customer = _dbContext.Customers.Find(customerId);

            Console.Write("Input New Last Name: ");
            string newLastName = Console.ReadLine();

            if (customer != null)
            {
                customer.CustomerLastName = newLastName;
                _dbContext.SaveChanges();
                Console.WriteLine("Customer last name updated successfully.");
            }
            else
            {
                Console.WriteLine("Customer not found.");
            }
            Console.WriteLine("Press any button to continue...");
            Console.ReadKey();
        }

        public void UpdateCustomerAge()
        {
            Console.WriteLine("Available CustomerIDs:");
            foreach (var customersID in _dbContext.Customers)
            {
                Console.WriteLine($"CustomerID: {customersID.CustomerID}, Name: {customersID.CustomerFirstName} {customersID.CustomerLastName}, " +
                    $"Age: {customersID.CustomerAge}");
            }

            Console.Write("Input Customer ID: ");
            int customerId = Convert.ToInt32(Console.ReadLine());

            var customer = _dbContext.Customers.Find(customerId);

            Console.Write("Input New Age: ");
            int newAge = Convert.ToInt32(Console.ReadLine());

            if (customer != null)
            {
                customer.CustomerAge = newAge;
                _dbContext.SaveChanges();
                Console.WriteLine("Customer age updated successfully.");
            }
            else
            {
                Console.WriteLine("Customer not found.");
            }
            Console.WriteLine("Press any button to continue...");
            Console.ReadKey();
        }

        public void UpdateCustomerEmail()
        {
            Console.WriteLine("Available CustomerIDs:");
            foreach (var customersID in _dbContext.Customers)
            {
                Console.WriteLine($"CustomerID: {customersID.CustomerID}, Name: {customersID.CustomerFirstName} {customersID.CustomerLastName}, " +
                    $"Email: {customersID.CustomerEmail}");
            }

            Console.Write("Input Customer ID: ");
            int customerId = Convert.ToInt32(Console.ReadLine());

            var customer = _dbContext.Customers.Find(customerId);

            Console.Write("Input New Email: ");
            string newEmail = Console.ReadLine();

            if (customer != null)
            {
                customer.CustomerEmail = newEmail;
                _dbContext.SaveChanges();
                Console.WriteLine("Customer email updated successfully.");
            }
            else
            {
                Console.WriteLine("Customer not found.");
            }
            Console.WriteLine("Press any button to continue...");
            Console.ReadKey();
        }

        public void UpdateCustomerPhone()
        {
            Console.WriteLine("Available CustomerIDs:");
            foreach (var customersID in _dbContext.Customers)
            {
                Console.WriteLine($"CustomerID: {customersID.CustomerID}, Name: {customersID.CustomerFirstName} {customersID.CustomerLastName}, " +
                    $"Phone: {customersID.CustomerPhone}");
            }

            Console.Write("Input Customer ID: ");
            int customerId = Convert.ToInt32(Console.ReadLine());

            var customer = _dbContext.Customers.Find(customerId);

            Console.Write("Input New Phone Number: ");
            string newPhone = Console.ReadLine();

            if (customer != null)
            {
                customer.CustomerPhone = newPhone;
                _dbContext.SaveChanges();
                Console.WriteLine("Customer phone updated successfully.");
            }
            else
            {
                Console.WriteLine("Customer not found.");
            }
            Console.WriteLine("Press any button to continue...");
            Console.ReadKey();
        }

        public void UpdateCustomerCountry()
        {
            Console.WriteLine("Available CustomerIDs:");
            foreach (var customersID in _dbContext.Customers)
            {
                Console.WriteLine($"CustomerID: {customersID.CustomerID}, Name: {customersID.CustomerFirstName} {customersID.CustomerLastName}, " +
                    $"{customersID.CustomerCountry}");
            }

            Console.Write("Input Customer ID: ");
            int customerId = Convert.ToInt32(Console.ReadLine());

            var customer = _dbContext.Customers.Find(customerId);

            Console.Write("Input New Country: ");
            string newCountry = Console.ReadLine();

            if (customer != null)
            {
                customer.CustomerCountry = newCountry;
                _dbContext.SaveChanges();
                Console.WriteLine("Customer country updated successfully.");
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
