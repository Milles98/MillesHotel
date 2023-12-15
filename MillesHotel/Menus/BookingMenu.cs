using Microsoft.EntityFrameworkCore;
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
    public static class BookingMenu
    {
        //Booking MainMenu Case 1 (1. Register Booking, 2. See Booking, 3. Update Booking, 4. Delete Booking, 0. Return to MainMenu)
        //Booking Menu Case 1 (Input: 1. CustomerID, RoomSize, Double/Single Room, AmountOfBeds, 0. Return to MainMenu)
        //Booking Menu Case 2 (1. See booking by bookingID, 2. See all bookings, 0. Return to MainMenu)
        //Booking Menu Case 3 (Input: BookingID, 1. NewBookingDate, 0. Return to MainMenu)
        //Booking Menu Case 4 (Input: 1. BookingID, 0. Return to MainMenu)
        //Booking Menu Case 0 (Return to MainMenu)
        public static void ShowBookingMenu(HotelDbContext dbContext)
        {
            BookingService bookingService = new BookingService(dbContext);

            int choice;

            do
            {
                Console.Clear();
                Console.WriteLine("╭───────────────────────╮");
                Console.WriteLine("│Booking Menu           │");
                Console.WriteLine("│1. Register Booking    │");
                Console.WriteLine("│2. See Booking         │");
                Console.WriteLine("│3. See all Bookings    │");
                Console.WriteLine("│4. Update Booking      │");
                Console.WriteLine("│5. Soft Delete Booking │");
                Console.WriteLine("│0. Return to MainMenu  │");
                Console.WriteLine("╰───────────────────────╯");

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
                            bookingService.ModifyBooking();
                            break;
                        case 5:
                            bookingService.CancelBooking();
                            break;
                        case 6:
                            bookingService.SoftDeleteBooking();
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

        private static void ShowUpdateBookingMenu(BookingService bookingService)
        {
            int updateChoice;

            do
            {
                Console.Clear();
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
                            UserMessage.ErrorMessage("Invalid choice. Please try again.");
                            break;
                    }
                }
                else
                {
                    UserMessage.ErrorMessage("Invalid input. Please enter a number.");
                }

            } while (updateChoice != 0);
        }
    }
}
