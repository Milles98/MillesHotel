using Microsoft.EntityFrameworkCore;
using MillesHotelLibrary.Data;
using MillesHotelLibrary.ExtraServices;
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
            try
            {
                Console.Write("Enter new customer first name (max 13 characters): ");
                string firstName = Console.ReadLine();

                Console.Write("Enter customer last name (max 13 characters): ");
                string lastName = Console.ReadLine();

                Console.Write("Enter customer age (between 2 and 150): ");
                if (int.TryParse(Console.ReadLine(), out int age) && age >= 2 && age <= 150)
                {
                    Console.Write("Enter customer email (max 25 characters): ");
                    string email = Console.ReadLine();

                    Console.Write("Enter customer phone (max 15 characters): ");
                    string phone = Console.ReadLine();

                    Console.Write("Enter customer country (max 9 characters): ");
                    string country = Console.ReadLine();

                    if (firstName.Length >= 2 && firstName.Length <= 13 &&
                        lastName.Length >= 2 && lastName.Length <= 13 &&
                        email.Length >= 2 && email.Length <= 25 &&
                        phone.Length <= 15 &&
                        country.Length <= 9)
                    {
                        var newCustomer = new Customer()
                        {
                            CustomerFirstName = char.ToUpper(firstName[0]) + firstName.Substring(1),
                            CustomerLastName = char.ToUpper(lastName[0]) + lastName.Substring(1),
                            CustomerAge = age,
                            CustomerEmail = char.ToUpper(email[0]) + email.Substring(1),
                            CustomerPhone = phone,
                            CustomerCountry = char.ToUpper(country[0]) + country.Substring(1),
                            IsActive = true
                        };

                        _dbContext.Customers.Add(newCustomer);
                        _dbContext.SaveChanges();
                        Message.InputSuccessMessage("Customer created successfully.");
                    }
                    else
                    {
                        Message.ErrorMessage("Input length requirements not met. Please try again.");
                    }
                }
                else
                {
                    Message.ErrorMessage("Invalid age input. Please enter a valid number between 2 and 150.");
                }
            }
            catch (Exception ex)
            {
                Message.ErrorMessage($"An error occurred: {ex.Message}");
            }

            Console.WriteLine("Press any button to continue...");
            Console.ReadKey();
        }
        public void GetCustomerByID()
        {
            try
            {
                foreach (var showCustomer in _dbContext.Customers)
                {
                    Console.WriteLine($"CustomerID: {showCustomer.CustomerID}");
                }

                Console.Write("Input Customer ID: ");
                if (int.TryParse(Console.ReadLine(), out int customerId))
                {
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
                        Message.ErrorMessage("Customer not found.");
                    }
                }
                else
                {
                    Message.ErrorMessage("Invalid input. Please enter a valid Customer ID.");
                }
            }
            catch (Exception ex)
            {
                Message.ErrorMessage($"An error occurred: {ex.Message}");
            }

            Console.WriteLine("Press any button to continue...");
            Console.ReadKey();
        }
        public void GetAllCustomers()
        {
            var customers = _dbContext.Customers.ToList();

            Console.WriteLine("╭─────────────╮─────────────╮─────────────╮─────╮─────────────────────────╮───────────────╮─────────╮──────────╮");
            Console.WriteLine("│Customer ID  │ First Name  │ Last Name   │ Age │ Email                   │ Phone         │ Country │ Status   │");
            Console.WriteLine("├─────────────┼─────────────┼─────────────┼─────┼─────────────────────────┼───────────────┼─────────┤──────────┤");

            foreach (var customer in customers)
            {
                if (customer.IsActive)
                {
                    Console.WriteLine($"│{customer.CustomerID,-13}│{customer.CustomerFirstName,-13}│{customer.CustomerLastName,-13}│" +
                        $"{customer.CustomerAge,-5}│{customer.CustomerEmail,-25}│{customer.CustomerPhone,-15}│{customer.CustomerCountry,-9}│ ACTIVE   │");
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red; // Set the text color for inactive customers
                    Console.WriteLine($"│{customer.CustomerID,-13}│{customer.CustomerFirstName,-13}│{customer.CustomerLastName,-13}│" +
                        $"{customer.CustomerAge,-5}│{customer.CustomerEmail,-25}│{customer.CustomerPhone,-15}│{customer.CustomerCountry,-9}│ INACTIVE │");
                    Console.ResetColor(); // Reset the text color to default
                }

                Console.WriteLine("├─────────────┼─────────────┼─────────────┼─────┼─────────────────────────┼───────────────┼─────────┤──────────┤");
            }

            Console.WriteLine("╰─────────────╯─────────────╯─────────────╯─────╯─────────────────────────╯───────────────╯─────────╯──────────╯");
            Console.WriteLine("Press any button to continue...");
            Console.ReadKey();
        }
        //Applikationen måste kontrollera om det finns bokningar innan den tar bort en kund.
        public void SoftDeleteCustomer()
        {
            try
            {
                foreach (var showCustomer in _dbContext.Customers)
                {
                    Console.WriteLine($"CustomerID: {showCustomer.CustomerID}");
                }

                Console.Write("Input Customer ID: ");
                if (int.TryParse(Console.ReadLine(), out int customerId))
                {
                    var customer = _dbContext.Customers.Include(c => c.Bookings).FirstOrDefault(c => c.CustomerID == customerId);

                    if (customer != null)
                    {
                        if (customer.Bookings == null || !customer.Bookings.Any())
                        {
                            customer.IsActive = false;
                            _dbContext.SaveChanges();
                            Message.InputSuccessMessage("Customer soft deleted successfully.");
                        }
                        else
                        {
                            Message.ErrorMessage("Cannot delete customer with associated bookings. Please remove bookings first.");
                        }
                    }
                    else
                    {
                        Message.ErrorMessage("Customer not found.");
                    }
                }
                else
                {
                    Message.ErrorMessage("Invalid input. Please enter a valid Customer ID.");
                }
            }
            catch (Exception ex)
            {
                Message.ErrorMessage($"An error occurred: {ex.Message}");
            }

            Console.WriteLine("Press any button to continue...");
            Console.ReadKey();
        }
        public void ReactiveCustomer()
        {
            try
            {
                foreach (var showCustomer in _dbContext.Customers.Where(c => !c.IsActive))
                {
                    Console.WriteLine($"Inactive CustomerID: {showCustomer.CustomerID}");
                }

                Console.Write("Input Inactive Customer ID to reactivate: ");
                if (int.TryParse(Console.ReadLine(), out int customerId))
                {
                    var customer = _dbContext.Customers.FirstOrDefault(c => c.CustomerID == customerId && !c.IsActive);

                    if (customer != null)
                    {
                        customer.IsActive = true;
                        _dbContext.SaveChanges();
                        Message.InputSuccessMessage("Customer reactivated successfully.");
                    }
                    else
                    {
                        Message.ErrorMessage("Inactive customer not found. Please enter a valid inactive Customer ID.");
                    }
                }
                else
                {
                    Message.ErrorMessage("Invalid input. Please enter a valid inactive Customer ID.");
                }
            }
            catch (Exception ex)
            {
                Message.ErrorMessage($"An error occurred: {ex.Message}");
            }

            Console.WriteLine("Press any button to continue...");
            Console.ReadKey();
        }
        public void UpdateCustomerFirstName()
        {
            try
            {
                Console.WriteLine("Available CustomerIDs:");
                foreach (var customersID in _dbContext.Customers)
                {
                    Console.WriteLine($"CustomerID: {customersID.CustomerID}, Name: {customersID.CustomerFirstName} {customersID.CustomerLastName}");
                }

                Console.Write("Input Customer ID: ");
                if (int.TryParse(Console.ReadLine(), out int customerId))
                {
                    var customer = _dbContext.Customers.Find(customerId);

                    if (customer != null)
                    {
                        Console.Write("Input New First Name (max 13 characters): ");
                        string newName = Console.ReadLine();

                        if (!string.IsNullOrWhiteSpace(newName) && newName.Length >= 2 && newName.Length <= 13)
                        {
                            customer.CustomerFirstName = char.ToUpper(newName[0]) + newName.Substring(1);
                            _dbContext.SaveChanges();
                            Message.InputSuccessMessage("Customer name updated successfully.");
                        }
                        else
                        {
                            Message.ErrorMessage("Invalid input for the new first name. Please enter a valid name with at least 2 characters.");
                        }
                    }
                    else
                    {
                        Message.ErrorMessage("Customer not found.");
                    }
                }
                else
                {
                    Message.ErrorMessage("Invalid input. Please enter a valid Customer ID.");
                }
            }
            catch (Exception ex)
            {
                Message.ErrorMessage($"An error occurred: {ex.Message}");
            }

            Console.WriteLine("Press any button to continue...");
            Console.ReadKey();
        }
        public void UpdateCustomerLastName()
        {
            try
            {
                Console.WriteLine("Available CustomerIDs:");
                foreach (var customersID in _dbContext.Customers)
                {
                    Console.WriteLine($"CustomerID: {customersID.CustomerID}, Name: {customersID.CustomerFirstName} {customersID.CustomerLastName}");
                }

                Console.Write("Input Customer ID: ");
                if (int.TryParse(Console.ReadLine(), out int customerId))
                {
                    var customer = _dbContext.Customers.Find(customerId);

                    if (customer != null)
                    {
                        Console.Write("Input New Last Name (max 13 characters): ");
                        string newLastName = Console.ReadLine();

                        if (!string.IsNullOrWhiteSpace(newLastName) && newLastName.Length >= 2 && newLastName.Length <= 13)
                        {
                            customer.CustomerLastName = char.ToUpper(newLastName[0]) + newLastName.Substring(1);
                            _dbContext.SaveChanges();
                            Message.InputSuccessMessage("Customer last name updated successfully.");
                        }
                        else
                        {
                            Message.ErrorMessage("Invalid input for the new last name. Please enter a valid last name with at least 2 characters.");
                        }
                    }
                    else
                    {
                        Message.ErrorMessage("Customer not found.");
                    }
                }
                else
                {
                    Message.ErrorMessage("Invalid input. Please enter a valid Customer ID.");
                }
            }
            catch (Exception ex)
            {
                Message.ErrorMessage($"An error occurred: {ex.Message}");
            }

            Console.WriteLine("Press any button to continue...");
            Console.ReadKey();
        }
        public void UpdateCustomerAge()
        {
            try
            {
                Console.WriteLine("Available CustomerIDs:");
                foreach (var customersID in _dbContext.Customers)
                {
                    Console.WriteLine($"CustomerID: {customersID.CustomerID}, Name: {customersID.CustomerFirstName} {customersID.CustomerLastName}, Age: {customersID.CustomerAge}");
                }

                Console.Write("Input Customer ID: ");
                if (int.TryParse(Console.ReadLine(), out int customerId))
                {
                    var customer = _dbContext.Customers.Find(customerId);

                    if (customer != null)
                    {
                        Console.Write("Input New Age (between 2 and 150 years): ");
                        if (int.TryParse(Console.ReadLine(), out int newAge))
                        {
                            if (newAge >= 2 && newAge <= 150)
                            {
                                customer.CustomerAge = newAge;
                                _dbContext.SaveChanges();
                                Message.InputSuccessMessage("Customer age updated successfully.");
                            }
                            else
                            {
                                Message.ErrorMessage("Invalid input for the new age. Please enter a valid age between 0 and 150.");
                            }
                        }
                        else
                        {
                            Message.ErrorMessage("Invalid input for the new age. Please enter a valid integer.");
                        }
                    }
                    else
                    {
                        Message.ErrorMessage("Customer not found.");
                    }
                }
                else
                {
                    Message.ErrorMessage("Invalid input. Please enter a valid Customer ID.");
                }
            }
            catch (Exception ex)
            {
                Message.ErrorMessage($"An error occurred: {ex.Message}");
            }

            Console.WriteLine("Press any button to continue...");
            Console.ReadKey();
        }
        public void UpdateCustomerEmail()
        {
            try
            {
                Console.WriteLine("Available CustomerIDs:");
                foreach (var customersID in _dbContext.Customers)
                {
                    Console.WriteLine($"CustomerID: {customersID.CustomerID}, Name: {customersID.CustomerFirstName} {customersID.CustomerLastName}, Email: {customersID.CustomerEmail}");
                }

                Console.Write("Input Customer ID: ");
                if (int.TryParse(Console.ReadLine(), out int customerId))
                {
                    var customer = _dbContext.Customers.Find(customerId);

                    if (customer != null)
                    {
                        Console.Write("Input New Email (max 25 characters): ");
                        string newEmail = Console.ReadLine();

                        if (IsValidEmail(newEmail) && newEmail.Length <= 25)
                        {
                            customer.CustomerEmail = char.ToUpper(newEmail[0]) + newEmail.Substring(1);
                            _dbContext.SaveChanges();
                            Message.InputSuccessMessage("Customer email updated successfully.");
                        }
                        else
                        {
                            Message.ErrorMessage("Invalid email format or length. Please enter a valid email address with a maximum length of 25 characters.");
                        }
                    }
                    else
                    {
                        Message.ErrorMessage("Customer not found.");
                    }
                }
                else
                {
                    Message.ErrorMessage("Invalid input. Please enter a valid Customer ID.");
                }
            }
            catch (Exception ex)
            {
                Message.ErrorMessage($"An error occurred: {ex.Message}");
            }

            Console.WriteLine("Press any button to continue...");
            Console.ReadKey();
        }
        public bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
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
            if (int.TryParse(Console.ReadLine(), out int customerId))
            {
                var customer = _dbContext.Customers.Find(customerId);

                if (customer != null)
                {
                    Console.Write("Input New Phone Number (max 15 characters): ");
                    string newPhone = Console.ReadLine();

                    if (IsValidPhoneNumber(newPhone) && newPhone.Length <= 15)
                    {
                        customer.CustomerPhone = newPhone;
                        _dbContext.SaveChanges();
                        Message.InputSuccessMessage("Customer phone updated successfully.");
                    }
                    else
                    {
                        Message.ErrorMessage("Invalid phone number format. Customer phone not updated.");
                    }
                }
                else
                {
                    Message.ErrorMessage("Customer not found.");
                }
            }
            else
            {
                Message.ErrorMessage("Invalid Customer ID. Please enter a valid number.");
            }

            Console.WriteLine("Press any button to continue...");
            Console.ReadKey();
        }
        public bool IsValidPhoneNumber(string phoneNumber)
        {
            return !string.IsNullOrEmpty(phoneNumber);
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
            if (int.TryParse(Console.ReadLine(), out int customerId))
            {
                var customer = _dbContext.Customers.Find(customerId);

                if (customer != null)
                {
                    Console.Write("Input New Country (max 9 characters): ");
                    string newCountry = Console.ReadLine();

                    if (!string.IsNullOrEmpty(newCountry) && newCountry.Length <= 9)
                    {
                        customer.CustomerCountry = char.ToUpper(newCountry[0]) + newCountry.Substring(1);
                        _dbContext.SaveChanges();
                        Message.InputSuccessMessage("Customer country updated successfully.");
                    }
                    else
                    {
                        Message.ErrorMessage("Invalid country input. Customer country not updated.");
                    }
                }
                else
                {
                    Message.ErrorMessage("Customer not found.");
                }
            }
            else
            {
                Message.ErrorMessage("Invalid Customer ID. Please enter a valid number.");
            }

            Console.WriteLine("Press any button to continue...");
            Console.ReadKey();
        }

    }
}
