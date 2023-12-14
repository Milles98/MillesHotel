using MillesHotelLibrary.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MillesHotelLibrary.Models
{
    public class Customer : ICustomer
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

        //En kund kan tas bort endast om det inte finns några bokningar kopplade till kunden.
        public bool IsActive
        {
            get
            {
                return Bookings?.All(b => !b.IsActive) ?? true;
            }
            set
            {
            }
        }

        // Navigationsproperty (om det finns relationer)
        public ICollection<Booking>? Bookings { get; set; }
        public ICollection<Invoice>? Invoices { get; set; }

    }
}
