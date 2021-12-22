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
using System.Windows.Navigation;
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
    /// Interaction logic for PurchaseInfoView.xaml
    /// </summary>
    public partial class PurchaseInfoView : UserControl
    {
        PurchaseController purchaseController;
        InventoryController inventoryController;
        DateTime fromDate;
        DateTime toDate;
        string error;
        List<Supplier> suppliers;
        List<PurchaseInvoiceHdr> invoiceHdrs;
        PurchaseInvoiceHdr invoiceHdr;

        public PurchaseInfoView()
        {
            InitializeComponent();
            purchaseController = new PurchaseController();
            inventoryController = new InventoryController();
            error = "";
            suppliers = new List<Supplier>();
            invoiceHdrs = new List<PurchaseInvoiceHdr>();
            invoiceHdr = new PurchaseInvoiceHdr();
        }
        /* User Control Loaded */
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            from_date.SelectedDate = DateTime.Now;
            to_date.SelectedDate = DateTime.Now;
            txt_purorder.Text = "";
            
            suppliers = inventoryController.getSuppliers("%", out error);
            Supplier sup = new Supplier();
            sup.SupplierCD = "%";
            sup.SupplierName = "[ALL]";
            suppliers.Insert(0, sup);

            cb_supplier.ItemsSource = suppliers;
            cb_supplier.SelectedValuePath = "SupplierCD";
            cb_supplier.DisplayMemberPath = "SupplierName";
            cb_supplier.SelectedIndex = 0;
            fromDate = Convert.ToDateTime(DateTime.Now.ToShortDateString() + " 00:00:00");
            toDate = Convert.ToDateTime(DateTime.Now.ToShortDateString() + " 23:59:59");
            invoiceHdrs = purchaseController.getPurchaseInvoices(fromDate, toDate, "%", cb_supplier.SelectedValue.ToString(),out error);
            grdPurLists.ItemsSource = invoiceHdrs;
        }
        /* Search Button Click */
        private void btn_search_Click(object sender, RoutedEventArgs e)
        {
            fromDate =(from_date.SelectedDate==null)? Convert.ToDateTime(DateTime.Now.ToShortDateString() + " 00:00:00") : Convert.ToDateTime(from_date.SelectedDate.Value.ToShortDateString() + " 00:00:00");
            toDate = (to_date.SelectedDate == null) ? Convert.ToDateTime(DateTime.Now.ToShortDateString() + " 00:00:00") : Convert.ToDateTime(to_date.SelectedDate.Value.ToShortDateString() + " 23:59:59");
            string sup = (cb_supplier.SelectedValue == null) ? "%" : cb_supplier.SelectedValue.ToString();
            string id = (txt_purorder.Text.ToString().Trim() == "") ? "%" : txt_purorder.Text.ToString();
            invoiceHdrs = purchaseController.getPurchaseInvoices(fromDate, toDate, id, sup, out error);
            grdPurLists.ItemsSource = invoiceHdrs;
        }
        /* Add Purchase Button Click */
        private void btn_add_puchase_Click(object sender, RoutedEventArgs e)
        {
            PurchaseOrderAddView orderAddView = new PurchaseOrderAddView();
            orderAddView.ShowDialog();
            UserControl_Loaded(sender, e);
        }

        /* View Button Click */
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            PurchaseInvoiceHdr obj = ((FrameworkElement)sender).DataContext as PurchaseInvoiceHdr;
            if(obj != null)
            {
                CommonFactory.PurInvoiceNo = "";
                CommonFactory.PurInvoiceNo = obj.PurInvoiceCD;
                PurchaseInvoiceViewer purchaseInvoiceViewer = new PurchaseInvoiceViewer();
                purchaseInvoiceViewer.ShowDialog();
            }
            
        }
        /* Delete Button Click */
        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            PurchaseInvoiceHdr obj = ((FrameworkElement)sender).DataContext as PurchaseInvoiceHdr;
            if (obj != null)
            {
               MessageBoxResult result =  MessageBox.Show("Are you sure do you want to delete? \n " +
                    "Purchase Inovoice No - " + obj.PurInvoiceCD, "Are you sure?", MessageBoxButton.YesNo, MessageBoxImage.Information);
                switch (result)
                {
                    case MessageBoxResult.Yes:
                        bool ret = purchaseController.deletePurchaseInformation(obj.PurOrderCD, obj.PurInvoiceCD, out error);
                        MessageBox.Show("Purchase Deleted!", "Deleted", MessageBoxButton.OK, MessageBoxImage.Information);
                        break;
                    case MessageBoxResult.No:
                        break;
                } 
            }
            UserControl_Loaded(sender, e);
        }

        private void btn_add_supplier_Click(object sender, RoutedEventArgs e)
        {
            PurchaseSupplierAdd supplierAdd = new PurchaseSupplierAdd();
            supplierAdd.ShowDialog();
        }
    }
}
