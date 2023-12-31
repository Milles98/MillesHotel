using MillesHotelLibrary.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MillesHotelLibrary.Interfaces
{
    public interface ICustomer
    {
        [Key]
        public int CustomerID { get; set; }

        [Required]
        [StringLength(13)]
        public string CustomerFirstName { get; set; }

        [Required]
        [StringLength(13)]
        public string CustomerLastName { get; set; }

        [Range(2, 150)]
        public int CustomerAge { get; set; }

        [Required]
        [StringLength(25)]
        public string CustomerEmail { get; set; }

        [StringLength(15)]
        public string CustomerPhone { get; set; }

        public bool IsActive { get; set; }

        public int CountryID { get; set; }
        public Country Country { get; set; }

        public List<Booking>? Bookings { get; set; }
    }
}
