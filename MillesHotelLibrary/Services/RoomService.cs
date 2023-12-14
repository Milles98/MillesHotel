using Microsoft.EntityFrameworkCore;
using MillesHotelLibrary.Data;
using MillesHotelLibrary.Interfaces;
using MillesHotelLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Intrinsics.Arm;
using System.Text;
using System.Threading.Tasks;

namespace MillesHotelLibrary.Services
{
    public class RoomService : IRoomService
    {
        private readonly HotelDbContext _dbContext;

        public RoomService(HotelDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void CreateRoom()
        {
            Console.Write("Enter room name (max 19 characters): ");
            string roomName = Console.ReadLine();

            if (roomName.Length > 19)
            {
                Console.WriteLine("Room name exceeds maximum length (19 characters). Room not created.");
            }
            else
            {
                Console.Write("Enter room size: ");
                if (int.TryParse(Console.ReadLine(), out int roomSize) && roomSize <= 3000)
                {
                    Console.Write("Enter room type (SingleRoom/DoubleRoom): ");
                    if (Enum.TryParse(Console.ReadLine(), out RoomType roomType))
                    {
                        int extraBedsCount = 0;

                        if (roomType == RoomType.DoubleRoom && roomSize > 250)
                        {
                            Console.Write("How many extra beds would you like? (Enter 1 for one, 2 for two, or 0 for none): ");
                            if (!int.TryParse(Console.ReadLine(), out extraBedsCount))
                            {
                                Console.WriteLine("Invalid input for extra beds. Room not created.");
                                return;
                            }
                        }

                        var newRoom = new Room
                        {
                            RoomName = roomName,
                            RoomSize = roomSize,
                            RoomType = roomType,
                            ExtraBedsCount = extraBedsCount,
                        };

                        _dbContext.Rooms.Add(newRoom);
                        _dbContext.SaveChanges();
                        Console.WriteLine("Room created successfully.");
                    }
                    else
                    {
                        Console.WriteLine("Invalid room type format. Room not created.");
                    }
                }
                else
                {
                    Console.WriteLine("Invalid room size format or room size exceeds maximum (10,000). Please enter a valid number.");
                }
            }

            Console.WriteLine("Press any button to continue...");
            Console.ReadKey();
        }

        public void GetRoomByID()
        {
            foreach (var rooms in _dbContext.Rooms)
            {
                Console.WriteLine($"RoomID: {rooms.RoomID}");
            }

            Console.Write("Enter room ID: ");
            if (int.TryParse(Console.ReadLine(), out int roomId))
            {
                var room = _dbContext.Rooms.Find(roomId);

                if (room != null)
                {
                    Console.WriteLine();
                    Console.WriteLine($"Room ID: {room.RoomID}");
                    Console.WriteLine($"Room Name: {room.RoomName}");
                    Console.WriteLine($"Room Size: {room.RoomSize}");
                    Console.WriteLine($"Room Type: {room.RoomType}");
                    Console.WriteLine($"Has Extra Beds: {room.ExtraBeds}");
                    Console.WriteLine($"Is Active: {room.IsActive}");
                }
                else
                {
                    Console.WriteLine("Room not found.");
                }
            }
            else
            {
                Console.WriteLine("Invalid room ID format. Please enter a valid number.");
            }

            Console.WriteLine("Press any button to continue...");
            Console.ReadKey();
        }

        public void GetAllRooms()
        {
            var rooms = _dbContext.Rooms.ToList();

            Console.WriteLine("╭───────────────╮───────────────────╮─────────────╮─────────────╮─────────────╮─────────────╮");
            Console.WriteLine("│ Room ID       | Room Name         | Room Size   | Room Type   | Extra Beds  | Occupied    │");
            Console.WriteLine("├───────────────┼───────────────────┼─────────────┼─────────────┼─────────────┼─────────────┤");

            foreach (var room in rooms)
            {
                Console.WriteLine($"│{room.RoomID,-15}│{room.RoomName,-19}│{room.RoomSize,-13}│{room.RoomType,-13}│{room.ExtraBedsCount,-13}│{room.IsActive,-13}│");
                Console.WriteLine("├───────────────┼───────────────────┼─────────────┼─────────────┼─────────────┼─────────────┤");
            }

            Console.WriteLine("╰───────────────╯───────────────────╯─────────────╯─────────────╯─────────────╯─────────────╯");
        }

        public void UpdateRoom()
        {
            GetAllRooms();

            Console.Write("Enter room ID to update: ");
            if (int.TryParse(Console.ReadLine(), out int roomId))
            {
                var room = _dbContext.Rooms.Find(roomId);

                if (room != null)
                {
                    Console.Write("Enter new room name: ");
                    string newRoomName = Console.ReadLine();

                    Console.Write("Enter new room size: ");
                    if (int.TryParse(Console.ReadLine(), out int newRoomSize))
                    {
                        Console.Write("Enter new room type (Double Room - true, Single Room - false): ");
                        if (Enum.TryParse(Console.ReadLine(), out RoomType newRoomType))
                        {
                            room.RoomName = newRoomName;
                            room.RoomSize = newRoomSize;
                            room.RoomType = newRoomType;
                            _dbContext.SaveChanges();
                            Console.WriteLine("Room information updated successfully.");
                        }
                        else
                        {
                            Console.WriteLine("Invalid room type format. Room information not updated.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Invalid room size format. Room information not updated.");
                    }
                }
                else
                {
                    Console.WriteLine("Room not found.");
                }
            }
            else
            {
                Console.WriteLine("Invalid room ID format. Please enter a valid number.");
            }

            Console.WriteLine("Press any button to continue...");
            Console.ReadKey();
        }

        public void SoftDeleteRoom()
        {
            foreach (var rooms in _dbContext.Rooms)
            {
                Console.WriteLine($"RoomID: {rooms.RoomID}, RoomType: {rooms.RoomType}, RoomSize: {rooms.RoomSize}, IsActive: {rooms.IsActive}");
            }

            Console.Write("Enter room ID to soft delete: ");
            if (int.TryParse(Console.ReadLine(), out int roomId))
            {
                var room = _dbContext.Rooms.Find(roomId);

                if (room != null)
                {
                    room.IsActive = false;
                    _dbContext.SaveChanges();
                    Console.WriteLine("Room soft deleted successfully.");
                }
                else
                {
                    Console.WriteLine("Room not found.");
                }
            }
            else
            {
                Console.WriteLine("Invalid room ID format. Please enter a valid number.");
            }

            Console.WriteLine("Press any button to continue...");
            Console.ReadKey();
        }

        //Det skall gå att avboka ett rum eller ändra en bokning.

        //Applikationen skall hantera bokningar och visa vilka rum som är lediga under en viss period.
        //Det skall gå att söka på ett datum eller datumintervall och antal personer och få fram alla lediga rum som motsvarar sökningen.
        public List<Room> GetAvailableRooms(DateTime startDate, DateTime endDate, int numPeople)
        {
            var bookedRoomIds = _dbContext.Bookings
                .Where(b => b.IsActive && !(b.BookingEndDate <= startDate || b.BookingStartDate >= endDate))
                .Select(b => b.RoomID)
                .ToList();

            var availableRooms = _dbContext.Rooms
                .Where(r => !bookedRoomIds.Contains(r.RoomID) && r.IsActive && r.RoomSize >= numPeople)
                .ToList();

            return availableRooms;
        }

    }
}
