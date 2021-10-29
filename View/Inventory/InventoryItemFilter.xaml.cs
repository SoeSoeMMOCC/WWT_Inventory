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
using WWT_Inventory.Model.Inventory;

namespace WWT_Inventory.View.Inventory
{
    /// <summary>
    /// Interaction logic for InventoryItemFilter.xaml
    /// </summary>
    public partial class InventoryItemFilter : Window
    {
        InventoryController inventoryController;
        string error;
        List<Category> categories;
        List<SubCategory> subCategories;
        public string selCatCD, selSubCatCD, selItem, selShortCode;
        public InventoryItemFilter()
        {
            InitializeComponent();
            inventoryController = new InventoryController();
            error = "";
            categories = new List<Category>();
            subCategories = new List<SubCategory>();
            selCatCD = selSubCatCD = selItem = selShortCode = "";
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            categories = inventoryController.getCategories("%", out error);
            subCategories = inventoryController.getSubCategories("%", "%", out error);
            Category cat = new Category();
            cat.CategoryCD = "%";
            cat.CategoryName = "[ALL]";
            categories.Insert(0, cat);

            cb_category.ItemsSource = categories;
            cb_category.SelectedValuePath = "CategoryCD";
            cb_category.DisplayMemberPath = "CategoryName";
            cb_category.SelectedIndex = 0;

            txt_name.Text = "";
            txt_shortcode.Text = "";
        }
        private void txt_shortcode_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                cb_category.Focus();
            }
        }

        private void cb_category_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                cb_subcategory.Focus();
            }
        }

        private void btn_search_Click(object sender, RoutedEventArgs e)
        {
            selCatCD = cb_category.SelectedValue.ToString();
            selSubCatCD = cb_subcategory.SelectedValue.ToString();
            selItem = txt_name.Text.ToString().Trim();
            selShortCode = txt_shortcode.Text.ToString().Trim();
            if (selItem == "")
                selItem = "%";
            if (selShortCode == "")
                selShortCode = "%";

            this.Close();
        }

        private void btn_search_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                btn_search_Click(sender, e);
            }
            else if (e.Key == Key.Right)
            {
                btn_cancel.Focus();
            }
        }

        private void cb_category_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(cb_category.SelectedValue != null || cb_category.SelectedValue.ToString() != "")
            {
                subCategories = inventoryController.getSubCategories(cb_category.SelectedValue.ToString(), "%", out error);
                SubCategory subCategory = new SubCategory();
                subCategory.SubCategoryCD = "%";
                subCategory.SubCategoryName = "[ALL]";
                subCategories.Insert(0, subCategory);
                cb_subcategory.ItemsSource = subCategories;
                cb_subcategory.SelectedValuePath = "SubCategoryCD";
                cb_subcategory.DisplayMemberPath = "SubCategoryName";
                cb_subcategory.SelectedIndex = 0;
            }
        }

        private void cb_subcategory_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                btn_search.Focus();
            }
        }

        private void txt_name_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                txt_shortcode.Focus();
            }
        }

        
    }
}
