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
            modelBuilder.Entity<Invoice>()
                .Property(i => i.InvoiceAmount)
                .HasColumnType("decimal(18, 2)");

            modelBuilder.Entity<Room>()
                .Property(r => r.RoomPrice)
                .HasColumnType("decimal(18, 2)");

            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(@"server=localhost;initial catalog=MillesHotel;integrated security=true;TrustServerCertificate=True;");
            }
        }
    }
}
