using Microsoft.EntityFrameworkCore;
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
        public void CreateInvoice()
        {
            Console.Write("Enter invoice amount: ");
            if (double.TryParse(Console.ReadLine(), out double invoiceAmount))
            {
                Console.Write("Enter invoice due date (yyyy-mm-dd): ");
                if (DateTime.TryParse(Console.ReadLine(), out DateTime invoiceDue))
                {
                    Console.Write("Enter customer ID: ");
                    if (int.TryParse(Console.ReadLine(), out int customerId))
                    {
                        var newInvoice = new Invoice
                        {
                            InvoiceAmount = invoiceAmount,
                            InvoiceDue = invoiceDue,
                            IsPaid = false,
                            CustomerID = customerId
                        };

                        _dbContext.Invoices.Add(newInvoice);
                        _dbContext.SaveChanges();
                        Message.InputSuccessMessage("Invoice created successfully.");
                    }
                    else
                    {
                        Message.ErrorMessage("Invalid customer ID format. Invoice not created.");
                    }
                }
                else
                {
                    Message.ErrorMessage("Invalid date format. Invoice not created.");
                }
            }
            else
            {
                Message.ErrorMessage("Invalid amount format. Invoice not created.");
            }

            Console.WriteLine("Press any button to continue...");
            Console.ReadKey();
        }
        public void GetInvoiceByID()
        {
            foreach (var showInvoice in _dbContext.Invoices)
            {
                Console.WriteLine($"InvoiceID: {showInvoice.InvoiceID}");
            }

            Console.Write("Enter invoice ID: ");
            if (int.TryParse(Console.ReadLine(), out int invoiceId))
            {
                var invoice = _dbContext.Invoices.Find(invoiceId);

                if (invoice != null)
                {
                    Console.WriteLine();
                    Console.WriteLine($"Invoice ID: {invoice.InvoiceID}");
                    Console.WriteLine($"Invoice Amount: {invoice.InvoiceAmount.ToString("C2") ?? "N/A"}");
                    Console.WriteLine($"Invoice Due: {invoice.InvoiceDue.ToString("yyyy-MM-dd")}");
                    Console.WriteLine($"Is Paid: {invoice.IsPaid}");
                    Console.WriteLine($"Customer ID: {invoice.CustomerID}");
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

            Console.WriteLine("Press any button to continue...");
            Console.ReadKey();
        }
        public void GetAllInvoices()
        {
            var invoices = _dbContext.Invoices.ToList();

            Console.WriteLine("╭──────────────╮────────────────────╮──────────────────╮────────────╮────────────╮");
            Console.WriteLine("│ Invoice ID   │ Invoice Due        │ Invoice Amount   │ Customer ID│ Status     │");
            Console.WriteLine("├──────────────┼────────────────────┼──────────────────┼────────────┤────────────┤");

            foreach (var invoice in invoices)
            {
                if (!invoice.IsPaid)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"│{invoice.InvoiceID,-14}│{invoice.InvoiceDue.ToString("yyyy-MM-dd"),-20}│" +
                        $"{invoice.InvoiceAmount.ToString("C2") ?? "N/A",-18}│{invoice.CustomerID,-12}│ NOT PAID   │");
                    Console.ResetColor();
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"│{invoice.InvoiceID,-14}│{invoice.InvoiceDue.ToString("yyyy-MM-dd"),-20}│" +
                        $"{invoice.InvoiceAmount.ToString("C2") ?? "N/A",-18}│{invoice.CustomerID,-12}│ PAID       │");
                    Console.ResetColor();
                }
                Console.WriteLine("├──────────────┼────────────────────┼──────────────────┼────────────┤────────────┤");
            }

            Console.WriteLine("╰──────────────╯────────────────────╯──────────────────╯────────────╯────────────╯");
            Console.WriteLine("Press any button to continue...");
            Console.ReadKey();
        }
        public void UpdateInvoice()
        {
            foreach (var showInvoice in _dbContext.Invoices)
            {
                Console.WriteLine($"InvoiceID: {showInvoice.InvoiceID}");
            }

            Console.Write("Enter invoice ID to update: ");
            if (int.TryParse(Console.ReadLine(), out int invoiceId))
            {
                var invoice = _dbContext.Invoices.Find(invoiceId);

                if (invoice != null)
                {
                    Console.Write("Enter new invoice amount: ");
                    if (double.TryParse(Console.ReadLine(), out double newInvoiceAmount))
                    {
                        Console.Write("Enter new invoice due date (yyyy-mm-dd): ");
                        if (DateTime.TryParse(Console.ReadLine(), out DateTime newInvoiceDue))
                        {
                            invoice.InvoiceAmount = newInvoiceAmount;
                            invoice.InvoiceDue = newInvoiceDue;
                            _dbContext.SaveChanges();
                            Message.InputSuccessMessage("Invoice information updated successfully.");
                        }
                        else
                        {
                            Message.ErrorMessage("Invalid date format. Invoice information not updated.");
                        }
                    }
                    else
                    {
                        Message.ErrorMessage("Invalid amount format. Invoice information not updated.");
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

            Console.Write("Enter invoice ID to soft delete: ");
            if (int.TryParse(Console.ReadLine(), out int invoiceId))
            {
                var invoice = _dbContext.Invoices.Find(invoiceId);

                if (invoice != null)
                {
                    invoice.IsActive = false;
                    _dbContext.SaveChanges();
                    Message.InputSuccessMessage("Invoice soft deleted successfully.");
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
        //Applikationen skall kunna registrera en inkommen betalning på en faktura.
        public void RegisterPayment()
        {
            GetAllInvoices();

            Console.Write("Enter invoice ID: ");
            if (int.TryParse(Console.ReadLine(), out int invoiceId))
            {
                var invoice = _dbContext.Invoices.Find(invoiceId);

                if (invoice != null)
                {
                    Console.WriteLine($"Amount Due: {invoice.InvoiceAmount.ToString("C2")}");

                    Console.Write("Enter payment amount: ");
                    if (double.TryParse(Console.ReadLine(), out double paymentAmount))
                    {
                        if (paymentAmount <= 0)
                        {
                            Message.ErrorMessage("Payment amount must be greater than zero.");
                        }
                        else if (paymentAmount > invoice.InvoiceAmount)
                        {
                            Message.ErrorMessage("Payment amount cannot exceed the invoice amount.");
                        }
                        else
                        {
                            invoice.InvoiceAmount -= paymentAmount;

                            if (invoice.InvoiceAmount <= 0)
                            {
                                invoice.IsPaid = true;
                                invoice.IsActive = false;
                            }

                            _dbContext.SaveChanges();
                            Message.InputSuccessMessage("Payment registered successfully.");
                        }
                    }
                    else
                    {
                        Message.ErrorMessage("Invalid payment amount.");
                    }
                }
                else
                {
                    Message.ErrorMessage("Invoice not found or not active.");
                }
            }
            else
            {
                Message.ErrorMessage("Invalid invoice ID.");
            }

            Console.WriteLine("Press any button to continue...");
            Console.ReadKey();
        }
        //Om inte en betalning registrerats inom 10 dagar efter att bokningen är gjord annulleras bokningen dvs den upphör att gälla.
        public void CheckAndDeactivateOverdueBookings()
        {
            var overdueBookings = _dbContext.Bookings
                .Where(b => b.IsBooked && b.BookingStartDate <= DateTime.Now.AddDays(-10) && b.Invoice != null && b.Invoice.IsPaid)
                .ToList();

            foreach (var booking in overdueBookings)
            {
                // Deactivate the booking
                booking.IsBooked = false;

                // Deactivate the associated invoice
                booking.Invoice.IsActive = false;
                booking.Invoice.IsPaid = false;
            }

            _dbContext.SaveChanges();
        }
    }
}
