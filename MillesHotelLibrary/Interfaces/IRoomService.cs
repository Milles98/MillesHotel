using MillesHotel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MillesHotelLibrary.Interfaces
{
    public interface IRoomService
    {
        Room CreateRoom(Room newRoom);
        Room GetRoomByID(int roomID);
        void UpdateRoom(Room updatedRoom);
        void DeleteRoom(int roomID);
    }
}
