using Microsoft.EntityFrameworkCore;
using MillesHotel.Data;
using MillesHotel.Interfaces;
using MillesHotel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MillesHotel.Services
{
    public class InvoiceService : IInvoiceService
    {
        private readonly HotelDbContext _dbContext;

        public InvoiceService(DbContextOptionsBuilder<HotelDbContext> options)
        {
            _dbContext = new HotelDbContext(options.Options);
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
                    int customerId = Convert.ToInt32(Console.ReadLine());

                    var newInvoice = new Invoice
                    {
                        InvoiceAmount = invoiceAmount,
                        InvoiceDue = invoiceDue,
                        IsActive = true,
                        CustomerID = customerId
                    };

                    _dbContext.Invoices.Add(newInvoice);
                    _dbContext.SaveChanges();
                    Console.WriteLine("Invoice created successfully.");
                }
                else
                {
                    Console.WriteLine("Invalid date format. Invoice not created.");
                }
            }
            else
            {
                Console.WriteLine("Invalid amount format. Invoice not created.");
            }

            Console.WriteLine("Press any button to continue...");
            Console.ReadKey();
        }

        public void GetInvoiceByID()
        {
            foreach (var showInvoice in _dbContext.Invoices)
            {
                Console.WriteLine($"CustomerID: {showInvoice.InvoiceID}");
            }

            Console.Write("Enter invoice ID: ");
            int invoiceId = Convert.ToInt32(Console.ReadLine());

            var invoice = _dbContext.Invoices.Find(invoiceId);

            if (invoice != null)
            {
                Console.WriteLine($"Invoice ID: {invoice.InvoiceID}");
                Console.WriteLine($"Invoice Amount: {invoice.InvoiceAmount}");
                Console.WriteLine($"Invoice Due: {invoice.InvoiceDue}");
                Console.WriteLine($"Is Active: {invoice.IsActive}");
                Console.WriteLine($"Customer ID: {invoice.CustomerID}");
            }
            else
            {
                Console.WriteLine("Invoice not found.");
            }

            Console.WriteLine("Press any button to continue...");
            Console.ReadKey();
        }

        public void UpdateInvoice()
        {
            foreach (var showInvoice in _dbContext.Invoices)
            {
                Console.WriteLine($"CustomerID: {showInvoice.InvoiceID}");
            }

            Console.Write("Enter invoice ID to update: ");
            int invoiceId = Convert.ToInt32(Console.ReadLine());

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
                        Console.WriteLine("Invoice information updated successfully.");
                    }
                    else
                    {
                        Console.WriteLine("Invalid date format. Invoice information not updated.");
                    }
                }
                else
                {
                    Console.WriteLine("Invalid amount format. Invoice information not updated.");
                }
            }
            else
            {
                Console.WriteLine("Invoice not found.");
            }

            Console.WriteLine("Press any button to continue...");
            Console.ReadKey();
        }

        public void SoftDeleteInvoice()
        {
            foreach (var showInvoice in _dbContext.Invoices)
            {
                Console.WriteLine($"InvoiceID: {showInvoice.InvoiceID}");
            }

            Console.Write("Enter invoice ID to soft delete: ");
            int invoiceId = Convert.ToInt32(Console.ReadLine());

            var invoice = _dbContext.Invoices.Find(invoiceId);

            if (invoice != null)
            {
                invoice.IsActive = false;
                _dbContext.SaveChanges();
                Console.WriteLine("Invoice soft deleted successfully.");
            }
            else
            {
                Console.WriteLine("Invoice not found.");
            }

            Console.WriteLine("Press any button to continue...");
            Console.ReadKey();
        }
    }
}
