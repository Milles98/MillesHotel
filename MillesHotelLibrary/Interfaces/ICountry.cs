using MillesHotelLibrary.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MillesHotelLibrary.Interfaces
{
    public interface ICountry
    {
        [Key]
        public int CountryID { get; set; }

        [StringLength(9)]
        public string CountryName { get; set; }

        public List<Customer> Customers { get; set; }
    }
}
