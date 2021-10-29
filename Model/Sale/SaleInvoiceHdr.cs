using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WWT_Inventory.Model.Sale
{
    public class SaleInvoiceHdr
    {
        public string SaleInvoiceCD { get; set; }
        public string SaleOrderCD { get; set; }
        public DateTime SaleInvoiceDate { get; set; }
        public string CustomerCD { get; set; }
        public string CustName { get; set; }
        public int Qty { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal Tax { get; set; }
        public decimal Amount { get; set; }
        public decimal RemainAmount { get; set; }
        public bool isactive { get; set; }
        public string DeviceID { get; set; }
        public string SaleTypeCD { get; set; }
    }
}
