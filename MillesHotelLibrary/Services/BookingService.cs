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
            try
            {
                Console.Clear();
                Console.WriteLine("=================================================================");
                foreach (var customerID in _dbContext.Customer)
                {
                    Console.WriteLine($"CustomerID: {customerID.CustomerID}, Customer Name: {customerID.CustomerFirstName} {customerID.CustomerLastName}");
                }
                Console.WriteLine("=================================================================");

                Console.Write("\nEnter Customer ID to book: ");
                if (int.TryParse(Console.ReadLine(), out int customerId))
                {
                    var customer = _dbContext.Customer.FirstOrDefault(c => c.CustomerID == customerId && c.CustomerAge >= 18);

                    if (customer != null)
                    {
                        Console.Write("Enter booking date (yyyy-MM-dd): ");
                        if (DateTime.TryParse(Console.ReadLine(), out DateTime bookingDate))
                        {
                            if (bookingDate.Date < DateTime.Now.Date)
                            {
                                Message.ErrorMessage("Invalid booking date. Please enter a date equal to or later than today. Booking not created.");
                                Console.ReadKey();
                                return;
                            }

                            Console.Write("Enter number of nights (max 20): ");
                            if (int.TryParse(Console.ReadLine(), out int numberOfNights) && numberOfNights > 0 && numberOfNights <= 20)
                            {
                                var availableRooms = _dbContext.Room
                                    .Where(room => room.Bookings
                                    .All(b =>
                                        bookingDate >= b.BookingEndDate ||
                                        b.BookingStartDate >= bookingDate.AddDays(numberOfNights) ||
                                        !b.IsBooked && b.CustomerID != customerId))
                                    .ToList();

                                if (availableRooms.Any())
                                {
                                    Console.Clear();
                                    Console.WriteLine("Available Rooms:");
                                    foreach (var room in availableRooms)
                                    {
                                        int roomSize = room.RoomSize;
                                        decimal roomPrice = room.RoomPrice;

                                        Console.WriteLine($"RoomID: {room.RoomID} {room.RoomName,-21} {room.RoomType,-11} " +
                                            $"{roomSize,-5}kvm,  Price per Night: {roomPrice,-5}kr");
                                    }

                                    Console.Write("Enter room ID to book: ");
                                    if (int.TryParse(Console.ReadLine(), out int roomId))
                                    {
                                        var selectedRoom = availableRooms.FirstOrDefault(room => room.RoomID == roomId);

                                        if (selectedRoom != null)
                                        {
                                            var isRoomAvailable = !_dbContext.Booking
                                            .Where(b => b.CustomerID == customerId && b.IsBooked)
                                            .Any(b =>
                                             bookingDate < b.BookingEndDate && bookingDate
                                            .AddDays(numberOfNights) > b.BookingStartDate
                                            );

                                            if (isRoomAvailable)
                                            {
                                                var newBooking = new Booking
                                                {
                                                    BookingStartDate = bookingDate,
                                                    BookingEndDate = bookingDate.AddDays(numberOfNights),
                                                    IsBooked = true,
                                                    CustomerID = customer.CustomerID,
                                                    RoomID = roomId
                                                };

                                                decimal roomPrice = selectedRoom.RoomPrice;

                                                var invoiceAmount = roomPrice * numberOfNights;

                                                var newInvoice = new Invoice
                                                {
                                                    InvoiceAmount = invoiceAmount,
                                                    InvoiceDue = newBooking.BookingEndDate,
                                                    IsPaid = false,
                                                };

                                                newBooking.Invoice = newInvoice;

                                                _dbContext.Booking.Add(newBooking);
                                                _dbContext.Invoice.Add(newInvoice);

                                                _dbContext.SaveChanges();
                                                Message.InputSuccessMessage("\nBooking created successfully!");

                                                Console.WriteLine("===================================================================================");
                                                Console.WriteLine($"Booking details:\n{newBooking}");
                                                Console.WriteLine($"\nInvoice details:\n{newInvoice}");
                                                Console.WriteLine("===================================================================================");
                                            }
                                            else
                                            {
                                                Message.ErrorMessage("The customer has already booked a room for the selected dates. Booking not created.");
                                            }
                                        }
                                        else
                                        {
                                            Message.ErrorMessage("Invalid room selection or the room is not available. Booking not created.");
                                        }
                                    }
                                    else
                                    {
                                        Message.ErrorMessage("Invalid room ID. Booking not created.");
                                    }
                                }
                                else
                                {
                                    Message.ErrorMessage("No rooms are available for the selected dates. Booking not created.");
                                    foreach (var nextDate in _dbContext.Booking)
                                    {
                                        Console.WriteLine($"Next available dates: Room ID {nextDate.RoomID}, " +
                                            $"Start Date: {nextDate.BookingStartDate.ToString("yyyy-MM-dd")}, End Date: {nextDate.BookingEndDate.ToString("yyyy-MM-dd")}");
                                    }
                                }
                            }
                            else
                            {
                                Message.ErrorMessage("Invalid number of nights. Booking not created.");
                            }
                        }
                        else
                        {
                            Message.ErrorMessage("Invalid date format. Please use yyyy-MM-dd. Booking not created.");
                        }
                    }
                    else
                    {
                        Message.ErrorMessage("Customer with the entered ID does not exist. Booking not created." +
                            "\nOr customer age is less than 18!");
                    }
                }
                else
                {
                    Message.ErrorMessage("Invalid Customer ID. Booking not created.");
                }
            }
            catch (Exception ex)
            {
                Message.ErrorMessage($"An error occurred: {ex.Message}");
            }

            Console.WriteLine("\nPress any button to continue...");
            Console.ReadKey();
        }
        public void GetBookingByID()
        {
            try
            {
                Console.Clear();
                foreach (var bookings in _dbContext.Booking)
                {
                    Console.WriteLine($"BookingID: {bookings.BookingID}");
                }

                Console.Write("Enter booking ID for detailed information: ");

                if (int.TryParse(Console.ReadLine(), out int bookingId))
                {
                    var booking = _dbContext.Booking
                        .Include(b => b.Room)
                        .Include(b => b.Customer)
                        .Include(b => b.Invoice)
                        .FirstOrDefault(b => b.BookingID == bookingId);

                    if (booking != null)
                    {
                        Console.Clear();
                        Console.WriteLine(">Detailed Booking Information<");

                        Console.WriteLine("\n====================================================================");
                        Console.WriteLine($"Booking ID: {booking.BookingID}");
                        Console.WriteLine($"Booking Start Date: {booking.BookingStartDate.ToString("yyyy-MM-dd")}");
                        Console.WriteLine($"Booking End Date: {booking.BookingEndDate.ToString("yyyy-MM-dd")}");

                        if (booking.Invoice != null)
                        {
                            Console.WriteLine($"\nInvoice ID: {booking.Invoice.InvoiceID}");
                            Console.WriteLine($"Amount Due: {booking.Invoice.InvoiceAmount.ToString("C2") ?? "N/A"}");
                            Console.WriteLine($"Amount Due Date {booking.Invoice.InvoiceDue.ToString("yyyy-MM-dd")}");
                            Console.WriteLine($"Invoice Paid: {booking.Invoice.IsPaid}");
                        }

                        Console.WriteLine($"\nRoom ID: {booking.RoomID}");
                        Console.WriteLine($"Room Name: {booking.Room.RoomName}");
                        Console.WriteLine($"Room Size: {booking.Room.RoomSize}");
                        Console.WriteLine($"Room ExtraBeds: {booking.Room.ExtraBeds}");
                        Console.WriteLine($"Room ExtraBedsCount: {booking.Room.ExtraBedsCount}");
                        Console.WriteLine($"Room Price: {booking.Room.RoomPrice}");
                        Console.WriteLine($"Room Type: {booking.Room?.RoomType ?? RoomType.SingleRoom}");

                        if (booking.Customer != null)
                        {
                            Console.WriteLine($"\nCustomer ID: {booking.CustomerID}" +
                                $"\nCustomer Name: {booking.Customer.CustomerFirstName} {booking.Customer.CustomerLastName}");
                        }
                        Console.WriteLine("====================================================================\n");
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
        public void GetAllBookings()
        {
            Console.Clear();
            var bookings = _dbContext.Booking.ToList();

            Console.WriteLine("╭─────────────╮───────────────────╮───────────────────╮─────────────╮──────────╮──────────╮");
            Console.WriteLine("│ Booking ID  │ Start Date        │ End Date          │ Customer ID │ Room ID  │ Status   │");
            Console.WriteLine("├─────────────┼───────────────────┼───────────────────┼─────────────┼──────────┤──────────┤");

            foreach (var booking in bookings)
            {
                if (booking.IsActive)
                {
                    Console.WriteLine($"│{booking.BookingID,-13}│{booking.BookingStartDate.ToString("yyyy-MM-dd"),-19}│{booking.BookingEndDate.ToString("yyyy-MM-dd"),-19}│{booking.CustomerID,-13}│{booking.RoomID,-10}│ BOOKED   │");
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"│{booking.BookingID,-13}│{booking.BookingStartDate.ToString("yyyy-MM-dd"),-19}│{booking.BookingEndDate.ToString("yyyy-MM-dd"),-19}│{booking.CustomerID,-13}│{booking.RoomID,-10}│ CANCELED │");
                    Console.ResetColor();
                }

                Console.WriteLine("├─────────────┼───────────────────┼───────────────────┼─────────────┼──────────┤──────────┤");
            }

            Console.WriteLine("╰─────────────╯───────────────────╯───────────────────╯─────────────╯──────────╯──────────╯");
        }
        public void UpdateBookingStartDate()
        {
            try
            {
                GetAllBookings();

                Console.Write("Enter booking ID to update startdate: ");
                if (int.TryParse(Console.ReadLine(), out int bookingId))
                {
                    var booking = _dbContext.Booking.Find(bookingId);

                    if (booking != null)
                    {
                        Console.Write("Enter new booking start date (yyyy-mm-dd): ");
                        if (DateTime.TryParse(Console.ReadLine(), out DateTime newBookingDate))
                        {
                            if (booking.BookingEndDate == null || newBookingDate <= booking.BookingEndDate)
                            {
                                booking.BookingStartDate = newBookingDate;
                                _dbContext.SaveChanges();
                                Message.InputSuccessMessage("Booking information updated successfully.");
                            }
                            else
                            {
                                Message.ErrorMessage("New start date cannot be later than the existing end date. Booking information not updated.");
                            }
                        }
                        else
                        {
                            Message.ErrorMessage("Invalid date format. Booking information not updated.");
                        }
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
        public void UpdateBookingEndDate()
        {
            try
            {
                GetAllBookings();

                Console.Write("Enter booking ID to update enddate: ");
                if (int.TryParse(Console.ReadLine(), out int bookingId))
                {
                    var booking = _dbContext.Booking.Find(bookingId);

                    if (booking != null)
                    {
                        Console.Write("Enter new booking end date (yyyy-mm-dd): ");
                        if (DateTime.TryParse(Console.ReadLine(), out DateTime newBookingDate))
                        {
                            if (newBookingDate >= booking.BookingStartDate)
                            {
                                booking.BookingEndDate = newBookingDate;
                                _dbContext.SaveChanges();
                                Message.InputSuccessMessage("Booking information updated successfully.");
                                Console.WriteLine("Booking information updated successfully.");
                            }
                            else
                            {
                                Message.ErrorMessage("End date must be equal to or later than the start date. Booking information not updated.");
                            }
                        }
                        else
                        {
                            Message.ErrorMessage("Invalid date format. Booking information not updated.");
                        }
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
        public void SoftDeleteBooking()
        {
            try
            {
                GetAllBookings();

                Console.Write("Enter booking ID to soft delete: ");
                if (int.TryParse(Console.ReadLine(), out int bookingId))
                {
                    var booking = _dbContext.Booking.Find(bookingId);

                    if (booking != null)
                    {
                        booking.IsActive = false;
                        _dbContext.SaveChanges();
                        Message.InputSuccessMessage("Booking soft deleted successfully.");
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
        public void CancelBooking()
        {
            try
            {
                GetAllBookings();

                Console.Write("Enter booking ID to cancel: ");
                if (int.TryParse(Console.ReadLine(), out int bookingId))
                {
                    var booking = _dbContext.Booking.Find(bookingId);

                    if (booking != null && booking.IsActive)
                    {
                        booking.IsBooked = false;
                        booking.IsActive = false;

                        if (booking.Invoice != null)
                        {
                            booking.Invoice.IsPaid = false;
                            booking.Invoice.IsActive = false;
                        }

                        _dbContext.SaveChanges();
                        Message.InputSuccessMessage("Booking canceled successfully.");
                    }
                    else
                    {
                        Message.ErrorMessage("Booking not found or already canceled.");
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
        public void ModifyBooking()
        {
            try
            {
                GetAllBookings();

                Console.Write("Enter booking ID to modify: ");
                if (int.TryParse(Console.ReadLine(), out int bookingId))
                {
                    var booking = _dbContext.Booking
                        .Include(b => b.Room)
                        .Include(b => b.Customer)
                        .Include(b => b.Invoice)
                        .FirstOrDefault(b => b.BookingID == bookingId);

                    if (booking != null && booking.IsActive)
                    {
                        Console.WriteLine($"Current Booking Information:\n{booking}");

                        Console.Write("Enter new booking start date (yyyy-MM-dd): ");
                        if (DateTime.TryParse(Console.ReadLine(), out DateTime newStartDate))
                        {
                            Console.Write("Enter new booking end date (yyyy-MM-dd): ");
                            if (DateTime.TryParse(Console.ReadLine(), out DateTime newEndDate))
                            {
                                if (newStartDate.Date >= DateTime.Now.Date && newEndDate.Date >= DateTime.Now.Date)
                                {
                                    if (newEndDate >= newStartDate)
                                    {
                                        booking.BookingStartDate = newStartDate;
                                        booking.BookingEndDate = newEndDate;

                                        if (booking.Invoice != null)
                                        {
                                            var roomSize = booking.Room?.RoomSize ?? 0;
                                            var roomPrice = booking.Room?.RoomPrice ?? 0;
                                            var numberOfNights = (newEndDate - newStartDate).Days;

                                            var invoiceAmount = roomPrice * numberOfNights;

                                            booking.Invoice.InvoiceAmount = invoiceAmount;
                                            booking.Invoice.InvoiceDue = newEndDate;
                                            booking.Invoice.IsPaid = invoiceAmount > 0;
                                        }

                                        _dbContext.SaveChanges();
                                        Message.InputSuccessMessage("Booking information modified successfully.");
                                    }
                                    else
                                    {
                                        Message.ErrorMessage("End date must be equal to or later than the start date. Booking information not modified.");
                                    }
                                }
                                else
                                {
                                    Message.ErrorMessage("Start and end dates must be equal to or later than today's date. Booking information not modified.");
                                }
                            }
                            else
                            {
                                Message.ErrorMessage("Invalid date format. Booking information not modified.");
                            }
                        }
                        else
                        {
                            Message.ErrorMessage("Invalid date format. Booking information not modified.");
                        }
                    }
                    else
                    {
                        Message.ErrorMessage("Booking not found or canceled.");
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
        public void SearchAvailableRooms()
        {
            try
            {
                Console.Clear();
                Console.Write("Enter date for room availability search (yyyy-MM-dd): ");
                if (DateTime.TryParse(Console.ReadLine(), out DateTime searchDate))
                {
                    var bookedRoomIds = _dbContext.Booking
                        .Where(b => b.BookingStartDate.Date <= searchDate.Date && b.BookingEndDate.Date >= searchDate.Date)
                        .Select(b => b.RoomID)
                        .ToList();

                    var availableRooms = _dbContext.Room
                        .Where(room => !bookedRoomIds.Contains(room.RoomID))
                        .Select(room => new { RoomID = room.RoomID, RoomName = room.RoomName })
                        .ToList();

                    Console.WriteLine("Available Rooms on {0}:", searchDate.ToShortDateString());
                    foreach (var room in availableRooms)
                    {
                        Console.WriteLine($"Room ID: {room.RoomID}, Room Name: {room.RoomName}");
                    }
                }
                else
                {
                    Message.ErrorMessage("Invalid date format. Please use yyyy-MM-dd.");
                }
            }
            catch (Exception ex)
            {
                Message.ErrorMessage($"An error occurred: {ex.Message}");
            }

            Console.WriteLine("Press any button to continue...");
            Console.ReadKey();
        }
        public void SearchAvailableIntervalRooms()
        {
            try
            {
                Console.Clear();
                Console.Write("Enter start date for room availability search (yyyy-MM-dd): ");
                if (DateTime.TryParse(Console.ReadLine(), out DateTime startDate))
                {
                    Console.Write("Enter end date for room availability search (yyyy-MM-dd): ");
                    if (DateTime.TryParse(Console.ReadLine(), out DateTime endDate))
                    {
                        var bookedRoomIds = _dbContext.Booking
                            .Where(b => (b.BookingStartDate.Date >= startDate.Date && b.BookingStartDate.Date <= endDate.Date) ||
                                        (b.BookingEndDate.Date >= startDate.Date && b.BookingEndDate.Date <= endDate.Date) ||
                                        (b.BookingStartDate.Date <= startDate.Date && b.BookingEndDate.Date >= endDate.Date))
                            .Select(b => b.RoomID)
                            .ToList();

                        var availableRooms = _dbContext.Room
                            .Where(room => !bookedRoomIds.Contains(room.RoomID))
                            .Select(room => new { RoomID = room.RoomID, RoomName = room.RoomName })
                            .Distinct()
                            .ToList();

                        Console.WriteLine($"Available Rooms between {startDate.ToShortDateString()} and {endDate.ToShortDateString()}:");
                        foreach (var room in availableRooms)
                        {
                            Console.WriteLine($"Room ID: {room.RoomID}, Room Name: {room.RoomName}");
                        }
                    }
                    else
                    {
                        Message.ErrorMessage("Invalid date format for end date. Please use yyyy-MM-dd.");
                    }
                }
                else
                {
                    Message.ErrorMessage("Invalid date format for start date. Please use yyyy-MM-dd.");
                }
            }
            catch (Exception ex)
            {
                Message.ErrorMessage($"An error occurred: {ex.Message}");
            }

            Console.WriteLine("Press any button to continue...");
            Console.ReadKey();
        }
        public void SearchCustomerBookings()
        {
            try
            {
                Console.Clear();
                foreach (var customer in _dbContext.Customer)
                {
                    Console.WriteLine($"Customer ID: {customer.CustomerID}, {customer.CustomerFirstName} {customer.CustomerLastName}");
                }
                Console.Write("\nEnter Customer ID to search for booked rooms: ");
                if (int.TryParse(Console.ReadLine(), out int customerID))
                {
                    var bookedRooms = _dbContext.Booking
                        .Where(b => b.CustomerID == customerID)
                        .Join(
                            _dbContext.Room,
                            booking => booking.RoomID,
                            room => room.RoomID,
                            (booking, room) => new { RoomID = room.RoomID, RoomName = room.RoomName }
                        )
                        .Distinct()
                        .ToList();

                    if (bookedRooms.Any())
                    {
                        Console.WriteLine($"Rooms booked by Customer ID {customerID}:");
                        foreach (var room in bookedRooms)
                        {
                            Console.WriteLine($"Room ID: {room.RoomID}, Room Name: {room.RoomName}");
                        }
                    }
                    else
                    {
                        Console.WriteLine($"Customer ID {customerID} currently has no rooms booked.");
                    }
                }
                else
                {
                    Message.ErrorMessage("Invalid Customer ID. Please enter a valid integer.");
                }
            }
            catch (Exception ex)
            {
                Message.ErrorMessage($"An error occurred: {ex.Message}");
            }

            Console.WriteLine("Press any button to continue...");
            Console.ReadKey();
        }


        //public void CreateBooking()
        //{
        //    Console.Write("Enter booking date (yyyy-MM-dd): ");
        //    if (DateTime.TryParse(Console.ReadLine(), out DateTime bookingDate))
        //    {
        //        if (bookingDate.Date < DateTime.Now.Date)
        //        {
        //            Message.ErrorMessage("Invalid booking date. Please enter a date equal to or later than today. Booking not created.");
        //            Console.ReadKey();
        //            return;
        //        }

        //        Console.Write("Enter number of nights (max 20): ");
        //        if (int.TryParse(Console.ReadLine(), out int numberOfNights) && numberOfNights > 0 && numberOfNights <= 20)
        //        {
        //            List<Customer> selectedCustomers = new List<Customer>();

        //            HashSet<int> uniqueCustomerIds = new HashSet<int>();

        //            for (int i = 0; i < 4; i++)
        //            {
        //                Console.Clear();
        //                Console.WriteLine("Available Customers:");
        //                foreach (var availableCustomer in _dbContext.Customer.Where(c => c.CustomerAge >= 18))
        //                {
        //                    Console.WriteLine($"CustomerID: {availableCustomer.CustomerID}, Name: " +
        //                        $"{availableCustomer.CustomerFirstName} {availableCustomer.CustomerLastName}");
        //                }

        //                Console.WriteLine("\nHow many customers will be booked? Decide below!");
        //                Console.WriteLine("The first customer ID entered will be the one to receive the invoice.\n");

        //                Console.Write($"Enter Customer {i + 1} ID (0 to finish): ");
        //                if (int.TryParse(Console.ReadLine(), out int customerId) && customerId > 0)
        //                {
        //                    if (uniqueCustomerIds.Add(customerId))
        //                    {
        //                        var customer = _dbContext.Customer.FirstOrDefault(c => c.CustomerID == customerId && c.CustomerAge >= 18);

        //                        if (customer != null)
        //                        {
        //                            selectedCustomers.Add(customer);

        //                            Message.InputSuccessMessage($"\nCustomer with ID {customerId} is added to the booking.");
        //                            Thread.Sleep(2000);
        //                        }
        //                        else
        //                        {
        //                            Message.ErrorMessage($"Customer with ID {customerId} does not exist or " +
        //                                $"is less than 18 years old. Please enter a valid ID.");
        //                            i--;
        //                        }
        //                    }
        //                    else
        //                    {
        //                        Message.ErrorMessage($"Customer with ID {customerId} has already been selected. " +
        //                            $"Please enter a different ID.");
        //                        i--;
        //                        Thread.Sleep(1000);
        //                    }
        //                }
        //                else
        //                {
        //                    break;
        //                }
        //            }

        //            if (selectedCustomers.Count > 0)
        //            {
        //                Console.Clear();
        //                Console.WriteLine("Selected Customers:");
        //                foreach (var customer in selectedCustomers)
        //                {
        //                    Console.WriteLine($"CustomerID: {customer.CustomerID}, Customer Name: " +
        //                        $"{customer.CustomerFirstName} {customer.CustomerLastName}");
        //                }

        //                var firstCustomer = selectedCustomers.First();

        //                var availableRooms = _dbContext.Room
        //                    .Where(room => room.Bookings
        //                        .All(b => bookingDate >= b.BookingEndDate || b.BookingStartDate >= bookingDate.AddDays(numberOfNights)))
        //                    .ToList();

        //                if (availableRooms.Any())
        //                {
        //                    Console.WriteLine("Available Rooms:");
        //                    foreach (var room in availableRooms)
        //                    {
        //                        int roomSize = room.RoomSize;
        //                        double roomPrice = room.RoomPrice;

        //                        Console.WriteLine($"RoomID: {room.RoomID} {room.RoomName,-21} {room.RoomType,-11} " +
        //                            $"{roomSize,-5}kvm,  Price per Night: {roomPrice,-5}kr");
        //                    }

        //                    Console.Write("Enter room ID: ");
        //                    if (int.TryParse(Console.ReadLine(), out int roomId))
        //                    {
        //                        var selectedRoom = availableRooms.FirstOrDefault(room => room.RoomID == roomId);

        //                        if (selectedRoom != null)
        //                        {
        //                            var isRoomAvailable = selectedRoom.Bookings == null || selectedRoom.Bookings.All(b =>
        //                                bookingDate >= b.BookingEndDate ||
        //                                b.BookingStartDate >= bookingDate.AddDays(7) || !b.IsBooked);

        //                            if (isRoomAvailable)
        //                            {
        //                                var newBooking = new Booking
        //                                {
        //                                    BookingStartDate = bookingDate,
        //                                    BookingEndDate = bookingDate.AddDays(numberOfNights),
        //                                    IsBooked = true,
        //                                    RoomID = roomId,
        //                                    Customers = selectedCustomers,
        //                                    CustomerID = firstCustomer.CustomerID
        //                                };

        //                                double roomPrice = selectedRoom.RoomPrice;

        //                                var invoiceAmount = roomPrice * numberOfNights;

        //                                var newInvoice = new Invoice
        //                                {
        //                                    InvoiceAmount = invoiceAmount,
        //                                    InvoiceDue = newBooking.BookingEndDate,
        //                                    IsPaid = false,
        //                                };

        //                                newBooking.Invoice = newInvoice;

        //                                _dbContext.Booking.Add(newBooking);
        //                                _dbContext.Invoice.Add(newInvoice);

        //                                _dbContext.SaveChanges();
        //                                Message.InputSuccessMessage("\nBooking created successfully.");

        //                                Console.WriteLine($"\nBooking details:\n{newBooking}");
        //                                Console.WriteLine($"Invoice details:\n{newInvoice}");

        //                            }
        //                            else
        //                            {
        //                                Message.ErrorMessage("The room is not available for the selected dates. Booking not created.");
        //                            }
        //                        }
        //                        else
        //                        {
        //                            Message.ErrorMessage("Invalid room selection or the room is not available. Booking not created.");
        //                        }
        //                    }
        //                    else
        //                    {
        //                        Message.ErrorMessage("Invalid room ID. Booking not created.");
        //                    }
        //                }
        //                else
        //                {
        //                    Message.ErrorMessage("No rooms are available for the selected dates. Booking not created.");
        //                    foreach (var nextDate in _dbContext.Booking)
        //                    {
        //                        Console.WriteLine($"Next available dates: Room ID {nextDate.RoomID}, " +
        //                            $"Start Date: {nextDate.BookingStartDate.ToString("yyyy-MM-dd")}, " +
        //                            $"End Date: {nextDate.BookingEndDate.ToString("yyyy-MM-dd")}");
        //                    }
        //                }
        //            }
        //            else
        //            {
        //                Message.ErrorMessage("No customers selected. Booking not created.");
        //            }
        //        }
        //        else
        //        {
        //            Message.ErrorMessage("Invalid number of nights. Booking not created.");
        //        }
        //    }
        //    else
        //    {
        //        Message.ErrorMessage("Invalid date format. Please use yyyy-MM-dd. Booking not created.");
        //    }

        //    Console.WriteLine("\nPress any button to continue...");
        //    Console.ReadKey();
        //}
    }
}
