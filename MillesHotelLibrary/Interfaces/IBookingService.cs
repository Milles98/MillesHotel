using Microsoft.EntityFrameworkCore;
using MillesHotelLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MillesHotelLibrary.Interfaces
{
    public interface IBookingService
    {
        void CreateBooking();
        void DisplayCustomerList();
        bool TryGetCustomer(int customerId, out Customer customer);
        bool TryGetBookingDetails(out DateTime bookingDate, out int numberOfNights);
        List<Room> GetAvailableRooms(int customerId, DateTime bookingDate, int numberOfNights);
        void DisplayAvailableRooms(List<Room> availableRooms);
        void DisplayNextAvailableDates();
        bool TryCreateBooking(Customer customer, int roomId, DateTime bookingDate, int numberOfNights);
        void GetBookingByID();
        void GetAllBookings();
        void UpdateBookingStartDate();
        void UpdateBookingEndDate();
        void CancelBooking();
        void ModifyBooking();
        void SearchAvailableRooms();
        void SearchAvailableIntervalRooms();
        void SearchCustomerBookings();
    }
}
