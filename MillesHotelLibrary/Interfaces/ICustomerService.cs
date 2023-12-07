using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MillesHotelLibrary.Interfaces
{
    public interface ICustomerService
    {
        public void AddCustomer();
        public void ReadCustomer();
        public void UpdateCustomer();
        public void RemoveCustomer();
    }
}
