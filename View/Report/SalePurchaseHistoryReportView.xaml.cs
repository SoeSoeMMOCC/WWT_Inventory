using Microsoft.Reporting.WinForms;
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
using System.Windows.Shapes;
using WWT_Inventory.Controller.Inventory;
using WWT_Inventory.Controller.Report;
using WWT_Inventory.Model;
using WWT_Inventory.Model.Inventory;
using WWT_Inventory.Model.Reports;

namespace WWT_Inventory.View.Report
{
    /// <summary>
    /// Interaction logic for SalePurchaseHistoryReportView.xaml
    /// </summary>
    public partial class SalePurchaseHistoryReportView : Window
    {
        DateTime fromDate, toDate;
        InventoryController inventoryController;
        InventoryReportsController reportController;
        List<Unit> units;
        List<Item> items;
        List<SalePurchaseHistory> salePurchaseHistories;
        string error;
        string selItemCD, selUnitCD = "";
        HeaderClass headerClass;
        public SalePurchaseHistoryReportView()
        {
            InitializeComponent();
            fromDate = toDate = DateTime.Now;
            inventoryController = new InventoryController();
            reportController = new InventoryReportsController();
            units = new List<Unit>();
            items = new List<Item>();
            salePurchaseHistories = new List<SalePurchaseHistory>();
            error = "";
            headerClass = new HeaderClass();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            from_date.SelectedDate = fromDate;
            to_date.SelectedDate = toDate;
            Item item = new Item();
            item.ItemCD = "%";
            item.ItemName = "[ALL]";
            items = inventoryController.getInventoryItemList("%", "%", out error);
            items.Insert(0, item);
            cb_item.ItemsSource = items;
            cb_item.DisplayMemberPath = "ItemName";
            cb_item.SelectedValuePath = "ItemCD";
            cb_item.SelectedIndex = 0;

            Unit unit = new Unit();
            unit.UnitCD = "%";
            unit.UnitName = "[ALL]";

            units = inventoryController.getUnits("%", out error);
            units.Insert(0, unit);
            cb_unit.ItemsSource = units;
            cb_unit.DisplayMemberPath = "UnitName";
            cb_unit.SelectedValuePath = "UnitCD";
            cb_unit.SelectedIndex = 0;
            from_date.Focus();
        }
        private void btn_search_Click(object sender, RoutedEventArgs e)
        {
            fromDate = Convert.ToDateTime(from_date.SelectedDate.Value.ToShortDateString() + " 00:00:00");
            toDate = Convert.ToDateTime(to_date.SelectedDate.Value.ToShortDateString() + " 23:59:59");
            selItemCD = (cb_item.SelectedValue == null) ? "" : cb_item.SelectedValue.ToString();
            selUnitCD = (cb_unit.SelectedValue == null) ? "" : cb_unit.SelectedValue.ToString();
            salePurchaseHistories = reportController.getSalePurchseHistories(fromDate, toDate, selItemCD, selUnitCD, out error);
            headerClass = new HeaderClass();
            headerClass.Address1 = ConfigurationManager.AppSettings.Get("Address1");
            headerClass.Address2 = ConfigurationManager.AppSettings.Get("Address2");
            headerClass.PhoneNo = ConfigurationManager.AppSettings.Get("Phone");
            headerClass.Email = ConfigurationManager.AppSettings.Get("Email");
            headerClass.Content = ConfigurationManager.AppSettings.Get("Content");
            headerClass.CompanyName = ConfigurationManager.AppSettings.Get("CompanyName");
            List<HeaderClass> lstHeader = new List<HeaderClass>();
            lstHeader.Add(headerClass);

            ReportDataSource rptdatasource1 = new ReportDataSource("DataSet1", salePurchaseHistories);
            ReportDataSource rptdatasource2 = new ReportDataSource("DataSet2", lstHeader);
            this.rpt_salepurchasehistory.Reset();
            this.rpt_salepurchasehistory.LocalReport.DataSources.Clear();
            this.rpt_salepurchasehistory.LocalReport.DataSources.Add(rptdatasource1);
            this.rpt_salepurchasehistory.LocalReport.DataSources.Add(rptdatasource2);
            this.rpt_salepurchasehistory.LocalReport.ReportEmbeddedResource = "WWT_Inventory.ReportPreviews.InventoryReports.SalePurchaseHistoryRpt.rdlc";
            //thirpt_salepurchasehistoryin.ZoomMode = ZoomMode.Percent;
            //thirpt_salepurchasehistoryin.ZoomPercent = 100;
            this.rpt_salepurchasehistory.SetDisplayMode(Microsoft.Reporting.WinForms.DisplayMode.PrintLayout);
            this.rpt_salepurchasehistory.ZoomMode = ZoomMode.PageWidth;
            this.rpt_salepurchasehistory.RefreshReport();
        }
        private void btn_search_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                btn_search_Click(sender, e);
        }

        private void from_date_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                to_date.Focus();
        }

        private void to_date_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                cb_item.Focus();
        }

        private void cb_item_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                cb_unit.Focus();
        }

        private void cb_unit_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                btn_search.Focus();
        }
    }
}
