using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MillesHotelLibrary.Data;
using MillesHotelLibrary.Services;
using Autofac;
using MillesHotelLibrary.Interfaces;

namespace MillesHotel
{
    public class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            using (var container = AutofacService.RegisteredContainers())
            {
                var dbContext = container.Resolve<HotelDbContext>();

                var bookingService = container.Resolve<IBookingService>();
                var adminService = container.Resolve<IAdminService>();
                var customerService = container.Resolve<ICustomerService>();
                var invoiceService = container.Resolve<IInvoiceService>();
                var roomService = container.Resolve<IRoomService>();

                var app = new AppStart(dbContext, bookingService, adminService, customerService, invoiceService, roomService);
                app.Build();
            }
        }
    }
}
