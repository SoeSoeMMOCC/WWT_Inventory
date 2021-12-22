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

namespace WWT_Inventory.View.Purchase
{
    /// <summary>
    /// Interaction logic for PurchaseSupplierAdd.xaml
    /// </summary>
    public partial class PurchaseSupplierAdd : Window
    {
        InventoryController inventoryController;
        string error;
        List<Supplier> suppliers;
        List<City> cities;
        Supplier selSupplier, newSupplier;
        public PurchaseSupplierAdd()
        {
            InitializeComponent();
            inventoryController = new InventoryController();
            error = "";
            suppliers = new List<Supplier>();
            cities = new List<City>();
            selSupplier = newSupplier = new Supplier();

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            suppliers = inventoryController.getSuppliers("%", out error);
            grdSuplists.ItemsSource = null;
            grdSuplists.ItemsSource = suppliers;
            cities = inventoryController.getCities("%", out error);
            cleardata();
           
        }
        private void cleardata()
        {
            cb_city.ItemsSource = cities;
            cb_city.SelectedValuePath = "CityCD";
            cb_city.DisplayMemberPath = "CityName";
            cb_city.SelectedIndex = 0;

            txt_name.Text = "";
            txt_address.Text = "";
            txt_phone.Text = "";
            txt_email.Text = "";
            txt_name.Focus();
            CommonFactory.isNew = true;
            selSupplier = new Supplier();
            newSupplier = new Supplier();
            btn_save.Content = "SAVE";
        }

        private void txt_name_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                cb_city.Focus();
        }

        private void cb_city_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                txt_address.Focus();
        }

        private void txt_address_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                txt_phone.Focus();
        }

        private void txt_phone_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                txt_email.Focus();
        }

        private void btn_save_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                btn_save_Click(sender, e);
        }

        private void btn_save_Click(object sender, RoutedEventArgs e)
        {
            if (txt_name.Text.ToString().Trim() == "")
            {
                MessageBox.Show("Please Enter Supplier Name.", "Required Supplier Name.", MessageBoxButton.OK, MessageBoxImage.Error);
                txt_name.Focus();
                return;
            }
            if (CommonFactory.isNew)
            {
                string N_CD = inventoryController.generateNoseries(WWT_Inventory.Properties.Settings.Default.DeviceID, "Supplier", out error);
                newSupplier.SupplierCD = N_CD;
                newSupplier.SupplierName = txt_name.Text.ToString().Trim();
                newSupplier.CityCD = cb_city.SelectedValue.ToString();
                newSupplier.Address = txt_address.Text.ToString().Trim();
                newSupplier.PhoneNo = txt_phone.Text.ToString().Trim();
                newSupplier.Email = txt_email.Text.ToString().Trim();
                newSupplier.isactive = true;
                bool save = inventoryController.saveSupplier(newSupplier, out error);
                if (error == "" && save)
                {
                    MessageBox.Show("Supplier Saved.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show(error, "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                    txt_name.Focus();
                }
            }
            else
            {
                selSupplier.SupplierName = txt_name.Text.ToString().Trim();
                selSupplier.CityCD = cb_city.SelectedValue.ToString();
                selSupplier.Address = txt_address.Text.ToString().Trim();
                selSupplier.PhoneNo = txt_phone.Text.ToString().Trim();
                selSupplier.Email = txt_email.Text.ToString().Trim();
                selSupplier.isactive = true;
                bool update = inventoryController.updateSupplier(selSupplier, out error);
                if (error == "" && update)
                {
                    MessageBox.Show("Supplier Updated.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show(error, "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                    txt_name.Focus();
                }
            }
            /* Clearing Data After Update or Save Item Information */
            Window_Loaded(sender, e);
        }

        private void btn_update_Click(object sender, RoutedEventArgs e)
        {
            Supplier obj = ((FrameworkElement)sender).DataContext as Supplier;
            btn_save.Content = "UPDATE";
            if (obj.SupplierCD != null)
            {
                selSupplier = obj;
                CommonFactory.isNew = false;
                txt_name.Text = selSupplier.SupplierName;
                txt_address.Text = selSupplier.Address;
                txt_phone.Text = selSupplier.PhoneNo;
                txt_email.Text = selSupplier.Email;
                txt_name.Focus();
            }
        }

        private void txt_email_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                btn_save.Focus();
        }

        private void btn_delete_Click(object sender, RoutedEventArgs e)
        {
            Supplier obj = ((FrameworkElement)sender).DataContext as Supplier;
            if (obj.SupplierCD != null)
            {
                MessageBoxResult result = MessageBox.Show("Are you sure to delete - " + obj.SupplierName, "Are you sure?", MessageBoxButton.YesNo, MessageBoxImage.Information);
                switch (result)
                {
                    case MessageBoxResult.Yes:
                        bool ret = inventoryController.deleteSupplier(obj, out error);
                        MessageBox.Show("Supplier Deleted.", "Deleted", MessageBoxButton.OK, MessageBoxImage.Information);
                        break;
                    case MessageBoxResult.No:
                        break;
                }
                Window_Loaded(sender, e);
            }
            //Do whatever you wanted to do with MyObject.ID
        }
    }
}
