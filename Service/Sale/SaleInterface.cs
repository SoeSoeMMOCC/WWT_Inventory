using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WWT_Inventory.Model.Sale;

namespace WWT_Inventory.Service.Sale
{
    public interface SaleInterface
    {
        #region 'Save Region'
        bool saveSaleInformation(SaleOrderHdr orderHdr,List<SaleOrderDetail> saleOrders, out string error);
        bool saveSaleOrderDetail(string SaleOrderCD,List<SaleOrderDetail> saleOrders, out string error);
        bool saveSaleInvoiceDetail(string SaleInvoiceCD,List<SaleOrderDetail> saleInvoices, out string error);
        bool saveSaleInvoiceHdr(string SaleInvoiceCD,SaleOrderHdr saleInvoiceHdr, out string error);
        bool saveCustomer(Customer customer, out string error);
        #endregion
        #region 'Get Region'

        List<SaleInvoiceHdr> GetSaleInvoices(DateTime fromDate, DateTime toDate, string SaleInoivceCD, string CustCD, out string error);
        List<Customer> GetCustomers(string CustCD, out string error);
        List<SaleTypes> GetSaleTypes(string SaleTypeCD, out string error);
        SaleInvoiceHdr getSaleInvoiceHdr(string SaleInvoiceCD, out string error);
        List<SaleInvoiceDetail> getSaleInvoiceDetails(string SaleInoviceCD, out string error);

        #endregion

        #region 'Delete Region'
        bool deleteSaleInformation(string SaleOrderNo, string SaleInvoiceNo, out string error);
        #endregion

        #region 'Update Region'
        bool updateCustomer(Customer customer, out string error);
        #endregion
    }
}
