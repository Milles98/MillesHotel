using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MillesHotelLibrary.Interfaces
{
    public interface IRoomService
    {
        void CreateRoom();
        void GetRoomByID();
        void GetAllRooms();
        void UpdateRoomName();
        void UpdateRoomPrice();
        void UpdateRoomSize();
        void UpdateRoomType();
        void AddExtraBeds();
        void SoftDeleteRoom();
    }
}
