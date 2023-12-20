using Autofac;
using MillesHotelLibrary.Data;
using MillesHotelLibrary.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MillesHotelLibrary.Services
{
    public static class AutofacService
    {
        public static IContainer RegisteredContainers()
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<BookingService>().As<IBookingService>();
            builder.RegisterType<AdminService>().As<IAdminService>();
            builder.RegisterType<CustomerService>().As<ICustomerService>();
            builder.RegisterType<RoomService>().As<IRoomService>();
            builder.RegisterType<InvoiceService>().As<IInvoiceService>();

            builder.Register(c =>
            {
                var dbContext = DbConfiguration.StartDatabase();
                return dbContext;
            }).As<HotelDbContext>().InstancePerLifetimeScope();

            return builder.Build();
        }
    }
}
