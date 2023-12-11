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
                Console.Write("Enter customer ID: ");
                int customerId = Convert.ToInt32(Console.ReadLine());

                Console.Write("Enter room ID: ");
                int roomId = Convert.ToInt32(Console.ReadLine());

                var newBooking = new Booking
                {
                    BookingDate = bookingDate,
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
        }

        public void GetBookingByID()
        {
            Console.Write("Enter booking ID: ");
            int bookingId = Convert.ToInt32(Console.ReadLine());

            var booking = _dbContext.Bookings.Find(bookingId);

            if (booking != null)
            {
                Console.WriteLine($"Booking ID: {booking.BookingID}");
                Console.WriteLine($"Booking Date: {booking.BookingDate}");
                Console.WriteLine($"Is Active: {booking.IsActive}");
                Console.WriteLine($"Customer ID: {booking.CustomerID}");
                Console.WriteLine($"Room ID: {booking.RoomID}");
            }
            else
            {
                Console.WriteLine("Booking not found.");
            }
        }

        public void UpdateBooking()
        {
            Console.Write("Enter booking ID to update: ");
            int bookingId = Convert.ToInt32(Console.ReadLine());

            var booking = _dbContext.Bookings.Find(bookingId);

            if (booking != null)
            {
                Console.Write("Enter new booking date (yyyy-mm-dd): ");
                if (DateTime.TryParse(Console.ReadLine(), out DateTime newBookingDate))
                {
                    booking.BookingDate = newBookingDate;
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

        public void DeleteBooking()
        {
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
        }

    }
}
