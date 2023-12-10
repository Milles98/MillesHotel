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
        public int CustomerID { get; set; }

        public string CustomerFirstName { get; set; }

        public string CustomerLastName { get; set; }

        public int CustomerAge { get; set; }

        public string CustomerEmail { get; set; }

        public string CustomerPhone { get; set; }

        public string CustomerCountry { get; set; }
        public bool IsActive { get; set; }
    }
}
