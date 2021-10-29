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
    /// Interaction logic for InventorySubCategoryAdd.xaml
    /// </summary>
    public partial class InventorySubCategoryAdd : Window
    {
        InventoryController inventoryController;
        string error;
        List<SubCategory> subCategories;
        List<Category> categories;
        SubCategory subCategory;
        public InventorySubCategoryAdd()
        {
            InitializeComponent();
            inventoryController = new InventoryController();
            error = "";
            subCategory = new SubCategory();
            subCategories = new List<SubCategory>();
            categories = new List<Category>();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            CommonFactory.isNew = true;
            subCategory = new SubCategory();
            txt_cd.Text = "";
            txt_name.Text = "";
            txt_cd.IsEnabled = false;
            btn_save.Content = "SAVE";
            txt_name.Focus();
            categories = inventoryController.getCategories("%", out error);
            cb_category.ItemsSource = categories;
            cb_category.SelectedValuePath = "CategoryCD";
            cb_category.DisplayMemberPath = "CategoryName";
            cb_category.SelectedIndex = 0;
            subCategories = inventoryController.getSubCategories("%", "%", out error);
            grdSubCatLists.ItemsSource = subCategories;
            
        }

        private void txt_cd_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                txt_name.Focus();
        }

        private void txt_name_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                cb_category.Focus();
        }

        private void cb_category_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                btn_save.Focus();
        }

        private void btn_save_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                btn_save_Click(sender, e);
        }

        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            SubCategory obj = ((FrameworkElement)sender).DataContext as SubCategory;
            
            if (obj.SubCategoryCD != null)
            {
                btn_save.Content = "UPDATE";
                CommonFactory.isNew = false;
                subCategory = obj;
                txt_cd.Text = subCategory.SubCategoryCD;
                txt_cd.IsEnabled = false;
                txt_name.Text = subCategory.SubCategoryName;
                cb_category.SelectedIndex = categories.FindIndex(x => x.CategoryCD == subCategory.CategoryCD);
                txt_name.Focus();
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            SubCategory obj = ((FrameworkElement)sender).DataContext as SubCategory;
            if (obj.SubCategoryCD != null)
            {
                MessageBoxResult result = MessageBox.Show("Are you sure to delete -  " + obj.SubCategoryName, "Are you sure?", MessageBoxButton.YesNo, MessageBoxImage.Information);
                switch (result)
                {
                    case MessageBoxResult.Yes:
                        bool ret = inventoryController.deleteSubCategory(obj, out error);
                        MessageBox.Show("SubCategory Deleted.", "Deleted", MessageBoxButton.OK, MessageBoxImage.Information);
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
            subCategory.SubCategoryName = txt_name.Text.ToString().Trim();
            subCategory.CategoryCD = cb_category.SelectedValue.ToString();
            subCategory.isactive = true;
            if (CommonFactory.isNew)
            {
                string N_CD = inventoryController.generateNoseries(WWT_Inventory.Properties.Settings.Default.DeviceID, "SubCategory", out error);
                subCategory.SubCategoryCD = N_CD;
                bool save = inventoryController.saveSubCategory(subCategory, out error);
                if (error == "" && save)
                {
                    MessageBox.Show("SubCategory Saved.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show(error, "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                    txt_name.Focus();
                }
            }
            else
            {
                bool update = inventoryController.updateSubCategoryData(subCategory, out error);
                if (error == "" && update)
                {
                    MessageBox.Show("SubCategory Updated.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
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
