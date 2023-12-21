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
            Console.Write("Enter room name (max 19 characters): ");
            string roomName = Console.ReadLine();

            if (roomName.Length > 19)
            {
                Message.ErrorMessage("Room name exceeds maximum length (19 characters). Room not created.");
            }
            else
            {
                Console.Write("Enter new room size (between 20 and 3000): ");
                if (int.TryParse(Console.ReadLine(), out int roomSize) && (roomSize >= 20 && roomSize <= 3000))
                {
                    Console.Write("Enter room type (SingleRoom/DoubleRoom): ");
                    if (Enum.TryParse(Console.ReadLine(), out RoomType roomType))
                    {
                        int extraBedsCount = 0;

                        if (roomType == RoomType.DoubleRoom && roomSize >= 250)
                        {
                            Console.Write("How many extra beds would you like? (Enter 1 for one, 2 for two, or 0 for none): ");
                            if (!int.TryParse(Console.ReadLine(), out extraBedsCount))
                            {
                                Message.ErrorMessage("Invalid input for extra beds. Room not created.");
                                return;
                            }
                        }

                        Console.Write("Enter room price per night (between 250 and 3500): ");
                        if (double.TryParse(Console.ReadLine(), out double roomPrice) && (roomPrice >= 250 && roomPrice <= 3500))
                        {
                            var newRoom = new Room
                            {
                                RoomName = roomName,
                                RoomSize = roomSize,
                                RoomType = roomType,
                                ExtraBedsCount = extraBedsCount,
                                RoomPrice = roomPrice,
                                RoomBooked = true
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
                    Message.ErrorMessage("Invalid room size format or room size exceeds maximum (10,000). Please enter a valid number.");
                }
            }

            Console.WriteLine("Press any button to continue...");
            Console.ReadKey();
        }
        public void GetRoomByID()
        {
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
                    Console.WriteLine();
                    Console.WriteLine($"Room ID: {room.RoomID}");
                    Console.WriteLine($"Room Name: {room.RoomName}");
                    Console.WriteLine($"Room Size: {room.RoomSize}");
                    Console.WriteLine($"Room Type: {room.RoomType}");
                    Console.WriteLine($"Has Extra Beds: {room.ExtraBeds}");
                    Console.WriteLine($"Is Active: {room.RoomBooked}");
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
        public void GetAllRooms()
        {
            var rooms = _dbContext.Room.ToList();

            Console.WriteLine("╭───────────────╮───────────────────╮─────────────╮─────────────╮─────────────╮─────────────╮");
            Console.WriteLine("│ Room ID       | Room Name         | Room Size   | Room Type   | Extra Beds  | Price       │");
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

            Console.Write("Enter room ID to update: ");
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

            Console.Write("Enter room ID to update: ");
            if (int.TryParse(Console.ReadLine(), out int roomId))
            {
                var room = _dbContext.Room.Find(roomId);

                if (room != null)
                {
                    Console.WriteLine("Lowest: 250, Max: 3500");
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

            Console.Write("Enter room ID to update: ");
            if (int.TryParse(Console.ReadLine(), out int roomId))
            {
                var room = _dbContext.Room.Find(roomId);

                if (room != null)
                {
                    Console.Write("Enter new room size (between 20 and 3000): ");
                    if (int.TryParse(Console.ReadLine(), out int newRoomSize) && newRoomSize >= 20 && newRoomSize <= 3000)
                    {
                        room.RoomSize = newRoomSize;
                        _dbContext.SaveChanges();
                        Message.InputSuccessMessage("Room size updated successfully.");
                    }
                    else
                    {
                        Message.ErrorMessage("Invalid room size. Room size must be between 20 and 3000. Room size not updated.");
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

            Console.Write("Enter room ID to update: ");
            if (int.TryParse(Console.ReadLine(), out int roomId))
            {
                var room = _dbContext.Room.Find(roomId);

                if (room != null)
                {
                    Console.Write("Enter new room type (SingleRoom/DoubleRoom): ");
                    if (Enum.TryParse(Console.ReadLine(), out RoomType newRoomType))
                    {
                        room.RoomType = newRoomType;

                        if (room.RoomType == RoomType.DoubleRoom && room.RoomSize >= 250)
                        {
                            AddExtraBeds(room);
                        }

                        _dbContext.SaveChanges();
                        Message.InputSuccessMessage("Room type updated successfully.");
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
                        if (room.Bookings == null || !room.Bookings.Any())
                        {
                            room.IsActive = false;
                            _dbContext.SaveChanges();
                            Message.InputSuccessMessage("Room soft deleted successfully.");
                        }
                        else
                        {
                            Message.ErrorMessage("Cannot delete room with associated bookings. Please remove bookings first.");
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
        //Det skall gå att avboka ett rum eller ändra en bokning.

        //Applikationen skall hantera bokningar och visa vilka rum som är lediga under en viss period.
        //Det skall gå att söka på ett datum eller datumintervall och antal personer och få fram alla lediga rum som motsvarar sökningen.
        public List<Room> GetAvailableRooms(DateTime startDate, DateTime endDate, int numPeople)
        {
            var bookedRoomIds = _dbContext.Booking
                .Where(b => b.IsBooked && !(b.BookingEndDate <= startDate || b.BookingStartDate >= endDate))
                .Select(b => b.RoomID)
                .ToList();

            var availableRooms = _dbContext.Room
                .Where(r => !bookedRoomIds.Contains(r.RoomID) && r.RoomBooked && r.RoomSize >= numPeople)
                .ToList();

            return availableRooms;
        }
    }
}
