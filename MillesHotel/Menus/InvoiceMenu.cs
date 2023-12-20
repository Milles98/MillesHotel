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
        //Invoice MainMenu Case 4 (1. Register Invoice, 2. See Invoice, 3. Update Invoice, 4. Delete Invoice, 0. Return to MainMenu)
        //Invoice Menu Case 1 (Input: CustomerID, 1. InvoiceAmount, InvoiceDue, 0. Return to MainMenu)
        //Invoice Menu Case 2 (1. See Invoice, 2. See all Invoice, 0. Return to MainMenu)
        //Invoice Menu Case 3 (Input invoiceID, 1. UpdateInvoiceAmount, 2. UpdateInvoiceDue, 0. Return to MainMenu)
        //Invoice Menu Case 4 (Input: 1. InvoiceID, 0. Return to MainMenu)
        public static void ShowInvoiceMenu(IInvoiceService invoiceService)
        {
            int choice;

            //betala 10 dagar efter bokning

            do
            {
                Console.Clear();
                Message.MillesHotelMessage();
                Console.WriteLine("╭───────────────────────╮");
                Console.WriteLine("│Invoice Menu           │");
                Console.WriteLine("│1. Register Invoice    │");
                Console.WriteLine("│2. See Invoice         │");
                Console.WriteLine("│3. See all Invoices    │");
                Console.WriteLine("│4. Update Invoice      │");
                Console.WriteLine("│5. Soft Delete Invoice │");
                Console.WriteLine("│6. Register payment    │");
                Console.WriteLine("│0. Return to MainMenu  │");
                Console.WriteLine("╰───────────────────────╯");

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
                            invoiceService.UpdateInvoice();
                            break;
                        case 5:
                            invoiceService.SoftDeleteInvoice();
                            break;
                        case 6:
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
    }
}
