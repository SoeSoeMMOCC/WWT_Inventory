using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
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
using WWT_Inventory.Factory;
using WWT_Inventory.View;
using WWT_Inventory.View.Inventory;
using WWT_Inventory.View.Purchase;
using WWT_Inventory.View.Report;
using WWT_Inventory.View.Sale;

namespace WWT_Inventory
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            ItemInfoView itemInfoView = new ItemInfoView();
            gridcontent.Content = itemInfoView;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.Hide();
            Login loginView = new Login();
            loginView.ShowDialog();
            if (loginView.status)
            {
                if (loginView.role == ConfigurationManager.AppSettings.Get("AdminRole"))
                {
                    CommonFactory.isAdmin = true;
                    this.Show();
                }                    
                else if (loginView.role == ConfigurationManager.AppSettings.Get("SaleRole"))
                {
                    CommonFactory.isAdmin = false;
                    SaleOrderAddView saleOrder = new SaleOrderAddView();
                    saleOrder.ShowDialog();
                    //Application.Current.Shutdown();
                }
                    

            }
        }
        private void gridHeader_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void btn_Power_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void btn_max_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Maximized;
        }

        private void btn_min_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void btn_restore_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Normal;
        }

        private void btnPurchaseOrder_Click(object sender, RoutedEventArgs e)
        {
            gridcontent.Content = null;
            PurchaseInfoView purchaseInfo = new PurchaseInfoView();
            gridcontent.Content = purchaseInfo;
        }

        private void btn_item_Click(object sender, RoutedEventArgs e)
        {
            gridcontent.Content = null;
            ItemInfoView infoView = new ItemInfoView();
            gridcontent.Content = infoView;
        }

        private void btn_sale_Click(object sender, RoutedEventArgs e)
        {
            gridcontent.Content = null;
            SaleInfoView saleInfoView = new SaleInfoView();
            gridcontent.Content = saleInfoView;
        }

        private void btnReport_Click(object sender, RoutedEventArgs e)
        {
            gridcontent.Content = null;
            //RemainStockQtyReportView remainStockQty = new RemainStockQtyReportView();
            //remainStockQty.ShowDialog();
            ReportListView reportList = new ReportListView();
            gridcontent.Content = reportList;
        }
    }
}
