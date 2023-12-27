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
                DisplayCustomerList();

                Console.Write("Enter Customer ID to book: ");

                if (!int.TryParse(Console.ReadLine(), out int customerId) || !TryGetCustomer(customerId, out var customer))
                {
                    Message.ErrorMessage("Invalid Customer ID. Booking not created.");
                    Console.ReadKey();
                    return;
                }

                if (!TryGetBookingDetails(out var bookingDate, out var numberOfNights))
                {
                    Console.ReadKey();
                    return;
                }

                var availableRooms = GetAvailableRooms(customerId, bookingDate, numberOfNights);

                if (!availableRooms.Any())
                {
                    Message.ErrorMessage("No rooms are available for the selected dates. Booking not created.");
                    DisplayNextAvailableDates();
                    Console.ReadKey();
                    return;
                }

                DisplayAvailableRooms(availableRooms);

                if (!int.TryParse(Console.ReadLine(), out int roomId) || !TryCreateBooking(customer, roomId, bookingDate, numberOfNights))
                {
                    Console.ReadKey();
                    return;
                }

                Message.InputSuccessMessage("Booking created successfully!");
            }
            catch (Exception ex)
            {
                Message.ErrorMessage($"An error occurred: {ex.Message}");
            }

            Console.WriteLine("\nPress any button to continue...");
            Console.ReadKey();
        }
        public void DisplayCustomerList()
        {
            Console.WriteLine("====================================================");

            var activeCustomers = _dbContext.Customer.Where(c => c.IsActive);

            foreach (var customer in activeCustomers)
            {
                Console.WriteLine($"CustomerID: {customer.CustomerID}, Customer Name: {customer.CustomerFirstName} {customer.CustomerLastName}");
            }

            Console.WriteLine("====================================================");
        }
        public bool TryGetCustomer(int customerId, out Customer customer)
        {
            customer = _dbContext.Customer.FirstOrDefault(c => c.CustomerID == customerId && c.CustomerAge >= 18);

            if (customer == null)
            {
                Message.ErrorMessage("Customer with the entered ID does not exist. Booking not created.\nOr customer age is less than 18!");
                return false;
            }

            return true;
        }
        public bool TryGetBookingDetails(out DateTime bookingDate, out int numberOfNights)
        {
            bookingDate = DateTime.MinValue;
            numberOfNights = 0;
            DateTime threeMonthsFromNow = DateTime.UtcNow.AddMonths(3);

            Console.WriteLine($"\nYou can only book rooms for dates within the next 3 months from today, latest {threeMonthsFromNow:yyyy-MM-dd}");

            Console.Write("Enter booking date (yyyy-MM-dd): ");
            if (!DateTime.TryParse(Console.ReadLine(), out bookingDate) || bookingDate.Date < DateTime.UtcNow.Date
                || bookingDate.Date > DateTime.UtcNow.AddMonths(3).Date)
            {

                Message.ErrorMessage($"Invalid booking date. Please enter a date equal to " +
                    $"or later than today and within the next 3 months (until {threeMonthsFromNow:yyyy-MM-dd}). Booking not created.");
                return false;
            }

            Console.Write("Enter number of nights (max 14): ");
            if (!int.TryParse(Console.ReadLine(), out numberOfNights) || numberOfNights <= 0 || numberOfNights > 14)
            {
                Message.ErrorMessage("Invalid number of nights. Booking not created.");
                return false;
            }

            return true;
        }
        public List<Room> GetAvailableRooms(int customerId, DateTime bookingDate, int numberOfNights)
        {
            DateTime endDate = bookingDate.AddDays(numberOfNights);

            var availableRooms = _dbContext.Room
                .Where(room => room.IsActive &&
                !_dbContext.Booking
                .Any(b => b.RoomID == room.RoomID && b.IsActive &&
                bookingDate < b.BookingEndDate && endDate > b.BookingStartDate))
                .ToList();

            return availableRooms;
        }
        public void DisplayAvailableRooms(List<Room> availableRooms)
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
        }
        public void DisplayNextAvailableDates()
        {
            foreach (var nextDate in _dbContext.Booking)
            {
                Console.WriteLine($"Next available dates: Room ID {nextDate.RoomID}, " +
                    $"Start Date: {nextDate.BookingStartDate.ToString("yyyy-MM-dd")}, End Date: {nextDate.BookingEndDate.ToString("yyyy-MM-dd")}");
            }
        }
        public bool TryCreateBooking(Customer customer, int roomId, DateTime bookingDate, int numberOfNights)
        {
            var selectedRoom = _dbContext.Room.FirstOrDefault(room => room.RoomID == roomId);

            if (selectedRoom == null)
            {
                Message.ErrorMessage("Invalid room selection or the room is not available. Booking not created.");
                return false;
            }

            var existingBookings = _dbContext.Booking
                .Where(b => b.RoomID == roomId || b.CustomerID == customer.CustomerID)
                .ToList();

            Console.WriteLine($"Input Booking Date: {bookingDate:yyyy-MM-dd}");

            var isRoomAvailable = existingBookings.All(b =>
                bookingDate >= b.BookingEndDate || bookingDate.AddDays(numberOfNights) <= b.BookingStartDate || !b.IsActive);

            if (!isRoomAvailable)
            {
                Message.ErrorMessage("Booking not created. The selected room is not available for the chosen dates.");
                Message.ErrorMessage("Or chosen customer already has a booking during the chosen dates.");
                return false;
            }

            var newBooking = new Booking
            {
                BookingStartDate = bookingDate,
                BookingEndDate = bookingDate.AddDays(numberOfNights),
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

            Console.WriteLine("===================================================================================");
            Console.WriteLine($"Booking details:\n{newBooking}");
            Console.WriteLine($"\nInvoice details:\n{newInvoice}");
            Console.WriteLine("===================================================================================");

            return true;
        }
        public void GetBookingByID()
        {
            try
            {
                Console.Clear();
                foreach (var bookings in _dbContext.Booking.Include(c => c.Customer))
                {
                    Console.WriteLine($"BookingID: {bookings.BookingID}, Customer: " +
                        $"{bookings.Customer.CustomerFirstName} {bookings.Customer.CustomerLastName}");
                }

                Console.Write("\nEnter booking ID for detailed information: ");

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

                        Console.WriteLine("\n=========================================");
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
                        Console.WriteLine("=========================================\n");
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
                        var roomId = booking.RoomID;

                        booking.IsOccupied();
                        booking.IsActive = false;

                        if (booking.Invoice != null)
                        {
                            booking.Invoice.IsPaid = false;
                            booking.Invoice.IsActive = false;
                        }

                        _dbContext.SaveChanges();

                        var room = _dbContext.Room.Find(roomId);
                        if (room != null)
                        {
                            if (room.Bookings != null)
                            {
                                foreach (var roomBooking in room.Bookings)
                                {
                                    roomBooking.IsOccupied();
                                }
                            }

                            room.IsRoomBooked();

                            _dbContext.SaveChanges();
                        }

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

                    Console.Clear();
                    Console.WriteLine("Available Rooms on {0}:\n", searchDate.ToShortDateString());
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

            Console.WriteLine("\nPress any button to continue...");
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

                        Console.Clear();
                        Console.WriteLine($"Available Rooms between {startDate.ToShortDateString()} and {endDate.ToShortDateString()}:\n");
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

            Console.WriteLine("\nPress any button to continue...");
            Console.ReadKey();
        }
        public void SearchCustomerBookings()
        {
            try
            {
                Console.Clear();
                DisplayCustomerList();
                Console.Write("\nEnter Customer ID to search for which rooms they have booked: ");
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
                        Console.Clear();
                        Console.WriteLine($"Rooms booked by Customer ID {customerID}:\n");
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

            Console.WriteLine("\nPress any button to continue...");
            Console.ReadKey();
        }
    }
}
