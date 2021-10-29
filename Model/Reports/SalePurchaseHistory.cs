using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WWT_Inventory.Model.Reports
{
    public class SalePurchaseHistory
    {
        public string ItemCD { get; set; }
        public string ItemName { get; set; }
        public string UnitCD { get; set; }
        public string UnitName { get; set; }
        public int PurQty { get; set; }
        public decimal PurAmount { get; set; }
        public int SaleQty { get; set; }
        public decimal SaleAmount { get; set; }
    }
}
