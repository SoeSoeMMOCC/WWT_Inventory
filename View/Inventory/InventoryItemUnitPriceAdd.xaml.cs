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
using WWT_Inventory.Factory;
using WWT_Inventory.Model.Inventory;

namespace WWT_Inventory.View.Inventory
{
    /// <summary>
    /// Interaction logic for InventoryItemUnitPriceAdd.xaml
    /// </summary>
    public partial class InventoryItemUnitPriceAdd : Window
    {
        InventoryController inventoryController;
        string error = "";
        ItemUnit itemUnit;
        List<ItemUnit> itemUnits;
        List<Unit> units;
        List<Item> items;
        ItemUnit oldItemUnit;
        public InventoryItemUnitPriceAdd()
        {
            InitializeComponent();
            inventoryController = new InventoryController();
            error = "";
            itemUnit = new ItemUnit();
            units = new List<Unit>();
            itemUnits = new List<ItemUnit>();
            items = new List<Item>();
            oldItemUnit = new ItemUnit();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            itemUnits = inventoryController.getItemUnits(CommonFactory.selItem.ItemCD, "%",out error);
            units = inventoryController.getUnits("%", out error);
            items = inventoryController.getInventoryItemList("%", "%", out error);
            if (CommonFactory.selItem.ItemCD != null)
            {
                grdUnitPriceList.ItemsSource = itemUnits;
                txt_itemcd.Text = CommonFactory.selItem.ItemCD;
                txt_itemcd.IsEnabled = false;
                txt_itemname.Text = CommonFactory.selItem.ItemName;
                txt_itemname.IsEnabled = false;
                cb_unit.ItemsSource = units;
                cb_unit.SelectedValuePath = "UnitCD";
                cb_unit.DisplayMemberPath = "UnitName";
                cb_unit.SelectedIndex = 0;
                CommonFactory.isNew = true;
                txt_qty.Text = "1";
                txt_qty.IsEnabled = true;
                txt_saleprice.Text = "0";
                txt_purprice.Text = "0";
                btn_save.Content = "SAVE";
                cb_unit.Focus();
            }
        }

        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            ItemUnit obj = ((FrameworkElement)sender).DataContext as ItemUnit;
            btn_save.Content = "UPDATE";
            if (obj!=null)
            {
                oldItemUnit = new ItemUnit();
                oldItemUnit = obj;
                itemUnit = obj;
                CommonFactory.isNew = false;
                cb_unit.SelectedIndex = units.FindIndex(x => x.UnitCD == obj.UnitCD);
                /* Checking Base Unit or Not */
                if (items.Find(x => x.ItemCD == obj.ItemCD && x.UnitCD == obj.UnitCD) != null)
                {
                    txt_qty.IsEnabled = false;
                }
                /*End Checking Base Unit or Not */
                txt_qty.Text = obj.BaseQty.ToString();
                txt_saleprice.Text = obj.SalePrice.ToString();
                txt_purprice.Text = obj.PurPrice.ToString();
                cb_unit.Focus();
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            ItemUnit obj = ((FrameworkElement)sender).DataContext as ItemUnit;
            /* Checking Base Unit or Not */
            if(items.Find(x=>x.ItemCD==obj.ItemCD && x.UnitCD == obj.UnitCD) != null)
            {
                MessageBox.Show("ItemUnit Can't Delete.", "Already Item Base Unit.", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            /*End Checking Base Unit or Not */
            if (obj.ItemCD != null && obj.UnitCD!=null)
            {
                MessageBoxResult result = MessageBox.Show("Are you sure to delete ?", "Are you sure?", MessageBoxButton.YesNo, MessageBoxImage.Information);
                switch (result)
                {
                    case MessageBoxResult.Yes:
                        bool ret = inventoryController.deleteItemUnit(obj, out error);
                        MessageBox.Show("ItemUnit Deleted.", "Deleted", MessageBoxButton.OK, MessageBoxImage.Information);
                        break;
                    case MessageBoxResult.No:
                        break;
                }
                Window_Loaded(sender, e);
            }
            //Do whatever you wanted to do with MyObject.ID
        }

        private void txt_itemcd_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                txt_itemname.Focus();
        }

        private void txt_itemname_KeyDown(object sender, KeyEventArgs e)
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
                txt_saleprice.Focus();
        }

        private void txt_saleprice_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                btn_save.Focus();
        }

        private void btn_save_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                btn_save_Click(sender, e);
        }

        private void btn_save_Click(object sender, RoutedEventArgs e)
        {
            ItemUnit newItemUnit = new ItemUnit();
            newItemUnit.ItemCD = txt_itemcd.Text.ToString();
            newItemUnit.UnitCD = cb_unit.SelectedValue.ToString();
            newItemUnit.BaseQty = Int32.TryParse(txt_qty.Text.ToString(), out int i) ? Convert.ToInt32(txt_qty.Text.ToString()) : 1;
            newItemUnit.PurPrice = Decimal.TryParse(txt_purprice.Text.ToString(), out Decimal d) ? Convert.ToDecimal(txt_purprice.Text.ToString()) : 0;
            newItemUnit.SalePrice = Decimal.TryParse(txt_saleprice.Text.ToString(), out d) ? Convert.ToDecimal(txt_saleprice.Text.ToString()) : 0;
            newItemUnit.isactive = true;
            if ((Int32.TryParse(txt_qty.Text.ToString(), out i) ? Convert.ToInt32(txt_qty.Text.ToString()) : 1) == 1)
            {
                newItemUnit.isBase = true;
            }
            else
            {
                newItemUnit.isBase = false;
            }
            if (CommonFactory.isNew)
            {
                /* Checking Already Exists or Not */
                if(itemUnits.Find(x=>x.ItemCD== newItemUnit.ItemCD && x.UnitCD== newItemUnit.UnitCD) != null)
                {
                    MessageBox.Show("Item Unit Already Defined.", "Already Exists.", MessageBoxButton.OK, MessageBoxImage.Information);
                    Window_Loaded(sender, e);
                    return;
                }
                /* End Checking Already Exists or Not */

                /* Checking Second or Third ItemUnit Qty It cann't be 1 need to More than 1*/
                if ((Int32.TryParse(txt_qty.Text.ToString(), out i) ? Convert.ToInt32(txt_qty.Text.ToString()) : 1) == 1)
                {
                    MessageBox.Show("Second or Third Unit Base Qty Cannt' Insert 1.", "Already Defined.", MessageBoxButton.OK, MessageBoxImage.Information);
                    Window_Loaded(sender, e);
                    return;
                }
                /*End Checking Second or Third ItemUnit Qty It cann't be 1 need to More than 1*/

                bool save = inventoryController.saveItemUnit(newItemUnit, out error);
                if (error == "" && save)
                {
                    MessageBox.Show("ItemUnit Saved.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show(error, "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                    cb_unit.Focus();
                }
            }
            else
            {
                itemUnits = itemUnits.FindAll(x => x.ItemCD == itemUnit.ItemCD && x.UnitCD != itemUnit.UnitCD);
                if (itemUnits.Find(x => x.ItemCD == newItemUnit.ItemCD && x.UnitCD == newItemUnit.UnitCD) != null)
                {
                    MessageBox.Show("Item Unit Already Defined.", "Already Exists.", MessageBoxButton.OK, MessageBoxImage.Information);
                    Window_Loaded(sender, e);
                    return;
                }
                bool update = inventoryController.updateItemUnit(itemUnit,newItemUnit, out error);
                if (error == "" && update)
                {
                    MessageBox.Show("Category Updated.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show(error, "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                    cb_unit.Focus();
                }
            }
            /* Clearing Data After Update or Save Item Information */
            Window_Loaded(sender, e);
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
