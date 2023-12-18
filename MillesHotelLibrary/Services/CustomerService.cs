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
                Console.Write("Enter new customer first name: ");
                string firstName = Console.ReadLine();

                Console.Write("Enter customer last name: ");
                string lastName = Console.ReadLine();

                Console.Write("Enter customer age (between 2 and 150): ");
                if (int.TryParse(Console.ReadLine(), out int age) && age >= 2 && age <= 150)
                {
                    Console.Write("Enter customer email: ");
                    string email = Console.ReadLine();

                    Console.Write("Enter customer phone: ");
                    string phone = Console.ReadLine();

                    Console.Write("Enter customer country: ");
                    string country = Console.ReadLine();

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
                    UserMessage.InputSuccessMessage("Customer created successfully.");
                }
                else
                {
                    UserMessage.ErrorMessage("Invalid age input. Please enter a valid number between 2 and 150.");
                }
            }
            catch (Exception ex)
            {
                UserMessage.ErrorMessage($"An error occurred: {ex.Message}");
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
                        UserMessage.ErrorMessage("Customer not found.");
                    }
                }
                else
                {
                    UserMessage.ErrorMessage("Invalid input. Please enter a valid Customer ID.");
                }
            }
            catch (Exception ex)
            {
                UserMessage.ErrorMessage($"An error occurred: {ex.Message}");
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
                            UserMessage.InputSuccessMessage("Customer soft deleted successfully.");
                        }
                        else
                        {
                            UserMessage.ErrorMessage("Cannot delete customer with associated bookings. Please remove bookings first.");
                        }
                    }
                    else
                    {
                        UserMessage.ErrorMessage("Customer not found.");
                    }
                }
                else
                {
                    UserMessage.ErrorMessage("Invalid input. Please enter a valid Customer ID.");
                }
            }
            catch (Exception ex)
            {
                UserMessage.ErrorMessage($"An error occurred: {ex.Message}");
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
                        UserMessage.InputSuccessMessage("Customer reactivated successfully.");
                    }
                    else
                    {
                        UserMessage.ErrorMessage("Inactive customer not found. Please enter a valid inactive Customer ID.");
                    }
                }
                else
                {
                    UserMessage.ErrorMessage("Invalid input. Please enter a valid inactive Customer ID.");
                }
            }
            catch (Exception ex)
            {
                UserMessage.ErrorMessage($"An error occurred: {ex.Message}");
            }

            Console.WriteLine("Press any button to continue...");
            Console.ReadKey();
        }
        public void UpdateCustomerFirstName()
        {
            try
            {
                Console.WriteLine("Available CustomerIDs:");
                foreach (var customerID in _dbContext.Customers)
                {
                    Console.WriteLine($"CustomerID: {customerID.CustomerID}, Name: {customerID.CustomerFirstName} {customerID.CustomerLastName}");
                }

                Console.Write("Input Customer ID: ");
                if (int.TryParse(Console.ReadLine(), out int customerId))
                {
                    var customer = _dbContext.Customers.Find(customerId);

                    if (customer != null)
                    {
                        Console.Write("Input New First Name: ");
                        string newName = Console.ReadLine();

                        if (!string.IsNullOrWhiteSpace(newName))
                        {
                            customer.CustomerFirstName = char.ToUpper(newName[0]) + newName.Substring(1);
                            _dbContext.SaveChanges();
                            UserMessage.InputSuccessMessage("Customer name updated successfully.");
                        }
                        else
                        {
                            UserMessage.ErrorMessage("Invalid input for the new first name. Please enter a valid name.");
                        }
                    }
                    else
                    {
                        UserMessage.ErrorMessage("Customer not found.");
                    }
                }
                else
                {
                    UserMessage.ErrorMessage("Invalid input. Please enter a valid Customer ID.");
                }
            }
            catch (Exception ex)
            {
                UserMessage.ErrorMessage($"An error occurred: {ex.Message}");
            }

            Console.WriteLine("Press any button to continue...");
            Console.ReadKey();
        }
        public void UpdateCustomerLastName()
        {
            try
            {
                Console.WriteLine("Available CustomerIDs:");
                foreach (var customerID in _dbContext.Customers)
                {
                    Console.WriteLine($"CustomerID: {customerID.CustomerID}, Name: {customerID.CustomerFirstName} {customerID.CustomerLastName}");
                }

                Console.Write("Input Customer ID: ");
                if (int.TryParse(Console.ReadLine(), out int customerId))
                {
                    var customer = _dbContext.Customers.Find(customerId);

                    if (customer != null)
                    {
                        Console.Write("Input New Last Name: ");
                        string newLastName = Console.ReadLine();

                        if (!string.IsNullOrWhiteSpace(newLastName))
                        {
                            customer.CustomerLastName = char.ToUpper(newLastName[0]) + newLastName.Substring(1);
                            _dbContext.SaveChanges();
                            UserMessage.InputSuccessMessage("Customer last name updated successfully.");
                        }
                        else
                        {
                            UserMessage.ErrorMessage("Invalid input for the new last name. Please enter a valid last name.");
                        }
                    }
                    else
                    {
                        UserMessage.ErrorMessage("Customer not found.");
                    }
                }
                else
                {
                    UserMessage.ErrorMessage("Invalid input. Please enter a valid Customer ID.");
                }
            }
            catch (Exception ex)
            {
                UserMessage.ErrorMessage($"An error occurred: {ex.Message}");
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
                                UserMessage.InputSuccessMessage("Customer age updated successfully.");
                            }
                            else
                            {
                                UserMessage.ErrorMessage("Invalid input for the new age. Please enter a valid age between 0 and 150.");
                            }
                        }
                        else
                        {
                            UserMessage.ErrorMessage("Invalid input for the new age. Please enter a valid integer.");
                        }
                    }
                    else
                    {
                        UserMessage.ErrorMessage("Customer not found.");
                    }
                }
                else
                {
                    UserMessage.ErrorMessage("Invalid input. Please enter a valid Customer ID.");
                }
            }
            catch (Exception ex)
            {
                UserMessage.ErrorMessage($"An error occurred: {ex.Message}");
            }

            Console.WriteLine("Press any button to continue...");
            Console.ReadKey();
        }
        public void UpdateCustomerEmail()
        {
            try
            {
                Console.WriteLine("Available CustomerIDs:");
                foreach (var customerID in _dbContext.Customers)
                {
                    Console.WriteLine($"CustomerID: {customerID.CustomerID}, Name: {customerID.CustomerFirstName} " +
                        $"{customerID.CustomerLastName}, Email: {customerID.CustomerEmail}");
                }

                Console.Write("Input Customer ID: ");
                if (int.TryParse(Console.ReadLine(), out int customerId))
                {
                    var customer = _dbContext.Customers.Find(customerId);

                    if (customer != null)
                    {
                        Console.Write("Input New Email: ");
                        string newEmail = Console.ReadLine();

                        if (!string.IsNullOrWhiteSpace(newEmail) && IsValidEmail(newEmail))
                        {
                            customer.CustomerEmail = char.ToUpper(newEmail[0]) + newEmail.Substring(1);
                            _dbContext.SaveChanges();
                            UserMessage.InputSuccessMessage("Customer email updated successfully.");
                        }
                        else
                        {
                            UserMessage.ErrorMessage("Invalid email format. Please enter a valid email address.");
                        }
                    }
                    else
                    {
                        UserMessage.ErrorMessage("Customer not found.");
                    }
                }
                else
                {
                    UserMessage.ErrorMessage("Invalid input. Please enter a valid Customer ID.");
                }
            }
            catch (Exception ex)
            {
                UserMessage.ErrorMessage($"An error occurred: {ex.Message}");
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
            try
            {
                Console.WriteLine("Available CustomerIDs:");
                foreach (var customerID in _dbContext.Customers)
                {
                    Console.WriteLine($"CustomerID: {customerID.CustomerID}, Name: {customerID.CustomerFirstName} {customerID.CustomerLastName}, Phone: {customerID.CustomerPhone}");
                }

                Console.Write("Input Customer ID: ");
                if (int.TryParse(Console.ReadLine(), out int customerId))
                {
                    var customer = _dbContext.Customers.Find(customerId);

                    if (customer != null)
                    {
                        Console.Write("Input New Phone Number: ");
                        string newPhone = Console.ReadLine();

                        if (!string.IsNullOrWhiteSpace(newPhone) && IsValidPhoneNumber(newPhone))
                        {
                            customer.CustomerPhone = newPhone;
                            _dbContext.SaveChanges();
                            UserMessage.InputSuccessMessage("Customer phone updated successfully.");
                        }
                        else
                        {
                            UserMessage.ErrorMessage("Invalid phone number format. Please enter a valid phone number.");
                        }
                    }
                    else
                    {
                        UserMessage.ErrorMessage("Customer not found.");
                    }
                }
                else
                {
                    UserMessage.ErrorMessage("Invalid Customer ID. Please enter a valid number.");
                }
            }
            catch (Exception ex)
            {
                UserMessage.ErrorMessage($"An error occurred: {ex.Message}");
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
            try
            {
                Console.WriteLine("Available CustomerIDs:");
                foreach (var customerID in _dbContext.Customers)
                {
                    Console.WriteLine($"CustomerID: {customerID.CustomerID}, Name: {customerID.CustomerFirstName} {customerID.CustomerLastName}, Country: {customerID.CustomerCountry}");
                }

                Console.Write("Input Customer ID: ");
                if (int.TryParse(Console.ReadLine(), out int customerId))
                {
                    var customer = _dbContext.Customers.Find(customerId);

                    if (customer != null)
                    {
                        Console.Write("Input New Country: ");
                        string newCountry = Console.ReadLine();

                        if (!string.IsNullOrWhiteSpace(newCountry) && newCountry.Length <= 9)
                        {
                            customer.CustomerCountry = char.ToUpper(newCountry[0]) + newCountry.Substring(1);
                            _dbContext.SaveChanges();
                            UserMessage.InputSuccessMessage("Customer country updated successfully.");
                        }
                        else
                        {
                            UserMessage.ErrorMessage("Invalid country input. Please enter a valid country name.");
                        }
                    }
                    else
                    {
                        UserMessage.ErrorMessage("Customer not found.");
                    }
                }
                else
                {
                    UserMessage.ErrorMessage("Invalid Customer ID. Please enter a valid number.");
                }
            }
            catch (Exception ex)
            {
                UserMessage.ErrorMessage($"An error occurred: {ex.Message}");
            }

            Console.WriteLine("Press any button to continue...");
            Console.ReadKey();
        }

    }
}
