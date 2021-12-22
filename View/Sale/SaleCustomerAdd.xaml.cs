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
    /// Interaction logic for SaleCustomerAdd.xaml
    /// </summary>
    public partial class SaleCustomerAdd : Window
    {
        SaleController saleController;
        InventoryController inventoryController;
        string error;
        List<Customer> customers;
        List<City> cities;
        Customer selCustomer, newCustomer;
        public SaleCustomerAdd()
        {
            InitializeComponent();
            saleController = new SaleController();
            inventoryController = new InventoryController();
            error = "";
            customers = new List<Customer>();
            cities = new List<City>();
            selCustomer = newCustomer = new Customer();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            customers = saleController.GetCustomers("%", out error);
            grdCustLists.ItemsSource = null;
            grdCustLists.ItemsSource = customers;
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
            txt_name.Focus();
            CommonFactory.isNew = true;
            selCustomer = new Customer();
            newCustomer = new Customer();
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
                btn_save.Focus();
        }

        private void btn_save_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                btn_save_Click(sender, e);
        }

        private void btn_update_Click(object sender, RoutedEventArgs e)
        {
            Customer obj = ((FrameworkElement)sender).DataContext as Customer;
            btn_save.Content = "UPDATE";
            if (obj.CustCD != null)
            {
                selCustomer = obj;
                CommonFactory.isNew = false;
                txt_name.Text = selCustomer.CustName;
                txt_address.Text = selCustomer.Address;
                txt_phone.Text = selCustomer.PhoneNo;
                txt_name.Focus();
            }
        }

        private void btn_delete_Click(object sender, RoutedEventArgs e)
        {
            Customer obj = ((FrameworkElement)sender).DataContext as Customer;
            if (obj.CustCD != null)
            {
                MessageBoxResult result = MessageBox.Show("Are you sure to delete - " + obj.CustName, "Are you sure?", MessageBoxButton.YesNo, MessageBoxImage.Information);
                switch (result)
                {
                    case MessageBoxResult.Yes:
                        obj.isactive = false;
                        bool ret = saleController.updateCustomer(obj, out error);
                        MessageBox.Show("Customer Deleted.", "Deleted", MessageBoxButton.OK, MessageBoxImage.Information);
                        break;
                    case MessageBoxResult.No:
                        break;
                }
                Window_Loaded(sender, e);
            }
            //Do whatever you wanted to do with MyObject.ID
        }

        private void btn_save_Click(object sender, RoutedEventArgs e)
        {
            if (txt_name.Text.ToString().Trim() == "")
            {
                MessageBox.Show("Please Enter Customer Name.", "Required Customer Name.", MessageBoxButton.OK, MessageBoxImage.Error);
                txt_name.Focus();
                return;
            }
            if (CommonFactory.isNew)
            {
                string N_CD = inventoryController.generateNoseries(WWT_Inventory.Properties.Settings.Default.DeviceID, "Customer", out error);
                newCustomer.CustCD = N_CD;
                newCustomer.CustName = txt_name.Text.ToString().Trim();
                newCustomer.CityCD = cb_city.SelectedValue.ToString();
                newCustomer.Address = txt_address.Text.ToString().Trim();
                newCustomer.PhoneNo = txt_phone.Text.ToString().Trim();
                newCustomer.isactive = true;
                bool save = saleController.saveCustomer(newCustomer, out error);
                if (error == "" && save)
                {
                    MessageBox.Show("Customer Saved.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show(error, "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                    txt_name.Focus();
                }
            }
            else
            {
                selCustomer.CustName = txt_name.Text.ToString().Trim();
                selCustomer.CityCD = cb_city.SelectedValue.ToString();
                selCustomer.Address = txt_address.Text.ToString().Trim();
                selCustomer.PhoneNo = txt_phone.Text.ToString().Trim();
                selCustomer.isactive = true;
                bool update = saleController.updateCustomer(selCustomer, out error);
                if (error == "" && update)
                {
                    MessageBox.Show("Customer Updated.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
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
    }
}
