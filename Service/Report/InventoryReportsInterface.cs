using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WWT_Inventory.Model.Reports;

namespace WWT_Inventory.Service.Report
{
    public interface InventoryReportsInterface
    {
        List<RemainStock> getRemainStocks(string item_cd, string unit_cd, out string error);
        List<ItemUnitSummary> getItemUnitSummary(string item_cd, string unit_cd, out string error);
        List<SaleDetailSummary> getSaleDetailSummaries(DateTime fromDate,DateTime toDate,string item_cd, string unit_cd, out string error);
        List<SalePurchaseHistory> getSalePurchseHistories(DateTime fromDate,DateTime toDate,string item_cd, string unit_cd, out string error);
    }
}
