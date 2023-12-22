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
    public static class CustomerMenu
    {
        public static void ShowCustomerMenu(ICustomerService customerService)
        {
            int choice;

            do
            {
                Console.Clear();
                Message.MillesHotelMessage();
                Console.WriteLine("╭────────────────────────╮");
                Console.WriteLine("│Customer Menu           │");
                Console.WriteLine("│1. Register Customer    │");
                Console.WriteLine("│2. See Customer Details │");
                Console.WriteLine("│3. See all Customers    │");
                Console.WriteLine("│4. Update Customer      │");
                Console.WriteLine("│5. Soft Delete Customer │");
                Console.WriteLine("│6. Reactivate Customer  │");
                Console.WriteLine("│0. Return to MainMenu   │");
                Console.WriteLine("╰────────────────────────╯");

                Console.Write("Enter your choice: ");
                if (int.TryParse(Console.ReadLine(), out choice))
                {
                    switch (choice)
                    {
                        case 1:
                            customerService.CreateCustomer();
                            break;
                        case 2:
                            customerService.GetCustomerByID();
                            break;
                        case 3:
                            customerService.GetAllCustomers();
                            Console.WriteLine("Press any button to continue...");
                            Console.ReadKey();
                            break;
                        case 4:
                            ShowUpdateCustomerMenu(customerService);
                            break;
                        case 5:
                            customerService.SoftDeleteCustomer();
                            break;
                        case 6:
                            customerService.ReactivateCustomer();
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

        private static void ShowUpdateCustomerMenu(ICustomerService customerService)
        {
            int updateChoice;

            do
            {
                Console.Clear();
                Message.MillesHotelMessage();
                Console.WriteLine("╭──────────────────────────╮");
                Console.WriteLine("│Update Customer Details   │");
                Console.WriteLine("│1. Update Name            │");
                Console.WriteLine("│2. Update Last Name       │");
                Console.WriteLine("│3. Update Age             │");
                Console.WriteLine("│4. Update Email           │");
                Console.WriteLine("│5. Update Phone           │");
                Console.WriteLine("│6. Update Country         │");
                Console.WriteLine("│0. Return to Customer Menu│");
                Console.WriteLine("╰──────────────────────────╯");

                Console.Write("Enter your choice: ");
                if (int.TryParse(Console.ReadLine(), out updateChoice))
                {
                    switch (updateChoice)
                    {
                        case 1:
                            customerService.UpdateCustomerFirstName();
                            break;
                        case 2:
                            customerService.UpdateCustomerLastName();
                            break;
                        case 3:
                            customerService.UpdateCustomerAge();
                            break;
                        case 4:
                            customerService.UpdateCustomerEmail();
                            break;
                        case 5:
                            customerService.UpdateCustomerPhone();
                            break;
                        case 6:
                            customerService.UpdateCustomerCountry();
                            break;
                        case 0:
                            Console.WriteLine("Returning to Customer Menu...");
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
