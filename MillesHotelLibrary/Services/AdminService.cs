﻿using Microsoft.EntityFrameworkCore;
using MillesHotelLibrary.Data;
using MillesHotelLibrary.ExtraServices;
using MillesHotelLibrary.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MillesHotelLibrary.Services
{
    public class AdminService : IAdminService
    {
        private readonly HotelDbContext _dbContext;

        public AdminService(HotelDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void DeleteRoom()
        {
            try
            {
                foreach (var room in _dbContext.Rooms)
                {
                    Console.WriteLine($"RoomID: {room.RoomID}, RoomType: {room.RoomType}, RoomSize: {room.RoomSize}, IsActive: {room.IsActive}");
                }

                Console.Write("Enter room ID to delete: ");
                if (int.TryParse(Console.ReadLine(), out int roomId))
                {
                    var room = _dbContext.Rooms.Include(r => r.Bookings).FirstOrDefault(r => r.RoomID == roomId);

                    if (room != null)
                    {
                        _dbContext.Rooms.Remove(room);
                        _dbContext.SaveChanges();
                        Message.InputSuccessMessage("Room permanently deleted.");
                    }
                    else
                    {
                        Message.ErrorMessage("Room not found.");
                    }
                }
                else
                {
                    Message.ErrorMessage("Invalid room ID format. Please enter a valid number.");
                }
            }
            catch (Exception ex)
            {
                Message.ErrorMessage($"An error occurred: {ex.Message}");
            }

            Console.WriteLine("Press any button to continue...");
            Console.ReadKey();
        }

        public void DeleteCustomer()
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
                            _dbContext.Customers.Remove(customer);
                            _dbContext.SaveChanges();
                            Message.InputSuccessMessage("Customer permanently deleted.");
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

        public void DeleteInvoice()
        {
            Console.Write("Enter invoice ID to soft delete: ");
            if (int.TryParse(Console.ReadLine(), out int invoiceId))
            {
                var invoice = _dbContext.Invoices.Find(invoiceId);

                if (invoice != null)
                {
                    _dbContext.Invoices.Remove(invoice);
                    _dbContext.SaveChanges();
                    Message.InputSuccessMessage("Invoice soft deleted successfully.");
                }
                else
                {
                    Message.ErrorMessage("Invoice not found.");
                }
            }
            else
            {
                Message.ErrorMessage("Invalid invoice ID format. Please enter a valid number.");
            }

            Console.WriteLine("Press any button to continue...");
            Console.ReadKey();
        }

        public void DeleteBooking()
        {
            try
            {

                Console.Write("Enter booking ID to soft delete: ");
                if (int.TryParse(Console.ReadLine(), out int bookingId))
                {
                    var booking = _dbContext.Bookings.Find(bookingId);

                    if (booking != null)
                    {
                        _dbContext.Bookings.Remove(booking);
                        _dbContext.SaveChanges();
                        Message.InputSuccessMessage("Booking permanently deleted.");
                    }
                    else
                    {
                        Message.ErrorMessage("Booking not found.");
                    }
                }
                else
                {
                    Message.ErrorMessage("Invalid booking ID. Please enter a valid number.");
                }
            }
            catch (Exception ex)
            {
                Message.ErrorMessage($"An error occurred: {ex.Message}");
            }

            Console.WriteLine("Press any button to continue...");
            Console.ReadKey();
        }
        public void GetNumberOfCustomers()
        {
            var numberOfCustomers = _dbContext.Customers.Count();
            Console.WriteLine($"Number of Customers: {numberOfCustomers}");

            Console.WriteLine("Press any button to continue...");
            Console.ReadKey();
        }

        public void GetTop10Customers()
        {
            var topCustomers = _dbContext.Customers.OrderByDescending(c => c.CustomerID).Take(10);
            foreach (var customer in topCustomers)
            {
                Console.WriteLine($"CustomerID: {customer.CustomerID}");
            }
            Console.WriteLine("Press any button to continue...");
            Console.ReadKey();
        }

        public void GetTop10CustomersByBooking()
        {
            var topCustomersByBooking = _dbContext.Customers
                .Select(c => new
                {
                    Customer = c,
                    BookingCount = c.Bookings.Count
                })
                .OrderByDescending(x => x.BookingCount)
                .Take(10)
                .Select(x => x.Customer);

            foreach (var customer in topCustomersByBooking)
            {
                Console.WriteLine($"CustomerID: {customer.CustomerID}, Bookings: {customer.Bookings?.Count ?? 0}");
            }

            Console.WriteLine("Press any button to continue...");
            Console.ReadKey();
        }

        public void GetTop10CustomersByCountry()
        {
            var topCustomersByCountry = _dbContext.Customers
                .GroupBy(c => c.CustomerCountry)
                .AsEnumerable()
                .OrderByDescending(group => group.Count())
                .Take(10);

            foreach (var group in topCustomersByCountry)
            {
                Console.WriteLine($"{group.Key}: {group.Count()} customers");
            }

            Console.WriteLine("Press any button to continue...");
            Console.ReadKey();
        }

    }
}