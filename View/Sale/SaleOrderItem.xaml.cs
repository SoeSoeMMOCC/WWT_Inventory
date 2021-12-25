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
using WWT_Inventory.Controller.Sale;
using WWT_Inventory.Factory;
using WWT_Inventory.Model.Inventory;
using WWT_Inventory.Model.Sale;

namespace WWT_Inventory.View.Sale
{
    /// <summary>
    /// Interaction logic for SaleOrderItem.xaml
    /// </summary>
    public partial class SaleOrderItem : Window
    {
        SaleController saleController;
        InventoryController inventoryController;
        string error;
        List<ItemUnit> itemUnits;
        List<SaleTypes> saleTypes;
        public static bool status;
        public SaleOrderItem()
        {
            InitializeComponent();
            saleController = new SaleController();
            inventoryController = new InventoryController();
            error = "";
            itemUnits = new List<ItemUnit>();
            saleTypes = new List<SaleTypes>();
            status = true;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            itemUnits = inventoryController.getItemUnits(CommonFactory.selSaleOrderDetail.ItemCD, "%", out error);
            txt_name.Text = CommonFactory.selSaleOrderDetail.ItemName;
            txt_name.IsEnabled = false;
            txt_shortcode.Text = CommonFactory.selSaleOrderDetail.ShortCode;
            txt_shortcode.IsEnabled = false;

            cb_unit.ItemsSource = itemUnits;
            cb_unit.SelectedValuePath = "UnitCD";
            cb_unit.DisplayMemberPath = "UnitName";
            cb_unit.SelectedIndex = itemUnits.FindIndex(x => x.UnitCD == CommonFactory.selSaleOrderDetail.UnitCD);
            CommonFactory.selSaleOrderDetail.PurPrice = itemUnits.Find(x => x.UnitCD == CommonFactory.selSaleOrderDetail.UnitCD).PurPrice;

            txt_qty.Text = CommonFactory.selSaleOrderDetail.Qty.ToString();
            txt_qty.Focus();
            txt_qty.SelectAll();
            txt_saleprice.Text = CommonFactory.selSaleOrderDetail.Price.ToString();
            cb_unit.Focus();

            saleTypes = saleController.GetSaleTypes("%", out error);

            cb_saletype.ItemsSource = saleTypes;
            cb_saletype.SelectedValuePath = "SaleTypeCD";
            cb_saletype.DisplayMemberPath = "SaleType";
            cb_saletype.SelectedIndex = saleTypes.FindIndex(x => x.SaleTypeCD == CommonFactory.selSaleOrderDetail.SaleTypeCD);
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
            {
                txt_qty.Focus();
                txt_qty.SelectAll();
            }
                
        }

        private void cb_unit_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cb_unit.SelectedValue != null || cb_unit.SelectedValue.ToString() != null)
            {
                ItemUnit item = itemUnits.Find(x => x.UnitCD == cb_unit.SelectedValue.ToString() && x.ItemCD == CommonFactory.selSaleOrderDetail.ItemCD);
                if (item != null)
                {
                    txt_saleprice.Text = item.SalePrice.ToString();
                    CommonFactory.selSaleOrderDetail.PurPrice = item.PurPrice;
                }
            }
        }

        private void txt_qty_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                txt_saleprice.Focus();
                txt_saleprice.SelectAll();
            }
        }


        private void cb_saletype_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                btn_update.Focus();
        }

        private void btn_update_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                btn_update_Click(sender, e);
            else if (e.Key == Key.Right)
                btn_cancel.Focus();
        }

        private void btn_update_Click(object sender, RoutedEventArgs e)
        {
            if (cb_unit.SelectedValue == null ||
                txt_qty.Text.ToString().Trim() == "" || txt_qty.Text.ToString().Trim() == null || txt_qty.Text.ToString().Trim() == "0" ||
                txt_saleprice.Text.ToString().Trim() == "" || txt_saleprice.Text.ToString().Trim() == null || txt_qty.Text.ToString().Trim() == "0" ||
                cb_saletype.SelectedValue == null)
            {
                MessageBox.Show("Invalid Input!", "Invalid", MessageBoxButton.OK, MessageBoxImage.Error);
                cb_unit.Focus();
                return;
            }
            else
            {
                CommonFactory.selSaleOrderDetail.UnitCD = cb_unit.SelectedValue.ToString();
                CommonFactory.selSaleOrderDetail.SaleTypeCD = cb_saletype.SelectedValue.ToString();
                CommonFactory.selSaleOrderDetail.UnitName = cb_unit.Text.ToString();
                CommonFactory.selSaleOrderDetail.SaleType = cb_saletype.Text.ToString();
                CommonFactory.selSaleOrderDetail.Qty = Convert.ToInt32(txt_qty.Text.ToString());
                if (cb_saletype.Text.ToString() == "FOC")
                {
                    CommonFactory.selSaleOrderDetail.Price = 0;
                    CommonFactory.selSaleOrderDetail.Amount = 0;
                }
                else
                {
                    CommonFactory.selSaleOrderDetail.Price = Convert.ToDecimal(txt_saleprice.Text.ToString());
                    CommonFactory.selSaleOrderDetail.Amount = Convert.ToInt32(txt_qty.Text.ToString()) * Convert.ToDecimal(txt_saleprice.Text.ToString());
                }                
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

        private void txt_saleprice_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                cb_saletype.Focus();
        }
        private void PreviewIntegerInput(object sender, TextCompositionEventArgs e)
        {
            //Regex regex = new Regex("[^0-9]+");
            //e.Handled = regex.IsMatch(e.Text);
            CommonFactory.InputIntegerCheck(sender, e);
        }
        private void PreviewDecimalInput(object sender, TextCompositionEventArgs e)
        {
            //Regex regex = new Regex("^[.][0-9]+$|^[0-9]*[.]{0,1}[0-9]*$");
            //e.Handled = !regex.IsMatch((sender as TextBox).Text.Insert((sender as TextBox).SelectionStart, e.Text));
            CommonFactory.InputDecimalCheck(sender, e);
        }
    }
}
