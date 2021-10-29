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
using WWT_Inventory.Controller.Purchase;
using WWT_Inventory.Factory;
using WWT_Inventory.Model;
using WWT_Inventory.Model.Purchase;

namespace WWT_Inventory.View.Purchase.ReportViews
{
    /// <summary>
    /// Interaction logic for PurchaseInvoiceViewer.xaml
    /// </summary>
    public partial class PurchaseInvoiceViewer : Window
    {
        PurchaseController purchaseController;
        List<PurchaseInvoiceDetail1> purchaseInvoiceDetails;
        PurchaseInvoiceHdr purchaseInvoiceHdr;
        HeaderClass headerClass;
        string error;
        public PurchaseInvoiceViewer()
        {
            InitializeComponent();
            purchaseController = new PurchaseController();
            purchaseInvoiceDetails = new List<PurchaseInvoiceDetail1>();
            purchaseInvoiceHdr = new PurchaseInvoiceHdr();
            error = "";
            headerClass = new HeaderClass();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            purchaseInvoiceHdr = purchaseController.getPurchaseInvoiceHdr(CommonFactory.PurInvoiceNo, out error);
            purchaseInvoiceDetails = purchaseController.getPurchaseInvoiceDetail(CommonFactory.PurInvoiceNo, out error);
            headerClass = new HeaderClass();
            headerClass.Address1 = ConfigurationManager.AppSettings.Get("Address1");
            headerClass.Address2 = ConfigurationManager.AppSettings.Get("Address2");
            headerClass.PhoneNo = ConfigurationManager.AppSettings.Get("Phone");
            headerClass.Email = ConfigurationManager.AppSettings.Get("Email");
            headerClass.Content = ConfigurationManager.AppSettings.Get("Content");
            headerClass.CompanyName = ConfigurationManager.AppSettings.Get("CompanyName");
            List<HeaderClass> lstHeader = new List<HeaderClass>();
            lstHeader.Add(headerClass);
            List<PurchaseInvoiceHdr> inoviceHdr = new List<PurchaseInvoiceHdr>();
            inoviceHdr.Add(purchaseInvoiceHdr);

            ReportDataSource rptdatasource1 = new ReportDataSource("DataSet1", inoviceHdr);
            ReportDataSource rptdatasource2 = new ReportDataSource("DataSet2", purchaseInvoiceDetails);
            ReportDataSource rptdatasource3 = new ReportDataSource("DataSet3", lstHeader);
            this.rpt_purchase.Reset();
            this.rpt_purchase.LocalReport.DataSources.Clear();
            this.rpt_purchase.LocalReport.DataSources.Add(rptdatasource1);
            this.rpt_purchase.LocalReport.DataSources.Add(rptdatasource2);
            this.rpt_purchase.LocalReport.DataSources.Add(rptdatasource3);
            this.rpt_purchase.LocalReport.ReportEmbeddedResource = "WWT_Inventory.ReportPreviews.PurchaseInvoicePreview.rdlc";
            //this.rpt_purchase.ZoomMode = ZoomMode.FullPage;
            this.rpt_purchase.SetDisplayMode(Microsoft.Reporting.WinForms.DisplayMode.PrintLayout);
            this.rpt_purchase.RefreshReport();
        }
    }
}
