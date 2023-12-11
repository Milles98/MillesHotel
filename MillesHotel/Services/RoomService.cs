using Microsoft.EntityFrameworkCore;
using MillesHotel.Data;
using MillesHotel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MillesHotel.Services
{
    public class RoomService
    {
        private readonly HotelDbContext _dbContext;

        public RoomService(DbContextOptionsBuilder<HotelDbContext> options)
        {
            _dbContext = new HotelDbContext(options.Options);
        }

        public void CreateRoom()
        {
            Console.Write("Enter room size: ");
            if (int.TryParse(Console.ReadLine(), out int roomSize))
            {
                Console.Write("Enter room type (SingleRoom/DoubleRoom): ");
                if (Enum.TryParse(Console.ReadLine(), out RoomType roomType))
                {
                    Console.Write("Does the room have extra beds? (true/false): ");
                    bool extraBeds = bool.Parse(Console.ReadLine());

                    var newRoom = new Room
                    {
                        RoomSize = roomSize,
                        RoomType = roomType,
                        ExtraBeds = extraBeds,
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
                Console.WriteLine("Invalid room size format. Room not created.");
            }

            Console.WriteLine("Press any button to continue...");
            Console.ReadKey();
        }

        public void GetRoomByID()
        {
            Console.Write("Enter room ID: ");
            int roomId = Convert.ToInt32(Console.ReadLine());

            var room = _dbContext.Rooms.Find(roomId);

            if (room != null)
            {
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

            Console.WriteLine("Press any button to continue...");
            Console.ReadKey();
        }

        public void UpdateRoom()
        {
            Console.Write("Enter room ID to update: ");
            int roomId = Convert.ToInt32(Console.ReadLine());

            var room = _dbContext.Rooms.Find(roomId);

            if (room != null)
            {
                Console.Write("Enter new room size: ");
                if (int.TryParse(Console.ReadLine(), out int newRoomSize))
                {
                    Console.Write("Enter new room type (Double Room - true, Single Room - false): ");
                    if (Enum.TryParse(Console.ReadLine(), out RoomType newRoomType))
                    {
                        Console.Write("Does the room have extra beds? (true/false): ");
                        bool newExtraBeds = bool.Parse(Console.ReadLine());

                        room.RoomSize = newRoomSize;
                        room.RoomType = newRoomType;
                        room.ExtraBeds = newExtraBeds;
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

            Console.WriteLine("Press any button to continue...");
            Console.ReadKey();
        }

        public void DeleteRoom()
        {
            Console.Write("Enter room ID to delete: ");
            int roomId = Convert.ToInt32(Console.ReadLine());

            var room = _dbContext.Rooms.Find(roomId);

            if (room != null)
            {
                _dbContext.Rooms.Remove(room);
                _dbContext.SaveChanges();
                Console.WriteLine("Room deleted successfully.");
            }
            else
            {
                Console.WriteLine("Room not found.");
            }

            Console.WriteLine("Press any button to continue...");
            Console.ReadKey();
        }

    }
}
