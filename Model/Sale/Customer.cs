using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WWT_Inventory.Model.Sale
{
    public class Customer
    {
        public string CustCD { get; set; }
        public string CustName { get; set; }
        public string CityCD { get; set; }
        public string CityName { get; set; }
        public string Address { get; set; }
        public string PhoneNo { get; set; }
        public bool isactive { get; set; }
    }
}
