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
    /// Interaction logic for ItemUnitSummaryReportView.xaml
    /// </summary>
    public partial class ItemUnitSummaryReportView : Window
    {
        InventoryController inventoryController;
        InventoryReportsController inventoryReportsController;
        string error;
        List<ItemUnitSummary> itemUnitSummaries;
        List<Unit> units;
        List<Item> items;
        HeaderClass headerClass;
        public ItemUnitSummaryReportView()
        {
            InitializeComponent();
            inventoryController = new InventoryController();
            inventoryReportsController = new InventoryReportsController();
            error = "";
            itemUnitSummaries = new List<ItemUnitSummary>();
            units = new List<Unit>();
            items = new List<Item>();
            headerClass = new HeaderClass();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            items = inventoryController.getInventoryItemList("%", "%", out error);
            units = inventoryController.getUnits("%", out error);
            Item item = new Item();
            item.ItemCD = "%";
            item.ItemName = "[ALL]";
            items.Insert(0, item);

            Unit unit = new Unit();
            unit.UnitCD = "%";
            unit.UnitName = "[ALL]";
            units.Insert(0, unit);

            cb_item.ItemsSource = items;
            cb_item.SelectedValuePath = "ItemCD";
            cb_item.DisplayMemberPath = "ItemName";
            cb_item.SelectedIndex = 0;

            cb_unit.ItemsSource = units;
            cb_unit.SelectedValuePath = "UnitCD";
            cb_unit.DisplayMemberPath = "UnitName";
            cb_unit.SelectedIndex = 0;
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

        private void btn_search_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                btn_search_Click(sender, e);
        }

        private void btn_search_Click(object sender, RoutedEventArgs e)
        {
            itemUnitSummaries = inventoryReportsController.getItemUnitSummary(cb_item.SelectedValue.ToString(),
                cb_unit.SelectedValue.ToString(), out error);
            headerClass = new HeaderClass();
            headerClass.Address1 = ConfigurationManager.AppSettings.Get("Address1");
            headerClass.Address2 = ConfigurationManager.AppSettings.Get("Address2");
            headerClass.PhoneNo = ConfigurationManager.AppSettings.Get("Phone");
            headerClass.Email = ConfigurationManager.AppSettings.Get("Email");
            headerClass.Content = ConfigurationManager.AppSettings.Get("Content");
            headerClass.CompanyName = ConfigurationManager.AppSettings.Get("CompanyName");
            List<HeaderClass> lstHeader = new List<HeaderClass>();
            lstHeader.Add(headerClass);

            ReportDataSource rptdatasource1 = new ReportDataSource("DataSet1", itemUnitSummaries);
            ReportDataSource rptdatasource2 = new ReportDataSource("DataSet2", lstHeader);
            this.rpt_itemunitsummary.Reset();
            this.rpt_itemunitsummary.LocalReport.DataSources.Clear();
            this.rpt_itemunitsummary.LocalReport.DataSources.Add(rptdatasource1);
            this.rpt_itemunitsummary.LocalReport.DataSources.Add(rptdatasource2);
            this.rpt_itemunitsummary.LocalReport.ReportEmbeddedResource = "WWT_Inventory.ReportPreviews.InventoryReports.InventoryItemUnitSummaryRpt.rdlc";
            //thirpt_itemunitsummaryin.ZoomMode = ZoomMode.Percent;
            //thirpt_itemunitsummaryin.ZoomPercent = 100;
            this.rpt_itemunitsummary.SetDisplayMode(Microsoft.Reporting.WinForms.DisplayMode.PrintLayout);
            this.rpt_itemunitsummary.ZoomMode = ZoomMode.PageWidth;
            this.rpt_itemunitsummary.RefreshReport();
        }
    }
}
