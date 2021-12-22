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
    /// Interaction logic for InventoryCategoryAdd.xaml
    /// </summary>
    public partial class InventoryCategoryAdd : Window
    {
        InventoryController inventoryController;
        string error;
        List<Category> categories;
        Category category;
        public InventoryCategoryAdd()
        {
            InitializeComponent();
            inventoryController = new InventoryController();
            error = "";
            categories = new List<Category>();
            category = new Category();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            txt_cd.IsEnabled = false;
            txt_name.Text = "";
            txt_name.Focus();
            txt_cd.Text = "";
            btn_save.Content = "SAVE";
            CommonFactory.isNew = true;
            category = new Category();
            categories = inventoryController.getCategories("%", out error);
            grdCatLists.ItemsSource = categories;
        }

        private void grdItemLists_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

        }

        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            Category obj = ((FrameworkElement)sender).DataContext as Category;
            btn_save.Content = "UPDATE";
            if (obj.CategoryCD != null)
            {
                category = obj;
                CommonFactory.isNew = false;
                txt_cd.Text = category.CategoryCD;
                txt_cd.IsEnabled = false;
                txt_name.Text = category.CategoryName;
                txt_name.Focus();
            }
            
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            Category obj = ((FrameworkElement)sender).DataContext as Category;
            if (obj.CategoryCD != null)
            {
                MessageBoxResult result = MessageBox.Show("Are you sure to delete - " + obj.CategoryName, "Are you sure?", MessageBoxButton.YesNo, MessageBoxImage.Information);
                switch (result)
                {
                    case MessageBoxResult.Yes:
                        bool ret = inventoryController.deleteCategory(obj, out error);
                        MessageBox.Show("Category Deleted.", "Deleted", MessageBoxButton.OK, MessageBoxImage.Information);
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
            {
                txt_name.Focus();
            }
            
        }

        private void txt_name_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                btn_save.Focus();
            }
        }

        private void btn_save_Click(object sender, RoutedEventArgs e)
        {
            if (txt_name.Text.ToString().Trim() == "")
            {
                MessageBox.Show("Please Enter Category Name.", "Required Category Name.", MessageBoxButton.OK, MessageBoxImage.Error);
                txt_name.Focus();
                return;
            }

            category.CategoryName = txt_name.Text.ToString().Trim();
            category.isactive = true;
            
            if (CommonFactory.isNew)
            {
                string N_CD = inventoryController.generateNoseries(WWT_Inventory.Properties.Settings.Default.DeviceID, "Category", out error);
                category.CategoryCD = N_CD;
                bool save = inventoryController.saveCategory(category, out error);
                if (error == "" && save)
                {
                    MessageBox.Show("Category Saved.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show(error, "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                    txt_name.Focus();
                }
            }
            else
            {
                bool update = inventoryController.updateCategoryData(category, out error);
                if (error == "" && update)
                {
                    MessageBox.Show("Category Updated.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
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

        private void btn_save_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                btn_save_Click(sender, e);
            }
            
        }
    }
}
