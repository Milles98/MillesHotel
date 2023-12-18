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
        [StringLength(13)]
        public string CustomerFirstName { get; set; } = string.Empty;

        [Required]
        [StringLength(13)]
        public string CustomerLastName { get; set; } = string.Empty;

        public int CustomerAge { get; set; }

        [Required]
        [StringLength(25)]
        public string CustomerEmail { get; set; } = string.Empty;

        [StringLength(15)]
        public string CustomerPhone { get; set; } = string.Empty;

        [StringLength(9)]
        public string CustomerCountry { get; set; } = string.Empty;

        //En kund kan tas bort endast om det inte finns några bokningar kopplade till kunden.
        public bool IsActive { get; set; } = true;
        //get
        //{
        //    return Bookings?.Any() ?? true;
        //}
        //set
        //{
        //}

        public List<Booking>? Bookings { get; set; }
        public List<Invoice>? Invoices { get; set; }

    }
}
