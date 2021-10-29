using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WWT_Inventory.Model.Sale
{
    public class SaleOrderHdr
    {
        public string SaleOrderCD { get; set; }
        public DateTime SaleOrderDate { get; set; }
        public string CustomerCD { get; set; }
        public int Qty { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal Tax { get; set; }
        public decimal Amount { get; set; }
        public bool isactive { get; set; }
        public DateTime ts { get; set; }
        public string DeviceID { get; set; }
        public string SaleTypeCD { get; set; }
    }
}
