using Microsoft.EntityFrameworkCore;
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
                Console.Clear();
                foreach (var room in _dbContext.Room)
                {
                    Console.WriteLine($"RoomID: {room.RoomID}, RoomType: {room.RoomType}, RoomSize: {room.RoomSize}, IsActive: {room.IsActive}");
                }

                Console.Write("Enter room ID to permanently delete: ");
                if (int.TryParse(Console.ReadLine(), out int roomId))
                {
                    var room = _dbContext.Room.Include(r => r.Bookings).FirstOrDefault(r => r.RoomID == roomId);

                    if (room != null)
                    {
                        if (room.Bookings == null || !room.Bookings.Any())
                        {
                            _dbContext.Room.Remove(room);
                            _dbContext.SaveChanges();
                            Message.InputSuccessMessage("Room permanently deleted.");
                        }
                        else
                        {
                            Message.ErrorMessage("Cannot delete the room because it has associated bookings. Please delete the bookings first.");
                        }
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
                Console.Clear();
                foreach (var showCustomer in _dbContext.Customer)
                {
                    Console.WriteLine($"CustomerID: {showCustomer.CustomerID}, Name: " +
                        $"{showCustomer.CustomerFirstName} {showCustomer.CustomerLastName}");
                }

                Message.ErrorMessage(">>WARNING<< Associated bookings will be removed if invoice is paid!");
                Console.Write("Input Customer ID: ");
                if (int.TryParse(Console.ReadLine(), out int customerId))
                {
                    var customer = _dbContext.Customer
                        .Include(c => c.Bookings)
                        .ThenInclude(b => b.Invoice)
                        .FirstOrDefault(c => c.CustomerID == customerId);

                    if (customer != null)
                    {
                        if (customer.Bookings == null || customer.Bookings.All(b => b.Invoice == null || b.Invoice.IsPaid))
                        {
                            _dbContext.Customer.Remove(customer);
                            _dbContext.SaveChanges();
                            Message.InputSuccessMessage("Customer permanently deleted.");
                        }
                        else
                        {
                            Message.ErrorMessage("Cannot delete customer with associated unpaid bookings. Please remove unpaid bookings first.");
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
            Console.Clear();
            Console.Write("Enter invoice ID to permanently delete: ");
            if (int.TryParse(Console.ReadLine(), out int invoiceId))
            {
                var invoice = _dbContext.Invoice.Find(invoiceId);

                if (invoice != null)
                {
                    _dbContext.Invoice.Remove(invoice);
                    _dbContext.SaveChanges();
                    Message.InputSuccessMessage("Invoice permanently deleted successfully.");
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
                Console.Clear();
                foreach (var showBooking in _dbContext.Booking)
                {
                    Console.WriteLine($"Booking ID: {showBooking.BookingID}");
                }

                Console.Write("Enter booking ID to permanently delete: ");
                if (int.TryParse(Console.ReadLine(), out int bookingId))
                {
                    var booking = _dbContext.Booking.Find(bookingId);

                    if (booking != null)
                    {
                        _dbContext.Booking.Remove(booking);
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
            Console.Clear();
            var numberOfCustomers = _dbContext.Customer.Count();
            Console.WriteLine("============================");
            Console.WriteLine($"Number of Customers: {numberOfCustomers}");
            Console.WriteLine("============================");

            Console.WriteLine("\nPress any button to continue...");
            Console.ReadKey();
        }
        public void GetTop10CustomersByBooking()
        {
            Console.Clear();
            var topCustomersByBooking = _dbContext.Customer
                .Select(c => new
                {
                    Customer = c,
                    BookingCount = c.Bookings.Count
                })
                .AsEnumerable()
                .OrderByDescending(x => x.BookingCount)
                .Take(10)
                .Select((x, index) => new
                {
                    Rank = index + 1,
                    Customer = x.Customer,
                    BookingCount = x.BookingCount
                });

            Console.WriteLine("=========================================================");
            foreach (var item in topCustomersByBooking)
            {
                Console.WriteLine($"{item.Rank}. {item.Customer.CustomerFirstName} " +
                    $"{item.Customer.CustomerLastName}, Bookings: {item.BookingCount}");
            }
            Console.WriteLine("=========================================================");

            Console.WriteLine("\nPress any button to continue...");
            Console.ReadKey();
        }
        public void GetTop10CustomersByCountry()
        {
            Console.Clear();

            var customerCountsByCountry = _dbContext.Customer
                .GroupBy(c => c.CountryID)
                .Select(g => new
                {
                    CountryID = g.Key,
                    CustomerCount = g.Count()
                })
                .ToList();

            var topCustomersByCountry = customerCountsByCountry
                .Join(_dbContext.Country,
                      customerCount => customerCount.CountryID,
                      country => country.CountryID,
                      (customerCount, country) => new
                      {
                          CountryName = country.CountryName,
                          CustomerCount = customerCount.CustomerCount
                      })
                .OrderByDescending(group => group.CustomerCount)
                .Take(10)
                .Select((group, index) => new
                {
                    Rank = index + 1,
                    Country = group.CountryName,
                    CustomerCount = group.CustomerCount
                });

            Console.WriteLine("=========================================================");
            foreach (var item in topCustomersByCountry)
            {
                Console.WriteLine($"{item.Rank}. {item.Country}: {item.CustomerCount} customers");
            }
            Console.WriteLine("=========================================================");

            Console.WriteLine("\nPress any button to continue...");
            Console.ReadKey();
        }


    }
}
