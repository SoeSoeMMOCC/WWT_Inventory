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
    /// Interaction logic for InventoryItemAdd.xaml
    /// </summary>
    public partial class InventoryItemAdd : Window
    {
        InventoryController inventoryController;
        List<Unit> units;
        List<Supplier> suppliers;
        List<Category> categories;
        List<SubCategory> subCategories;
        string error;
        public InventoryItemAdd()
        {
            InitializeComponent();
            inventoryController = new InventoryController();
            error = "";
            units = new List<Unit>();
            suppliers = new List<Supplier>();
            categories = new List<Category>();
            subCategories = new List<SubCategory>();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            units = inventoryController.getUnits("%", out error);
            suppliers = inventoryController.getSuppliers("%", out error);
            categories = inventoryController.getCategories("%", out error);
            txt_name.Focus();
            subCategories = inventoryController.getSubCategories("%", "%", out error);
            if (CommonFactory.isNew)
            {
                NewFunction();
            }
            else
            {
                UpdateFunction();
            }
        }
        public void NewFunction()
        {
            btn_delete.Content = "CANCEL";
            btn_save.Content = "SAVE";
            cb_unit.ItemsSource = units;
            cb_unit.SelectedValuePath = "UnitCD";
            cb_unit.DisplayMemberPath = "UnitName";
            cb_unit.SelectedIndex = 0;

            cb_supplier.ItemsSource = suppliers;
            cb_supplier.SelectedValuePath = "SupplierCD";
            cb_supplier.DisplayMemberPath = "SupplierName";
            cb_supplier.SelectedIndex = 0;

            cb_category.ItemsSource = categories;
            int ind = categories.FindIndex(x => x.CategoryCD == "C001");
            cb_category.SelectedValuePath = "CategoryCD";
            cb_category.DisplayMemberPath = "CategoryName";
            cb_category.SelectedIndex = 0;

            //cb_subcategory.ItemsSource = subCategories.Where(x => x.CategoryCD == categories[0].CategoryCD).ToList();
            //cb_subcategory.SelectedValuePath = "SubCategoryCD";
            //cb_subcategory.DisplayMemberPath = "SubCategoryName";
            //cb_subcategory.SelectedIndex = 0;
            txt_name.Text = "";
            txt_maxqty.Text = "0";
            txt_minqty.Text = "0";
            txt_purprice.Text = "0";
            txt_sellingprice.Text = "0";
            txt_shortcode.Text = "";
        }
        void UpdateFunction()
        {
            btn_delete.Content = "CANCEL";
            btn_save.Content = "UPDATE";
            txt_name.Text = CommonFactory.updateItem.ItemName;
            txt_minqty.Text = CommonFactory.updateItem.MinQty.ToString();
            txt_maxqty.Text = CommonFactory.updateItem.MaxQty.ToString();
            txt_purprice.Text = CommonFactory.updateItem.PurPrice.ToString();
            txt_sellingprice.Text = CommonFactory.updateItem.SalePrice.ToString();
            txt_shortcode.Text = CommonFactory.updateItem.ShortCode.ToString();

            cb_unit.ItemsSource = units;
            cb_unit.SelectedValuePath = "UnitCD";
            cb_unit.DisplayMemberPath = "UnitName";
            cb_unit.SelectedIndex = units.FindIndex(x=>x.UnitCD==CommonFactory.updateItem.UnitCD);

            cb_supplier.ItemsSource = suppliers;
            cb_supplier.SelectedValuePath = "SupplierCD";
            cb_supplier.DisplayMemberPath = "SupplierName";
            cb_supplier.SelectedIndex = suppliers.FindIndex(x=>x.SupplierCD==CommonFactory.updateItem.SupplierCD);

            cb_category.ItemsSource = categories;
            cb_category.SelectedValuePath = "CategoryCD";
            cb_category.DisplayMemberPath = "CategoryName";
            cb_category.SelectedIndex = categories.FindIndex(x=>x.CategoryCD==CommonFactory.updateItem.CategoryCD);

            cb_subcategory.SelectedIndex = subCategories.FindIndex(x => x.SubCategoryCD == CommonFactory.updateItem.SubCategoryCD);
        }
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void txt_name_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                txt_shortcode.Focus();
            }
        }

        private void cb_unit_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                btn_save.Focus();
            }
        }

        private void cb_category_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                cb_subcategory.Focus();
            }
        }

        private void cb_category_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            cb_subcategory.ItemsSource = null;
            if (cb_category.SelectedValue != null || cb_category.SelectedValue.ToString() !="")
            {
                subCategories = inventoryController.getSubCategories(cb_category.SelectedValue.ToString(), "%", out error);
                cb_subcategory.ItemsSource = subCategories;
                cb_subcategory.SelectedValuePath = "SubCategoryCD";
                cb_subcategory.DisplayMemberPath = "SubCategoryName";
                cb_subcategory.SelectedIndex = 0;
                //if (CommonFactory.isNew == false)
                //{
                //    cb_subcategory.SelectedIndex = subCategories.FindIndex(x => x.SubCategoryCD == CommonFactory.updateItem.SubCategoryCD);
                //}
                //else
                //{
                //    cb_subcategory.SelectedIndex =0;
                //}
                
            }
        }

        private void cb_subcategory_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                txt_minqty.Focus();
            }
        }

        private void txt_minqty_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                txt_maxqty.Focus();
            }
        }

        private void txt_maxqty_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                txt_purprice.Focus();
            }
        }

        private void txt_purprice_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                txt_sellingprice.Focus();
            }
        }

        private void txt_sellingprice_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                cb_supplier.Focus();
            }
        }

        private void cb_supplier_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                cb_unit.Focus();
            }
        }

        private void btn_save_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                btn_save_Click(sender,e);
            }
            else if(e.Key == Key.Right)
            {
                btn_delete.Focus();
            }
            else if (e.Key == Key.Up)
            {
                cb_supplier.Focus();
            }
        }
        public Item GetInput()
        {
            Item item = new Item();
            item.ItemName = txt_name.Text.ToString();
            item.UnitCD = cb_unit.SelectedValue.ToString();
            item.CategoryCD = cb_category.SelectedValue.ToString();
            item.SubCategoryCD = cb_subcategory.SelectedValue.ToString();
            item.SupplierCD = cb_supplier.SelectedValue.ToString();
            item.MinQty = Convert.ToInt32(txt_minqty.Text.ToString());
            item.MaxQty = Convert.ToInt32(txt_maxqty.Text.ToString());
            item.PurPrice = Convert.ToDecimal(txt_purprice.Text.ToString());
            item.SalePrice = Convert.ToDecimal(txt_sellingprice.Text.ToString());
            item.isactive = true;
            item.ShortCode = txt_shortcode.Text.ToString().Trim();
            return item;
        }
        private void btn_save_Click(object sender, RoutedEventArgs e)
        {
            if((txt_name.Text.ToString()==null || txt_name.Text.ToString()== "") ||
                (txt_shortcode.Text.ToString() == null || txt_shortcode.Text.ToString() == "") ||
                (txt_minqty.Text.ToString() == null || txt_minqty.Text.ToString() == "") ||
                (txt_maxqty.Text.ToString() == null || txt_maxqty.Text.ToString() == "") ||
               (txt_purprice.Text.ToString() == null || txt_purprice.Text.ToString() == "")||
               (txt_sellingprice.Text.ToString() == null || txt_sellingprice.Text.ToString() == "")||
               cb_category.SelectedIndex == -1 || cb_subcategory.SelectedIndex == -1 ||
               cb_unit.SelectedIndex == -1 || cb_supplier.SelectedIndex ==-1
               )
            {
                MessageBox.Show("Please enter valid information, please check your input values.", "Invalid Input.", MessageBoxButton.OK, MessageBoxImage.Error);
                txt_name.Focus();
            }
            else
            {
                Item saveItem = GetInput();
                if (CommonFactory.isNew)
                {
                    string N_ItemCD = inventoryController.generateNoseries(WWT_Inventory.Properties.Settings.Default.DeviceID, "Item", out error);
                    saveItem.ItemCD = N_ItemCD;
                    bool save = inventoryController.saveItem(saveItem, out error);
                    if (error == "" && save)
                    {
                        MessageBox.Show("Item Saved.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show(error, "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                        txt_name.Focus();
                    }
                }
                else
                {
                    saveItem.ItemCD = CommonFactory.updateItem.ItemCD;
                    bool update = inventoryController.updateItemData(saveItem, out error);
                    if (error == "" && update)
                    {
                        MessageBox.Show("Item Updated.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show(error, "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                        txt_name.Focus();
                    }
                }
                /* Clearing Data After Update or Save Item Information */
                CommonFactory.isNew = true;
                CommonFactory.updateItem = new Item();                
            }
        }

        private void btn_delete_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btn_delete_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                btn_delete_Click(sender, e);
            }
            else if (e.Key == Key.Left)
            {
                btn_save.Focus();
            }
            else if (e.Key == Key.Up)
            {
                cb_unit.Focus();
            }
        }

        private void txt_shortcode_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                cb_category.Focus();
            }
        }
    }
}
