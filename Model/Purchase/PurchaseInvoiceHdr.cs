using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WWT_Inventory.Model.Purchase
{
    public class PurchaseInvoiceHdr
    {
        public string PurInvoiceCD { get; set; }
        public string PurOrderCD { get; set; }
        public string PurInvoiceDate { get; set; }
        public int Qty { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal Tax { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal Amount { get; set; }
        public decimal ReceivedAmount { get; set; }
        public decimal RemainAmount { get; set; }
        public bool isactive { get; set; }
        public string SupplierCD { get; set; }
        public string SupplierName { get; set; }
        public string DeviceID { get; set; }
    }
}
