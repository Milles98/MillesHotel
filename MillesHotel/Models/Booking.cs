using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MillesHotel.Models
{
    public class Booking
    {
        [Key]
        public int BookingID { get; set; }

        [Required]
        public DateTime BookingStartDate { get; set; }
        public DateTime BookingEndDate { get; set; }
        public bool IsActive { get; set; }

        // Foreign key för att koppla till Customer
        public int CustomerID { get; set; }
        public Customer? Customer { get; set; }

        // Foreign key för att koppla till Room
        public int? RoomID { get; set; }
        public Room? Room { get; set; }
        // Foreign key för att koppla till Invoice
        public Invoice? Invoice { get; set; }

    }
}
