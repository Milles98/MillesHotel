using Microsoft.EntityFrameworkCore;
using MillesHotel.Data;
using MillesHotel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MillesHotel.Services
{
    public class BookingService
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
                Console.Write("Enter customer ID: ");
                int customerId = Convert.ToInt32(Console.ReadLine());


                foreach (var roomID in _dbContext.Rooms)
                {
                    Console.WriteLine($"RoomID: {roomID.RoomID} Roomtype {roomID.RoomType}");
                }
                Console.Write("Enter room ID: ");
                int roomId = Convert.ToInt32(Console.ReadLine());

                var newBooking = new Booking
                {
                    BookingStartDate = bookingDate,
                    BookingEndDate = bookingDate.AddDays(7),
                    IsActive = true,
                    CustomerID = customerId,
                    RoomID = roomId
                };

                _dbContext.Bookings.Add(newBooking);
                _dbContext.SaveChanges();
                Console.WriteLine("Booking created successfully.");
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

            var booking = _dbContext.Bookings.Include(b => b.Room).FirstOrDefault(b => b.BookingID == bookingId);

            if (booking != null)
            {
                Console.WriteLine($"Booking ID: {booking.BookingID}");
                Console.WriteLine($"Booking Start Date: {booking.BookingStartDate}");
                Console.WriteLine($"Booking End Date: {booking.BookingEndDate}");
                Console.WriteLine($"Room Type: {booking.Room?.RoomType ?? RoomType.SingleRoom}");
                Console.WriteLine($"Is Active: {booking.IsActive}");
                Console.WriteLine($"Customer ID: {booking.CustomerID}");
                Console.WriteLine($"Room ID: {booking.Room?.RoomID ?? 0}");
            }
            else
            {
                Console.WriteLine("Booking not found.");
            }

            Console.WriteLine("Press any button to continue...");
            Console.ReadKey();
        }

        public void UpdateBooking()
        {
            PrintAllBookings();

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

        public void DeleteBooking()
        {
            PrintAllBookings();

            Console.Write("Enter booking ID to delete: ");
            int bookingId = Convert.ToInt32(Console.ReadLine());

            var booking = _dbContext.Bookings.Find(bookingId);

            if (booking != null)
            {
                _dbContext.Bookings.Remove(booking);
                _dbContext.SaveChanges();
                Console.WriteLine("Booking deleted successfully.");
            }
            else
            {
                Console.WriteLine("Booking not found.");
            }

            Console.WriteLine("Press any button to continue...");
            Console.ReadKey();
        }

        public void PrintAllBookings()
        {
            foreach (var booking in _dbContext.Bookings)
            {
                Console.WriteLine($"BookingID: {booking.BookingID}");
                Console.WriteLine($"Booking Start Date: {booking.BookingStartDate}");
                Console.WriteLine($"Booking End Date: {booking.BookingEndDate}");
                Console.WriteLine($"Is Active: {booking.IsActive}");
                Console.WriteLine($"CustomerID: {booking.CustomerID}");
                Console.WriteLine($"RoomID: {booking.RoomID}");
                Console.WriteLine();
            }
        }

    }
}
