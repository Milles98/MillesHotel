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
            int choice;

            do
            {
                Console.Clear();
                Console.WriteLine("╭───────────────────────────────╮");
                Console.WriteLine("│Admin Menu                     │");
                Console.WriteLine("│1. Permanently Delete Room     │");
                Console.WriteLine("│2. Permanently Delete Customer │");
                Console.WriteLine("│3. Permanently Delete Booking  │");
                Console.WriteLine("│4. Permanently Delete Invoice  │");
                Console.WriteLine("│0. Return to MainMenu          │");
                Console.WriteLine("╰───────────────────────────────╯");

                Console.Write("Enter your choice: ");
                if (int.TryParse(Console.ReadLine(), out choice))
                {
                    switch (choice)
                    {
                        case 1:
                            break;
                        case 0:
                            Console.WriteLine("Returning to MainMenu...");
                            break;
                        default:
                            UserMessage.ErrorMessage("Invalid choice. Please try again.");
                            break;
                    }
                }
                else
                {
                    UserMessage.ErrorMessage("Invalid input. Please enter a number.");
                }

            } while (choice != 0);
        }
    }
}
