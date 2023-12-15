using MillesHotelLibrary.Interfaces;
using MillesHotelLibrary.Data;
using MillesHotelLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MillesHotelLibrary.ExtraServices;

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
                if (bookingDate.Date < DateTime.Now.Date)
                {
                    UserMessage.ErrorMessage("Invalid booking date. Please enter a date equal to or later than today. Booking not created.");
                    Console.ReadKey();
                    return;
                }
                Console.Write("Enter number of nights: ");
                if (int.TryParse(Console.ReadLine(), out int numberOfNights) && numberOfNights > 0)
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
                                bookingDate >= b.BookingEndDate || b.BookingStartDate >= bookingDate.AddDays(numberOfNights)))
                                .ToList();


                            if (availableRooms.Any())
                            {
                                Console.WriteLine("Available Rooms:");
                                foreach (var room in availableRooms)
                                {
                                    int roomSize = room.RoomSize;
                                    int pricePerNight;

                                    if (roomSize < 100)
                                    {
                                        pricePerNight = 750;
                                    }
                                    else if (roomSize < 1000)
                                    {
                                        pricePerNight = 1500;
                                    }
                                    else if (roomSize <= 3000)
                                    {
                                        pricePerNight = 3500;
                                    }
                                    else
                                    {
                                        pricePerNight = 3500;
                                    }

                                    Console.WriteLine($"RoomID: {room.RoomID} {room.RoomName,-21} {room.RoomType,-11} {roomSize,-5}kvm,  Price per Night: {pricePerNight,-5}kr");
                                }

                                Console.Write("Enter room ID: ");
                                if (int.TryParse(Console.ReadLine(), out int roomId))
                                {
                                    var selectedRoom = availableRooms.FirstOrDefault(room => room.RoomID == roomId);

                                    if (selectedRoom != null)
                                    {
                                        var isRoomAvailable = selectedRoom.Bookings == null || selectedRoom.Bookings.All(b =>
                                            bookingDate >= b.BookingEndDate ||
                                            b.BookingStartDate >= bookingDate.AddDays(7) || !b.IsActive);

                                        if (isRoomAvailable)
                                        {
                                            var newBooking = new Booking
                                            {
                                                BookingStartDate = bookingDate,
                                                BookingEndDate = bookingDate.AddDays(numberOfNights),
                                                IsActive = true,
                                                CustomerID = customerId,
                                                RoomID = roomId
                                            };

                                            int roomSize = selectedRoom.RoomSize;

                                            int pricePerNight;
                                            if (roomSize < 100)
                                            {
                                                pricePerNight = 750;
                                            }
                                            else if (roomSize < 1000)
                                            {
                                                pricePerNight = 1500;
                                            }
                                            else if (roomSize <= 3000)
                                            {
                                                pricePerNight = 3500;
                                            }
                                            else
                                            {
                                                pricePerNight = 3500;
                                            }

                                            var invoiceAmount = pricePerNight * numberOfNights;

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
                                            UserMessage.InputSuccessMessage("\nBooking created successfully.");

                                            Console.WriteLine($"\nBooking details:\n{newBooking}");
                                            Console.WriteLine($"Invoice details:\n{newInvoice}");

                                        }
                                        else
                                        {
                                            UserMessage.ErrorMessage("The room is not available for the selected dates. Booking not created.");
                                        }
                                    }
                                    else
                                    {
                                        UserMessage.ErrorMessage("Invalid room selection or the room is not available. Booking not created.");
                                    }
                                }
                                else
                                {
                                    UserMessage.ErrorMessage("Invalid room ID. Booking not created.");
                                }
                            }
                            else
                            {
                                UserMessage.ErrorMessage("No rooms are available for the selected dates. Booking not created.");
                                foreach (var nextDate in _dbContext.Bookings)
                                {
                                    Console.WriteLine($"Next available dates: Room ID {nextDate.RoomID}, " +
                                        $"Start Date: {nextDate.BookingStartDate.ToString("yyyy-MM-dd")}, End Date: {nextDate.BookingEndDate.ToString("yyyy-MM-dd")}");
                                }
                            }
                        }
                        else
                        {
                            UserMessage.ErrorMessage("Customer with the entered ID does not exist. Booking not created.");
                        }
                    }
                    else
                    {
                        UserMessage.ErrorMessage("Invalid Customer ID. Booking not created.");
                    }
                }
                else
                {
                    UserMessage.ErrorMessage("Invalid number of nights. Please enter a positive integer. Booking not created.");
                }
            }
            else
            {
                UserMessage.ErrorMessage("Invalid date format. Please use yyyy-MM-dd. Booking not created.");
            }

            Console.WriteLine("\nPress any button to continue...");
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
                        UserMessage.ErrorMessage("Booking not found.");
                    }
                }
                else
                {
                    UserMessage.ErrorMessage("Invalid booking ID. Please enter a valid number.");
                }
            }
            catch (Exception ex)
            {
                UserMessage.ErrorMessage($"An error occurred: {ex.Message}");
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
                            UserMessage.InputSuccessMessage("Booking information updated successfully.");
                        }
                        else
                        {
                            UserMessage.ErrorMessage("Invalid date format. Booking information not updated.");
                        }
                    }
                    else
                    {
                        UserMessage.ErrorMessage("Booking not found.");
                    }
                }
                else
                {
                    UserMessage.ErrorMessage("Invalid booking ID. Please enter a valid number.");
                }
            }
            catch (Exception ex)
            {
                UserMessage.ErrorMessage($"An error occurred: {ex.Message}");
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
                                UserMessage.InputSuccessMessage("Booking information updated successfully.");
                                Console.WriteLine("Booking information updated successfully.");
                            }
                            else
                            {
                                UserMessage.ErrorMessage("End date must be equal to or later than the start date. Booking information not updated.");
                            }
                        }
                        else
                        {
                            UserMessage.ErrorMessage("Invalid date format. Booking information not updated.");
                        }
                    }
                    else
                    {
                        UserMessage.ErrorMessage("Booking not found.");
                    }
                }
                else
                {
                    UserMessage.ErrorMessage("Invalid booking ID. Please enter a valid number.");
                }
            }
            catch (Exception ex)
            {
                UserMessage.ErrorMessage($"An error occurred: {ex.Message}");
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
                        UserMessage.InputSuccessMessage("Booking soft deleted successfully.");
                    }
                    else
                    {
                        UserMessage.ErrorMessage("Booking not found.");
                    }
                }
                else
                {
                    UserMessage.ErrorMessage("Invalid booking ID. Please enter a valid number.");
                }
            }
            catch (Exception ex)
            {
                UserMessage.ErrorMessage($"An error occurred: {ex.Message}");
            }

            Console.WriteLine("Press any button to continue...");
            Console.ReadKey();
        }
        public void CancelBooking()
        {
            try
            {
                GetAllBookings();

                Console.Write("Enter booking ID to cancel: ");
                if (int.TryParse(Console.ReadLine(), out int bookingId))
                {
                    var booking = _dbContext.Bookings.Find(bookingId);

                    if (booking != null)
                    {
                        booking.IsActive = false;

                        if (booking.Invoice != null)
                        {
                            booking.Invoice.IsActive = false;
                        }

                        _dbContext.SaveChanges();
                        UserMessage.InputSuccessMessage("Booking canceled successfully.");
                    }
                    else
                    {
                        UserMessage.ErrorMessage("Booking not found.");
                    }
                }
                else
                {
                    UserMessage.ErrorMessage("Invalid booking ID. Please enter a valid number.");
                }
            }
            catch (Exception ex)
            {
                UserMessage.ErrorMessage($"An error occurred: {ex.Message}");
            }

            Console.WriteLine("Press any button to continue...");
            Console.ReadKey();
        }
        public void ModifyBooking()
        {
            try
            {
                GetAllBookings();

                Console.Write("Enter booking ID to modify: ");
                if (int.TryParse(Console.ReadLine(), out int bookingId))
                {
                    var booking = _dbContext.Bookings
                        .Include(b => b.Room)
                        .Include(b => b.Customer)
                        .Include(b => b.Invoice)
                        .FirstOrDefault(b => b.BookingID == bookingId);

                    if (booking != null)
                    {
                        Console.WriteLine($"Current Booking Information:\n{booking}");

                        Console.Write("Enter new booking start date (yyyy-MM-dd): ");
                        if (DateTime.TryParse(Console.ReadLine(), out DateTime newStartDate))
                        {
                            Console.Write("Enter new booking end date (yyyy-MM-dd): ");
                            if (DateTime.TryParse(Console.ReadLine(), out DateTime newEndDate))
                            {
                                if (newEndDate >= newStartDate)
                                {
                                    booking.BookingStartDate = newStartDate;
                                    booking.BookingEndDate = newEndDate;

                                    if (booking.Invoice != null)
                                    {
                                        var pricePerNight = CalculatePricePerNight(booking.Room?.RoomSize ?? 0);
                                        var numberOfNights = (newEndDate - newStartDate).Days;

                                        booking.Invoice.InvoiceAmount = pricePerNight * numberOfNights;
                                        booking.Invoice.InvoiceDue = newEndDate;
                                        booking.Invoice.IsActive = booking.Invoice.InvoiceAmount > 0;
                                    }

                                    _dbContext.SaveChanges();
                                    UserMessage.InputSuccessMessage("Booking information modified successfully.");
                                }
                                else
                                {
                                    UserMessage.ErrorMessage("End date must be equal to or later than the start date. Booking information not modified.");
                                }
                            }
                            else
                            {
                                UserMessage.ErrorMessage("Invalid date format. Booking information not modified.");
                            }
                        }
                        else
                        {
                            UserMessage.ErrorMessage("Invalid date format. Booking information not modified.");
                        }
                    }
                    else
                    {
                        UserMessage.ErrorMessage("Booking not found.");
                    }
                }
                else
                {
                    UserMessage.ErrorMessage("Invalid booking ID. Please enter a valid number.");
                }
            }
            catch (Exception ex)
            {
                UserMessage.ErrorMessage($"An error occurred: {ex.Message}");
            }

            Console.WriteLine("Press any button to continue...");
            Console.ReadKey();
        }
        private int CalculatePricePerNight(int roomSize)
        {
            if (roomSize < 100)
            {
                return 750;
            }
            else if (roomSize < 1000)
            {
                return 1500;
            }
            else if (roomSize <= 3000)
            {
                return 3500;
            }
            else
            {
                return 3500;
            }
        }
        private void DisplayAvailableRooms(DateTime startDate, DateTime endDate)
        {
            var availableRooms = _dbContext.Rooms
                .Where(room => room.Bookings.All(b =>
                    startDate >= b.BookingEndDate || endDate >= b.BookingStartDate || !b.IsActive))
                .ToList();

            if (availableRooms.Any())
            {
                Console.WriteLine("Available Rooms:");
                foreach (var room in availableRooms)
                {
                    int roomSize = room.RoomSize;
                    int pricePerNight = CalculatePricePerNight(roomSize);

                    Console.WriteLine($"RoomID: {room.RoomID} {room.RoomName,-21} {room.RoomType,-11} {roomSize,-5}kvm,  Price per Night: {pricePerNight,-5}kr");
                }
            }
            else
            {
                UserMessage.ErrorMessage("No rooms are available for the selected dates.");
            }
        }
        public void SearchDateRoom()
        {
            try
            {
                Console.Write("Enter date for room search (yyyy-MM-dd): ");
                if (DateTime.TryParse(Console.ReadLine(), out DateTime searchDate))
                {
                    DisplayAvailableRooms(searchDate, searchDate.AddDays(1));
                }
                else
                {
                    UserMessage.ErrorMessage("Invalid date format. Please use yyyy-MM-dd.");
                }
            }
            catch (Exception ex)
            {
                UserMessage.ErrorMessage($"An error occurred: {ex.Message}");
            }

            Console.WriteLine("Press any button to continue...");
            Console.ReadKey();
        }
        public void SearchDateIntervalRoom()
        {
            try
            {
                Console.Write("Enter start date for room search (yyyy-MM-dd): ");
                if (DateTime.TryParse(Console.ReadLine(), out DateTime startDate))
                {
                    Console.Write("Enter end date for room search (yyyy-MM-dd): ");
                    if (DateTime.TryParse(Console.ReadLine(), out DateTime endDate))
                    {
                        DisplayAvailableRooms(startDate, endDate);
                    }
                    else
                    {
                        UserMessage.ErrorMessage("Invalid date format for end date. Please use yyyy-MM-dd.");
                    }
                }
                else
                {
                    UserMessage.ErrorMessage("Invalid date format for start date. Please use yyyy-MM-dd.");
                }
            }
            catch (Exception ex)
            {
                UserMessage.ErrorMessage($"An error occurred: {ex.Message}");
            }

            Console.WriteLine("Press any button to continue...");
            Console.ReadKey();
        }
        public void SearchCustomerRoom()
        {
            try
            {
                // Add logic to search based on customer details if needed
                Console.WriteLine("Searching for customers to see available rooms...");
                // ...

                // Display available rooms
                DisplayAvailableRooms(DateTime.Now, DateTime.Now.AddDays(1)); // Change this to fit your logic
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
