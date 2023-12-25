using Microsoft.EntityFrameworkCore;
using MillesHotelLibrary.Data;
using MillesHotelLibrary.ExtraServices;
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
            Console.Clear();
            Console.Write("Enter room name (max 19 characters): ");
            string roomName = Console.ReadLine();

            if (roomName.Length > 19)
            {
                Message.ErrorMessage("Room name exceeds maximum length (19 characters). Room not created.");
            }
            else
            {
                Console.Write("Enter new room size (between 10 and 100kvm): ");
                if (int.TryParse(Console.ReadLine(), out int roomSize) && (roomSize >= 10 && roomSize <= 100))
                {
                    Console.Write("Enter room type (SingleRoom/DoubleRoom): ");
                    if (Enum.TryParse(Console.ReadLine(), out RoomType roomType))
                    {
                        int extraBedsCount = 0;

                        if (roomType == RoomType.DoubleRoom && roomSize >= 30)
                        {
                            Console.WriteLine("Roomsize is over or equal to 30kvm, you can now decide extra beds!");
                            Console.Write("How many extra beds would you like? (Enter 1 for one, 2 for two, or 0 for none): ");

                            if (!int.TryParse(Console.ReadLine(), out extraBedsCount))
                            {
                                Message.ErrorMessage("Invalid input for extra beds. Room not created.");
                                return;
                            }

                        }

                        Console.Write("Enter room price per night (between 250 kr and 3500 kr): ");
                        if (decimal.TryParse(Console.ReadLine(), out decimal roomPrice) && (roomPrice >= 250 && roomPrice <= 3500))
                        {
                            var newRoom = new Room
                            {
                                RoomName = roomName,
                                RoomSize = roomSize,
                                RoomType = roomType,
                                ExtraBeds = extraBedsCount > 0 && extraBedsCount <= 2,
                                ExtraBedsCount = extraBedsCount,
                                RoomPrice = roomPrice,
                            };

                            _dbContext.Room.Add(newRoom);
                            _dbContext.SaveChanges();
                            Message.InputSuccessMessage("Room created successfully.");
                        }
                        else
                        {
                            Message.ErrorMessage("Invalid room price format or room price exceeds the allowed range. Please enter a valid number.");
                        }
                    }
                    else
                    {
                        Message.ErrorMessage("Invalid room type format. Room not created.");
                    }
                }
                else
                {
                    Message.ErrorMessage("Invalid room size format or room size exceeds maximum (100). Please enter a valid number.");
                }
            }

            Console.WriteLine("Press any button to continue...");
            Console.ReadKey();
        }
        public void GetRoomByID()
        {
            Console.Clear();
            foreach (var rooms in _dbContext.Room)
            {
                Console.WriteLine($"RoomID: {rooms.RoomID}");
            }

            Console.Write("Enter room ID: ");
            if (int.TryParse(Console.ReadLine(), out int roomId))
            {
                var room = _dbContext.Room.Find(roomId);

                if (room != null)
                {
                    Console.Clear();
                    Console.WriteLine("===================================");
                    Console.WriteLine($"Room ID: {room.RoomID}");
                    Console.WriteLine($"Room Name: {room.RoomName}");
                    Console.WriteLine($"Room Size: {room.RoomSize} m²");
                    Console.WriteLine($"Room Price/Night: {room.RoomPrice.ToString("C2")}");
                    Console.WriteLine($"Room Type: {room.RoomType}");
                    Console.WriteLine($"Has Extra Beds: {room.ExtraBeds}");
                    Console.WriteLine($"Extra Beds Count: {room.ExtraBedsCount}");
                    Console.WriteLine($"IsActive: {room.IsActive}");
                    Console.WriteLine("===================================");
                }
                else
                {
                    Message.ErrorMessage("Room not found.");
                }
            }
            else
            {
                Message.ErrorMessage("Invalid room ID format. Please enter a valid number.");
            }

            Console.WriteLine("\nPress any button to continue...");
            Console.ReadKey();
        }
        public void GetAllRooms()
        {
            Console.Clear();
            var rooms = _dbContext.Room.ToList();

            Console.WriteLine("╭───────────────╮───────────────────╮─────────────╮─────────────╮─────────────╮─────────────╮");
            Console.WriteLine("│ Room ID       | Room Name         | Room Size   | Room Type   | Extra Beds  | Price/Night │");
            Console.WriteLine("├───────────────┼───────────────────┼─────────────┼─────────────┼─────────────┼─────────────┤");

            foreach (var room in rooms)
            {
                Console.WriteLine($"│{room.RoomID,-15}│{room.RoomName,-19}│{room.RoomSize,-11}m\u00B2│{room.RoomType,-13}│{room.ExtraBedsCount,-13}│{room.RoomPrice,-13:C2}│");
                Console.WriteLine("├───────────────┼───────────────────┼─────────────┼─────────────┼─────────────┼─────────────┤");
            }

            Console.WriteLine("╰───────────────╯───────────────────╯─────────────╯─────────────╯─────────────╯─────────────╯");
        }
        public void UpdateRoomName()
        {
            GetAllRooms();

            Console.Write("Enter room ID to update name: ");
            if (int.TryParse(Console.ReadLine(), out int roomId))
            {
                var room = _dbContext.Room.Find(roomId);

                if (room != null)
                {
                    Console.Write("Enter new room name (max 19 characters): ");
                    string newRoomName = Console.ReadLine();

                    if (!string.IsNullOrWhiteSpace(newRoomName) && newRoomName.Length <= 19)
                    {
                        room.RoomName = newRoomName;
                        _dbContext.SaveChanges();
                        Message.InputSuccessMessage("Room name updated successfully.");
                    }
                    else
                    {
                        Message.ErrorMessage("Invalid room name. Room name must not be empty and should be 19 characters or less. Room name not updated.");
                    }
                }
                else
                {
                    Message.ErrorMessage("Room not found.");
                }
            }
            else
            {
                Message.ErrorMessage("Invalid room ID format. Please enter a valid number.");
            }

            Console.WriteLine("Press any button to continue...");
            Console.ReadKey();
        }
        public void UpdateRoomPrice()
        {
            GetAllRooms();

            Console.Write("Enter room ID to update price: ");
            if (int.TryParse(Console.ReadLine(), out int roomId))
            {
                var room = _dbContext.Room.Find(roomId);

                if (room != null)
                {
                    Console.WriteLine("Lowest: 250 kr, Max: 3500 kr");
                    Console.Write("Enter new room price: ");
                    if (int.TryParse(Console.ReadLine(), out int newRoomPrice))
                    {
                        if (newRoomPrice >= 250 && newRoomPrice <= 3500)
                        {
                            room.RoomPrice = newRoomPrice;
                            _dbContext.SaveChanges();
                            Message.InputSuccessMessage("Room price updated successfully.");
                        }
                        else
                        {
                            Message.ErrorMessage("Invalid room price. Room price not updated.");
                        }
                    }
                    else
                    {
                        Message.ErrorMessage("Invalid input for room price. Room price not updated.");
                    }
                }
                else
                {
                    Message.ErrorMessage("Room not found.");
                }
            }
            else
            {
                Message.ErrorMessage("Invalid room ID format. Please enter a valid number.");
            }

            Console.WriteLine("Press any button to continue...");
            Console.ReadKey();
        }
        public void UpdateRoomSize()
        {
            GetAllRooms();

            Console.Write("Enter room ID to update size: ");
            if (int.TryParse(Console.ReadLine(), out int roomId))
            {
                var room = _dbContext.Room.Find(roomId);

                if (room != null)
                {
                    Console.Write("Enter new room size (between 10 and 100): ");
                    if (int.TryParse(Console.ReadLine(), out int newRoomSize) && newRoomSize >= 10 && newRoomSize <= 100)
                    {
                        room.RoomSize = newRoomSize;
                        _dbContext.SaveChanges();
                        Message.InputSuccessMessage("Room size updated successfully.");
                    }
                    else
                    {
                        Message.ErrorMessage("Invalid room size. Room size must be between 10 and 100. Room size not updated.");
                    }
                }
                else
                {
                    Message.ErrorMessage("Room not found.");
                }
            }
            else
            {
                Message.ErrorMessage("Invalid room ID format. Please enter a valid number.");
            }

            Console.WriteLine("Press any button to continue...");
            Console.ReadKey();
        }
        public void UpdateRoomType()
        {
            GetAllRooms();

            Console.Write("Enter room ID to update room type: ");
            if (int.TryParse(Console.ReadLine(), out int roomId))
            {
                var room = _dbContext.Room.Find(roomId);

                if (room != null)
                {
                    Console.Write("Enter new room type (SingleRoom/DoubleRoom): ");
                    if (Enum.TryParse(Console.ReadLine(), out RoomType newRoomType))
                    {
                        if (newRoomType == RoomType.SingleRoom && room.RoomSize < 30)
                        {
                            room.RoomType = newRoomType;
                            _dbContext.SaveChanges();
                            Message.InputSuccessMessage("Room type updated successfully.");
                        }
                        else if (newRoomType == RoomType.DoubleRoom && room.RoomSize >= 30 && room.RoomSize <= 100)
                        {
                            room.RoomType = newRoomType;
                            if (room.RoomType == RoomType.DoubleRoom && room.RoomSize >= 30)
                            {
                                AddExtraBeds(room);
                            }
                            _dbContext.SaveChanges();
                            Message.InputSuccessMessage("Room type updated successfully.");
                        }
                        else
                        {
                            Message.ErrorMessage("Invalid room type or room size. Room type not updated.");
                        }
                    }
                    else
                    {
                        Message.ErrorMessage("Invalid room type format. Room type not updated.");
                    }
                }
                else
                {
                    Message.ErrorMessage("Room not found.");
                }
            }
            else
            {
                Message.ErrorMessage("Invalid room ID format. Please enter a valid number.");
            }

            Console.WriteLine("Press any button to continue...");
            Console.ReadKey();
        }
        public void AddExtraBeds(Room room)
        {
            Console.Write("Enter number of extra beds (0, 1, or 2): ");
            if (int.TryParse(Console.ReadLine(), out int extraBedsCount) && extraBedsCount >= 0 && extraBedsCount <= 2)
            {
                room.ExtraBedsCount = extraBedsCount;
                _dbContext.SaveChanges();
                Message.InputSuccessMessage("Extra beds added successfully.");
            }
            else
            {
                Message.ErrorMessage("Invalid input for extra beds. Please enter 0, 1, or 2 extra beds. Extra beds not added.");
            }
        }
        public void SoftDeleteRoom()
        {
            try
            {
                Console.Clear();
                foreach (var room in _dbContext.Room)
                {
                    Console.WriteLine($"RoomID: {room.RoomID}, RoomType: {room.RoomType}, RoomSize: {room.RoomSize}, IsActive: {room.IsActive}");
                }

                Console.Write("Enter room ID to soft delete: ");
                if (int.TryParse(Console.ReadLine(), out int roomId))
                {
                    var room = _dbContext.Room.Include(r => r.Bookings).FirstOrDefault(r => r.RoomID == roomId);

                    if (room != null)
                    {
                        // Check if all bookings have Booking.IsActive = false
                        if (room.Bookings == null || room.Bookings.All(b => !b.IsActive))
                        {
                            // Set Room.IsActive to false
                            room.IsActive = false;
                            _dbContext.SaveChanges();
                            Message.InputSuccessMessage("Room soft deleted successfully.");
                        }
                        else
                        {
                            Message.ErrorMessage("Cannot delete room. Some bookings are still active.");
                        }
                    }
                    else
                    {
                        Message.ErrorMessage("Room not found.");
                    }
                }
                else
                {
                    Message.ErrorMessage("Invalid room ID format. Please enter a valid number.");
                }
            }
            catch (Exception ex)
            {
                Message.ErrorMessage($"An error occurred: {ex.Message}");
            }

            Console.WriteLine("Press any button to continue...");
            Console.ReadKey();
        }
        public void ReactivateRoom()
        {
            try
            {
                Console.Clear();
                foreach (var showRoom in _dbContext.Room.Where(r => !r.IsActive))
                {
                    Console.WriteLine($"RoomID: {showRoom.RoomID}, RoomType: {showRoom.RoomType}, " +
                        $"RoomSize: {showRoom.RoomSize}, IsActive: {showRoom.IsActive}");
                }

                Console.Write("Enter room ID to reactivate: ");
                if (int.TryParse(Console.ReadLine(), out int roomId))
                {
                    var room = _dbContext.Room.Find(roomId);

                    if (room != null)
                    {
                        room.IsActive = true;
                        _dbContext.SaveChanges();
                        Message.InputSuccessMessage("Room reactivated successfully.");
                    }
                    else
                    {
                        Message.ErrorMessage("Room not found.");
                    }
                }
                else
                {
                    Message.ErrorMessage("Invalid room ID format. Please enter a valid number.");
                }
            }
            catch (Exception ex)
            {
                Message.ErrorMessage($"An error occurred: {ex.Message}");
            }

            Console.WriteLine("Press any button to continue...");
            Console.ReadKey();
        }
        public List<Room> GetAvailableRooms(DateTime startDate, DateTime endDate, int numPeople)
        {
            var bookedRoomIds = _dbContext.Booking
                .Where(b => b.IsOccupied() && !(b.BookingEndDate <= startDate || b.BookingStartDate >= endDate))
                .Select(b => b.RoomID)
                .ToList();

            var availableRooms = _dbContext.Room
                .Where(r => !bookedRoomIds.Contains(r.RoomID) && r.IsRoomBooked() && r.RoomSize >= numPeople)
                .ToList();

            return availableRooms;
        }
    }
}
