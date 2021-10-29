using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using WWT_Inventory.Controller.Inventory;
using WWT_Inventory.Controller.Purchase;
using WWT_Inventory.Factory;
using WWT_Inventory.Model.Inventory;

namespace WWT_Inventory.View.Purchase
{
    /// <summary>
    /// Interaction logic for PurchaseOrderItem.xaml
    /// </summary>
    public partial class PurchaseOrderItem : Window
    {
        InventoryController inventoryController;
        PurchaseController purchaseController;
        List<ItemUnit> itemUnits;
        string error;
        public static bool status;
        public PurchaseOrderItem()
        {
            InitializeComponent();
            purchaseController = new PurchaseController();
            inventoryController = new InventoryController();
            itemUnits = new List<ItemUnit>();
            status = true;
            error = "";
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            itemUnits = inventoryController.getItemUnits(CommonFactory.selPurchaseOrderDetail.ItemCD, "%", out error);
            txt_name.Text = CommonFactory.selPurchaseOrderDetail.ItemName;
            txt_name.IsEnabled = false;
            txt_shortcode.Text = CommonFactory.selPurchaseOrderDetail.ShortCode;
            txt_shortcode.IsEnabled = false;

            cb_unit.ItemsSource = itemUnits;
            cb_unit.SelectedValuePath = "UnitCD";
            cb_unit.DisplayMemberPath = "UnitName";
            cb_unit.SelectedIndex = itemUnits.FindIndex(x => x.UnitCD == CommonFactory.selPurchaseOrderDetail.UnitCD);

            txt_qty.Text = CommonFactory.selPurchaseOrderDetail.Qty.ToString();
            txt_qty.Focus();
            txt_qty.SelectAll();
            txt_purprice.Text = CommonFactory.selPurchaseOrderDetail.Price.ToString();
            cb_unit.Focus();

        }

        private void txt_name_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                txt_shortcode.Focus();
        }

        private void txt_shortcode_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                cb_unit.Focus();
        }

        private void cb_unit_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                txt_qty.Focus();
        }

        private void txt_qty_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                txt_purprice.Focus();
        }

        private void txt_purprice_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                btn_update.Focus();
        }

        private void btn_update_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Right)
                btn_cancel.Focus();
            else if (e.Key == Key.Enter)
                btn_update_Click(sender, e);
        }

        private void btn_update_Click(object sender, RoutedEventArgs e)
        {
            if(cb_unit.SelectedValue==null ||
                txt_qty.Text.ToString().Trim()=="" || txt_qty.Text.ToString().Trim() == null || txt_qty.Text.ToString().Trim() == "0" ||
                txt_purprice.Text.ToString().Trim() == "" || txt_purprice.Text.ToString().Trim() == null || txt_qty.Text.ToString().Trim() == "0")
            {
                MessageBox.Show("Invalid Input!", "Invalid", MessageBoxButton.OK, MessageBoxImage.Error);
                cb_unit.Focus();
                return;
            }
            else
            {
                CommonFactory.selPurchaseOrderDetail.UnitCD = cb_unit.SelectedValue.ToString();
                CommonFactory.selPurchaseOrderDetail.UnitName = cb_unit.Text.ToString();
                CommonFactory.selPurchaseOrderDetail.Qty = Convert.ToInt32(txt_qty.Text.ToString());
                CommonFactory.selPurchaseOrderDetail.Price = Convert.ToDecimal(txt_purprice.Text.ToString());
                CommonFactory.selPurchaseOrderDetail.Amount = Convert.ToInt32(txt_qty.Text.ToString()) * Convert.ToDecimal(txt_purprice.Text.ToString());
                status = true;
                this.Close();
            }
        }

        private void btn_cancel_Click(object sender, RoutedEventArgs e)
        {
            status = false;
            this.Close();
        }

        private void btn_cancel_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                btn_cancel_Click(sender, e);
            else if (e.Key == Key.Left)
                btn_update.Focus();
        }

        private void cb_unit_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(cb_unit.SelectedValue!=null || cb_unit.SelectedValue.ToString() != null)
            {
                ItemUnit item = itemUnits.Find(x => x.UnitCD == cb_unit.SelectedValue.ToString() && x.ItemCD == CommonFactory.selPurchaseOrderDetail.ItemCD);
                if (item != null)
                {
                    txt_purprice.Text = item.PurPrice.ToString();
                }
            }
        }
    }
}
