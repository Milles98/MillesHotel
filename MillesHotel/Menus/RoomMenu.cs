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
    public static class RoomMenu
    {
        //Room MainMenu Case 3 (1. Register Room, 2. See Room, 3. Update Room, 4. Delete Room, 0. Return to MainMenu)
        //Room Menu Case 1 (Input: 1. RoomSize, Double/Single Room, Extra beds, 0. Return to MainMenu)
        //Room Menu Case 2 (1. See room by roomID, 2. see all rooms, 0. Return to MainMenu)
        //Room Menu Case 3 (Input: RoomID, 1. NewRoomSize, 2. NewDouble/SingleRoom, 3. Extrabeds, 0. Return to MainMenu)
        //Room Menu Case 4 (Input: 1. RoomID, 0. Return to MainMenu)
        //Room Menu Case 0 (Return to MainMenu)
        public static void ShowRoomMenu(HotelDbContext dbContext)
        {
            RoomService roomService = new RoomService(dbContext);
            int choice;

            do
            {
                Console.Clear();
                Console.WriteLine("╭──────────────────────╮");
                Console.WriteLine("│Room Menu             │");
                Console.WriteLine("│1. Register Room      │");
                Console.WriteLine("│2. See Room           │");
                Console.WriteLine("│3. See all Rooms      │");
                Console.WriteLine("│4. Update Room        │");
                Console.WriteLine("│5. Soft Delete Room   │");
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

        private static void ShowUpdateRoomMenu(RoomService roomService)
        {
            int updateChoice;

            do
            {
                Console.Clear();
                Console.WriteLine("╭───────────────────────────────╮");
                Console.WriteLine("│Update Room Details            │");
                Console.WriteLine("│1. Update Room Name            │");
                Console.WriteLine("│2. Update Room Price           │");
                Console.WriteLine("│3. Update Room Size            │");
                Console.WriteLine("│4. Update Room Type            │");
                Console.WriteLine("│5. Add extra beds              │");
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
                        case 5:
                            roomService.AddExtraBeds();
                            break;
                        case 0:
                            Console.WriteLine("Returning to Room Menu...");
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
