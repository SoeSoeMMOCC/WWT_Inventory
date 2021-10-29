using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WWT_Inventory.Factory;
using WWT_Inventory.Model.Reports;
using WWT_Inventory.Service.Report;

namespace WWT_Inventory.Controller.Report
{
    public class InventoryReportsController:InventoryReportsInterface
    {
        IDbConnection conn;
        public InventoryReportsController()
        {
            conn = DBFactory.InvDB();
        }
        public List<RemainStock> getRemainStocks(string item_cd,string unit_cd,out string error)
        {
            error = "";
            List<RemainStock> remainStocks = new List<RemainStock>();
            string sql = "exec rpt_remainingstockqty @p_itemcd,@p_unitcd;";
            var parameters = new DynamicParameters();
            parameters.Add("p_itemcd", item_cd, DbType.String);
            parameters.Add("p_unitcd", unit_cd, DbType.String);
            try
            {
                remainStocks = conn.Query<RemainStock>(sql, param: parameters).ToList();
            }
            catch (Exception e)
            {
                error = "Error to Retrieve Stock Remaing Information.\n" + e.Message;
            }
            return remainStocks;
        }
        public List<ItemUnitSummary> getItemUnitSummary(string item_cd, string unit_cd, out string error)
        {
            error = "";
            List<ItemUnitSummary> itemUnitSummaries = new List<ItemUnitSummary>();
            string sql = "exec rpt_itemunitprice_summary @p_itemcd,@p_unitcd;";
            var parameters = new DynamicParameters();
            parameters.Add("p_itemcd", item_cd, DbType.String);
            parameters.Add("p_unitcd", unit_cd, DbType.String);
            try
            {
                itemUnitSummaries = conn.Query<ItemUnitSummary>(sql, param: parameters).ToList();
            }
            catch (Exception e)
            {
                error = "Error to ItemUnitSummary Information.\n" + e.Message;
            }
            return itemUnitSummaries;
        }

        public List<SaleDetailSummary> getSaleDetailSummaries(
            DateTime fromDate, 
            DateTime toDate, 
            string item_cd, 
            string unit_cd, out string error)
        {
            error = "";
            List<SaleDetailSummary> saleDetailSummaries = new List<SaleDetailSummary>();
            string sql = "exec rpt_saledetail_summary @p_unitcd,@p_itemcd,@p_fromdate,@p_todate;";
            var parameters = new DynamicParameters();
            parameters.Add("p_unitcd", unit_cd, DbType.String);
            parameters.Add("p_itemcd", item_cd, DbType.String);
            parameters.Add("p_fromdate", fromDate, DbType.DateTime);
            parameters.Add("p_todate", toDate, DbType.DateTime);
            try
            {
                saleDetailSummaries = conn.Query<SaleDetailSummary>(sql, param: parameters).ToList();
            }
            catch (Exception e)
            {
                error = "Error to Sale Summary Information.\n" + e.Message;
            }
            return saleDetailSummaries;
        }
        public List<SalePurchaseHistory> getSalePurchseHistories(DateTime fromDate, 
            DateTime toDate, 
            string item_cd, 
            string unit_cd, 
            out string error)
        {
            error = "";
            List<SalePurchaseHistory> salePurchaseHistories = new List<SalePurchaseHistory>();
            string sql = "exec rpt_salepurchase_history @p_fromdate,@p_todate,@p_itemcd,@p_unitcd;";
            var parameters = new DynamicParameters();
            parameters.Add("p_fromdate", fromDate, DbType.DateTime);
            parameters.Add("p_todate", toDate, DbType.DateTime);
            parameters.Add("p_itemcd", item_cd, DbType.String);
            parameters.Add("p_unitcd", unit_cd, DbType.String);

            try
            {
                salePurchaseHistories = conn.Query<SalePurchaseHistory>(sql, param: parameters).ToList();
            }
            catch (Exception e)
            {
                error = "Error to Sale Summary Information.\n" + e.Message;
            }
            return salePurchaseHistories;
        }
    }
}
