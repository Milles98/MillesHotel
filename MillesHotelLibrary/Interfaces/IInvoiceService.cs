using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MillesHotelLibrary.Interfaces
{
    public interface IInvoiceService
    {
        public void AddInvoice();
        public void ReadInvoice();
        public void UpdateInvoice();
        public void RemoveInvoice();
    }
}
