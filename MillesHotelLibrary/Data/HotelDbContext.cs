using Microsoft.EntityFrameworkCore;
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
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Invoice> Invoices { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure the relationship between Booking and Invoice
            modelBuilder.Entity<Booking>()
                .HasOne(b => b.Invoice)
                .WithOne(i => i.Booking)
                .HasForeignKey<Invoice>(i => i.BookingID)
                .IsRequired(false);  // Add this line to make the relationship optional

            // Configure the relationship between Booking and Room
            modelBuilder.Entity<Booking>()
                .HasOne(b => b.Room)
                .WithOne(r => r.Booking)
                .HasForeignKey<Room>(r => r.BookingID)
                .IsRequired(false);  // Add this line to make the relationship optional

            // Configure the relationship between Invoice and Customer
            modelBuilder.Entity<Invoice>()
                .HasOne(i => i.Customer)
                .WithMany()
                .HasForeignKey(i => i.CustomerID)
                .OnDelete(DeleteBehavior.Restrict);

            // Explicitly specify the foreign key property for the Booking relationship
            modelBuilder.Entity<Invoice>()
                .HasOne(i => i.Booking)
                .WithOne(b => b.Invoice)
                .HasForeignKey<Invoice>(i => i.BookingID)
                .IsRequired(false);

            // Explicitly specify the foreign key property for the Customer relationship
            modelBuilder.Entity<Invoice>()
                .HasOne(i => i.Customer)
                .WithMany(c => c.Invoices)
                .HasForeignKey(i => i.CustomerID)
                .OnDelete(DeleteBehavior.Restrict);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("MillesHotelContextConnection");
            }
        }
    }
}
