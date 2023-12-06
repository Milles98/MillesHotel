using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MillesHotel.Entities
{
    public class Room
    {
        private List<Room> _rooms;
        public Room()
        {
            _rooms = new List<Room>();
        }
        // [Key]
        public int RoomID { get; set; }
        public int RoomSize { get; set; }
        public bool RoomType { get; set; }
        public bool ExtraBeds { get; set; }
        //public int InvoiceID (FK)
        //public int BookingID (FK)

    }
}
