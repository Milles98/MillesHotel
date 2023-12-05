using Microsoft.EntityFrameworkCore;
using MillesHotel.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MillesHotel
{
    public class HotelDbContext : DbContext
    {
        public HotelDbContext(DbContextOptions<HotelDbContext> options) : base(options)
        {
        }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Invoice> Invoices { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("server=localhost;initial catalog=MillesHotel;integrated security=true");
            }
        }

    }
}
