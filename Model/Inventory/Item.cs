using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WWT_Inventory.Model.Inventory
{
    public class Item
    {
        public string ItemCD { get; set; }
        public string ItemName { get; set; }
        public string UnitCD { get; set; }
        public decimal SalePrice { get; set; }
        public decimal PurPrice { get; set; }
        public int MinQty { get; set; }
        public int MaxQty { get; set; }
        public int Qty { get; set; }
        public bool isactive { get; set; }
        public string CategoryCD { get; set; }
        public string SubCategoryCD { get; set; }
        public string SupplierCD { get; set; }
        public string SupplierName { get; set; }
        public string UnitName { get; set; }
        public string CategoryName { get; set; }
        public string SubCategoryName { get; set; }
        public string ShortCode { get; set; }
    }
}
