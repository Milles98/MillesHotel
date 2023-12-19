using MillesHotelLibrary.Data;
using MillesHotelLibrary.ExtraServices;
using MillesHotelLibrary.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MillesHotel.Menus
{
    public static class AdminMenu
    {
        public static void ShowAdminMenu(HotelDbContext dbContext)
        {
            AdminService adminService = new AdminService(dbContext);

            int choice;

            //hur många kunder som finns
            //top 10 lista
            //top 10 by booking
            //top 10 by country

            do
            {
                Console.Clear();
                Message.MillesHotelMessage();
                Console.WriteLine("╭───────────────────────────────╮");
                Console.WriteLine("│Admin Menu                     │");
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("│1. Permanent deletion (DANGER) │");
                Console.ResetColor();
                Console.WriteLine("│2. Top 10 info                 │");
                Console.WriteLine("│0. Return to MainMenu          │");
                Console.WriteLine("╰───────────────────────────────╯");

                Console.Write("Enter your choice: ");
                if (int.TryParse(Console.ReadLine(), out choice))
                {
                    switch (choice)
                    {
                        case 1:
                            ShowDeletionMenu(adminService);
                            break;
                        case 2:
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

        private static void ShowDeletionMenu(AdminService adminService)
        {
            int updateChoice;

            do
            {
                Console.Clear();
                Message.MillesHotelMessage();
                Console.WriteLine("╭───────────────────────────────╮");
                Console.WriteLine("│Permanent deletion Menu        │");
                Console.WriteLine("│1. Permanently Delete Room     │");
                Console.WriteLine("│2. Permanently Delete Customer │");
                Console.WriteLine("│3. Permanently Delete Booking  │");
                Console.WriteLine("│4. Permanently Delete Invoice  │");
                Console.WriteLine("│0. Return to Admin Menu        │");
                Console.WriteLine("╰───────────────────────────────╯");

                Console.Write("Enter your choice: ");
                if (int.TryParse(Console.ReadLine(), out updateChoice))
                {
                    switch (updateChoice)
                    {
                        case 1:
                            adminService.DeleteRoom();
                            break;
                        case 2:
                            adminService.DeleteCustomer();
                            break;
                        case 3:
                            adminService.DeleteBooking();
                            break;
                        case 4:
                            adminService.DeleteInvoice();
                            break;
                        case 0:
                            Console.WriteLine("Returning to Admin Menu...");
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

            } while (updateChoice != 0);
        }
    }
}
