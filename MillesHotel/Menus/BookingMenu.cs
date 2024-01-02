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
    public static class BookingMenu
    {
        public static void ShowBookingMenu(IBookingService bookingService)
        {
            int choice;

            do
            {
                Console.Clear();
                Message.MillesHotelMessage();
                Console.WriteLine("╭──────────────────────────────╮");
                Console.WriteLine("│Booking Menu                  │");
                Console.WriteLine("│1. Register Booking           │");
                Console.WriteLine("│2. See Booking Details        │");
                Console.WriteLine("│3. See all Bookings           │");
                Console.WriteLine("│4. Update Booking             │");
                Console.WriteLine("│5. Cancel/Soft Delete Booking │");
                Console.WriteLine("│6. Search Specific Date       │");
                Console.WriteLine("│7. Search Date Interval       │");
                Console.WriteLine("│8. Search Customer Booking    │");
                Console.WriteLine("│0. Return to MainMenu         │");
                Console.WriteLine("╰──────────────────────────────╯");

                Console.Write("Enter your choice: ");
                if (int.TryParse(Console.ReadLine(), out choice))
                {
                    switch (choice)
                    {
                        case 1:
                            bookingService.CreateBooking();
                            break;
                        case 2:
                            bookingService.GetBookingByID();
                            break;
                        case 3:
                            bookingService.GetAllBookings();
                            Console.WriteLine("Press any button to continue...");
                            Console.ReadKey();
                            break;
                        case 4:
                            ShowUpdateBookingMenu(bookingService);
                            break;
                        case 5:
                            bookingService.CancelBooking();
                            break;
                        case 6:
                            bookingService.SearchAvailableRooms();
                            break;
                        case 7:
                            bookingService.SearchAvailableIntervalRooms();
                            break;
                        case 8:
                            bookingService.SearchCustomerBookings();
                            break;
                        case 0:
                            Console.WriteLine("Returning to MainMenu...");
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

        private static void ShowUpdateBookingMenu(IBookingService bookingService)
        {
            int updateChoice;

            do
            {
                Console.Clear();
                Message.MillesHotelMessage();
                Console.WriteLine("╭───────────────────────────────╮");
                Console.WriteLine("│Update Booking Details         │");
                Console.WriteLine("│1. Update Booking Start Date   │");
                Console.WriteLine("│2. Update Booking End Date     │");
                Console.WriteLine("│0. Return to Booking Menu      │");
                Console.WriteLine("╰───────────────────────────────╯");

                Console.Write("Enter your choice: ");
                if (int.TryParse(Console.ReadLine(), out updateChoice))
                {
                    switch (updateChoice)
                    {
                        case 1:
                            bookingService.UpdateBookingStartDate();
                            break;
                        case 2:
                            bookingService.UpdateBookingEndDate();
                            break;
                        case 0:
                            Console.WriteLine("Returning to Booking Menu...");
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

            } while (updateChoice != 0);
        }
    }
}
