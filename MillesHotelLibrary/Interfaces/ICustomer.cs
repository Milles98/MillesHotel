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
        public string? CustomerFirstName { get; set; }

        [Required]
        public string? CustomerLastName { get; set; }

        public int CustomerAge { get; set; }

        [Required]
        public string? CustomerEmail { get; set; }

        public string? CustomerPhone { get; set; }

        public string? CustomerCountry { get; set; }
        //public bool IsActive { get; set; }

        // Navigationsproperty (om det finns relationer)
        public List<Booking>? Bookings { get; set; }
        public List<Invoice>? Invoices { get; set; }
    }
}
