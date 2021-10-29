using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WWT_Inventory.Controller.Inventory;
using WWT_Inventory.Factory;
using WWT_Inventory.Model.Purchase;
using WWT_Inventory.Service.Purchase;

namespace WWT_Inventory.Controller.Purchase
{
    public class PurchaseController : PurchaseInterface
    {
        IDbConnection conn;
        public PurchaseController()
        {
            conn = DBFactory.InvDB();
        }
        #region 'Save Region'
        public bool savePurchaseInformation(
            PurchaseOrderHdr orderHdr,
            List<PurchaseOrderDetail> orderDetail,
            out string error)
        {
            error = "";
            string sql = "exec insert_purchaseorder_hdr @p_PurOrderCD,@p_PurOrderDate,@p_Qty," +
                "@p_TotalAmount,@p_Tax,@p_DiscountAmount,@p_Amount,@p_isactive," +
                "@p_SupplierCD,@p_DeviceID";
            var parameters = new DynamicParameters();
            parameters.Add("p_PurOrderCD",orderHdr.PurOrderCD , DbType.String);
            parameters.Add("p_PurOrderDate", orderHdr.PurOrderDate , DbType.DateTime);
            parameters.Add("p_Qty", orderHdr.Qty , DbType.Int32);
            parameters.Add("p_TotalAmount", orderHdr.TotalAmount , DbType.Decimal);
            parameters.Add("p_Tax", orderHdr.Tax , DbType.Decimal);
            parameters.Add("p_DiscountAmount", orderHdr.DiscountAmount , DbType.Decimal);
            parameters.Add("p_Amount", orderHdr.Amount , DbType.Decimal);
            parameters.Add("p_isactive", orderHdr.isactive , DbType.Boolean);
            parameters.Add("p_SupplierCD", orderHdr.SupplierCD, DbType.String);
            parameters.Add("p_DeviceID", orderHdr.DeviceID , DbType.String);
            try
            {
                string Ins_CD = Convert.ToString(conn.ExecuteScalar(sql, param: parameters));
                InventoryController inventoryController = new InventoryController();
                string N_ItemCD = inventoryController.generateNoseries(WWT_Inventory.Properties.Settings.Default.DeviceID, "PurchaseInvoice", out error);
                CommonFactory.PurInvoiceNo = N_ItemCD;
                bool ret = true;
                ret = savePurchaseOrderDetail(orderHdr.PurOrderCD,orderDetail, out error);
                if (ret)
                {
                    ret = savePurchaseInvoiceHdr(N_ItemCD, orderHdr, out error);
                    if (ret)
                    {
                        ret = savePurchaseInvoiceDetail(N_ItemCD, orderDetail, out error);
                        if (!ret)
                        {
                            return false;
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
                
                               
                return true;
            }
            catch (Exception e)
            {
                error = "error to insert OrderHdr " + e.Message;
                return false;
            }
        }
        public bool savePurchaseOrderDetail(string PurOrderCD,
            List<PurchaseOrderDetail> orderDetail,
            out string error)
        {
            error = "";
            foreach(PurchaseOrderDetail orderDetail1 in orderDetail)
            {
                string sql = "exec insert_purchaseorder_detail @p_PurDetailCD,@p_PurOrderCD," +
                "@p_ItemCD,@p_UnitCD,@p_Qty,@p_Price,@p_Amount,@p_isactive,@p_DiscountPer," +
                "@p_DiscountAmt,@p_TotalAmount";
                var parameters = new DynamicParameters();
                parameters.Add("p_PurDetailCD", orderDetail1.PurDetailCD, DbType.String);
                parameters.Add("p_PurOrderCD", PurOrderCD, DbType.String);
                parameters.Add("p_ItemCD", orderDetail1.ItemCD, DbType.String);
                parameters.Add("p_UnitCD", orderDetail1.UnitCD, DbType.String);
                parameters.Add("p_Qty", orderDetail1.Qty, DbType.Int32);
                parameters.Add("p_Price", orderDetail1.Price, DbType.Decimal);
                parameters.Add("p_Amount", orderDetail1.Amount, DbType.Decimal);
                parameters.Add("p_isactive", orderDetail1.isactive, DbType.Boolean);
                parameters.Add("p_DiscountPer", orderDetail1.DiscountPer, DbType.Decimal);
                parameters.Add("p_DiscountAmt", orderDetail1.DiscountAmt, DbType.Decimal);
                parameters.Add("p_TotalAmount", orderDetail1.TotalAmount, DbType.Decimal);
                try
                {
                    string Ins_CD = Convert.ToString(conn.ExecuteScalar(sql, param: parameters));
                }
                catch (Exception e)
                {
                    error = "error to insert Detail " + e.Message;
                    return false;
                }
            }
            return true;
        }
        public bool savePurchaseInvoiceHdr(string PurInvoiceCD,
            PurchaseOrderHdr invoiceHdr,
            out string error)
        {
            error = "";
            string sql = "exec insert_purchaseinvoice_hdr @p_PurInvoiceCD,@p_PurOrderCD, " +
                "@p_PurInvoiceDate, @p_Qty, @p_TotalAmount, @p_Tax, @p_DiscountAmount," +
                "@p_Amount, @p_ReceivedAmount, @p_RemainAmount,@p_isactive,@p_SupplierCD,@p_DeviceID";            
            var parameters = new DynamicParameters();
            parameters.Add("p_PurInvoiceCD", PurInvoiceCD, DbType.String);
            parameters.Add("p_PurOrderCD", invoiceHdr.PurOrderCD, DbType.String);
            parameters.Add("p_PurInvoiceDate", DateTime.Now, DbType.DateTime);
            parameters.Add("p_Qty", invoiceHdr.Qty, DbType.Int32);
            parameters.Add("p_TotalAmount", invoiceHdr.TotalAmount, DbType.Decimal);
            parameters.Add("p_Tax", invoiceHdr.Tax, DbType.Decimal);
            parameters.Add("p_DiscountAmount", invoiceHdr.DiscountAmount, DbType.Decimal);
            parameters.Add("p_Amount", invoiceHdr.Amount, DbType.Decimal);
            parameters.Add("p_ReceivedAmount", invoiceHdr.ReceivedAmount, DbType.Decimal);
            parameters.Add("p_RemainAmount", invoiceHdr.RemainAmount, DbType.Decimal);
            parameters.Add("p_isactive", invoiceHdr.isactive, DbType.Boolean);
            parameters.Add("p_SupplierCD", invoiceHdr.SupplierCD, DbType.String);
            parameters.Add("p_DeviceID", invoiceHdr.DeviceID, DbType.String);
            try
            {
                string Ins_CD = Convert.ToString(conn.ExecuteScalar(sql, param: parameters));
                return true;
            }
            catch (Exception e)
            {
                error = "error to insert Hdr " + e.Message;
                return false;
            }
        }
        public bool savePurchaseInvoiceDetail(string PurInvoiceCD,
            List<PurchaseOrderDetail> invoiceDetail,
            out string error)
        {
            error = "";
            foreach(PurchaseOrderDetail invoiceDetail1 in invoiceDetail)
            {
                string sql = "exec insert_purchaseinvoice_detail @p_PurInvDetailCD,@p_PurInvoiceCD," +
                "@p_ItemCD,@p_UnitCD,@p_Qty,@p_Price,@p_ToalAmount,@p_DiscountPer," +
                "@p_DiscountAmt,@p_Amount,@p_isactive";
                var parameters = new DynamicParameters();
                parameters.Add("p_PurInvDetailCD", Guid.NewGuid().ToString().Replace("-", ""), DbType.String);
                parameters.Add("p_PurInvoiceCD", PurInvoiceCD, DbType.String);
                parameters.Add("p_ItemCD", invoiceDetail1.ItemCD, DbType.String);
                parameters.Add("p_UnitCD", invoiceDetail1.UnitCD, DbType.String);
                parameters.Add("p_Qty", invoiceDetail1.Qty, DbType.String);
                parameters.Add("p_Price", invoiceDetail1.Price, DbType.String);
                parameters.Add("p_ToalAmount", invoiceDetail1.TotalAmount, DbType.String);
                parameters.Add("p_DiscountPer", invoiceDetail1.DiscountPer, DbType.String);
                parameters.Add("p_DiscountAmt", invoiceDetail1.DiscountAmt, DbType.String);
                parameters.Add("p_Amount", invoiceDetail1.Amount, DbType.String);
                parameters.Add("p_isactive", invoiceDetail1.isactive, DbType.String);
                try
                {
                    string Ins_CD = Convert.ToString(conn.ExecuteScalar(sql, param: parameters));
                }
                catch (Exception e)
                {
                    error = "error to insert Detail " + e.Message;
                    return false;
                }
            }
            
            return true;
        }
        #endregion
        #region 'Get Region'
        public List<PurchaseInvoiceHdr> getPurchaseInvoices
            (DateTime fromdate, 
            DateTime todate, 
            string PurchaseInvoiceCD, 
            string SupplierCD, out string error)
        {
            error = "";
            List<PurchaseInvoiceHdr> invoiceHdrs = new List<PurchaseInvoiceHdr>();
            string sql = "exec get_purchaseinvoice @p_fromdate,@p_todate,@p_PurInvoiceCD,@p_SupplierCD;";
            var parameters = new DynamicParameters();
            parameters.Add("p_fromdate", fromdate, DbType.DateTime);
            parameters.Add("p_todate", todate, DbType.DateTime);
            parameters.Add("p_PurInvoiceCD", PurchaseInvoiceCD, DbType.String);
            parameters.Add("p_SupplierCD", SupplierCD, DbType.String);
            try
            {
                invoiceHdrs = conn.Query<PurchaseInvoiceHdr>(sql, param: parameters).ToList();
            }
            catch (Exception e)
            {
                error = "Error to Retrieve Purchase InvoiceHdr Information.\n" + e.Message;
            }
            return invoiceHdrs;
        }

        public PurchaseInvoiceHdr getPurchaseInvoiceHdr
            (string PurchaseInvoiceCD, out string error)
        {
            error = "";
            PurchaseInvoiceHdr invoiceHdrs = new PurchaseInvoiceHdr();
            string sql = "exec get_purchaseinvoice_hdr @p_PurInvoiceCD;";
            var parameters = new DynamicParameters();
            parameters.Add("p_PurInvoiceCD", PurchaseInvoiceCD, DbType.String);
            try
            {
                invoiceHdrs = conn.Query<PurchaseInvoiceHdr>(sql, param: parameters).FirstOrDefault();
            }
            catch (Exception e)
            {
                error = "Error to Retrieve Purchase InvoiceHdr Information.\n" + e.Message;
            }
            return invoiceHdrs;
        }

        public List<PurchaseInvoiceDetail1> getPurchaseInvoiceDetail
            (string PurchaseInvoiceCD, out string error)
        {
            error = "";
            List<PurchaseInvoiceDetail1> invoiceHdrs = new List<PurchaseInvoiceDetail1>();
            string sql = "exec get_purchaseinvoice_det @p_PurInvoiceCD;";
            var parameters = new DynamicParameters();
            parameters.Add("p_PurInvoiceCD", PurchaseInvoiceCD, DbType.String);
            try
            {
                invoiceHdrs = conn.Query<PurchaseInvoiceDetail1>(sql, param: parameters).ToList();
            }
            catch (Exception e)
            {
                error = "Error to Retrieve Purchase InvoiceHdr Information.\n" + e.Message;
            }
            return invoiceHdrs;
        }
        #endregion
        #region 'Update Region'
        public bool updateItemUnitAfterPurchase(string PurInoviceCD, out string error)
        {
            error = "";
            string sql = "exec update_itemunit_price_after_purchase_invoice @p_PurInvoiceCD";
            var parameters = new DynamicParameters();
            parameters.Add("p_PurInvoiceCD", PurInoviceCD, DbType.String);
            
            try
            {
                string Ins_CD = Convert.ToString(conn.ExecuteScalar(sql, param: parameters));
                return true;
            }
            catch (Exception e)
            {
                error = "error to update ItemUnit After Purchase" + e.Message;
            }
            return true;
        }
        #endregion
        #region 'Delete Region'
        public bool deletePurchaseInformation(string PurOrderNo, string PurInvoiceNo, out string error)
        {
            error = "";
            string sql = "exec delete_purchaseinformation @p_PurOrderNo,@p_PurInvoiceNo";
            var parameters = new DynamicParameters();
            parameters.Add("p_PurOrderNo", PurOrderNo, DbType.String);
            parameters.Add("p_PurInvoiceNo", PurInvoiceNo, DbType.String);
            try
            {
                string Ins_CD = Convert.ToString(conn.ExecuteScalar(sql, param: parameters));
                return true;
            }
            catch (Exception e)
            {
                error = "error to delete Purchase Information" + e.Message;
            }
            return true;
        }
        #endregion

    }
}
