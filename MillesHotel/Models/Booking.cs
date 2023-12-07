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
        private List<Booking> _booking;
        public Booking()
        {
            _booking = new List<Booking>();
        }
        [Key]
        public int BookingID { get; set; }
        public DateTime BookingDate { get; set; }

        List<Customer> customers = new List<Customer>();
        //public int CustomerID (FK)
    }
}
