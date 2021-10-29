using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WWT_Inventory.Model.Reports
{
    public class SaleDetailSummary
    {
        public string ItemCD { get; set; }
        public string ItemName { get; set; }
        public string UnitCD { get; set; }
        public string UnitName { get; set; }
        public int SaleQty { get; set; }
        public decimal PurPrice { get; set; }
        public decimal PurAmount { get; set; }
        public decimal SalePrice { get; set; }
        public decimal SaleAmount { get; set; }
        public DateTime InvoiceDate { get; set; }
    }
}
