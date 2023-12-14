using MillesHotelLibrary.Interfaces;
using MillesHotelLibrary.Data;
using MillesHotelLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace MillesHotelLibrary.Services
{
    public class BookingService : IBookingService
    {
        private readonly HotelDbContext _dbContext;

        public BookingService(HotelDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void CreateBooking()
        {
            Console.Write("Enter booking date (yyyy-MM-dd): ");
            if (DateTime.TryParse(Console.ReadLine(), out DateTime bookingDate))
            {
                foreach (var customerID in _dbContext.Customers)
                {
                    Console.WriteLine($"CustomerID: {customerID.CustomerID}, Customer Name: {customerID.CustomerFirstName} {customerID.CustomerLastName}");
                }

                Console.Write("Enter Customer ID: ");
                if (int.TryParse(Console.ReadLine(), out int customerId))
                {
                    var customerExists = _dbContext.Customers.Any(c => c.CustomerID == customerId);

                    if (customerExists)
                    {
                        var availableRooms = _dbContext.Rooms
                        .Where(room => room.Bookings.All(b =>
                        bookingDate >= b.BookingEndDate || b.BookingStartDate >= bookingDate.AddDays(7)))
                        .ToList();


                        if (availableRooms.Any())
                        {
                            Console.WriteLine("Available Rooms:");
                            foreach (var room in availableRooms)
                            {
                                Console.WriteLine($"RoomID: {room.RoomID}, Roomtype: {room.RoomType}, Room Size: {room.RoomSize}kvm, Room Name: {room.RoomName}");
                            }

                            Console.Write("Enter room ID: ");
                            if (int.TryParse(Console.ReadLine(), out int roomId))
                            {
                                var selectedRoom = availableRooms.FirstOrDefault(room => room.RoomID == roomId);

                                if (selectedRoom != null)
                                {
                                    var isRoomAvailable = selectedRoom.Bookings.All(b =>
                                        bookingDate >= b.BookingEndDate ||
                                        b.BookingStartDate >= bookingDate.AddDays(7));

                                    if (isRoomAvailable)
                                    {
                                        var newBooking = new Booking
                                        {
                                            BookingStartDate = bookingDate,
                                            BookingEndDate = bookingDate.AddDays(7),
                                            IsActive = true,
                                            CustomerID = customerId,
                                            RoomID = roomId
                                        };

                                        var invoiceAmount = 1000 * (newBooking.BookingEndDate - newBooking.BookingStartDate).TotalDays;

                                        var newInvoice = new Invoice
                                        {
                                            InvoiceAmount = invoiceAmount,
                                            InvoiceDue = newBooking.BookingEndDate,
                                            IsActive = true,
                                            CustomerID = customerId,
                                            Customer = newBooking.Customer,
                                        };

                                        newBooking.Invoice = newInvoice;

                                        _dbContext.Bookings.Add(newBooking);
                                        _dbContext.Invoices.Add(newInvoice);

                                        _dbContext.SaveChanges();
                                        Console.WriteLine("Booking created successfully.");
                                    }
                                    else
                                    {
                                        Console.WriteLine("The room is not available for the selected dates. Booking not created.");
                                    }
                                }
                                else
                                {
                                    Console.WriteLine("Invalid room selection or the room is not available. Booking not created.");
                                }
                            }
                            else
                            {
                                Console.WriteLine("Invalid room ID. Booking not created.");
                            }
                        }
                        else
                        {
                            Console.WriteLine("No rooms are available for the selected dates. Booking not created.");
                            foreach (var nextDate in _dbContext.Bookings)
                            {
                                Console.WriteLine($"Next available dates: Room ID {nextDate.RoomID}, " +
                                    $"Start Date: {nextDate.BookingStartDate.ToString("yyyy-MM-dd")}, End Date: {nextDate.BookingEndDate.ToString("yyyy-MM-dd")}");
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine("Customer with the entered ID does not exist. Booking not created.");
                    }
                }
                else
                {
                    Console.WriteLine("Invalid Customer ID. Booking not created.");
                }
            }
            else
            {
                Console.WriteLine("Invalid date format. Please use yyyy-MM-dd. Booking not created.");
            }

            Console.WriteLine("Press any button to continue...");
            Console.ReadKey();
        }

        public void GetBookingByID()
        {
            try
            {
                foreach (var bookingID in _dbContext.Bookings)
                {
                    Console.WriteLine($"BookingID: {bookingID.BookingID}");
                }

                Console.Write("Enter booking ID: ");

                if (int.TryParse(Console.ReadLine(), out int bookingId))
                {
                    var booking = _dbContext.Bookings
                        .Include(b => b.Room)
                        .Include(b => b.Customer)
                        .Include(b => b.Invoice)
                        .FirstOrDefault(b => b.BookingID == bookingId);

                    if (booking != null)
                    {
                        Console.WriteLine($"\nBooking ID: {booking.BookingID}");
                        Console.WriteLine($"Booking Start Date: {booking.BookingStartDate.ToString("yyyy-MM-dd")}");
                        Console.WriteLine($"Booking End Date: {booking.BookingEndDate.ToString("yyyy-MM-dd")}");

                        if (booking.Invoice != null)
                        {
                            Console.WriteLine($"\nAmount Due: {booking.Invoice?.InvoiceAmount.ToString("C2") ?? "N/A"}");
                            Console.WriteLine($"Amount Due Date {booking.Invoice?.InvoiceDue.ToString("yyyy-MM-dd")}");
                        }

                        Console.WriteLine($"\nRoom ID: {booking.RoomID}");
                        Console.WriteLine($"Room Type: {booking.Room?.RoomType ?? RoomType.SingleRoom}");

                        if (booking.Customer != null)
                        {
                            Console.WriteLine($"\nCustomer ID: {booking.CustomerID}" +
                                $"\nCustomer Name: {booking.Customer.CustomerFirstName} {booking.Customer.CustomerLastName}\n");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Booking not found.");
                    }
                }
                else
                {
                    Console.WriteLine("Invalid booking ID. Please enter a valid number.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }

            Console.WriteLine("Press any button to continue...");
            Console.ReadKey();
        }

        public void GetAllBookings()
        {
            var bookings = _dbContext.Bookings.ToList();

            Console.WriteLine("╭─────────────╮───────────────────╮───────────────────╮────────────╮─────────────╮──────────╮");
            Console.WriteLine("│ Booking ID  │ Start Date        │ End Date          │ Is Active  │ Customer ID │ Room ID  │");
            Console.WriteLine("├─────────────┼───────────────────┼───────────────────┼────────────┼─────────────┼──────────┤");

            foreach (var booking in bookings)
            {
                Console.WriteLine($"│{booking.BookingID,-13}│{booking.BookingStartDate.ToString("yyyy-MM-dd"),-19}│{booking.BookingEndDate.ToString("yyyy-MM-dd"),-19}│{booking.IsActive,-12}│{booking.CustomerID,-13}│{booking.RoomID,-10}│");
                Console.WriteLine("├─────────────┼───────────────────┼───────────────────┼────────────┼─────────────┼──────────┤");
            }

            Console.WriteLine("╰─────────────╯───────────────────╯───────────────────╯────────────╯─────────────╯──────────╯");
        }

        public void UpdateBookingStartDate()
        {
            try
            {
                GetAllBookings();

                Console.Write("Enter booking ID to update: ");
                if (int.TryParse(Console.ReadLine(), out int bookingId))
                {
                    var booking = _dbContext.Bookings.Find(bookingId);

                    if (booking != null)
                    {
                        Console.Write("Enter new booking start date (yyyy-mm-dd): ");
                        if (DateTime.TryParse(Console.ReadLine(), out DateTime newBookingDate))
                        {
                            booking.BookingStartDate = newBookingDate;
                            _dbContext.SaveChanges();
                            Console.WriteLine("Booking information updated successfully.");
                        }
                        else
                        {
                            Console.WriteLine("Invalid date format. Booking information not updated.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Booking not found.");
                    }
                }
                else
                {
                    Console.WriteLine("Invalid booking ID. Please enter a valid number.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }

            Console.WriteLine("Press any button to continue...");
            Console.ReadKey();
        }

        public void UpdateBookingEndDate()
        {
            try
            {
                GetAllBookings();

                Console.Write("Enter booking ID to update: ");
                if (int.TryParse(Console.ReadLine(), out int bookingId))
                {
                    var booking = _dbContext.Bookings.Find(bookingId);

                    if (booking != null)
                    {
                        Console.Write("Enter new booking end date (yyyy-mm-dd): ");
                        if (DateTime.TryParse(Console.ReadLine(), out DateTime newBookingDate))
                        {
                            if (newBookingDate >= booking.BookingStartDate)
                            {
                                booking.BookingEndDate = newBookingDate;
                                _dbContext.SaveChanges();
                                Console.WriteLine("Booking information updated successfully.");
                            }
                            else
                            {
                                Console.WriteLine("End date must be equal to or later than the start date. Booking information not updated.");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Invalid date format. Booking information not updated.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Booking not found.");
                    }
                }
                else
                {
                    Console.WriteLine("Invalid booking ID. Please enter a valid number.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }

            Console.WriteLine("Press any button to continue...");
            Console.ReadKey();
        }

        public void SoftDeleteBooking()
        {
            try
            {
                GetAllBookings();

                Console.Write("Enter booking ID to soft delete: ");
                if (int.TryParse(Console.ReadLine(), out int bookingId))
                {
                    var booking = _dbContext.Bookings.Find(bookingId);

                    if (booking != null)
                    {
                        booking.IsActive = false;
                        _dbContext.SaveChanges();
                        Console.WriteLine("Booking soft deleted successfully.");
                    }
                    else
                    {
                        Console.WriteLine("Booking not found.");
                    }
                }
                else
                {
                    Console.WriteLine("Invalid booking ID. Please enter a valid number.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }

            Console.WriteLine("Press any button to continue...");
            Console.ReadKey();
        }

    }
}
