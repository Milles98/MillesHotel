using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MillesHotel.Entities
{
    public class Customer
    {
        private List<Customer> _customer;

        // [Key]
        public int CustomerID { get; set; }
        public string CustomerName { get; set; }
        public int Age { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Country { get; set; }

        public Customer()
        {
            _customer = new List<Customer>();
        }
    }
}
