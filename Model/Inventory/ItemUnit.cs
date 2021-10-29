using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WWT_Inventory.Model.Inventory
{
    public class ItemUnit
    {
        public string ItemCD { get; set; }
        public string ItemName { get; set; }
        public string UnitName { get; set; }
        public string UnitCD { get; set; }
        public int BaseQty { get; set; }
        public decimal SalePrice { get; set; }
        public decimal PurPrice { get; set; }
        public bool isBase { get; set; }
        public bool isactive { get; set; }
    }
}
