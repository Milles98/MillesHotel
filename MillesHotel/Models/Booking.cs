using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MillesHotel.Entities
{
    public class Booking
    {
        [Key]
        public int BookingID { get; set; }

        [Required]
        public DateTime BookingDate { get; set; }

        // Foreign key för att koppla till Customer
        public int CustomerID { get; set; }
        public Customer Customer { get; set; }

        // Foreign key för att koppla till Room
        public int RoomID { get; set; }
        public Room Room { get; set; }

    }
}
