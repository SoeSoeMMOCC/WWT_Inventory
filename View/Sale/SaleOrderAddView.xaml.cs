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
using WWT_Inventory.Model.Inventory;
using WWT_Inventory.Model.Sale;
using WWT_Inventory.Factory;
using WWT_Inventory.View.Sale.ReportViews;

namespace WWT_Inventory.View.Sale
{
    /// <summary>
    /// Interaction logic for SaleOrderAddView.xaml
    /// </summary>
    public partial class SaleOrderAddView : Window
    {
        SaleController saleController;
        InventoryController inventoryController;
        string error;
        List<Category> categories;
        List<SubCategory> subCategories;
        List<Item> items;
        Item selItem;
        List<SaleOrderDetail> saleOrderDetails;
        SaleOrderHdr saleOrderHdr;
        List<Customer> customers;
        Decimal total, discount, tax, amount;
        int totalQty;
        public SaleOrderAddView()
        {
            InitializeComponent();
            saleController = new SaleController();
            inventoryController = new InventoryController();
            error = "";
            categories = new List<Category>();
            subCategories = new List<SubCategory>();
            items = new List<Item>();
            selItem = new Item();
            saleOrderDetails = new List<SaleOrderDetail>();
            saleOrderHdr = new SaleOrderHdr();
            customers = new List<Customer>();
            total = discount = tax = amount = 0;
            totalQty = 0;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            items = inventoryController.getInventoryItemList("%", "%", out error);
            categories = inventoryController.getCategories("%", out error);
            subCategories = inventoryController.getSubCategories("%", "%", out error);
            customers = saleController.GetCustomers("%", out error);

            cb_category.ItemsSource = categories;
            cb_category.SelectedValuePath = "CategoryCD";
            cb_category.DisplayMemberPath = "CategoryName";
            cb_category.SelectedIndex = 0;

            txt_itemcd.Text = "";

            cb_customer.ItemsSource = customers;
            cb_customer.SelectedValuePath = "CustCD";
            cb_customer.DisplayMemberPath = "CustName";
            cb_customer.SelectedIndex = 0;
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
            CommonFactory.SaleInvoiceNo = "";
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
            if (cb_category.SelectedValue != null || cb_category.SelectedValue.ToString() != "")
            {
                cb_subcategory.ItemsSource = subCategories.FindAll(x => x.CategoryCD == cb_category.SelectedValue.ToString());
                cb_subcategory.SelectedValuePath = "SubCategoryCD";
                cb_subcategory.DisplayMemberPath = "SubCategoryName";
                cb_subcategory.SelectedIndex = 0;
            }
        }

        private void cb_subcategory_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                txt_itemcd.Focus();
            }
        }

        private void txt_itemcd_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                btn_search.Focus();
            }
        }

        private void btn_search_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Right)
            {
                btn_save.Focus();
            }
            else if (e.Key == Key.Enter)
            {
                btn_search_Click(sender, e);
            }
        }

        private void btn_save_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Right)
            {
                btn_close.Focus();
            }
            else if (e.Key == Key.Enter)
            {
                btn_save_Click(sender, e);
            }
        }
        private void btn_close_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Left)
            {
                btn_save.Focus();
            }
            else if (e.Key == Key.Enter)
            {
                btn_close_Click(sender, e);
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
                grdItemList.ItemsSource = items.FindAll(x => x.CategoryCD == cate_cd && x.SubCategoryCD == sub_cd && (x.ItemCD == item_cd || x.ItemName == item_cd));
            }
        }

        private void btn_save_Click(object sender, RoutedEventArgs e)
        {
            string N_OrderCD = inventoryController.generateNoseries(WWT_Inventory.Properties.Settings.Default.DeviceID, "SaleOrder", out error);
            saleOrderHdr.SaleOrderCD = N_OrderCD;
            saleOrderHdr.SaleOrderDate = date.SelectedDate.Value;
            saleOrderHdr.Qty = totalQty;
            saleOrderHdr.TotalAmount = Decimal.TryParse(txt_totalamount.Text.ToString().Trim(), out decimal d) ? d : 0;
            saleOrderHdr.Tax = Decimal.TryParse(txt_tax.Text.ToString().Trim(), out d) ? d : 0;
            saleOrderHdr.DiscountAmount = Decimal.TryParse(txt_discount.Text.ToString().Trim(), out d) ? d : 0;
            saleOrderHdr.Amount = Decimal.TryParse(txt_amount.Text.ToString().Trim(), out d) ? d : 0;
            saleOrderHdr.CustomerCD = (cb_customer.SelectedValue == null) ? "" : cb_customer.SelectedValue.ToString();
            saleOrderHdr.isactive = true;
            saleOrderHdr.DeviceID = WWT_Inventory.Properties.Settings.Default.DeviceID;
            bool ret = saleController.saveSaleInformation(saleOrderHdr, saleOrderDetails, out error);
            if (ret && error == "")
            {
                MessageBox.Show("Sale Saved.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                SaleInvoiceViewer saleInvoiceViewer = new SaleInvoiceViewer();
                saleInvoiceViewer.ShowDialog();
                this.Close();
            }
            else
            {
                ret = saleController.deleteSaleInformation(N_OrderCD, CommonFactory.SaleInvoiceNo, out error);
                MessageBox.Show("Sale Failed.\nSorry, Try Again.", "Fail", MessageBoxButton.OK, MessageBoxImage.Information);
            }

        }

        private void btn_close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void grdItemList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Item selItem = (Item)grdItemList.SelectedItem;
            if (selItem != null)
            {
                Guid g = Guid.NewGuid();
                string detailID = g.ToString().Replace("-", "");
                SaleOrderDetail orderDetail = new SaleOrderDetail();
                orderDetail.SaleDetailCD = detailID;
                orderDetail.ItemCD = selItem.ItemCD;
                orderDetail.ShortCode = selItem.ShortCode;
                orderDetail.ItemName = selItem.ShortCode + "( " + selItem.ItemName + " )";
                orderDetail.UnitCD = selItem.UnitCD;
                orderDetail.UnitName = selItem.UnitName;
                orderDetail.Price = selItem.SalePrice;
                orderDetail.PurPrice = selItem.PurPrice;
                orderDetail.Qty = 1;
                orderDetail.Amount = selItem.SalePrice * 1;
                orderDetail.DiscountPer = 0;
                orderDetail.DiscountAmt = 0;
                orderDetail.TotalAmount = selItem.PurPrice * 1;
                orderDetail.isactive = true;
                orderDetail.SaleTypeCD = "ST001";
                orderDetail.SaleType = "SALE";
                saleOrderDetails.Add(orderDetail);
                grdOrderList.ItemsSource = null;
                grdOrderList.ItemsSource = saleOrderDetails;

                calculateAmount();
            }
        }

        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            SaleOrderDetail obj = ((FrameworkElement)sender).DataContext as SaleOrderDetail;
            if (obj != null)
            {
                CommonFactory.selSaleOrderDetail = obj;
                SaleOrderItem orderItem = new SaleOrderItem();
                orderItem.ShowDialog();
                if (SaleOrderItem.status)
                {
                    int index = saleOrderDetails.FindIndex(x => x.SaleDetailCD == CommonFactory.selSaleOrderDetail.SaleDetailCD);
                    saleOrderDetails[index].UnitCD = CommonFactory.selSaleOrderDetail.UnitCD;
                    saleOrderDetails[index].UnitName = CommonFactory.selSaleOrderDetail.UnitName;
                    saleOrderDetails[index].Qty = CommonFactory.selSaleOrderDetail.Qty;
                    saleOrderDetails[index].Price = CommonFactory.selSaleOrderDetail.Price;
                    saleOrderDetails[index].PurPrice = CommonFactory.selSaleOrderDetail.PurPrice;
                    saleOrderDetails[index].Amount = CommonFactory.selSaleOrderDetail.Amount;
                    saleOrderDetails[index].TotalAmount = CommonFactory.selSaleOrderDetail.Amount;
                    saleOrderDetails[index].SaleType = CommonFactory.selSaleOrderDetail.SaleType;
                    saleOrderDetails[index].SaleTypeCD = CommonFactory.selSaleOrderDetail.SaleTypeCD;

                    grdOrderList.ItemsSource = null;
                    grdOrderList.ItemsSource = saleOrderDetails;
                    CommonFactory.selSaleOrderDetail = new SaleOrderDetail();
                    calculateAmount();
                }
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            SaleOrderDetail obj = ((FrameworkElement)sender).DataContext as SaleOrderDetail;
            if (obj != null)
            {
                saleOrderDetails.RemoveAt(saleOrderDetails.FindIndex(x => x.SaleDetailCD == obj.SaleDetailCD));
                grdOrderList.ItemsSource = null;
                grdOrderList.ItemsSource = saleOrderDetails;
                calculateAmount();
            }
        }

        private void calculateAmount()
        {
            total = tax = discount = amount = 0;
            totalQty = 0;
            foreach (SaleOrderDetail det in saleOrderDetails)
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

        private void txt_discount_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                calculateAmount();
                txt_tax.Focus();
            }
                
        }

        private void txt_tax_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                calculateAmount();
                btn_save.Focus();
            }
        }
    }
}
