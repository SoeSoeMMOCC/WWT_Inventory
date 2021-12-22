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
    /// Interaction logic for InventoryUnitAdd.xaml
    /// </summary>
    public partial class InventoryUnitAdd : Window
    {
        InventoryController inventoryController;
        string error;
        List<Unit> units;
        Unit unit;
        public InventoryUnitAdd()
        {
            InitializeComponent();
            inventoryController = new InventoryController();
            error = "";
            units = new List<Unit>();
            unit = new Unit();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            txt_cd.IsEnabled = false;
            txt_name.Text = "";
            txt_name.Focus();
            txt_cd.Text = "";
            btn_save.Content = "SAVE";
            CommonFactory.isNew = true;
            unit = new Unit();
            units = inventoryController.getUnits("%", out error);
            grdUnitLists.ItemsSource = units;
        }

        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            Unit obj = ((FrameworkElement)sender).DataContext as Unit;
            btn_save.Content = "UPDATE";
            if (obj.UnitCD != null)
            {
                unit = obj;
                CommonFactory.isNew = false;
                txt_cd.Text = unit.UnitCD;
                txt_cd.IsEnabled = false;
                txt_name.Text = unit.UnitName;
                txt_name.Focus();
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            Unit obj = ((FrameworkElement)sender).DataContext as Unit;
            if (obj.UnitName != null)
            {
                MessageBoxResult result = MessageBox.Show("Are you sure to delete - " + obj.UnitName+" ? ", "Are you sure?", MessageBoxButton.YesNo, MessageBoxImage.Information);
                switch (result)
                {
                    case MessageBoxResult.Yes:
                        bool ret = inventoryController.deleteUnit(obj, out error);
                        MessageBox.Show("Unit Deleted.", "Deleted", MessageBoxButton.OK, MessageBoxImage.Information);
                        break;
                    case MessageBoxResult.No:
                        break;
                }
                Window_Loaded(sender, e);
            }
            //Do whatever you wanted to do with MyObject.ID
        }

        private void txt_cd_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                txt_name.Focus();
        }

        private void txt_name_KeyDown(object sender, KeyEventArgs e)
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
            if (txt_name.Text.ToString().Trim() == "")
            {
                MessageBox.Show("Please Enter Unit Name.", "Required Unit Name.", MessageBoxButton.OK, MessageBoxImage.Error);
                txt_name.Focus();
                return;
            }
            unit.UnitName = txt_name.Text.ToString().Trim();
            unit.isactive = true;
            if (CommonFactory.isNew)
            {
                string N_CD = inventoryController.generateNoseries(WWT_Inventory.Properties.Settings.Default.DeviceID, "Unit", out error);
                unit.UnitCD = N_CD;
                bool save = inventoryController.saveUnit(unit, out error);
                if (error == "" && save)
                {
                    MessageBox.Show("Unit Saved.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show(error, "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                    txt_name.Focus();
                }
            }
            else
            {
                bool update = inventoryController.updateUnit(unit, out error);
                if (error == "" && update)
                {
                    MessageBox.Show("Unit Updated.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
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
