﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using MillesHotel;

#nullable disable

namespace MillesHotel.Migrations
{
    [DbContext(typeof(HotelDbContext))]
    [Migration("20231207162102_test")]
    partial class test
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("MillesHotelLibrary.Models.Booking", b =>
                {
                    b.Property<int>("BookingID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("BookingID"));

                    b.Property<DateTime>("BookingDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("CustomerID")
                        .HasColumnType("int");

                    b.Property<int>("RoomID")
                        .HasColumnType("int");

                    b.HasKey("BookingID");

                    b.HasIndex("CustomerID");

                    b.ToTable("Bookings");
                });

            modelBuilder.Entity("MillesHotelLibrary.Models.Customer", b =>
                {
                    b.Property<int>("CustomerID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("CustomerID"));

                    b.Property<int>("CustomerAge")
                        .HasColumnType("int");

                    b.Property<string>("CustomerCountry")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CustomerEmail")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CustomerFirstName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CustomerLastName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CustomerPhone")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("CustomerID");

                    b.ToTable("Customers");
                });

            modelBuilder.Entity("MillesHotelLibrary.Models.Invoice", b =>
                {
                    b.Property<int>("InvoiceID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("InvoiceID"));

                    b.Property<int>("CustomerID")
                        .HasColumnType("int");

                    b.Property<double>("InvoiceAmount")
                        .HasColumnType("float");

                    b.Property<DateTime>("InvoiceDue")
                        .HasColumnType("datetime2");

                    b.HasKey("InvoiceID");

                    b.HasIndex("CustomerID");

                    b.ToTable("Invoices");
                });

            modelBuilder.Entity("MillesHotelLibrary.Models.Room", b =>
                {
                    b.Property<int>("RoomID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("RoomID"));

                    b.Property<int>("BookingID")
                        .HasColumnType("int");

                    b.Property<bool>("ExtraBeds")
                        .HasColumnType("bit");

                    b.Property<int>("RoomSize")
                        .HasColumnType("int");

                    b.Property<bool>("RoomType")
                        .HasColumnType("bit");

                    b.HasKey("RoomID");

                    b.HasIndex("BookingID")
                        .IsUnique();

                    b.ToTable("Rooms");
                });

            modelBuilder.Entity("MillesHotelLibrary.Models.Booking", b =>
                {
                    b.HasOne("MillesHotelLibrary.Models.Customer", "Customer")
                        .WithMany("Bookings")
                        .HasForeignKey("CustomerID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Customer");
                });

            modelBuilder.Entity("MillesHotelLibrary.Models.Invoice", b =>
                {
                    b.HasOne("MillesHotelLibrary.Models.Customer", "Customer")
                        .WithMany("Invoices")
                        .HasForeignKey("CustomerID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Customer");
                });

            modelBuilder.Entity("MillesHotelLibrary.Models.Room", b =>
                {
                    b.HasOne("MillesHotelLibrary.Models.Booking", "Booking")
                        .WithOne("Room")
                        .HasForeignKey("MillesHotelLibrary.Models.Room", "BookingID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Booking");
                });

            modelBuilder.Entity("MillesHotelLibrary.Models.Booking", b =>
                {
                    b.Navigation("Room")
                        .IsRequired();
                });

            modelBuilder.Entity("MillesHotelLibrary.Models.Customer", b =>
                {
                    b.Navigation("Bookings");

                    b.Navigation("Invoices");
                });
#pragma warning restore 612, 618
        }
    }
}