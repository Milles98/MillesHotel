using MillesHotelLibrary.Interfaces;
using MillesHotel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MillesHotelLibrary.Services
{
    public class RoomService : IRoomService
    {
        private List<Room> rooms = new List<Room>();

        public Room CreateRoom(Room newRoom)
        {
            return newRoom;
        }

        public Room GetRoomByID(int roomID)
        {
            return rooms[roomID];
        }

        public void UpdateRoom(Room updatedRoom)
        {

        }

        public void DeleteRoom(int roomID)
        {

        }

    }
}
