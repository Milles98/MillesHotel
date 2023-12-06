﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MillesHotel.Menus
{
    public static class MainMenu
    {
        //MainMenu (Switch case, 1. Booking, 2. Customer, 3. Room, 4. Invoice, 0. Exit Program)
        public static void ShowMenu()
        {
            int choice;

            do
            {
                Console.Clear();
                Console.WriteLine("Main Menu");
                Console.WriteLine("1. Booking");
                Console.WriteLine("2. Customer");
                Console.WriteLine("3. Room");
                Console.WriteLine("4. Invoice");
                Console.WriteLine("0. Exit Program");

                Console.Write("Enter your choice: ");
                if (int.TryParse(Console.ReadLine(), out choice))
                {
                    switch (choice)
                    {
                        case 1:
                            BookingMenu.ShowBookingMenu();
                            break;
                        case 2:
                            CustomerMenu.ShowCustomerMenu();
                            break;
                        case 3:
                            RoomMenu.ShowRoomMenu();
                            break;
                        case 4:
                            InvoiceMenu.ShowInvoiceMenu();
                            break;
                        case 0:
                            Console.WriteLine("Exiting program...");
                            Environment.Exit(0);
                            break;
                        default:
                            Console.WriteLine("Invalid choice. Please try again.");
                            break;
                    }
                }
                else
                {
                    Console.WriteLine("Invalid input. Please enter a number.");
                }

            } while (choice != 0);
        }
    }
}
