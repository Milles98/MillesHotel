using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MillesHotelLibrary.Interfaces
{
    public interface IRoomService
    {
        public void AddRoom();
        public void ReadRoom();
        public void UpdateRoom();
        public void RemoveRoom();
    }
}
