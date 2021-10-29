using ExcelDataReader;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using WWT_Inventory.Controller.Inventory;
using WWT_Inventory.Model.Inventory;

namespace WWT_Inventory.View.Inventory
{
    /// <summary>
    /// Interaction logic for InventoryItemImportView.xaml
    /// </summary>
    public partial class InventoryItemImportView : Window
    {
        DataTableCollection tableCollection;
        InventoryController inventoryController;
        List<Supplier> suppliers;
        List<Unit> units;
        List<Item> items;
        List<Item> alreadyItems;
        List<Category> categories;
        List<SubCategory> subCategories;
        string error;

        public InventoryItemImportView()
        {
            InitializeComponent();
            inventoryController = new InventoryController();
            suppliers = new List<Supplier>();
            units = new List<Unit>();
            items = new List<Item>();
            categories = new List<Category>();
            subCategories = new List<SubCategory>();
            error = "";
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            suppliers = inventoryController.getSuppliers("%", out error);
            units = inventoryController.getUnits("%", out error);
            items = inventoryController.getInventoryItemList("%", "%", out error);
            categories = inventoryController.getCategories("%", out error);
            subCategories = inventoryController.getSubCategories("%", "%", out error);
            btn_import.IsEnabled = false;
        }
        private void btn_browse_Click(object sender, RoutedEventArgs e)
        {
            var defaultValues = ConfigurationManager.GetSection("DefaultValues") as NameValueCollection;
            alreadyItems = new List<Item>();
            items = new List<Item>();
            using (OpenFileDialog openFileDialog = new OpenFileDialog() { Filter = "Excel Workbook|*.xlsx|Excel 97-2003 Workbook|*.xls" })
            {
                if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    txt_fileName.Text = openFileDialog.FileName;
                    try
                    {
                        var stream = File.Open(openFileDialog.FileName, FileMode.Open, FileAccess.Read);
                        using (stream)
                        {
                            using (IExcelDataReader reader = ExcelReaderFactory.CreateReader(stream))
                            {
                                DataSet result = reader.AsDataSet(new ExcelDataSetConfiguration()
                                {
                                    ConfigureDataTable = (_) => new ExcelDataTableConfiguration() { UseHeaderRow = true }
                                });
                                tableCollection = result.Tables;
                                //int s_index = 0;
                                //int f_index = 0;
                                foreach (DataTable table in tableCollection)
                                {
                                    foreach (DataRow dr in table.Rows)
                                    {
                                        Item item = new Item();
                                        item.ItemName = dr.Field<String>("ItemName");
                                        item.ShortCode = Convert.ToString(dr.Field<Double>("ShortCode"));
                                        Unit unit = units.Find(x => x.UnitName == dr.Field<String>("Unit"));    
                                        item.UnitCD = (unit.UnitCD==null)? defaultValues.Get("UnitCD"):unit.UnitCD;
                                        item.UnitName = (unit.UnitCD == null) ? defaultValues.Get("UnitName") : unit.UnitName;

                                        Category cat = categories.Find(x => x.CategoryName == dr.Field<String>("Category"));
                                        item.CategoryCD = (cat.CategoryCD==null)? defaultValues.Get("CategoryCD"):cat.CategoryCD;
                                        item.CategoryName = (cat.CategoryCD==null)? defaultValues.Get("CategoryName"):cat.CategoryName;

                                        SubCategory subcat = subCategories.Find(x => x.SubCategoryName == dr.Field<String>("SubCategory"));
                                        item.SubCategoryCD = (subcat.SubCategoryCD==null)? defaultValues.Get("SubCategoryCD"):subcat.SubCategoryCD;
                                        item.SubCategoryName = (subcat.SubCategoryCD==null)? defaultValues.Get("SubCategoryName"):subcat.SubCategoryName;

                                        Supplier sup = suppliers.Find(x => x.SupplierName == dr.Field<String>("Supplier"));
                                        item.SupplierCD = (sup.SupplierCD==null)? defaultValues.Get("SupplierCD") :sup.SupplierCD;
                                        item.SupplierName = (sup.SupplierCD==null)? defaultValues.Get("SupplierName") :sup.SupplierName;


                                        item.MinQty = Convert.ToInt32(dr.Field<Double>("MaxQty"));
                                        item.MaxQty = Convert.ToInt32(dr.Field<Double>("MinQty"));
                                        item.PurPrice = Convert.ToDecimal(dr.Field<Double>("PurPrice"));
                                        item.SalePrice = Convert.ToDecimal(dr.Field<Double>("SalePrice"));
                                        item.isactive = true;
                                        List<Item> itemList = inventoryController.checkItemInfo(item.ItemName, item.ShortCode, out error);
                                        if (itemList.Count() > 0)
                                            /* Item Information Already Exists */
                                            alreadyItems.Add(item);
                                        else
                                            /* Add New Item Information */
                                            items.Add(item);
                                    }
                                }
                                grdItemtbl.ItemsSource = null;
                                grdItemtbl.ItemsSource = items;
                                btn_import.IsEnabled = true;
                                /* Messaging About Already Exists items */
                                if (alreadyItems.Count() > 0)
                                {
                                    string msg = "System, Already have the following Items.\n";
                                    foreach (Item item in alreadyItems)
                                    {
                                        msg += "Item Name - " + item.ItemName + " / Short Code - " + item.ShortCode + ".\n";
                                    }
                                    System.Windows.MessageBox.Show(msg);
                                }

                            }
                        }

                    }
                    catch (Exception ex)
                    {
                        System.Windows.MessageBox.Show(ex.Message, "Error To Open", MessageBoxButton.OK, MessageBoxImage.Error);
                        txt_fileName.Focus();
                        return;
                    }

                }
            }
        }

        private void btn_import_Click(object sender, RoutedEventArgs e)
        {
            alreadyItems = new List<Item>();
            if (items.Count() > 0)
            {
                foreach(Item item in items)
                {
                    string N_ItemCD = inventoryController.generateNoseries(WWT_Inventory.Properties.Settings.Default.DeviceID, "Item", out error);
                    item.ItemCD = N_ItemCD;
                    bool ret = inventoryController.saveItem(item, out error); ;
                    if (!ret)
                        alreadyItems.Add(item);
                }
            }
            else
            {
                System.Windows.MessageBox.Show("No, Data to Import.", "Try Again.", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            if (alreadyItems.Count() > 0)
            {
                grdItemtbl.ItemsSource = null;
                grdItemtbl.ItemsSource = alreadyItems;
                System.Windows.MessageBox.Show("Errors, to save following items.", "Sorry.", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            else
            {
                grdItemtbl.ItemsSource = null;
                System.Windows.MessageBox.Show("Succss!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                txt_fileName.Text = "";
                Window_Loaded(sender, e);
            }
        }

        private void btn_close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        
    }
}
