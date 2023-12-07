using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MillesHotel.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MillesHotelLibrary.Models;

namespace MillesHotel
{
    public class HotelDbContext : DbContext
    {
        public HotelDbContext()
        {

        }
        public HotelDbContext(DbContextOptions<HotelDbContext> options) : base(options)
        {
        }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Invoice> Invoices { get; set; }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    if (!optionsBuilder.IsConfigured)
        //    {
        //        var connectionString = new ConfigurationBuilder()
        //            .AddJsonFile("appsettings.json")
        //            .Build()
        //            .GetConnectionString("MillesHotelContextConnection");

        //        optionsBuilder.UseSqlServer(connectionString);
        //    }
        //}
    }
}
