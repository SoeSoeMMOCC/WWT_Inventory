using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WWT_Inventory.Model.Inventory;

namespace WWT_Inventory.Service.Inventory
{
    public interface InventoryInterface
    {
        #region "Get Regions"
        List<Item> getInventoryItemList(string CategoryCD, string SubCategoryCD, out string error);
        List<Item> getInventoryItemFilterList(string CategoryCD, string SubCategoryCD,string ItemName,string ShortCode, out string error);
        List<Category> getCategories(string CategoryCD, out string error);
        List<SubCategory> getSubCategories(string CategoryCD, string SubCategoryCD, out string error);
        List<Unit> getUnits(string UnitCD, out string error);
        List<Supplier> getSuppliers(string SupplierCD, out string error);
        List<City> getCities(string CityCD, out string error);
        List<ItemUnit> getItemUnits(string ItemCD,string UnitCD, out string error);
        List<Stock> getStockList(string StockCD, string ItemCD, out string error);
        List<Item> checkItemInfo(string ItemName, string ShortCode, out string error);
        #endregion

        #region "Insert Region"
        bool saveItem(Item item, out string error);
        bool saveItemUnit(ItemUnit itemUnit, out string error);
        bool saveSupplier(Supplier supplier, out string error);
        bool saveCategory(Category category, out string error);
        bool saveSubCategory(SubCategory subCategory, out string error);
        bool saveUnit(Unit unit, out string error);
        bool saveStock(Stock stock, out string error);
        #endregion


        #region "Delete Region"
        bool deleteItem(Item item, out string error);
        bool deleteCategory(Category category, out string error);
        bool deleteSubCategory(SubCategory subcategory, out string error);
        bool deleteUnit(Unit unit, out string error);
        bool deleteItemUnit(ItemUnit unit, out string error);
        bool deleteSupplier(Supplier supplier, out string error);
        #endregion
        #region "Update Region"
        bool updateItemData(Item item, out string error);
        bool updateCategoryData(Category category, out string error);
        bool updateSubCategoryData(SubCategory subCategory, out string error);
        bool updateUnit(Unit unit, out string error);
        bool updateItemUnit(ItemUnit oldItemUnit,ItemUnit itemUnit, out string error);
        bool updateSupplier(Supplier supplier, out string error);
        bool updateStock(Stock stock, out string error);
        #endregion
        string generateNoseries(string DeviceID, string ModuleName, out string error);
    }
}
