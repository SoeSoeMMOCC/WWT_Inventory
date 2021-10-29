using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WWT_Inventory.Model.Sale
{
    public class SaleInvoiceDetail
    {
        public string SaleDetailCD { get; set; }
        public string SaleInvoiceCD { get; set; }
        public string ItemCD { get; set; }
        public string ItemName { get; set; }
        public string UnitCD { get; set; }
        public string UnitName { get; set; }
        public int Qty { get; set; }
        public decimal Price { get; set; }
        public decimal PurPrice { get; set; }
        public decimal DiscountPer { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal Amount { get; set; }
        public decimal TotalAmount { get; set; }
        public bool isactive { get; set; }
        public string SaleTypeCD { get; set; }
        public string SaleType { get; set; }
    }
}
