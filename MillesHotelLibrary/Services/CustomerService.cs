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
                Console.Clear();
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

                    Console.WriteLine("Available Countries:");
                    Console.WriteLine("====================");

                    var countries = _dbContext.Country.ToList();
                    foreach (var country in countries)
                    {
                        Console.WriteLine($"{country.CountryID} - {country.CountryName}");
                    }
                    Console.WriteLine("0 - Create a new country");
                    Console.WriteLine("====================");

                    Console.Write("Enter customer country ID: ");
                    if (int.TryParse(Console.ReadLine(), out int countryId))
                    {
                        if (countryId == 0)
                        {
                            Console.Write("Enter new country name (max 9 characters): ");
                            string newCountryName = Console.ReadLine()?.Trim();

                            if (!string.IsNullOrEmpty(newCountryName) && newCountryName.Length <= 9)
                            {
                                var newCountry = new Country { CountryName = newCountryName };
                                _dbContext.Country.Add(newCountry);
                                _dbContext.SaveChanges();

                                countryId = newCountry.CountryID;

                                Console.WriteLine($"New country '{newCountryName}' created with ID: {countryId}");
                            }
                            else
                            {
                                Message.ErrorMessage("Invalid country name input. Customer not created.");
                                return;
                            }
                        }

                        var existingCountry = _dbContext.Country.FirstOrDefault(c => c.CountryID == countryId);


                        if (existingCountry != null)
                        {
                            if (firstName.Length >= 2 && firstName.Length <= 13 &&
                                lastName.Length >= 2 && lastName.Length <= 13 &&
                                email.Length >= 2 && email.Length <= 25 &&
                                phone.Length <= 15)
                            {
                                var newCustomer = new Customer()
                                {
                                    CustomerFirstName = char.ToUpper(firstName[0]) + firstName.Substring(1),
                                    CustomerLastName = char.ToUpper(lastName[0]) + lastName.Substring(1),
                                    CustomerAge = age,
                                    CustomerEmail = char.ToUpper(email[0]) + email.Substring(1),
                                    CustomerPhone = phone,
                                    CountryID = countryId,
                                    IsActive = true
                                };

                                _dbContext.Customer.Add(newCustomer);
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
                            Message.ErrorMessage("Country not found. Customer not created.");
                        }
                    }
                    else
                    {
                        Message.ErrorMessage("Invalid country ID. Please enter a valid number.");
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

            Console.WriteLine("\nPress any button to continue...");
            Console.ReadKey();
        }

        public void GetCustomerByID()
        {
            try
            {
                Console.Clear();
                foreach (var showCustomer in _dbContext.Customer)
                {
                    Console.WriteLine($"CustomerID: {showCustomer.CustomerID}, Name: {showCustomer.CustomerFirstName} {showCustomer.CustomerLastName}");
                }

                Console.Write("\nInput Customer ID for detailed view: ");
                if (int.TryParse(Console.ReadLine(), out int customerId))
                {
                    var customer = _dbContext.Customer.Find(customerId);

                    if (customer != null)
                    {
                        Console.Clear();
                        Console.WriteLine("==================================================");
                        Console.WriteLine($"Customer ID: {customer.CustomerID}");
                        Console.WriteLine($"First Name: {customer.CustomerFirstName}");
                        Console.WriteLine($"Last Name: {customer.CustomerLastName}");
                        Console.WriteLine($"Age: {customer.CustomerAge}");
                        Console.WriteLine($"Email: {customer.CustomerEmail}");
                        Console.WriteLine($"Phone: {customer.CustomerPhone}");
                        Console.WriteLine($"Country: {customer.CountryID}");
                        Console.WriteLine($"Is Active: {customer.IsActive}");
                        Console.WriteLine("==================================================");
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

            Console.WriteLine("\nPress any button to continue...");
            Console.ReadKey();
        }
        public void GetAllCustomers()
        {
            Console.Clear();
            var customers = _dbContext.Customer.ToList();

            Console.WriteLine("╭─────────────╮─────────────╮─────────────╮─────╮─────────────────────────╮───────────────╮─────────╮──────────╮");
            Console.WriteLine("│Customer ID  │ First Name  │ Last Name   │ Age │ Email                   │ Phone         │ Country │ Status   │");
            Console.WriteLine("├─────────────┼─────────────┼─────────────┼─────┼─────────────────────────┼───────────────┼─────────┤──────────┤");

            foreach (var customer in customers)
            {
                var countryName = _dbContext.Country.Find(customer.CountryID)?.CountryName ?? "Unknown";

                if (customer.IsActive)
                {
                    Console.WriteLine($"│{customer.CustomerID,-13}│{customer.CustomerFirstName,-13}│{customer.CustomerLastName,-13}│" +
                        $"{customer.CustomerAge,-5}│{customer.CustomerEmail,-25}│{customer.CustomerPhone,-15}│{countryName,-9}│ ACTIVE   │");
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"│{customer.CustomerID,-13}│{customer.CustomerFirstName,-13}│{customer.CustomerLastName,-13}│" +
                        $"{customer.CustomerAge,-5}│{customer.CustomerEmail,-25}│{customer.CustomerPhone,-15}│{countryName,-9}│ INACTIVE │");
                    Console.ResetColor();
                }

                Console.WriteLine("├─────────────┼─────────────┼─────────────┼─────┼─────────────────────────┼───────────────┼─────────┤──────────┤");
            }

            Console.WriteLine("╰─────────────╯─────────────╯─────────────╯─────╯─────────────────────────╯───────────────╯─────────╯──────────╯");
        }
        public void SoftDeleteCustomer()
        {
            try
            {
                Console.Clear();
                foreach (var showCustomer in _dbContext.Customer)
                {
                    Console.WriteLine($"CustomerID: {showCustomer.CustomerID}, Name: " +
                        $"{showCustomer.CustomerFirstName} {showCustomer.CustomerLastName}");
                }

                Console.Write("Input Customer ID to soft delete: ");
                if (int.TryParse(Console.ReadLine(), out int customerId))
                {
                    var customer = _dbContext.Customer.Include(c => c.Bookings).FirstOrDefault(c => c.CustomerID == customerId);

                    if (customer != null)
                    {
                        if (customer.Bookings == null || customer.Bookings.All(b => !b.IsActive))
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
        public void ReactivateCustomer()
        {
            try
            {
                Console.Clear();
                foreach (var showCustomer in _dbContext.Customer.Where(c => !c.IsActive))
                {
                    Console.WriteLine($"Inactive CustomerID: {showCustomer.CustomerID}");
                }

                Console.Write("Input Inactive Customer ID to reactivate: ");
                if (int.TryParse(Console.ReadLine(), out int customerId))
                {
                    var customer = _dbContext.Customer.FirstOrDefault(c => c.CustomerID == customerId && !c.IsActive);

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
                Console.Clear();
                Console.WriteLine("Available CustomerIDs:");
                Console.WriteLine("=====================================================");
                foreach (var customersID in _dbContext.Customer)
                {
                    Console.WriteLine($"CustomerID: {customersID.CustomerID}, Name: {customersID.CustomerFirstName} {customersID.CustomerLastName}");
                }
                Console.WriteLine("=====================================================");

                Console.Write("\nInput Customer ID for new first name: ");
                if (int.TryParse(Console.ReadLine(), out int customerId))
                {
                    var customer = _dbContext.Customer.Find(customerId);

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

            Console.WriteLine("\nPress any button to continue...");
            Console.ReadKey();
        }
        public void UpdateCustomerLastName()
        {
            try
            {
                Console.Clear();
                Console.WriteLine("Available CustomerIDs:");
                Console.WriteLine("=====================================================");
                foreach (var customersID in _dbContext.Customer)
                {
                    Console.WriteLine($"CustomerID: {customersID.CustomerID}, Name: {customersID.CustomerFirstName} {customersID.CustomerLastName}");
                }
                Console.WriteLine("=====================================================");

                Console.Write("\nInput Customer ID for new last name: ");
                if (int.TryParse(Console.ReadLine(), out int customerId))
                {
                    var customer = _dbContext.Customer.Find(customerId);

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

            Console.WriteLine("\nPress any button to continue...");
            Console.ReadKey();
        }
        public void UpdateCustomerAge()
        {
            try
            {
                Console.Clear();
                Console.WriteLine("Available CustomerIDs:");
                Console.WriteLine("=====================================================");
                foreach (var customersID in _dbContext.Customer)
                {
                    Console.WriteLine($"CustomerID: {customersID.CustomerID}, Name: {customersID.CustomerFirstName} {customersID.CustomerLastName}, Age: {customersID.CustomerAge}");
                }
                Console.WriteLine("=====================================================");

                Console.Write("\nInput Customer ID for new age: ");
                if (int.TryParse(Console.ReadLine(), out int customerId))
                {
                    var customer = _dbContext.Customer.Find(customerId);

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

            Console.WriteLine("\nPress any button to continue...");
            Console.ReadKey();
        }
        public void UpdateCustomerEmail()
        {
            try
            {
                Console.Clear();
                Console.WriteLine("Available CustomerIDs:");
                Console.WriteLine("==================================================================");
                foreach (var customersID in _dbContext.Customer)
                {
                    Console.WriteLine($"CustomerID: {customersID.CustomerID}, Name: {customersID.CustomerFirstName} " +
                        $"{customersID.CustomerLastName}, Email: {customersID.CustomerEmail}");
                }
                Console.WriteLine("==================================================================");

                Console.Write("\nInput Customer ID for new email: ");
                if (int.TryParse(Console.ReadLine(), out int customerId))
                {
                    var customer = _dbContext.Customer.Find(customerId);

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

            Console.WriteLine("\nPress any button to continue...");
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
            Console.Clear();
            Console.WriteLine("Available CustomerIDs:");
            Console.WriteLine("=============================================================");
            foreach (var customersID in _dbContext.Customer)
            {
                Console.WriteLine($"CustomerID: {customersID.CustomerID}, Name: {customersID.CustomerFirstName} {customersID.CustomerLastName}, " +
                    $"Phone: {customersID.CustomerPhone}");
            }
            Console.WriteLine("=============================================================");

            Console.Write("\nInput Customer ID for new phone number: ");
            if (int.TryParse(Console.ReadLine(), out int customerId))
            {
                var customer = _dbContext.Customer.Find(customerId);

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

            Console.WriteLine("\nPress any button to continue...");
            Console.ReadKey();
        }
        public bool IsValidPhoneNumber(string phoneNumber)
        {
            return !string.IsNullOrEmpty(phoneNumber);
        }
        public void UpdateCustomerCountry()
        {
            Console.Clear();
            Console.WriteLine("Available CustomerIDs and Countries:");
            Console.WriteLine("=====================================================");

            var customerList = _dbContext.Customer.Include(c => c.Country).ToList();

            foreach (var customer in customerList)
            {
                Console.WriteLine($"CustomerID: {customer.CustomerID}, Name: {customer.CustomerFirstName} {customer.CustomerLastName}, " +
                    $"Country: {customer.Country?.CountryName ?? "Unknown"}");
            }
            Console.WriteLine("=====================================================");

            Console.Write("\nInput Customer ID for new country: ");
            if (int.TryParse(Console.ReadLine(), out int customerId))
            {
                var customer = customerList.FirstOrDefault(c => c.CustomerID == customerId);

                if (customer != null)
                {
                    Console.Write("Input New Country (max 9 characters): ");
                    string newCountryName = Console.ReadLine()?.Trim();

                    if (!string.IsNullOrEmpty(newCountryName) && newCountryName.Length <= 9)
                    {
                        var existingCountry = _dbContext.Country.FirstOrDefault(c => c.CountryName == newCountryName);

                        if (existingCountry != null)
                        {
                            customer.CountryID = existingCountry.CountryID;
                            _dbContext.SaveChanges();
                            Message.InputSuccessMessage("Customer country updated successfully.");
                        }
                        else
                        {
                            Console.Write("Country not found. Do you want to add a new country? (Y/N): ");
                            if (Console.ReadLine()?.ToUpper() == "Y")
                            {
                                var newCountry = new Country { CountryName = newCountryName };
                                _dbContext.Country.Add(newCountry);
                                _dbContext.SaveChanges();

                                customer.CountryID = newCountry.CountryID;
                                _dbContext.SaveChanges();
                                Message.InputSuccessMessage("Customer country updated successfully.");
                            }
                            else
                            {
                                Message.ErrorMessage("Customer country not updated.");
                            }
                        }
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

            Console.WriteLine("\nPress any button to continue...");
            Console.ReadKey();
        }



    }
}
