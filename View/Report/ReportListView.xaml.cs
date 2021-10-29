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

namespace WWT_Inventory.View.Report
{
    /// <summary>
    /// Interaction logic for ReportListView.xaml
    /// </summary>
    public partial class ReportListView : UserControl
    {
        public ReportListView()
        {
            InitializeComponent();
        }

        private void btn_remainingstock_Click(object sender, RoutedEventArgs e)
        {
            RemainStockQtyReportView remainStock = new RemainStockQtyReportView();
            remainStock.ShowDialog();
        }

        private void btn_itemunitsummary_Click(object sender, RoutedEventArgs e)
        {
            ItemUnitSummaryReportView itemUnitSummary = new ItemUnitSummaryReportView();
            itemUnitSummary.ShowDialog();
        }

        private void btn_saledetailreport_Click(object sender, RoutedEventArgs e)
        {
            SaleDetailSummaryReportView saleDetailSummary = new SaleDetailSummaryReportView();
            saleDetailSummary.ShowDialog();
        }

        private void btn_salepurchase_Click(object sender, RoutedEventArgs e)
        {
            SalePurchaseHistoryReportView salePurchaseHistory = new SalePurchaseHistoryReportView();
            salePurchaseHistory.ShowDialog();
        }
    }
}
