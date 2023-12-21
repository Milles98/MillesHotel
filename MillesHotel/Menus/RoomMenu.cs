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
    public static class RoomMenu
    {
        public static void ShowRoomMenu(IRoomService roomService)
        {
            int choice;

            do
            {
                Console.Clear();
                Message.MillesHotelMessage();
                Console.WriteLine("╭──────────────────────╮");
                Console.WriteLine("│Room Menu             │");
                Console.WriteLine("│1. Register Room      │");
                Console.WriteLine("│2. See Room Details   │");
                Console.WriteLine("│3. See all Rooms      │");
                Console.WriteLine("│4. Update Room        │");
                Console.WriteLine("│5. Soft Delete Room   │");
                Console.WriteLine("│6. Reactivate Room    │");
                Console.WriteLine("│0. Return to MainMenu │");
                Console.WriteLine("╰──────────────────────╯");

                Console.Write("Enter your choice: ");
                if (int.TryParse(Console.ReadLine(), out choice))
                {
                    switch (choice)
                    {
                        case 1:
                            roomService.CreateRoom();
                            break;
                        case 2:
                            roomService.GetRoomByID();
                            break;
                        case 3:
                            roomService.GetAllRooms();
                            Console.WriteLine("Press any button to continue...");
                            Console.ReadKey();
                            break;
                        case 4:
                            ShowUpdateRoomMenu(roomService);
                            break;
                        case 5:
                            roomService.SoftDeleteRoom();
                            break;
                        case 6:
                            roomService.ReactivateRoom();
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

        private static void ShowUpdateRoomMenu(IRoomService roomService)
        {
            int updateChoice;

            do
            {
                Console.Clear();
                Message.MillesHotelMessage();
                Console.WriteLine("╭───────────────────────────────╮");
                Console.WriteLine("│Update Room Details            │");
                Console.WriteLine("│1. Update Room Name            │");
                Console.WriteLine("│2. Update Room Price           │");
                Console.WriteLine("│3. Update Room Size            │");
                Console.WriteLine("│4. Update Room Type            │");
                Console.WriteLine("│0. Return to Booking Menu      │");
                Console.WriteLine("╰───────────────────────────────╯");

                Console.Write("Enter your choice: ");
                if (int.TryParse(Console.ReadLine(), out updateChoice))
                {
                    switch (updateChoice)
                    {
                        case 1:
                            roomService.UpdateRoomName();
                            break;
                        case 2:
                            roomService.UpdateRoomPrice();
                            break;
                        case 3:
                            roomService.UpdateRoomSize();
                            break;
                        case 4:
                            roomService.UpdateRoomType();
                            break;
                        case 0:
                            Console.WriteLine("Returning to Room Menu...");
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
