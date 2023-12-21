using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MillesHotelLibrary.Data;
using MillesHotel.Menus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MillesHotelLibrary.Interfaces;
using MillesHotelLibrary.Services;

namespace MillesHotel
{
    public class AppStart
    {
        private readonly HotelDbContext _dbContext;
        private readonly IBookingService _bookingService;
        private readonly IAdminService _adminService;
        private readonly ICustomerService _customerService;
        private readonly IInvoiceService _invoiceService;
        private readonly IRoomService _roomService;

        public AppStart(
            HotelDbContext dbContext,
            IBookingService bookingService,
            IAdminService adminService,
            ICustomerService customerService,
            IInvoiceService invoiceService,
            IRoomService roomService)
        {
            _dbContext = dbContext;
            _bookingService = bookingService;
            _adminService = adminService;
            _customerService = customerService;
            _invoiceService = invoiceService;
            _roomService = roomService;
        }
        public void Build()
        {
            UpdateBookingStatus(_dbContext);
            _invoiceService.CheckAndDeactivateOverdueBookings();

            bool programRunning = true;
            do
            {
                MainMenu.ShowMenu(
                _dbContext,
                _bookingService,
                _customerService,
                _roomService,
                _invoiceService,
                _adminService);
            } while (programRunning);

        }

        private void UpdateBookingStatus(HotelDbContext dbContext)
        {
            var bookings = dbContext.Booking.ToList();

            foreach (var booking in bookings)
            {
                if (booking.BookingEndDate < DateTime.Now)
                {
                    booking.IsBooked = false;
                }
            }

            dbContext.SaveChanges();
        }
    }
}
