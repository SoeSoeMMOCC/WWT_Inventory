using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
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
        public static bool isAdmin = true;
        public static void InputIntegerCheck(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
        public static void InputDecimalCheck(object sender,TextCompositionEventArgs e)
        {
            Regex regex = new Regex("^[.][0-9]+$|^[0-9]*[.]{0,1}[0-9]*$");
            e.Handled = !regex.IsMatch((sender as TextBox).Text.Insert((sender as TextBox).SelectionStart, e.Text));
        }
        
    }
}
