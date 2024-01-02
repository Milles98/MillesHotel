using Microsoft.EntityFrameworkCore;
using MillesHotelLibrary.Data;
using MillesHotelLibrary.ExtraServices;
using MillesHotelLibrary.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MillesHotel.Menus
{
    public static class MainMenu
    {
        public static void ShowMenu(HotelDbContext dbContext, IBookingService bookingService, ICustomerService customerService,
            IRoomService roomService, IInvoiceService invoiceService, IAdminService adminService)
        {
            int choice;

            do
            {
                Console.Clear();
                Message.MillesHotelMessage();
                Console.WriteLine("╭──────────────────╮");
                Console.WriteLine("│  Main Menu       │");
                Console.WriteLine("│ 1. Booking       │");
                Console.WriteLine("│ 2. Customer      │");
                Console.WriteLine("│ 3. Room          │");
                Console.WriteLine("│ 4. Invoice       │");
                Console.WriteLine("│ 5. Admin         │");
                Console.WriteLine("│ 0. Exit Program  │");
                Console.WriteLine("╰──────────────────╯");

                Console.Write("Enter your choice: ");
                if (int.TryParse(Console.ReadLine(), out choice))
                {
                    switch (choice)
                    {
                        case 1:
                            BookingMenu.ShowBookingMenu(bookingService);
                            break;
                        case 2:
                            CustomerMenu.ShowCustomerMenu(customerService);
                            break;
                        case 3:
                            RoomMenu.ShowRoomMenu(roomService);
                            break;
                        case 4:
                            InvoiceMenu.ShowInvoiceMenu(invoiceService);
                            break;
                        case 5:
                            AdminMenu.ShowAdminMenu(adminService);
                            break;
                        case 0:
                            Console.WriteLine("Exiting program...");
                            Environment.Exit(0);
                            break;
                        default:
                            Message.ErrorMessage("Invalid choice. Please try again.");
                            Thread.Sleep(1000);
                            break;
                    }
                }
                else
                {
                    Message.ErrorMessage("Invalid input. Please enter a number.");
                    Thread.Sleep(1000);
                }

            } while (choice != 0);
        }
    }
}
