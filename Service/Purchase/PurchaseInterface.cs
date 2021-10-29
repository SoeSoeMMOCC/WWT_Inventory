using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WWT_Inventory.Model.Purchase;

namespace WWT_Inventory.Service.Purchase
{
    public interface PurchaseInterface
    {
        #region 'Save Region'
        bool savePurchaseInformation(PurchaseOrderHdr orderHdr, List<PurchaseOrderDetail> orderDetail, out string error);
        bool savePurchaseOrderDetail(string PurOrderCD, List<PurchaseOrderDetail> orderDetail, out string error);
        bool savePurchaseInvoiceHdr(string PurInvoiceCD, PurchaseOrderHdr invoiceHdr, out string error);
        bool savePurchaseInvoiceDetail(string PurInvoiceCD, List<PurchaseOrderDetail> invoiceDetail, out string error);
        #endregion
        #region 'Get Region'
        List<PurchaseInvoiceHdr> getPurchaseInvoices(DateTime fromdate,DateTime todate,string PurchaseInvoiceCD,string SupplierCD, out string error);
        PurchaseInvoiceHdr getPurchaseInvoiceHdr(string PurchaseInvoiceCD, out string error);
        List<PurchaseInvoiceDetail1> getPurchaseInvoiceDetail(string PurchaseInvoiceCD, out string error);

        #endregion
        #region 'Update Region'
        bool updateItemUnitAfterPurchase(string PurInoviceCD, out string error);
        #endregion
        #region 'Delete Region'
        bool deletePurchaseInformation(string PurOrderNo, string PurInvoiceNo, out string error);
        #endregion
    }
}
