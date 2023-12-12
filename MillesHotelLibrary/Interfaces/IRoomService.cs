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
        void UpdateRoom();
        void SoftDeleteRoom();
    }
}
