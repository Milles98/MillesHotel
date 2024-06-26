﻿using Microsoft.EntityFrameworkCore;
using MillesHotelLibrary.Data;
using MillesHotelLibrary.ExtraServices;
using MillesHotelLibrary.Interfaces;
using MillesHotelLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MillesHotelLibrary.Services
{
    public class InvoiceService : IInvoiceService
    {
        private readonly HotelDbContext _dbContext;

        public InvoiceService(HotelDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public void GetInvoiceByID()
        {
            Console.Clear();
            foreach (var showInvoice in _dbContext.Invoice)
            {
                Console.WriteLine($"InvoiceID: {showInvoice.InvoiceID}");
            }

            Console.Write("Enter invoice ID for detailed view (0 to exit): ");
            if (int.TryParse(Console.ReadLine(), out int invoiceId))
            {
                if (invoiceId == 0)
                {
                    return;
                }

                var invoice = _dbContext.Booking
                    .Include(i => i.Invoice)
                    .Include(r => r.Room)
                    .Include(c => c.Customer)
                    .FirstOrDefault(i => i.InvoiceID == invoiceId);

                if (invoice != null)
                {
                    Console.Clear();
                    Console.WriteLine("=============================================================");
                    Console.WriteLine($"Invoice ID: {invoice.InvoiceID}");
                    Console.WriteLine($"Invoice Amount: {invoice.Invoice.InvoiceAmount.ToString("C2") ?? "N/A"}");
                    Console.WriteLine($"Price/night: {invoice.Room.RoomPrice.ToString("C2") ?? "N/A"}");
                    Console.WriteLine($"Invoice Due: {invoice.Invoice.InvoiceDue.ToString("yyyy-MM-dd")}");
                    Console.WriteLine($"Is Paid: {invoice.Invoice.IsPaid}");
                    Console.WriteLine($"Is Active: {invoice.IsActive}");
                    Console.WriteLine($"Customer ID: {invoice.CustomerID}");
                    Console.WriteLine($"Customer Name: {invoice.Customer.CustomerFirstName} {invoice.Customer.CustomerLastName}");
                    Console.WriteLine("=============================================================");
                }
                else
                {
                    Message.ErrorMessage("Invalid date format. Invoice not created.");
                }
            }
            else
            {
                Message.ErrorMessage("Invalid invoice ID format. Please enter a valid number.");
            }

            Console.WriteLine("\nPress any button to continue...");
            Console.ReadKey();
        }
        public void GetAllInvoices()
        {
            Console.Clear();
            var invoices = _dbContext.Booking.Include(i => i.Invoice).ToList();

            Console.WriteLine("╭──────────────╮────────────────────╮──────────────────╮────────────╮────────────╮");
            Console.WriteLine("│ Invoice ID   │ Invoice Due        │ Invoice Amount   │ Customer ID│ IsPaid     │");
            Console.WriteLine("├──────────────┼────────────────────┼──────────────────┼────────────┤────────────┤");

            foreach (var invoice in invoices)
            {
                var isPaid = invoice.Invoice.IsPaid;


                Console.ForegroundColor = isPaid ? ConsoleColor.Green : ConsoleColor.Red;
                Console.WriteLine($"│{invoice.InvoiceID,-14}│{invoice.Invoice.InvoiceDue.ToString("yyyy-MM-dd"),-20}│" +
                    $"{invoice.Invoice.InvoiceAmount.ToString("C2") ?? "N/A",-18}│{invoice.CustomerID,-12}│{isPaid,-12}│");
                Console.ResetColor();
                Console.WriteLine("├──────────────┼────────────────────┼──────────────────┼────────────┤────────────┤");
            }

            Console.WriteLine("╰──────────────╯────────────────────╯──────────────────╯────────────╯────────────╯");
        }
        public void GetAllPaidInvoices()
        {
            Console.Clear();
            var paidInvoices = _dbContext.Booking
                .Include(i => i.Invoice)
                .Where(b => b.Invoice != null && b.Invoice.IsPaid)
                .ToList();

            if (paidInvoices.Any())
            {
                Console.WriteLine("╭──────────────╮────────────────────╮──────────────────╮────────────╮─────────────╮─────────────╮");
                Console.WriteLine("│ Invoice ID   │ Invoice Due        │ Invoice Amount   │ Customer ID│ IsPaid      │ IsActive    │");
                Console.WriteLine("├──────────────┼────────────────────┼──────────────────┼────────────┤─────────────┤─────────────┤");

                foreach (var booking in paidInvoices)
                {
                    var invoice = booking.Invoice;

                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"│{invoice?.InvoiceID,-14}│{invoice?.InvoiceDue.ToString("yyyy-MM-dd"),-20}│" +
                        $"{invoice?.InvoiceAmount.ToString("C2") ?? "N/A",-18}│{booking?.CustomerID,-12}│{invoice?.IsPaid,-13}│{invoice?.IsActive,-13}│");

                    Console.ResetColor();
                    Console.WriteLine("├──────────────┼────────────────────┼──────────────────┼────────────┤─────────────┤─────────────┤");
                }

                Console.WriteLine("╰──────────────╯────────────────────╯──────────────────╯────────────╯─────────────╯─────────────╯");
            }
            else
            {
                Console.WriteLine("No invoices have been paid yet.\n");
            }
        }
        public void GetAllUnpaidInvoices()
        {
            Console.Clear();
            var unpaidInvoices = _dbContext.Booking
                .Include(i => i.Invoice)
                .Where(b => b.Invoice != null && !b.Invoice.IsPaid)
                .ToList();


            if (unpaidInvoices.Any())
            {
                Console.WriteLine("╭──────────────╮────────────────────╮──────────────────╮────────────╮─────────────╮─────────────╮");
                Console.WriteLine("│ Invoice ID   │ Invoice Due        │ Invoice Amount   │ Customer ID│ IsPaid      │ IsActive    │");
                Console.WriteLine("├──────────────┼────────────────────┼──────────────────┼────────────┤─────────────┤─────────────┤");
                foreach (var booking in unpaidInvoices)
                {
                    var invoice = booking.Invoice;

                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"│{invoice?.InvoiceID,-14}│{invoice?.InvoiceDue.ToString("yyyy-MM-dd"),-20}│" +
                        $"{invoice?.InvoiceAmount.ToString("C2") ?? "N/A",-18}│{booking?.CustomerID,-12}│{invoice?.IsPaid,-13}│{invoice?.IsActive,-13}│");

                    Console.ResetColor();
                    Console.WriteLine("├──────────────┼────────────────────┼──────────────────┼────────────┤─────────────┤─────────────┤");
                }

                Console.WriteLine("╰──────────────╯────────────────────╯──────────────────╯────────────╯─────────────╯─────────────╯");
            }
            else
            {
                Console.WriteLine("All invoices are paid.\n");
            }
        }
        public void UpdateInvoiceAmount()
        {
            Console.Clear();
            foreach (var showInvoice in _dbContext.Invoice)
            {
                Console.WriteLine($"InvoiceID: {showInvoice.InvoiceID}, Amount: {showInvoice.InvoiceAmount.ToString("C2")}");
            }

            Console.Write("Enter invoice ID to update invoice amount (0 to exit): ");
            if (int.TryParse(Console.ReadLine(), out int invoiceId))
            {
                if (invoiceId == 0)
                {
                    return;
                }

                var invoice = _dbContext.Invoice.Find(invoiceId);

                if (invoice != null)
                {
                    Console.Write("Enter new invoice amount: ");
                    if (decimal.TryParse(Console.ReadLine(), out decimal newInvoiceAmount) && (newInvoiceAmount >= 100 && newInvoiceAmount <= 100000))
                    {
                        invoice.InvoiceAmount = newInvoiceAmount;
                        _dbContext.SaveChanges();
                        Message.InputSuccessMessage("Invoice amount updated successfully.");
                    }
                    else
                    {
                        Message.ErrorMessage("Invalid amount format. Invoice amount not updated.");
                    }
                }
                else
                {
                    Message.ErrorMessage("Invoice not found.");
                }
            }
            else
            {
                Message.ErrorMessage("Invalid invoice ID format. Please enter a valid number.");
            }

            Console.WriteLine("Press any button to continue...");
            Console.ReadKey();
        }
        public void UpdateInvoiceDue()
        {
            Console.Clear();
            var activeInvoices = _dbContext.Invoice.Where(i => !i.IsPaid && i.IsActive);

            Console.WriteLine("Active Invoices:");

            foreach (var showInvoice in activeInvoices)
            {
                Console.WriteLine($"InvoiceID: {showInvoice.InvoiceID}, Due: {showInvoice.InvoiceDue}");
            }

            Console.Write("Enter invoice ID to update invoice due date (0 to exit): ");
            if (int.TryParse(Console.ReadLine(), out int invoiceId))
            {
                if (invoiceId == 0)
                {
                    return;
                }

                var invoice = _dbContext.Invoice.Find(invoiceId);

                if (invoice != null)
                {
                    Console.Write("Enter new invoice due date (yyyy-MM-dd): ");
                    if (DateTime.TryParse(Console.ReadLine(), out DateTime newInvoiceDue))
                    {
                        invoice.InvoiceDue = newInvoiceDue;
                        _dbContext.SaveChanges();
                        Message.InputSuccessMessage("Invoice due date updated successfully.");
                    }
                    else
                    {
                        Message.ErrorMessage("Invalid date format. Invoice due date not updated.");
                    }
                }
                else
                {
                    Message.ErrorMessage("Invoice not found.");
                }
            }
            else
            {
                Message.ErrorMessage("Invalid invoice ID format. Please enter a valid number.");
            }

            Console.WriteLine("Press any button to continue...");
            Console.ReadKey();
        }
        public void SoftDeleteInvoice()
        {
            GetAllInvoices();

            Console.Write("Enter invoice ID to soft delete (0 to exit): ");
            if (int.TryParse(Console.ReadLine(), out int invoiceId))
            {
                if (invoiceId == 0)
                {
                    return;
                }

                var invoice = _dbContext.Invoice.Find(invoiceId);

                if (invoice != null)
                {
                    Console.Write("Are you sure you want to inactivate the invoice? Type 'y' for yes, 'n' for no: ");
                    var confirmation = Console.ReadLine()?.ToLower();

                    if (confirmation == "y")
                    {
                        invoice.IsActive = false;
                        _dbContext.SaveChanges();
                        Message.InputSuccessMessage("Invoice soft deleted successfully.");
                    }
                    else if (confirmation == "n")
                    {
                        Message.InputSuccessMessage("Invoice inactivation canceled.");
                    }
                    else
                    {
                        Message.ErrorMessage("Invalid input. Please type 'y' for yes or 'n' for no.");
                    }
                }
                else
                {
                    Message.ErrorMessage("Invoice not found.");
                }
            }
            else
            {
                Message.ErrorMessage("Invalid invoice ID format. Please enter a valid number.");
            }

            Console.WriteLine("Press any button to continue...");
            Console.ReadKey();
        }
        public void RegisterPayment()
        {
            GetAllInvoices();

            Console.Write("Enter invoice ID (0 to exit): ");
            if (int.TryParse(Console.ReadLine(), out int invoiceId))
            {
                if (invoiceId == 0)
                {
                    return;
                }

                var invoice = _dbContext.Invoice.Find(invoiceId);

                if (invoice != null && invoice.IsActive)
                {
                    Console.WriteLine($"Amount Due: {invoice.InvoiceAmount.ToString("C2")}");

                    Console.Write("Enter payment amount: ");
                    if (decimal.TryParse(Console.ReadLine(), out decimal paymentAmount))
                    {
                        if (paymentAmount == invoice.InvoiceAmount)
                        {
                            invoice.IsPaid = true;
                            invoice.IsActive = false;

                            _dbContext.SaveChanges();
                            Message.InputSuccessMessage("Payment registered successfully.");
                        }
                        else
                        {
                            Message.ErrorMessage("Payment amount must be exactly equal to the invoice amount.");
                        }
                    }
                    else
                    {
                        Message.ErrorMessage("Invalid payment amount.");
                    }
                }
                else
                {
                    Message.ErrorMessage("Invoice not found, not active, or already paid.");
                }
            }
            else
            {
                Message.ErrorMessage("Invalid invoice ID.");
            }

            Console.WriteLine("Press any button to continue...");
            Console.ReadKey();
        }
        public void CheckAndDeactivateOverdueBookings()
        {
            var overdueBookings = _dbContext.Booking
                .Where(b => b.BookingStartDate <= DateTime.UtcNow.AddDays(-10) &&
                            b.Invoice != null &&
                            b.Invoice.IsPaid == false &&
                            b.Invoice.InvoiceDue <= DateTime.UtcNow)
                .AsEnumerable()
                .Where(b => b.IsOccupied());

            foreach (var booking in overdueBookings)
            {
                booking.IsActive = false;
                booking.IsOccupied();

                if (booking.Invoice != null)
                {
                    booking.Invoice.IsActive = false;
                    _dbContext.Entry(booking.Invoice).State = EntityState.Modified;
                }

                _dbContext.Entry(booking).State = EntityState.Modified;
            }

            _dbContext.SaveChanges();
        }
    }
}
