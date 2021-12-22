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
using WWT_Inventory.Controller.Purchase;
using WWT_Inventory.Factory;
using WWT_Inventory.Model.Inventory;
using WWT_Inventory.Model.Purchase;
using WWT_Inventory.View.Purchase.ReportViews;

namespace WWT_Inventory.View.Purchase
{
    /// <summary>
    /// Interaction logic for PurchaseOrderAddView.xaml
    /// </summary>
    public partial class PurchaseOrderAddView : Window
    {
        InventoryController inventoryController;
        PurchaseController purchaseController;
        List<Category> categories;
        List<Item> items;
        List<SubCategory> subCategories;
        Item selItem;
        List<Supplier> suppliers;
        List<PurchaseOrderDetail> orderDetails;
        PurchaseOrderHdr orderHdr;
        string error;
        Decimal total, discount, tax, amount;
        int totalQty = 0;
        List<Stock> stocks;
        List<ItemUnit> itemUnits;
        public PurchaseOrderAddView()
        {
            InitializeComponent();
            inventoryController = new InventoryController();
            purchaseController = new PurchaseController();
            categories = new List<Category>();
            items = new List<Item>();
            subCategories = new List<SubCategory>();
            selItem = new Item();
            suppliers = new List<Supplier>();
            orderDetails = new List<PurchaseOrderDetail>();
            error = "";
            orderHdr = new PurchaseOrderHdr();
            total = discount = tax = amount = 0;
            totalQty = 0;
            stocks = new List<Stock>();
            itemUnits = new List<ItemUnit>();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            items = inventoryController.getInventoryItemList("%", "%", out error);
            categories = inventoryController.getCategories("%", out error);
            subCategories = inventoryController.getSubCategories("%", "%", out error);
            suppliers = inventoryController.getSuppliers("%", out error);

            cb_category.ItemsSource = categories;
            cb_category.SelectedValuePath = "CategoryCD";
            cb_category.DisplayMemberPath = "CategoryName";
            cb_category.SelectedIndex = 0;

            txt_itemcd.Text = "";

            cb_supplier.ItemsSource = suppliers;
            cb_supplier.SelectedValuePath = "SupplierCD";
            cb_supplier.DisplayMemberPath = "SupplierName";
            cb_supplier.SelectedIndex = 0;
            grdItemList.ItemsSource = null;
            cb_category.Focus();
            txt_totalamount.Text = "0";
            txt_totalamount.IsEnabled = false;
            txt_amount.IsEnabled = false;
            txt_discount.Text = "0";
            txt_tax.Text = "0";
            txt_amount.Text = "0";
            total = discount = tax = amount = 0;
            date.SelectedDate = DateTime.Now;
            CommonFactory.PurInvoiceNo = "";

        }

        private void cb_category_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(cb_category.SelectedValue != null || cb_category.SelectedValue.ToString() != "")
            {
                cb_subcategory.ItemsSource = subCategories.FindAll(x => x.CategoryCD == cb_category.SelectedValue.ToString());
                cb_subcategory.SelectedValuePath = "SubCategoryCD";
                cb_subcategory.DisplayMemberPath = "SubCategoryName";
                cb_subcategory.SelectedIndex = 0;
            }
        }

        private void btn_search_Click(object sender, RoutedEventArgs e)
        {
            string cate_cd = (cb_category.SelectedValue == null) ? "" : cb_category.SelectedValue.ToString();
            string sub_cd = (cb_subcategory.SelectedValue == null) ? "" : cb_subcategory.SelectedValue.ToString();
            string item_cd = (txt_itemcd.Text.ToString().Trim() == "") ? "" : txt_itemcd.Text.ToString().Trim();            
            if (item_cd == "")
            {
                grdItemList.ItemsSource = items.FindAll(x => x.CategoryCD == cate_cd && x.SubCategoryCD == sub_cd);
            }
            else
            {
                grdItemList.ItemsSource = items.FindAll(x => x.CategoryCD == cate_cd && x.SubCategoryCD == sub_cd && (x.ItemCD == item_cd || x.ItemName==item_cd));
            }
        }

        private void grdItemList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Item selItem = (Item)grdItemList.SelectedItem;
            if (selItem != null)
            {
                Guid g = Guid.NewGuid();
                string detailID = g.ToString().Replace("-", "");
                PurchaseOrderDetail orderDetail = new PurchaseOrderDetail();
                orderDetail.PurDetailCD = detailID;
                orderDetail.ItemCD = selItem.ItemCD;
                orderDetail.ShortCode = selItem.ShortCode;
                orderDetail.ItemName = selItem.ShortCode+ "( "+selItem.ItemName +" )";
                orderDetail.UnitCD = selItem.UnitCD;
                orderDetail.UnitName = selItem.UnitName;
                orderDetail.Price = selItem.PurPrice;
                orderDetail.Qty = 1;
                orderDetail.Amount = selItem.PurPrice * 1;
                orderDetail.DiscountPer = 0;
                orderDetail.DiscountAmt = 0;
                orderDetail.TotalAmount = selItem.PurPrice * 1;
                orderDetail.Count = orderDetails.Count()+1;
                orderDetail.isactive = true;
                orderDetails.Add(orderDetail);
                grdOrderList.ItemsSource = null;
                grdOrderList.ItemsSource = orderDetails;

                calculateAmount();
            }
        }
        private void calculateAmount()
        {
            total = tax = discount = amount = 0;
            totalQty = 0;
            foreach (PurchaseOrderDetail det in orderDetails)
            {
                total += det.Amount;
                totalQty += 1;
            }
            txt_totalamount.Text = total.ToString();
            tax = Decimal.TryParse(txt_tax.Text.ToString().Trim(), out decimal d) ? d : 0;
            discount = Decimal.TryParse(txt_discount.Text.ToString().Trim(), out d) ? d : 0;
            amount = total - tax - discount;
            txt_amount.Text = amount.ToString();
        }

        private void btn_delete_Click(object sender, RoutedEventArgs e)
        {
            PurchaseOrderDetail obj = ((FrameworkElement)sender).DataContext as PurchaseOrderDetail;
            if (obj != null)
            {
                orderDetails.RemoveAt(orderDetails.FindIndex(x => x.PurDetailCD == obj.PurDetailCD));
                grdOrderList.ItemsSource = null;
                grdOrderList.ItemsSource = orderDetails;
                calculateAmount();
            }
        }

        private void btn_update_Click(object sender, RoutedEventArgs e)
        {
            PurchaseOrderDetail obj = ((FrameworkElement)sender).DataContext as PurchaseOrderDetail;
            if (obj != null)
            {
                CommonFactory.selPurchaseOrderDetail = obj;
                PurchaseOrderItem orderItem = new PurchaseOrderItem();
                orderItem.ShowDialog();
                if (PurchaseOrderItem.status)
                {
                    int index = orderDetails.FindIndex(x => x.PurDetailCD == CommonFactory.selPurchaseOrderDetail.PurDetailCD);
                    orderDetails[index].UnitCD = CommonFactory.selPurchaseOrderDetail.UnitCD;
                    orderDetails[index].UnitName = CommonFactory.selPurchaseOrderDetail.UnitName;
                    orderDetails[index].Qty = CommonFactory.selPurchaseOrderDetail.Qty;
                    orderDetails[index].Price = CommonFactory.selPurchaseOrderDetail.Price;
                    orderDetails[index].Amount = CommonFactory.selPurchaseOrderDetail.Amount;
                    orderDetails[index].TotalAmount = CommonFactory.selPurchaseOrderDetail.Amount;

                    grdOrderList.ItemsSource = null;
                    grdOrderList.ItemsSource = orderDetails;
                    CommonFactory.selPurchaseOrderDetail = new PurchaseOrderDetail();
                    calculateAmount();
                }
            }
        }

        private void cb_category_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key==Key.Enter)
                cb_subcategory.Focus();
        }

        private void cb_subcategory_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                txt_itemcd.Focus();
        }

        private void txt_itemcd_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                btn_search.Focus();
        }

        private void btn_search_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                btn_search_Click(sender, e);
            else if (e.Key == Key.Right)
                btn_save.Focus();

        }

        private void cb_supplier_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                date.Focus();
        }

        private void btn_save_Click(object sender, RoutedEventArgs e)
        {
            if(orderDetails.Count()<=0 || orderDetails == null)
            {
                MessageBox.Show("No Order Information.", "Invalid Order Count.", MessageBoxButton.OK, MessageBoxImage.Error);
                cb_category.Focus();
                return;
            }
            string N_OrderCD = inventoryController.generateNoseries(WWT_Inventory.Properties.Settings.Default.DeviceID, "PurchaseOrder", out error);
            orderHdr.PurOrderCD = N_OrderCD;
            orderHdr.PurOrderDate = date.SelectedDate.Value;
            orderHdr.Qty = totalQty;
            orderHdr.TotalAmount = Decimal.TryParse(txt_totalamount.Text.ToString().Trim(), out decimal d) ? d : 0;
            orderHdr.Tax = Decimal.TryParse(txt_tax.Text.ToString().Trim(), out  d) ? d : 0;
            orderHdr.DiscountAmount = Decimal.TryParse(txt_discount.Text.ToString().Trim(), out  d) ? d : 0;
            orderHdr.Amount = Decimal.TryParse(txt_amount.Text.ToString().Trim(), out  d) ? d : 0;
            orderHdr.RemainAmount = 0;
            orderHdr.ReceivedAmount = 0;
            orderHdr.SupplierCD = (cb_supplier.SelectedValue == null) ? "" : cb_supplier.SelectedValue.ToString();
            orderHdr.isactive = true;
            orderHdr.DeviceID = WWT_Inventory.Properties.Settings.Default.DeviceID;
            bool ret = purchaseController.savePurchaseInformation(orderHdr, orderDetails, out error);
            if (ret && error=="")
            {
                MessageBox.Show("Purchase Saved.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                PurchaseInvoiceViewer purchaseInvoiceViewer = new PurchaseInvoiceViewer();
                purchaseInvoiceViewer.ShowDialog();
                ret = purchaseController.updateItemUnitAfterPurchase(CommonFactory.PurInvoiceNo, out error);
                this.Close();
            }
            else
            {
                ret = purchaseController.deletePurchaseInformation(N_OrderCD, CommonFactory.PurInvoiceNo, out error);
                MessageBox.Show("Purchase Failed.\nSorry, Try Again.", "Fail", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void txt_discount_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key==Key.Enter)
                calculateAmount();
        }

        private void txt_tax_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key==Key.Enter)
                calculateAmount();
        }

        private void btn_save_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                btn_close_Click(sender, e);
            else if (e.Key==Key.Down)
                cb_supplier.Focus();
        }

        private void btn_close_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                btn_close_Click(sender, e);
            }
        }

        private void btn_close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
