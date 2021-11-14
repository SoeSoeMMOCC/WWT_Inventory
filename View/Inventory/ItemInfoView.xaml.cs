using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using WWT_Inventory.Controller.Inventory;
using WWT_Inventory.Factory;
using WWT_Inventory.Model.Inventory;

namespace WWT_Inventory.View.Inventory
{
    /// <summary>
    /// Interaction logic for ItemInfoView.xaml
    /// </summary>
    public partial class ItemInfoView : UserControl
    {
        InventoryController inventoryController;
        string error;
        List<Item> items = new List<Item>();
        public ItemInfoView()
        {
            InitializeComponent();
            inventoryController = new InventoryController();
            error = "";
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            items = inventoryController.getInventoryItemList("%", "%", out error);
            grdItemLists.ItemsSource = items;
        }
        //private void EventSetter_OnHandler(object sender, MouseButtonEventArgs e)
        //{
            

        //}

        private void grdItemLists_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Item selItem = (Item) grdItemLists.SelectedItem;
            if (selItem!= null)
            {
                CommonFactory.selItem = selItem;
            }

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Item obj = ((FrameworkElement)sender).DataContext as Item;
            if (obj.ItemCD != null)
            {
              MessageBoxResult result =  MessageBox.Show("Are you sure to delete - " + obj.ItemCD, "Are you sure?", MessageBoxButton.YesNo, MessageBoxImage.Information);
                switch (result) {
                    case MessageBoxResult.Yes:
                        bool ret = inventoryController.deleteItem(obj,out error);
                        MessageBox.Show("Item Deleted.","Deleted",MessageBoxButton.OK,MessageBoxImage.Information);
                        break;
                    case MessageBoxResult.No:
                        break;
                }
                UserControl_Loaded(sender, e);
            }
            //Do whatever you wanted to do with MyObject.ID
        }

        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            Item obj = ((FrameworkElement)sender).DataContext as Item;
            if (obj.ItemCD != null)
            {
                CommonFactory.isNew = false;
                CommonFactory.updateItem = obj;
                InventoryItemAdd itemAdd = new InventoryItemAdd();
                itemAdd.ShowDialog();
            }
            UserControl_Loaded(sender, e);
        }

        private void btn_add_Click(object sender, RoutedEventArgs e)
        {
            CommonFactory.isNew = true;
            InventoryItemAdd itemAdd = new InventoryItemAdd();
            itemAdd.ShowDialog();
            UserControl_Loaded(sender, e);
        }

        private void btn_add_category_Click(object sender, RoutedEventArgs e)
        {
            InventoryCategoryAdd categoryAdd = new InventoryCategoryAdd();
            categoryAdd.ShowDialog();
            UserControl_Loaded(sender, e);
        }

        private void btn_add_subcategory_Click(object sender, RoutedEventArgs e)
        {
            InventorySubCategoryAdd subCategoryAdd = new InventorySubCategoryAdd();
            subCategoryAdd.ShowDialog();
            UserControl_Loaded(sender, e);
        }

        private void btn_unit_add_Click(object sender, RoutedEventArgs e)
        {
            InventoryUnitAdd unitAdd = new InventoryUnitAdd();
            unitAdd.ShowDialog();
            UserControl_Loaded(sender, e);
        }

        private void btn_unitprice_Click(object sender, RoutedEventArgs e)
        {
            if (CommonFactory.selItem.ItemCD != null)
            {
                InventoryItemUnitPriceAdd unitPriceAdd = new InventoryItemUnitPriceAdd();
                unitPriceAdd.ShowDialog();
            }
            else
            {
                MessageBox.Show("Please select Item.", "No Seleted Item.", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            UserControl_Loaded(sender, e);
        }

        private void grdItemLists_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Item selItem = (Item)grdItemLists.SelectedItem;
            if (selItem != null)
            {
                CommonFactory.selItem = selItem;
            }
        }

        private void grdItemLists_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Item selItem = (Item)grdItemLists.SelectedItem;
            if (selItem != null)
            {
                CommonFactory.selItem = selItem;
            }
        }

        private void btn_search_Click(object sender, RoutedEventArgs e)
        {
            InventoryItemFilter filter = new InventoryItemFilter();
            filter.ShowDialog();
            items = new List<Item>();
            items = inventoryController.getInventoryItemFilterList(filter.selCatCD, 
                filter.selSubCatCD, filter.selItem, filter.selShortCode, out error);
            grdItemLists.ItemsSource = null;
            grdItemLists.ItemsSource = items;
            
        }

        private void btn_import_Click(object sender, RoutedEventArgs e)
        {
            InventoryItemImportView inventoryItemImport = new InventoryItemImportView();
            inventoryItemImport.ShowDialog();
            UserControl_Loaded(sender, e);
        }

        private void btn_export_Click(object sender, RoutedEventArgs e)
        {
            /* Getting the Item export location from Application Settings */
            var exportFolder = ConfigurationManager.GetSection("ExcelExportsPath") as NameValueCollection;
            string fileLocation = exportFolder.Get("itemExportLocation");

            Microsoft.Office.Interop.Excel.Application xcelApp = new Microsoft.Office.Interop.Excel.Application();
            Workbook mWorkBook = xcelApp.Application.Workbooks.Add(Type.Missing);
            int k = 1;
            //Getting the Header of the DataGrid and Exporting into the First Row of the Excel File
            for (int i = 1; i < grdItemLists.Columns.Count + 1; i++)
            {
                if (grdItemLists.Columns[i - 1].Header.ToString() == "ItemCD" || 
                    grdItemLists.Columns[i - 1].Header.ToString() == "ShortCode" || 
                    grdItemLists.Columns[i - 1].Header.ToString() == "Item Name" || 
                    grdItemLists.Columns[i - 1].Header.ToString() == "Unit" || 
                    grdItemLists.Columns[i - 1].Header.ToString() == "Category" || 
                    grdItemLists.Columns[i - 1].Header.ToString() == "SubCategory" || 
                    grdItemLists.Columns[i - 1].Header.ToString() == "Supplier" || 
                    grdItemLists.Columns[i - 1].Header.ToString() == "SalePrice" || 
                    grdItemLists.Columns[i - 1].Header.ToString() == "PurPrice" || 
                    grdItemLists.Columns[i - 1].Header.ToString() == "Qty")
                {
                    xcelApp.Cells[1, k] = grdItemLists.Columns[i - 1].Header;
                    xcelApp.Cells[1, k].Font.Bold = true;
                    k = k + 1;
                }

            }
            //Getting the ItemsSource of the DataGrid and Inserting into the each row of the Excel file
            List<Item> itemlist = (List<Item>)grdItemLists.ItemsSource;
            for (int i = 0; i < itemlist.Count; i++)
            {
                //for(int j = 0; j < grdItemLists.Columns.Count; j++)
                //{
                //DataGridRow row = (DataGridRow)grdItemLists.ItemContainerGenerator
                //.ContainerFromIndex(i);
                xcelApp.Cells[i + 2, 1] = itemlist[i].ItemCD;
                xcelApp.Cells[i + 2, 2] = itemlist[i].ShortCode;
                xcelApp.Cells[i + 2, 3] = itemlist[i].ItemName;
                xcelApp.Cells[i + 2, 4] = itemlist[i].UnitName;
                xcelApp.Cells[i + 2, 5] = itemlist[i].CategoryName;
                xcelApp.Cells[i + 2, 6] = itemlist[i].SubCategoryName;
                xcelApp.Cells[i + 2, 6] = itemlist[i].SupplierName;
                xcelApp.Cells[i + 2, 7] = itemlist[i].SalePrice;
                xcelApp.Cells[i + 2, 8] = itemlist[i].PurPrice;
                xcelApp.Cells[i + 2, 9] = itemlist[i].Qty;
                //}
            }
            xcelApp.Columns.AutoFit();
            xcelApp.Visible = true;
            string fileName = @"\ItemExcelExport_" + DateTime.Now.Month.ToString() + "_" + DateTime.Now.Day.ToString() + "_" + DateTime.Now.Year.ToString() + "_" + DateTime.Now.Hour.ToString() + "_" + DateTime.Now.Minute.ToString() + "_" + DateTime.Now.Millisecond.ToString();
            fileLocation = fileLocation + fileName;
            mWorkBook.SaveAs(fileLocation, Type.Missing);
            MessageBox.Show("Export Success", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
