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
            AssignInvoiceToBooking();

            Console.Write("Enter invoice amount: ");
            if (double.TryParse(Console.ReadLine(), out double invoiceAmount))
            {
                Console.Write("Enter invoice due date (yyyy-mm-dd): ");
                if (DateTime.TryParse(Console.ReadLine(), out DateTime invoiceDue))
                {
                    var newInvoice = new Invoice
                    {
                        InvoiceAmount = invoiceAmount,
                        InvoiceDue = invoiceDue,
                        IsPaid = false
                    };

                    _dbContext.Invoice.Add(newInvoice);
                    _dbContext.SaveChanges();
                    Message.InputSuccessMessage("Invoice created successfully.");
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

        public void AssignInvoiceToBooking()
        {
            foreach (var showBooking in _dbContext.Booking)
            {
                Console.WriteLine($"BookingID: {showBooking.BookingID}");
            }

            Console.Write("Enter Booking ID: ");
            if (int.TryParse(Console.ReadLine(), out int bookingId))
            {
                var booking = _dbContext.Booking.Find(bookingId);

                if (booking != null)
                {
                    var invoice = _dbContext.Invoice.LastOrDefault();

                    if (invoice != null)
                    {
                        booking.InvoiceID = invoice.InvoiceID;
                        _dbContext.SaveChanges();
                        Message.InputSuccessMessage("Invoice assigned to booking successfully.");
                    }
                    else
                    {
                        Message.ErrorMessage("No invoices found. Please create an invoice first.");
                    }
                }
                else
                {
                    Message.ErrorMessage("Booking not found.");
                }
            }
            else
            {
                Message.ErrorMessage("Invalid Booking ID format.");
            }

            Console.WriteLine("Press any button to continue...");
            Console.ReadKey();
        }
        public void GetInvoiceByID()
        {
            foreach (var showInvoice in _dbContext.Invoice)
            {
                Console.WriteLine($"InvoiceID: {showInvoice.InvoiceID}");
            }

            Console.Write("Enter invoice ID: ");
            if (int.TryParse(Console.ReadLine(), out int invoiceId))
            {
                var invoice = _dbContext.Booking
                    .Include(i => i.Invoice)
                    .FirstOrDefault(i => i.InvoiceID == invoiceId);


                //Denna ska användas och den ovan köra typ en select med anonymous class likt nedan
                //            var method5 = employees
                //.Select(emp => new
                //{
                //    AnonymousClassId = emp.Id,
                //    AnonymousClassEmail = emp.Email
                //})
                //.ToList();

                //var invoice = _dbContext.Booking
                //    .Include(i => i.Invoice)
                //    .FirstOrDefault(i => i.InvoiceID == invoiceId);

                if (invoice != null)
                {
                    Console.WriteLine();
                    Console.WriteLine($"Invoice ID: {invoice.InvoiceID}");
                    Console.WriteLine($"Invoice Amount: {invoice.Invoice.InvoiceAmount.ToString("C2") ?? "N/A"}");
                    Console.WriteLine($"Invoice Due: {invoice.Invoice.InvoiceDue.ToString("yyyy-MM-dd")}");
                    Console.WriteLine($"Is Paid: {invoice.Invoice.IsPaid}");
                    Console.WriteLine($"Is Active: {invoice.IsActive}");
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
        public void UpdateInvoice()
        {
            foreach (var showInvoice in _dbContext.Invoice)
            {
                Console.WriteLine($"InvoiceID: {showInvoice.InvoiceID}");
            }

            Console.Write("Enter invoice ID to update: ");
            if (int.TryParse(Console.ReadLine(), out int invoiceId))
            {
                var invoice = _dbContext.Invoice.Find(invoiceId);

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
                var invoice = _dbContext.Invoice.Find(invoiceId);

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
                var invoice = _dbContext.Invoice.Find(invoiceId);

                if (invoice != null && invoice.IsActive)
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
                            //invoice.InvoiceAmount -= Math.Abs(paymentAmount);

                            if (invoice.InvoiceAmount == paymentAmount)
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
        //Om inte en betalning registrerats inom 10 dagar efter att bokningen är gjord annulleras bokningen dvs den upphör att gälla.
        public void CheckAndDeactivateOverdueBookings()
        {
            var overdueBookings = _dbContext.Booking
                .Where(b => b.IsBooked && b.BookingStartDate <= DateTime.Now
                .AddDays(-10) && b.Invoice != null && b.Invoice.IsPaid);

            foreach (var booking in overdueBookings)
            {
                booking.IsBooked = false;

                booking.Invoice.IsActive = false;
                booking.Invoice.IsPaid = false;
            }

            _dbContext.SaveChanges();
        }
    }
}
