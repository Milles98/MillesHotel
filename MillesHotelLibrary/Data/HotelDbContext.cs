using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MillesHotelLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MillesHotelLibrary.Data
{
    public class HotelDbContext : DbContext
    {
        public HotelDbContext()
        {

        }

        public HotelDbContext(DbContextOptions<HotelDbContext> options) : base(options)
        {
        }
        public DbSet<Room> Room { get; set; }
        public DbSet<Customer> Customer { get; set; }
        public DbSet<Booking> Booking { get; set; }
        public DbSet<Invoice> Invoice { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure the relationship between Booking and Room
            //modelBuilder.Entity<Booking>()
            //    .HasOne(b => b.Room)
            //    .WithMany(r => r.Bookings)
            //    .HasForeignKey(b => b.RoomID)
            //    .IsRequired(false);
        }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    if (!optionsBuilder.IsConfigured)
        //    {
        //        optionsBuilder.UseSqlServer(@"server=localhost;initial catalog=MillesHotel;integrated security=true;TrustServerCertificate=True;");
        //    }
        //}
    }
}
