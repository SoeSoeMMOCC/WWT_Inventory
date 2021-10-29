using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WWT_Inventory.Model.Inventory
{
    public class Supplier
    {
        public string SupplierCD { get; set; }
        public string SupplierName { get; set; }
        public string CityCD { get; set; }
        public string CityName { get; set; }
        public bool isactive { get; set; }
        public string Address { get; set; }
        public string PhoneNo { get; set; }
        public string Email { get; set; }
    }
}
