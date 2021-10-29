using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WWT_Inventory.Model.Inventory;
using WWT_Inventory.Model.Purchase;
using WWT_Inventory.Model.Sale;

namespace WWT_Inventory.Factory
{
    public class CommonFactory
    {
        public static bool isNew = true;
        public static Item updateItem = new Item();
        public static Item selItem = new Item();
        public static PurchaseOrderDetail selPurchaseOrderDetail = new PurchaseOrderDetail();
        public static SaleOrderDetail selSaleOrderDetail = new SaleOrderDetail();
        public static string PurInvoiceNo = "";
        public static string SaleInvoiceNo = "";
        
    }
}
