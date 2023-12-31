using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace MillesHotelLibrary.Models
{
    public class Country
    {
        [Key]
        public int CountryID { get; set; }

        [StringLength(9)]
        public string CountryName { get; set; }

        public List<Customer> Customers { get; set; } = new List<Customer>();
    }
}
