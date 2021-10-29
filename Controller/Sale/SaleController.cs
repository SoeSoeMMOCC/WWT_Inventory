using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WWT_Inventory.Controller.Inventory;
using WWT_Inventory.Factory;
using WWT_Inventory.Model.Sale;
using WWT_Inventory.Service.Sale;

namespace WWT_Inventory.Controller.Sale
{
    public class SaleController : SaleInterface
    {
        IDbConnection conn;
        InventoryController inventoryController;
        public SaleController()
        {
            conn = DBFactory.InvDB();
            inventoryController = new InventoryController();
        }
        #region 'Save Region'
        public bool saveSaleInformation(SaleOrderHdr orderHdr, 
            List<SaleOrderDetail> saleOrders, out string error)
        {
            error = "";
            string sql = "exec insert_saleorder_hdr @p_SaleOrderCD,@p_SaleOrderDate," +
                "@p_CustomerCD,@p_Qty,@p_TotalAmount,@p_DiscountAmount," +
                "@p_Tax,@p_Amount,@p_isactive,@p_DeviceID";
            var parameters = new DynamicParameters();
            parameters.Add("p_SaleOrderCD", orderHdr.SaleOrderCD, DbType.String);
            parameters.Add("p_SaleOrderDate", orderHdr.SaleOrderDate, DbType.DateTime);
            parameters.Add("p_CustomerCD", orderHdr.CustomerCD, DbType.String);
            parameters.Add("p_Qty", orderHdr.Qty, DbType.Int32);
            parameters.Add("p_TotalAmount", orderHdr.TotalAmount, DbType.Decimal);
            parameters.Add("p_Tax", orderHdr.Tax, DbType.Decimal);
            parameters.Add("p_DiscountAmount", orderHdr.DiscountAmount, DbType.Decimal);
            parameters.Add("p_Amount", orderHdr.Amount, DbType.Decimal);
            parameters.Add("p_isactive", orderHdr.isactive, DbType.Boolean);
            parameters.Add("p_DeviceID", orderHdr.DeviceID, DbType.String);
            try
            {
                string Ins_CD = Convert.ToString(conn.ExecuteScalar(sql, param: parameters));
                string N_SaleOrder = inventoryController.generateNoseries(WWT_Inventory.Properties.Settings.Default.DeviceID, "SaleInvoice", out error);
                CommonFactory.SaleInvoiceNo = N_SaleOrder;
                bool ret = saveSaleOrderDetail(orderHdr.SaleOrderCD, saleOrders, out error);
                if (ret)
                {
                    ret = saveSaleInvoiceHdr(N_SaleOrder, orderHdr, out error);
                    if (ret)
                    {
                        ret = saveSaleInvoiceDetail(N_SaleOrder, saleOrders, out error);
                        if (!ret)
                        {
                            return false;
                        }
                    }                       
                    else
                        return false;
                }
                else
                    return false;
                return true;
            }
            catch (Exception e)
            {
                error = "error to insert OrderHdr " + e.Message;
                return false;
            }
        }
        public bool saveSaleOrderDetail(string SaleOrderCD, List<SaleOrderDetail> saleOrders, out string error)
        {
            error = "";
            foreach(SaleOrderDetail orderDetail1 in saleOrders)
            {
                string sql = "exec insert_saleorder_detail @p_SaleDetailCD,@p_SaleOrderCD," +
                "@p_ItemCD,@p_UnitCD,@p_Qty,@p_Price,@p_DiscountPer," +
                "@p_DiscountAmt,@p_Amount,@p_TotalAmount,@p_isactive,@p_SaleTypeCD,@p_PurPrice";
                var parameters = new DynamicParameters();
                parameters.Add("p_SaleDetailCD", orderDetail1.SaleDetailCD, DbType.String);
                parameters.Add("p_SaleOrderCD", SaleOrderCD, DbType.String);
                parameters.Add("p_ItemCD", orderDetail1.ItemCD, DbType.String);
                parameters.Add("p_UnitCD", orderDetail1.UnitCD, DbType.String);
                parameters.Add("p_Qty", orderDetail1.Qty, DbType.Int32);
                parameters.Add("p_Price", orderDetail1.Price, DbType.Decimal);
                parameters.Add("p_DiscountPer", orderDetail1.DiscountPer, DbType.Decimal);
                parameters.Add("p_DiscountAmt", orderDetail1.DiscountAmt, DbType.Decimal);
                parameters.Add("p_Amount", orderDetail1.Amount, DbType.Decimal);
                parameters.Add("p_TotalAmount", orderDetail1.TotalAmount, DbType.Decimal);
                parameters.Add("p_isactive", orderDetail1.isactive, DbType.Boolean);
                parameters.Add("p_SaleTypeCD", orderDetail1.SaleTypeCD, DbType.String);
                parameters.Add("p_PurPrice", orderDetail1.PurPrice, DbType.Decimal);
                try
                {
                    string Ins_CD = Convert.ToString(conn.ExecuteScalar(sql, param: parameters));
                }
                catch (Exception e)
                {
                    error = "error to insert OrderDetail" + e.Message;
                    return false;
                }
            }
            return true;
        }

        public bool saveSaleInvoiceHdr(string SaleInvoiceCD, SaleOrderHdr saleInvoiceHdr, out string error)
        {
            error = "";
            string sql = "exec insert_saleinvoice_hdr @p_SaleInvoiceCD,@p_SaleOrderCD," +
                "@p_SaleInvoiceDate,@p_CustomerCD,@p_Qty,@p_TotalAmount," +
                "@p_DiscountAmount,@p_Tax,@p_Amount,@p_RemainAmount,@p_isactive,@p_DeviceID";
            var parameters = new DynamicParameters();
            parameters.Add("p_SaleInvoiceCD", SaleInvoiceCD, DbType.String);
            parameters.Add("p_SaleOrderCD", saleInvoiceHdr.SaleOrderCD, DbType.String);
            parameters.Add("p_SaleInvoiceDate", saleInvoiceHdr.SaleOrderDate, DbType.DateTime);
            parameters.Add("p_CustomerCD", saleInvoiceHdr.CustomerCD, DbType.String);
            parameters.Add("p_Qty", saleInvoiceHdr.Qty, DbType.Int32);
            parameters.Add("p_TotalAmount", saleInvoiceHdr.TotalAmount, DbType.Decimal);
            parameters.Add("p_DiscountAmount", saleInvoiceHdr.DiscountAmount, DbType.Decimal);
            parameters.Add("p_Tax", saleInvoiceHdr.Tax, DbType.Decimal);
            parameters.Add("p_Amount", saleInvoiceHdr.Amount, DbType.Decimal);
            parameters.Add("p_RemainAmount", 0, DbType.Decimal);
            parameters.Add("p_isactive", saleInvoiceHdr.isactive, DbType.Boolean);
            parameters.Add("p_DeviceID", saleInvoiceHdr.DeviceID, DbType.String);
            try
            {
                string Ins_CD = Convert.ToString(conn.ExecuteScalar(sql, param: parameters));
                return true;
            }
            catch (Exception e)
            {
                error = "error to insert Invoice Hdr " + e.Message;
                return false;
            }
        }

        public bool saveSaleInvoiceDetail(string SaleInvoiceCD, List<SaleOrderDetail> saleInvoices, out string error)
        {
            error = "";
            foreach (SaleOrderDetail saleInvoice in saleInvoices)
            {
                string sql = "exec insert_saleinvoice_detail @p_SaleDetailCD,@p_SaleInvoiceCD," +
                    "@p_ItemCD,@p_UnitCD,@p_Qty,@p_Price,@p_DiscountPer,@p_DiscountAmount," +
                    "@p_Amount,@p_TotalAmount,@p_isactive,@p_SaleTypeCD,@p_PurPrice";
                var parameters = new DynamicParameters();
                parameters.Add("p_SaleDetailCD", Guid.NewGuid().ToString().Replace("-", ""), DbType.String);
                parameters.Add("p_SaleInvoiceCD", SaleInvoiceCD, DbType.String);
                parameters.Add("p_ItemCD", saleInvoice.ItemCD, DbType.String);
                parameters.Add("p_UnitCD", saleInvoice.UnitCD, DbType.String);
                parameters.Add("p_Qty", saleInvoice.Qty, DbType.Int32);
                parameters.Add("p_Price", saleInvoice.Price, DbType.Decimal);
                parameters.Add("p_DiscountPer", saleInvoice.DiscountPer, DbType.Decimal);
                parameters.Add("p_DiscountAmount", saleInvoice.DiscountAmt, DbType.Decimal);
                parameters.Add("p_Amount", saleInvoice.Amount, DbType.Decimal);
                parameters.Add("p_TotalAmount", saleInvoice.TotalAmount, DbType.Decimal);
                parameters.Add("p_isactive", saleInvoice.isactive, DbType.Boolean);
                parameters.Add("p_SaleTypeCD", saleInvoice.SaleTypeCD, DbType.String);
                parameters.Add("p_PurPrice", saleInvoice.PurPrice, DbType.Decimal);
                try
                {
                    string Ins_CD = Convert.ToString(conn.ExecuteScalar(sql, param: parameters));
                }
                catch (Exception e)
                {
                    error = "error to insert OrderDetail" + e.Message;
                    return false;
                }
            }
            return true;
        }

        public bool saveCustomer(Customer customer, out string error)
        {
            error = "";
            string sql = "exec insert_customer @p_CustCD,@p_CustName,@p_CityCD,@p_Address,@p_PhoneNo,@p_isactive";
            var parameters = new DynamicParameters();
            parameters.Add("p_CustCD", customer.CustCD, DbType.String);
            parameters.Add("p_CustName", customer.CustName, DbType.String);
            parameters.Add("p_CityCD", customer.CityCD, DbType.String);
            parameters.Add("p_Address", customer.Address, DbType.String);
            parameters.Add("p_PhoneNo", customer.PhoneNo, DbType.String);
            parameters.Add("p_isactive", customer.isactive, DbType.Boolean);
            try
            {
                string Ins_CD = Convert.ToString(conn.ExecuteScalar(sql, param: parameters));
                return true;
            }
            catch (Exception e)
            {
                error = "error to insert Customer " + e.Message;
                return false;
            }
        }

        public bool updateCustomer(Customer customer, out string error)
        {
            error = "";
            string sql = "exec update_customer @p_CustCD,@p_CustName,@p_CityCD,@p_Address,@p_PhoneNo,@p_isactive";
            var parameters = new DynamicParameters();
            parameters.Add("p_CustCD", customer.CustCD, DbType.String);
            parameters.Add("p_CustName", customer.CustName, DbType.String);
            parameters.Add("p_CityCD", customer.CityCD, DbType.String);
            parameters.Add("p_Address", customer.Address, DbType.String);
            parameters.Add("p_PhoneNo", customer.PhoneNo, DbType.String);
            parameters.Add("p_isactive", customer.isactive, DbType.Boolean);
            try
            {
                string Ins_CD = Convert.ToString(conn.ExecuteScalar(sql, param: parameters));
                return true;
            }
            catch (Exception e)
            {
                error = "error to update Customer " + e.Message;
                return false;
            }
        }
        #endregion

        #region 'Get Region'
        public List<SaleInvoiceHdr> GetSaleInvoices(DateTime fromDate, 
            DateTime toDate, string SaleInoivceCD, string CustCD, out string error)
        {
            error = "";
            List<SaleInvoiceHdr> invoiceHdrs = new List<SaleInvoiceHdr>();
            string sql = "exec get_saleinvoices @p_fromdate,@p_todate," +
                "@p_SaleInvoiceCD,@p_CustomerCD;";
            var parameters = new DynamicParameters();
            parameters.Add("p_fromdate", fromDate, DbType.DateTime);
            parameters.Add("p_todate", toDate, DbType.DateTime);
            parameters.Add("p_SaleInvoiceCD", SaleInoivceCD, DbType.String);
            parameters.Add("p_CustomerCD", CustCD, DbType.String);
            try
            {
                invoiceHdrs = conn.Query<SaleInvoiceHdr>(sql, param: parameters).ToList();
            }
            catch (Exception e)
            {
                error = "Error to Retrieve Sale InvoiceHdr Information.\n" + e.Message;
            }
            return invoiceHdrs;
        }
        public List<Customer> GetCustomers(string CustCD, out string error)
        {
            error = "";
            List<Customer> invoiceHdrs = new List<Customer>();
            string sql = "exec get_customer @p_CustCD;";
            var parameters = new DynamicParameters();
            parameters.Add("p_CustCD", CustCD, DbType.String);
            try
            {
                invoiceHdrs = conn.Query<Customer>(sql, param: parameters).ToList();
            }
            catch (Exception e)
            {
                error = "Error to Retrieve Sale InvoiceHdr Information.\n" + e.Message;
            }
            return invoiceHdrs;
        }

        public List<SaleTypes> GetSaleTypes(string SaleTypeCD, out string error)
        {
            error = "";
            List<SaleTypes> saleTypes = new List<SaleTypes>();
            string sql = "exec get_saletypes @p_SaleTypeCD;";
            var parameters = new DynamicParameters();
            parameters.Add("p_SaleTypeCD", SaleTypeCD, DbType.String);
            try
            {
                saleTypes = conn.Query<SaleTypes>(sql, param: parameters).ToList();
            }
            catch (Exception e)
            {
                error = "Error to Retrieve Sale Types Information.\n" + e.Message;
            }
            return saleTypes;
        }

        public SaleInvoiceHdr getSaleInvoiceHdr
           (string SaleInoivceCD, out string error)
        {
            error = "";
            SaleInvoiceHdr invoiceHdrs = new SaleInvoiceHdr();
            string sql = "exec get_saleinvoice_hdr @p_SaleInvoiceCD;";
            var parameters = new DynamicParameters();
            parameters.Add("p_SaleInvoiceCD", SaleInoivceCD, DbType.String);
            try
            {
                invoiceHdrs = conn.Query<SaleInvoiceHdr>(sql, param: parameters).FirstOrDefault();
            }
            catch (Exception e)
            {
                error = "Error to Retrieve Sale InvoiceHdr Information.\n" + e.Message;
            }
            return invoiceHdrs;
        }
        public List<SaleInvoiceDetail> getSaleInvoiceDetails
            (string SaleInvoiceCD, out string error)
        {
            error = "";
            List<SaleInvoiceDetail> invoiceDetails = new List<SaleInvoiceDetail>();
            string sql = "exec get_saleinvoice_det @p_SaleInvoiceCD;";
            var parameters = new DynamicParameters();
            parameters.Add("p_SaleInvoiceCD", SaleInvoiceCD, DbType.String);
            try
            {
                invoiceDetails = conn.Query<SaleInvoiceDetail>(sql, param: parameters).ToList();
            }
            catch (Exception e)
            {
                error = "Error to Retrieve Purchase InvoiceHdr Information.\n" + e.Message;
            }
            return invoiceDetails;
        }
        #endregion

        #region 'Delete Region'
        public bool  deleteSaleInformation(string SaleOrderNo, string SaleInvoiceNo, out string error)
        {
            error = "";
            string sql = "exec delete_saleinformation @p_SaleOrderNo,@p_SaleInoviceNo";
            var parameters = new DynamicParameters();
            parameters.Add("p_SaleOrderNo", SaleOrderNo, DbType.String);
            parameters.Add("p_SaleInoviceNo", SaleInvoiceNo, DbType.String);
            try
            {
                string Ins_CD = Convert.ToString(conn.ExecuteScalar(sql, param: parameters));
                return true;
            }
            catch (Exception e)
            {
                error = "error to delete Sale Information" + e.Message;
            }
            return true;
        }
        #endregion
    }
}
