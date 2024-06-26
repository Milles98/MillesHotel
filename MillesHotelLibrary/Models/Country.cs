﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using MillesHotelLibrary.Interfaces;

namespace MillesHotelLibrary.Models
{
    public class Country : ICountry
    {
        [Key]
        public int CountryID { get; set; }

        [StringLength(9)]
        public string CountryName { get; set; }

        public List<Customer> Customers { get; set; } = new List<Customer>();
    }
}
