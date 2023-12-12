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

        public BookingService(DbContextOptionsBuilder<HotelDbContext> options)
        {
            _dbContext = new HotelDbContext(options.Options);
        }

        public void CreateBooking()
        {
            Console.Write("Enter booking date (yyyy-mm-dd): ");
            if (DateTime.TryParse(Console.ReadLine(), out DateTime bookingDate))
            {
                foreach (var customerID in _dbContext.Customers)
                {
                    Console.WriteLine($"CustomerID: {customerID.CustomerID}, Customer Name: {customerID.CustomerFirstName} {customerID.CustomerLastName}");
                }

                Console.Write("Enter Customer ID: ");
                int customerId = Convert.ToInt32(Console.ReadLine());

                var bookedRoomIds = _dbContext.Bookings
                    .Where(b => bookingDate < b.BookingEndDate && b.BookingStartDate < bookingDate.AddDays(7))
                    .Select(b => b.RoomID)
                    .ToList();

                var availableRooms = _dbContext.Rooms
                    .AsEnumerable()
                    .Where(r => !r.IsActive)
                    .Where(r => !bookedRoomIds.Contains(r.RoomID))
                    .ToList();

                if (availableRooms.Any())
                {
                    Console.WriteLine("Available Rooms:");
                    foreach (var room in availableRooms)
                    {
                        Console.WriteLine($"RoomID: {room.RoomID} Roomtype: {room.RoomType}");
                    }

                    Console.Write("Enter room ID: ");
                    int roomId = Convert.ToInt32(Console.ReadLine());

                    // Check if the room is available
                    var selectedRoom = _dbContext.Rooms.Find(roomId);

                    if (selectedRoom != null && !selectedRoom.IsActive)
                    {
                        // Check if there are any overlapping bookings for the selected room
                        var isRoomAvailable = !_dbContext.Bookings
                            .Where(b => b.RoomID == roomId && b.IsActive &&
                            !(bookingDate < b.BookingEndDate && b.BookingStartDate < bookingDate.AddDays(7)))
                            .Any();

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

                            // Calculate InvoiceAmount based on the number of nights booked
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
                    Console.WriteLine("No rooms are available for the selected dates. Booking not created.");
                }
            }
            else
            {
                Console.WriteLine("Invalid date format. Booking not created.");
            }

            Console.WriteLine("Press any button to continue...");
            Console.ReadKey();
        }

        public void GetBookingByID()
        {
            foreach (var bookingID in _dbContext.Bookings)
            {
                Console.WriteLine($"BookingID: {bookingID.BookingID}");

            }

            Console.Write("Enter booking ID: ");
            int bookingId = Convert.ToInt32(Console.ReadLine());

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

            Console.WriteLine("Press any button to continue...");
            Console.ReadKey();
        }

        public void GetAllBookings()
        {
            var bookings = _dbContext.Bookings.ToList();
            foreach (var booking in bookings)
            {
                Console.WriteLine("Booking ID: " + booking.BookingID);
                Console.WriteLine("Start Date: " + booking.BookingStartDate);
                Console.WriteLine("End Date: " + booking.BookingEndDate);
                Console.WriteLine($"Is Active: {booking.IsActive}");
                Console.WriteLine("Customer ID: " + booking.CustomerID);
                Console.WriteLine("Room ID: " + booking.RoomID);
                Console.WriteLine();
            }
            Console.WriteLine("Press any button to continue...");
            Console.ReadKey();
        }

        public void UpdateBooking()
        {
            GetAllBookings();

            Console.Write("Enter booking ID to update: ");
            int bookingId = Convert.ToInt32(Console.ReadLine());

            var booking = _dbContext.Bookings.Find(bookingId);

            if (booking != null)
            {
                Console.Write("Enter new booking date (yyyy-mm-dd): ");
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

            Console.WriteLine("Press any button to continue...");
            Console.ReadKey();
        }

        public void SoftDeleteBooking()
        {
            GetAllBookings();

            Console.Write("Enter booking ID to soft delete: ");
            int bookingId = Convert.ToInt32(Console.ReadLine());

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

            Console.WriteLine("Press any button to continue...");
            Console.ReadKey();
        }

    }
}
