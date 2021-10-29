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
using WWT_Inventory.Controller.Sale;
using WWT_Inventory.Factory;
using WWT_Inventory.Model;
using WWT_Inventory.Model.Sale;

namespace WWT_Inventory.View.Sale.ReportViews
{
    /// <summary>
    /// Interaction logic for SaleInvoiceViewer.xaml
    /// </summary>
    public partial class SaleInvoiceViewer : Window
    {
        SaleController saleController;
        List<SaleInvoiceDetail> saleInvoiceDetails;
        SaleInvoiceHdr saleInvoiceHdr;
        HeaderClass headerClass;
        string error;
        public SaleInvoiceViewer()
        {
            InitializeComponent();
            saleController = new SaleController();
            saleInvoiceDetails = new List<SaleInvoiceDetail>();
            saleInvoiceHdr = new SaleInvoiceHdr();
            headerClass = new HeaderClass();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            saleInvoiceHdr = saleController.getSaleInvoiceHdr(CommonFactory.SaleInvoiceNo, out error);
            saleInvoiceDetails = saleController.getSaleInvoiceDetails(CommonFactory.SaleInvoiceNo, out error);
            headerClass = new HeaderClass();
            headerClass.Address1 = ConfigurationManager.AppSettings.Get("Address1");
            headerClass.Address2 = ConfigurationManager.AppSettings.Get("Address2");
            headerClass.PhoneNo = ConfigurationManager.AppSettings.Get("Phone");
            headerClass.Email = ConfigurationManager.AppSettings.Get("Email");
            headerClass.Content = ConfigurationManager.AppSettings.Get("Content");
            headerClass.CompanyName = ConfigurationManager.AppSettings.Get("CompanyName");
            List<HeaderClass> lstHeader = new List<HeaderClass>();
            lstHeader.Add(headerClass);
            List<SaleInvoiceHdr> inoviceHdr = new List<SaleInvoiceHdr>();
            inoviceHdr.Add(saleInvoiceHdr);

            ReportDataSource rptdatasource1 = new ReportDataSource("DataSet1", inoviceHdr);
            ReportDataSource rptdatasource2 = new ReportDataSource("DataSet2", saleInvoiceDetails);
            ReportDataSource rptdatasource3 = new ReportDataSource("DataSet3", lstHeader);
            this.rpt_sale.Reset();
            this.rpt_sale.LocalReport.DataSources.Clear();
            this.rpt_sale.LocalReport.DataSources.Add(rptdatasource1);
            this.rpt_sale.LocalReport.DataSources.Add(rptdatasource2);
            this.rpt_sale.LocalReport.DataSources.Add(rptdatasource3);
            this.rpt_sale.LocalReport.ReportEmbeddedResource = "WWT_Inventory.ReportPreviews.SaleInvoicePreview.rdlc";
            //thirpt_salese.ZoomMode = ZoomMode.FullPage;
            this.rpt_sale.SetDisplayMode(Microsoft.Reporting.WinForms.DisplayMode.PrintLayout);
            this.rpt_sale.RefreshReport();
        }
    }
}
