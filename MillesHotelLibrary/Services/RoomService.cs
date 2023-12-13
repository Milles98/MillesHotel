using Microsoft.EntityFrameworkCore;
using MillesHotelLibrary.Data;
using MillesHotelLibrary.Interfaces;
using MillesHotelLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
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
            Console.Write("Enter room size: ");
            if (int.TryParse(Console.ReadLine(), out int roomSize))
            {
                Console.Write("Enter room type (SingleRoom/DoubleRoom): ");
                if (Enum.TryParse(Console.ReadLine(), out RoomType roomType))
                {
                    var newRoom = new Room
                    {
                        RoomSize = roomSize,
                        RoomType = roomType,
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
                Console.WriteLine("Invalid room size format. Please enter a valid number.");
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
            Console.WriteLine("│ Room ID       | Room Name         | Room Size   | Room Type   | Extra Beds  | Is Active   │");
            Console.WriteLine("├───────────────┼───────────────────┼─────────────┼─────────────┼─────────────┼─────────────┤");

            foreach (var room in rooms)
            {
                Console.WriteLine($"│{room.RoomID,-15}│{room.RoomName,-19}│{room.RoomSize,-13}│{room.RoomType,-13}│{room.ExtraBeds,-13}│{room.IsActive,-13}│");
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
                    Console.Write("Enter new room size: ");
                    if (int.TryParse(Console.ReadLine(), out int newRoomSize))
                    {
                        Console.Write("Enter new room type (Double Room - true, Single Room - false): ");
                        if (Enum.TryParse(Console.ReadLine(), out RoomType newRoomType))
                        {
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

    }
}
