using Microsoft.EntityFrameworkCore;
using MillesHotelLibrary.Data;
using MillesHotelLibrary.ExtraServices;
using MillesHotelLibrary.Interfaces;
using MillesHotelLibrary.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MillesHotel.Menus
{
    public static class InvoiceMenu
    {
        public static void ShowInvoiceMenu(IInvoiceService invoiceService)
        {
            int choice;

            //betala 10 dagar efter bokning

            do
            {
                Console.Clear();
                Message.MillesHotelMessage();
                Console.WriteLine("╭─────────────────────────────╮");
                Console.WriteLine("│Invoice Menu                 │");
                Console.WriteLine("│1. Register Invoice          │");
                Console.WriteLine("│2. See Invoice Details       │");
                Console.WriteLine("│3. See all Invoices          │");
                Console.WriteLine("│4. See all Paid Invoices     │");
                Console.WriteLine("│5. See all Unpaid Invoices   │");
                Console.WriteLine("│6. Update Invoice            │");
                Console.WriteLine("│7. Soft Delete Invoice       │");
                Console.WriteLine("│8. Register Payment          │");
                Console.WriteLine("│0. Return to MainMenu        │");
                Console.WriteLine("╰─────────────────────────────╯");

                Console.Write("Enter your choice: ");
                if (int.TryParse(Console.ReadLine(), out choice))
                {
                    switch (choice)
                    {
                        case 1:
                            invoiceService.CreateInvoice();
                            break;
                        case 2:
                            invoiceService.GetInvoiceByID();
                            break;
                        case 3:
                            invoiceService.GetAllInvoices();
                            Console.WriteLine("Press any button to continue...");
                            Console.ReadKey();
                            break;
                        case 4:
                            invoiceService.GetAllPaidInvoices();
                            Console.WriteLine("Press any button to continue...");
                            Console.ReadKey();
                            break;
                        case 5:
                            invoiceService.GetAllUnpaidInvoices();
                            Console.WriteLine("Press any button to continue...");
                            Console.ReadKey();
                            break;
                        case 6:
                            UpdateInvoiceMenu(invoiceService);
                            break;
                        case 7:
                            invoiceService.SoftDeleteInvoice();
                            break;
                        case 8:
                            invoiceService.RegisterPayment();
                            break;
                        case 0:
                            Console.WriteLine("Returning to MainMenu...");
                            break;
                        default:
                            Message.ErrorMessage("Invalid choice. Please try again.");
                            break;
                    }
                }
                else
                {
                    Message.ErrorMessage("Invalid input. Please enter a number.");
                }

            } while (choice != 0);
        }

        public static void UpdateInvoiceMenu(IInvoiceService invoiceService)
        {
            int choice;

            do
            {
                Console.Clear();
                Message.MillesHotelMessage();
                Console.WriteLine("╭─────────────────────────────╮");
                Console.WriteLine("│Update Invoice Menu          │");
                Console.WriteLine("│1. Change Amount             │");
                Console.WriteLine("│2. Change Due Date           │");
                Console.WriteLine("│0. Back to Invoice Menu      │");
                Console.WriteLine("╰─────────────────────────────╯");

                Console.Write("Enter your choice: ");
                if (int.TryParse(Console.ReadLine(), out choice))
                {
                    switch (choice)
                    {
                        case 1:
                            invoiceService.UpdateInvoiceAmount();
                            break;
                        case 2:
                            invoiceService.UpdateInvoiceDue();
                            break;
                        case 0:
                            Console.WriteLine("Returning to Invoice Menu...");
                            break;
                        default:
                            Message.ErrorMessage("Invalid choice. Please try again.");
                            break;
                    }
                }
                else
                {
                    Message.ErrorMessage("Invalid input. Please enter a number.");
                }

            } while (choice != 0);
        }
    }
}
