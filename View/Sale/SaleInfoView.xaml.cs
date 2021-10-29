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
using WWT_Inventory.Controller.Purchase;
using WWT_Inventory.Controller.Sale;
using WWT_Inventory.Factory;
using WWT_Inventory.Model.Sale;
using WWT_Inventory.View.Purchase.ReportViews;
using WWT_Inventory.View.Sale.ReportViews;

namespace WWT_Inventory.View.Sale
{
    /// <summary>
    /// Interaction logic for SaleInfoView.xaml
    /// </summary>
    public partial class SaleInfoView : UserControl
    {
        SaleController saleController;
        string error;
        List<SaleInvoiceHdr> saleInvoiceHdrs;
        List<Customer> customers;
        DateTime fromDate, toDate;
        SaleInvoiceHdr selSaleInvoice;
        public SaleInfoView()
        {
            InitializeComponent();
            saleController = new SaleController();
            error = "";
            saleInvoiceHdrs = new List<SaleInvoiceHdr>();
            customers = new List<Customer>();
            fromDate = DateTime.Now;
            toDate = DateTime.Now;
            selSaleInvoice = new SaleInvoiceHdr();
        }
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            fromDate = Convert.ToDateTime(DateTime.Now.ToShortDateString() + " 00:00:00");
            toDate = Convert.ToDateTime(DateTime.Now.ToShortDateString() + " 23:59:59");
            from_date.SelectedDate = DateTime.Now;
            to_date.SelectedDate = DateTime.Now;
            txt_saleinvoice.Text = "";

            customers = saleController.GetCustomers("%", out error);
            Customer customer = new Customer();
            customer.CustCD = "%";
            customer.CustName = "[ALL]";
            customers.Insert(0, customer);

            cb_customer.ItemsSource = customers;
            cb_customer.SelectedValuePath = "CustCD";
            cb_customer.DisplayMemberPath = "CustName";
            cb_customer.SelectedIndex = 0;

            saleInvoiceHdrs = saleController.GetSaleInvoices(fromDate, toDate, "%", "%", out error);
            grdSaleList.ItemsSource = null;
            grdSaleList.ItemsSource = saleInvoiceHdrs;
            txt_saleinvoice.Focus();
        }

        private void btn_add_sale_Click(object sender, RoutedEventArgs e)
        {
            SaleOrderAddView saleOrderAddView = new SaleOrderAddView();
            saleOrderAddView.ShowDialog();
            UserControl_Loaded(sender, e);
        }

        /* View Button Click */
        private void ButtonView_Click(object sender, RoutedEventArgs e)
        {
            SaleInvoiceHdr obj = ((FrameworkElement)sender).DataContext as SaleInvoiceHdr;
            if (obj != null)
            {
                CommonFactory.SaleInvoiceNo = "";
                CommonFactory.SaleInvoiceNo = obj.SaleInvoiceCD;
                SaleInvoiceViewer saleInvoiceViewer = new SaleInvoiceViewer();
                saleInvoiceViewer.ShowDialog();
            }
        }
        /* Delete Button Click */
        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            SaleInvoiceHdr obj = ((FrameworkElement)sender).DataContext as SaleInvoiceHdr;
            if (obj != null)
            {
                MessageBoxResult result = MessageBox.Show("Are you sure do you want to delete? \n " +
                     "Sale Inovoice No - " + obj.SaleInvoiceCD, "Are you sure?", MessageBoxButton.YesNo, MessageBoxImage.Information);
                switch (result)
                {
                    case MessageBoxResult.Yes:
                        bool ret = saleController.deleteSaleInformation(obj.SaleOrderCD, obj.SaleInvoiceCD, out error);
                        MessageBox.Show("Sale Deleted!", "Deleted", MessageBoxButton.OK, MessageBoxImage.Information);
                        break;
                    case MessageBoxResult.No:
                        break;
                }
            }
            UserControl_Loaded(sender, e);
        }

        private void btn_add_customer_Click(object sender, RoutedEventArgs e)
        {
            SaleCustomerAdd customerAdd = new SaleCustomerAdd();
            customerAdd.ShowDialog();
        }

        private void btn_search_Click(object sender, RoutedEventArgs e)
        {
            fromDate = Convert.ToDateTime(from_date.SelectedDate.Value.ToShortDateString() + " 00:00:00");
            toDate = Convert.ToDateTime(to_date.SelectedDate.Value.ToShortDateString() + " 23:59:59");
            string sup = (cb_customer.SelectedValue == null) ? "%" : cb_customer.SelectedValue.ToString();
            string id = (txt_saleinvoice.Text.ToString().Trim() == "") ? "%" : txt_saleinvoice.Text.ToString();
            saleInvoiceHdrs = saleController.GetSaleInvoices(fromDate, toDate, "%", cb_customer.SelectedValue.ToString(), out error);
            grdSaleList.ItemsSource = null;
            grdSaleList.ItemsSource = saleInvoiceHdrs;
        }
    }
}
