using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WWT_Inventory.Model.Purchase
{
    public class PurchaseOrderDetail
    {
        public string PurDetailCD { get; set; }
        public string PurOrderCD { get; set; }
        public string ItemCD { get; set; }
        public string ItemName { get; set; }
        public string UnitCD { get; set; }
        public string UnitName { get; set; }
        public int Count { get; set; }
        public string ShortCode { get; set; }
        public int Qty { get; set; }
        public decimal Price { get; set; }
        public decimal Amount { get; set; }
        public bool isactive { get; set; }
        public decimal DiscountPer { get; set; }
        public decimal DiscountAmt { get; set; }
        public decimal TotalAmount { get; set; }
    }
}
