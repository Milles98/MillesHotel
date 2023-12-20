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
    public static class AdminMenu
    {
        public static void ShowAdminMenu(IAdminService adminService)
        {
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
                Message.ErrorMessage("│1. Permanent deletion (DANGER) │");
                Console.WriteLine("│2. Fun Information             │");
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
                            ShowTopTen(adminService);
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

        private static void ShowDeletionMenu(IAdminService adminService)
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

        private static void ShowTopTen(IAdminService adminService)
        {
            int updateChoice;

            do
            {
                Console.Clear();
                Message.MillesHotelMessage();
                Console.WriteLine("╭───────────────────────────────╮");
                Console.WriteLine("│Fun Information                │");
                Console.WriteLine("│1. Number of Customers         │");
                Console.WriteLine("│2. Top 10 Customers            │");
                Console.WriteLine("│3. Top 10 Customers by Booking │");
                Console.WriteLine("│4. Top 10 Customers by Country │");
                Console.WriteLine("│0. Return to Admin Menu        │");
                Console.WriteLine("╰───────────────────────────────╯");

                Console.Write("Enter your choice: ");
                if (int.TryParse(Console.ReadLine(), out updateChoice))
                {
                    switch (updateChoice)
                    {
                        case 1:
                            adminService.GetNumberOfCustomers();
                            break;
                        case 2:
                            adminService.GetTop10Customers();
                            break;
                        case 3:
                            adminService.GetTop10CustomersByBooking();
                            break;
                        case 4:
                            adminService.GetTop10CustomersByCountry();
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
