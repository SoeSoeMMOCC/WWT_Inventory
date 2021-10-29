using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WWT_Inventory.Model.Inventory
{
    public class Stock
    {
        public string StockCD { get; set; }
        public string ItemCD { get; set; }
        public decimal SaleQty { get; set; }
        public decimal PurQty { get; set; }
        public decimal ActualQty { get; set; }
        public string UnitCD { get; set; }
        public decimal AdjQty { get; set; }
        public string isactive { get; set; }
    }
}
