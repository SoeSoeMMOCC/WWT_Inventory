using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WWT_Inventory.Factory;
using WWT_Inventory.Model.Inventory;
using WWT_Inventory.Service.Inventory;

namespace WWT_Inventory.Controller.Inventory
{
    public class InventoryController : InventoryInterface
    {
        IDbConnection conn;
        public InventoryController()
        {
            conn = DBFactory.InvDB();
        }
        #region 'GetRegion'
        public List<Item> getInventoryItemList(string CategoryCD, string SubCategoryCD, out string error)
        {
            error = "";
            List<Item> items = new List<Item>();
            string sql = "exec get_itemlist @p_CategoryCD, @p_SubCategoryCD;";
            var parameters = new DynamicParameters();
            parameters.Add("p_CategoryCD", CategoryCD, DbType.String);
            parameters.Add("p_SubCategoryCD", SubCategoryCD, DbType.String);
            try
            {
                items = conn.Query<Item>(sql, param: parameters).ToList();
            }
            catch (Exception e)
            {
                error = "Error to Retrieve Item Information.\n" + e.Message;
            }
            return items;
        }

        public List<Item> getInventoryItemFilterList(string CategoryCD, 
            string SubCategoryCD, string ItemName, string ShortCode, out string error)
        {
            error = "";
            List<Item> items = new List<Item>();
            string sql = "exec get_itemlist_filter @p_CategoryCD, @p_SubCategoryCD,@p_Item,@p_ShortCode;";
            var parameters = new DynamicParameters();
            parameters.Add("p_CategoryCD", CategoryCD, DbType.String);
            parameters.Add("p_SubCategoryCD", SubCategoryCD, DbType.String);
            parameters.Add("p_Item", ItemName, DbType.String);
            parameters.Add("p_ShortCode", ShortCode, DbType.String);
            try
            {
                items = conn.Query<Item>(sql, param: parameters).ToList();
            }
            catch (Exception e)
            {
                error = "Error to Retrieve Item Information.\n" + e.Message;
            }
            return items;
        }
        public List<Category> getCategories(string CategoryCD, out string error)
        {            
            error = "";
            List<Category> categories = new List<Category>();
            string sql = "exec get_category @p_CategoryCD;";
            var parameters = new DynamicParameters();
            parameters.Add("p_CategoryCD", CategoryCD, DbType.String);
            try
            {
                categories = conn.Query<Category>(sql, param: parameters).ToList();
            }
            catch (Exception e)
            {
                error = "Error to Retrieve Category Information.\n" + e.Message;
            }
            return categories;
        }
        public List<SubCategory> getSubCategories(string CategoryCD, string SubCategoryCD, out string error)
        {
            error = "";
            List<SubCategory> subCategories = new List<SubCategory>();
            string sql = "exec get_subcategory @p_SubCategoryCD,@p_CategoryCD;";
            var parameters = new DynamicParameters();
            parameters.Add("p_SubCategoryCD", SubCategoryCD, DbType.String);
            parameters.Add("p_CategoryCD", CategoryCD, DbType.String);
            try
            {
                subCategories = conn.Query<SubCategory>(sql, param: parameters).ToList();
            }
            catch (Exception e)
            {
                error = "Error to Retrieve SubCategory Information.\n" + e.Message;
            }
            return subCategories;
        }
        public List<Unit> getUnits(string UnitCD, out string error)
        {
            error = "";
            List<Unit> units = new List<Unit>();
            string sql = "exec get_unitlist @p_UnitCD;";
            var parameters = new DynamicParameters();
            parameters.Add("p_UnitCD", UnitCD, DbType.String);
            try
            {
                units = conn.Query<Unit>(sql, param: parameters).ToList();
            }
            catch (Exception e)
            {
                error = "Error to Retrieve Unit Information.\n" + e.Message;
            }
            return units;
        }
        public List<Supplier> getSuppliers(string SupplierCD, out string error)
        {
            error = "";
            List<Supplier> suppliers = new List<Supplier>();
            string sql = "exec get_supplier @p_SupplierCD;";
            var parameters = new DynamicParameters();
            parameters.Add("p_SupplierCD", SupplierCD, DbType.String);
            try
            {
                suppliers = conn.Query<Supplier>(sql, param: parameters).ToList();
            }
            catch (Exception e)
            {
                error = "Error to Retrieve Supplier Information.\n" + e.Message;
            }
            return suppliers;
        }

        public List<City> getCities(string CityCD, out string error)
        {
            error = "";
            List<City> cities = new List<City>();
            string sql = "exec get_city @p_CityCD;";
            var parameters = new DynamicParameters();
            parameters.Add("p_CityCD", CityCD, DbType.String);
            try
            {
                cities = conn.Query<City>(sql, param: parameters).ToList();
            }
            catch (Exception e)
            {
                error = "Error to Retrieve City Information.\n" + e.Message;
            }
            return cities;
        }
        public List<ItemUnit> getItemUnits(string ItemCD,string UnitCD, out string error)
        {
            error = "";
            List<ItemUnit> units = new List<ItemUnit>();
            string sql = "exec get_itemunit @p_ItemCD,@p_UnitCD;";
            var parameters = new DynamicParameters();
            parameters.Add("p_ItemCD", ItemCD, DbType.String);
            parameters.Add("p_UnitCD", UnitCD, DbType.String);
            try
            {
                units = conn.Query<ItemUnit>(sql, param: parameters).ToList();
            }
            catch (Exception e)
            {
                error = "Error to Retrieve Unit Information.\n" + e.Message;
            }
            return units;
        }
        public List<Stock> getStockList(string StockCD, string ItemCD, out string error)
        {
            error = "";
            List<Stock> stocks = new List<Stock>();
            string sql = "exec get_inventorystock @p_StockCD,@p_ItemCD;";
            var parameters = new DynamicParameters();
            parameters.Add("p_StockCD", StockCD, DbType.String);
            parameters.Add("p_ItemCD", ItemCD, DbType.String);
            try
            {
                stocks = conn.Query<Stock>(sql, param: parameters).ToList();
            }
            catch (Exception e)
            {
                error = "Error to Retrieve Stock Information.\n" + e.Message;
            }
            return stocks;
        }
        public List<Item> checkItemInfo(string ItemName, string ShortCode, out string error)
        {
            error = "";
            List<Item> items = new List<Item>();
            string sql = "exec check_iteminfo_beforecheck @item_name,@short_code;";
            var parameters = new DynamicParameters();
            parameters.Add("item_name", ItemName, DbType.String);
            parameters.Add("short_code", ShortCode, DbType.String);
            try
            {
                items = conn.Query<Item>(sql, param: parameters).ToList();
            }
            catch (Exception e)
            {
                error = "Error to Retrieve Item Check Information.\n" + e.Message;
            }
            return items;
        }
        #endregion
        #region 'Save Region'
        public string generateNoseries(string DeviceID, string ModuleName, out string error)
        {
            error = "";
            string New_ID = "";
            string sql = "exec generating_noseries @DeviceID,@ModuleName;";
            var parameters = new DynamicParameters();
            parameters.Add("DeviceID", DeviceID, DbType.String);
            parameters.Add("ModuleName", ModuleName, DbType.String);
            try
            {
                New_ID = conn.Query<string>(sql, param: parameters).FirstOrDefault();
            }
            catch (Exception e)
            {
                error = "Error to Generate Noseries.\n" + e.Message;
            }
            return New_ID;
        }
        public bool saveItem(Item item, out string error)
        {
            error = "";
            string sql = "exec insert_item @p_ItemCD,@p_ItemName,@p_UnitCD,@p_SalePrice," +
                "@p_PurPrice,@p_MinQty,@p_MaxQty,@p_isactive,@p_CategoryCD," +
                "@p_SubCategoryCD,@p_SupplierCD,@p_ShortCode";
            var parameters = new DynamicParameters();
            parameters.Add("p_ItemCD", item.ItemCD, DbType.String);
            parameters.Add("p_ItemName", item.ItemName, DbType.String);
            parameters.Add("p_UnitCD", item.UnitCD, DbType.String);
            parameters.Add("p_SalePrice", item.SalePrice, DbType.Decimal);
            parameters.Add("p_PurPrice", item.PurPrice, DbType.Decimal);
            parameters.Add("p_MinQty", item.MinQty, DbType.Int32);
            parameters.Add("p_MaxQty", item.MaxQty, DbType.Int32);
            parameters.Add("p_isactive", item.isactive, DbType.Boolean);
            parameters.Add("p_CategoryCD", item.CategoryCD, DbType.String);
            parameters.Add("p_SubCategoryCD", item.SubCategoryCD, DbType.String);
            parameters.Add("p_SupplierCD", item.SupplierCD, DbType.String);
            parameters.Add("p_ShortCode", item.ShortCode, DbType.String);
            try
            {
                string Ins_CD = Convert.ToString(conn.ExecuteScalar(sql, param: parameters));

                /* Saving ItemUnit Information */
                #region 'Saveing ItemUNit Information of Item'
                ItemUnit itemUnit = new ItemUnit();
                itemUnit.ItemCD = item.ItemCD;
                itemUnit.UnitCD = item.UnitCD;
                itemUnit.BaseQty = 1;
                itemUnit.SalePrice = item.SalePrice;
                itemUnit.PurPrice = item.PurPrice;
                itemUnit.isactive = true;
                itemUnit.isBase = true;
                bool ret = saveItemUnit(itemUnit, out error);
                #endregion

                return true;
            }
            catch (Exception e)
            {
                error = "error to insert Item"+e.Message;
            }
            return true;
        }
        public bool saveItemUnit(ItemUnit itemUnit, out string error)
        {
            error = "";
            string sql = "exec insert_itemunit @p_ItemCD,@p_UnitCD," +
                "@p_BaseQty,@p_SalePrice,@p_PurPrice,@p_isBase,@p_isactive";
            var parameters = new DynamicParameters();
            parameters.Add("p_ItemCD", itemUnit.ItemCD, DbType.String);
            parameters.Add("p_UnitCD", itemUnit.UnitCD, DbType.String);
            parameters.Add("p_BaseQty", itemUnit.BaseQty, DbType.Int32);
            parameters.Add("p_SalePrice", itemUnit.SalePrice, DbType.Decimal);
            parameters.Add("p_PurPrice", itemUnit.PurPrice, DbType.Decimal);
            parameters.Add("p_isBase", itemUnit.isBase, DbType.Boolean);
            parameters.Add("p_isactive", itemUnit.isactive, DbType.Boolean);
            try
            {
                string Ins_CD = Convert.ToString(conn.ExecuteScalar(sql, param: parameters));
                return true;
            }
            catch (Exception e)
            {
                error = "error to insert Item Unit" + e.Message;
            }
            return true;
        }
        public bool saveSupplier(Supplier supplier, out string error)
        {
            error = "";
            string sql = "exec insert_supplier @p_SupplierCD,@p_SupplierName," +
                "@p_CityCD,@p_isactive,@p_Address,@p_PhoneNo,@p_Email";
            var parameters = new DynamicParameters();
            parameters.Add("p_SupplierCD", supplier.SupplierCD, DbType.String);
            parameters.Add("p_SupplierName", supplier.SupplierName, DbType.String);
            parameters.Add("p_CityCD", supplier.CityCD, DbType.String);
            parameters.Add("p_isactive", supplier.isactive, DbType.Boolean);
            parameters.Add("p_Address", supplier.Address, DbType.String);
            parameters.Add("p_PhoneNo", supplier.PhoneNo, DbType.String);
            parameters.Add("p_Email", supplier.Email, DbType.String);
            try
            {
                string Ins_CD = Convert.ToString(conn.ExecuteScalar(sql, param: parameters));
                return true;
            }
            catch (Exception e)
            {
                error = "error to insert Supplier" + e.Message;
            }
            return true;
        }

        public bool saveCategory(Category category, out string error)
        {
            error = "";
            string sql = "exec insert_category @p_CategoryCD,@p_CategoryName,@p_isactive";
            var parameters = new DynamicParameters();
            parameters.Add("p_CategoryCD", category.CategoryCD, DbType.String);
            parameters.Add("p_CategoryName", category.CategoryName, DbType.String);
            parameters.Add("p_isactive", category.isactive, DbType.Boolean);
            try
            {
                string Ins_CD = Convert.ToString(conn.ExecuteScalar(sql, param: parameters));
                return true;
            }
            catch (Exception e)
            {
                error = "error to insert Category" + e.Message;
            }
            return true;
        }
        public bool saveSubCategory(SubCategory subCategory, out string error)
        {
            error = "";
            string sql = "exec insert_subcategory @p_SubCategoryCD,@p_SubCategoryName,@p_CategoryCD,@p_isactive";
            var parameters = new DynamicParameters();
            parameters.Add("p_SubCategoryCD", subCategory.SubCategoryCD, DbType.String);
            parameters.Add("p_SubCategoryName", subCategory.SubCategoryName, DbType.String);
            parameters.Add("p_CategoryCD", subCategory.CategoryCD, DbType.String);
            parameters.Add("p_isactive", subCategory.isactive, DbType.Boolean);
            try
            {
                string Ins_CD = Convert.ToString(conn.ExecuteScalar(sql, param: parameters));
                return true;
            }
            catch (Exception e)
            {
                error = "error to insert SubCategory" + e.Message;
            }
            return true;
        }
        public bool saveUnit(Unit unit, out string error)
        {
            error = "";
            string sql = "exec insert_unit @p_UnitCD,@p_UnitName,@p_BaseQty,@p_isactive";
            var parameters = new DynamicParameters();
            parameters.Add("p_UnitCD", unit.UnitCD, DbType.String);
            parameters.Add("p_UnitName", unit.UnitName, DbType.String);
            parameters.Add("p_BaseQty", 1, DbType.String);
            parameters.Add("p_isactive", unit.isactive, DbType.Boolean);
            try
            {
                string Ins_CD = Convert.ToString(conn.ExecuteScalar(sql, param: parameters));
                return true;
            }
            catch (Exception e)
            {
                error = "error to insert Unit" + e.Message;
            }
            return true;
        }
        public bool saveStock(Stock stock, out string error)
        {
            error = "";
            string sql = "exec insert_inventorystock @p_StockCD,@p_ItemCD,@p_SaleQty," +
                "@p_PurQty,@p_ActualQty,@p_UnitCD,@p_AdjQty,@p_isactive";
            var parameters = new DynamicParameters();
            parameters.Add("p_StockCD", stock.StockCD, DbType.String);
            parameters.Add("p_ItemCD", stock.ItemCD, DbType.String);
            parameters.Add("p_SaleQty", stock.SaleQty, DbType.Decimal);
            parameters.Add("p_PurQty", stock.PurQty, DbType.Decimal);
            parameters.Add("p_ActualQty", stock.ActualQty, DbType.Decimal);
            parameters.Add("p_UnitCD", stock.UnitCD, DbType.String);
            parameters.Add("p_AdjQty", stock.AdjQty, DbType.Decimal);
            parameters.Add("p_isactive", stock.isactive, DbType.Boolean);
            try
            {
                string Ins_CD = Convert.ToString(conn.ExecuteScalar(sql, param: parameters));
                return true;
            }
            catch (Exception e)
            {
                error = "error to insert Stock" + e.Message;
            }
            return true;
        }
        #endregion

        #region 'Delete Region'

        public bool deleteItem(Item item, out string error)
        {
            error = "";
            string sql = "exec update_item @p_ItemCD;";
            var parameter = new DynamicParameters();
            parameter.Add("p_ItemCD", item.ItemCD, DbType.String);
            try
            {
                int ret = conn.Execute(sql, param: parameter);                
            }
            catch (Exception e)
            {
                error = e.Message;
                return false;
            }
            return true;
        }
        public bool deleteCategory(Category category, out string error)
        {
            error = "";
            string sql = "exec update_category @p_CategoryCD;";
            var parameter = new DynamicParameters();
            parameter.Add("p_CategoryCD", category.CategoryCD, DbType.String);
            try
            {
                int ret = conn.Execute(sql, param: parameter);
            }
            catch (Exception e)
            {
                error = e.Message;
                return false;
            }
            return true;
        }
        public bool deleteSubCategory(SubCategory subcategory, out string error)
        {
            error = "";
            string sql = "exec update_subcategory @p_SubCategoryCD;";
            var parameter = new DynamicParameters();
            parameter.Add("p_SubCategoryCD", subcategory.SubCategoryCD, DbType.String);
            try
            {
                int ret = conn.Execute(sql, param: parameter);
            }
            catch (Exception e)
            {
                error = e.Message;
                return false;
            }
            return true;
        }

        public bool deleteUnit(Unit unit, out string error)
        {
            error = "";
            string sql = "exec update_unit @p_UnitCD;";
            var parameter = new DynamicParameters();
            parameter.Add("p_UnitCD", unit.UnitCD, DbType.String);
            try
            {
                int ret = conn.Execute(sql, param: parameter);
            }
            catch (Exception e)
            {
                error = e.Message;
                return false;
            }
            return true;
        }
        public bool deleteItemUnit(ItemUnit unit, out string error)
        {
            error = "";
            string sql = "exec update_itemunit @p_ItemCD,@p_UnitCD;";
            var parameter = new DynamicParameters();
            parameter.Add("p_ItemCD", unit.ItemCD, DbType.String);
            parameter.Add("p_UnitCD", unit.UnitCD, DbType.String);
            try
            {
                int ret = conn.Execute(sql, param: parameter);
            }
            catch (Exception e)
            {
                error = e.Message;
                return false;
            }
            return true;
        }
        public bool deleteSupplier(Supplier supplier, out string error)
        {
            error = "";
            string sql = "exec update_supplier @p_SupplierCD;";
            var parameter = new DynamicParameters();
            parameter.Add("p_SupplierCD", supplier.SupplierCD, DbType.String);
            try
            {
                int ret = conn.Execute(sql, param: parameter);
            }
            catch (Exception e)
            {
                error = e.Message;
                return false;
            }
            return true;
        }
        #endregion
        #region 'Update Region'
        public bool updateItemData(Item item, out string error)
        {
            error = "";
            string sql = "exec update_itemdata @p_ItemCD,@p_ItemName,@p_UnitCD,@p_SalePrice," +
                "@p_PurPrice,@p_MinQty,@p_MaxQty,@p_isactive,@p_CategoryCD," +
                "@p_SubCategoryCD,@p_SupplierCD,@p_ShortCode";
            var parameters = new DynamicParameters();
            parameters.Add("p_ItemCD", item.ItemCD, DbType.String);
            parameters.Add("p_ItemName", item.ItemName, DbType.String);
            parameters.Add("p_UnitCD", item.UnitCD, DbType.String);
            parameters.Add("p_SalePrice", item.SalePrice, DbType.Decimal);
            parameters.Add("p_PurPrice", item.PurPrice, DbType.Decimal);
            parameters.Add("p_MinQty", item.MinQty, DbType.Int32);
            parameters.Add("p_MaxQty", item.MaxQty, DbType.Int32);
            parameters.Add("p_isactive", item.isactive, DbType.Boolean);
            parameters.Add("p_CategoryCD", item.CategoryCD, DbType.String);
            parameters.Add("p_SubCategoryCD", item.SubCategoryCD, DbType.String);
            parameters.Add("p_SupplierCD", item.SupplierCD, DbType.String);
            parameters.Add("p_ShortCode", item.ShortCode, DbType.String);
            try
            {
                string Ins_CD = Convert.ToString(conn.ExecuteScalar(sql, param: parameters));

                /* Saving ItemUnit Information */
                #region 'Saveing ItemUNit Information of Item'
                //ItemUnit itemUnit = new ItemUnit();
                //itemUnit.ItemCD = item.ItemCD;
                //itemUnit.UnitCD = item.UnitCD;
                //itemUnit.BaseQty = 1;
                //itemUnit.SalePrice = item.SalePrice;
                //itemUnit.PurPrice = item.PurPrice;
                //itemUnit.isactive = true;
                //itemUnit.isBase = true;
                //bool ret = updateItemUnit(itemUnit, out error);
                #endregion

                return true;
            }
            catch (Exception e)
            {
                error = "error to update Item" + e.Message;
            }
            return true;
        }
        public bool updateCategoryData(Category category, out string error)
        {
            error = "";
            string sql = "exec update_category_data @p_CategoryCD,@p_CategoryName,@p_isactive";
            var parameters = new DynamicParameters();
            parameters.Add("p_CategoryCD", category.CategoryCD, DbType.String);
            parameters.Add("p_CategoryName", category.CategoryName, DbType.String);
            parameters.Add("p_isactive", category.isactive, DbType.Boolean);
            try
            {
                string Ins_CD = Convert.ToString(conn.ExecuteScalar(sql, param: parameters));
                return true;
            }
            catch (Exception e)
            {
                error = "error to update Category" + e.Message;
            }
            return true;
        }
        public bool updateSubCategoryData(SubCategory subCategory, out string error)
        {
            error = "";
            string sql = "exec update_subcategory_data @p_SubCategoryCD,@p_SubCategoryName,@p_CategoryCD,@p_isactive";
            var parameters = new DynamicParameters();
            parameters.Add("p_SubCategoryCD", subCategory.SubCategoryCD, DbType.String);
            parameters.Add("p_SubCategoryName", subCategory.SubCategoryName, DbType.String);
            parameters.Add("p_CategoryCD", subCategory.CategoryCD, DbType.String);
            parameters.Add("p_isactive", subCategory.isactive, DbType.Boolean);
            try
            {
                string Ins_CD = Convert.ToString(conn.ExecuteScalar(sql, param: parameters));
                return true;
            }
            catch (Exception e)
            {
                error = "error to update SubCategory" + e.Message;
            }
            return true;
        }
        public bool updateUnit(Unit unit, out string error)
        {
            error = "";
            string sql = "exec update_unit_data @p_UnitCD,@p_UnitName,@p_BaseQty,@p_isactive";
            var parameters = new DynamicParameters();
            parameters.Add("p_UnitCD", unit.UnitCD, DbType.String);
            parameters.Add("p_UnitName", unit.UnitName, DbType.String);
            parameters.Add("p_BaseQty", 1, DbType.String);
            parameters.Add("p_isactive", unit.isactive, DbType.Boolean);
            try
            {
                string Ins_CD = Convert.ToString(conn.ExecuteScalar(sql, param: parameters));
                return true;
            }
            catch (Exception e)
            {
                error = "error to update Unit" + e.Message;
            }
            return true;
        }

        public bool updateItemUnit(ItemUnit oldItemUnit,ItemUnit itemUnit, out string error)
        {
            error = "";
            string sql = "exec update_itemunit_data @p_ItemCD,@p_UnitCD,@p_OldUnitCD," +
                "@p_BaseQty,@p_SalePrice,@p_PurPrice,@p_isBase,@p_isactive";
            var parameters = new DynamicParameters();
            parameters.Add("p_ItemCD", itemUnit.ItemCD, DbType.String);
            parameters.Add("p_UnitCD", itemUnit.UnitCD, DbType.String);
            parameters.Add("p_OldUnitCD", oldItemUnit.UnitCD, DbType.String);
            parameters.Add("p_BaseQty", itemUnit.BaseQty, DbType.Int32);
            parameters.Add("p_SalePrice", itemUnit.SalePrice, DbType.Decimal);
            parameters.Add("p_PurPrice", itemUnit.PurPrice, DbType.Decimal);
            parameters.Add("p_isBase", itemUnit.isBase, DbType.Boolean);
            parameters.Add("p_isactive", itemUnit.isactive, DbType.Boolean);
            try
            {
                string Ins_CD = Convert.ToString(conn.ExecuteScalar(sql, param: parameters));
                return true;
            }
            catch (Exception e)
            {
                error = "error to update Item Unit" + e.Message;
            }
            return true;
        }
        public bool updateSupplier(Supplier supplier, out string error)
        {
            error = "";
            string sql = "exec update_supplierdata @p_SupplierCD,@p_SupplierName," +
                "@p_CityCD,@p_isactive,@p_Address,@p_PhoneNo,@p_Email";
            var parameters = new DynamicParameters();
            parameters.Add("p_SupplierCD", supplier.SupplierCD, DbType.String);
            parameters.Add("p_SupplierName", supplier.SupplierName, DbType.String);
            parameters.Add("p_CityCD", supplier.CityCD, DbType.String);
            parameters.Add("p_isactive", supplier.isactive, DbType.Boolean);
            parameters.Add("p_Address", supplier.Address, DbType.String);
            parameters.Add("p_PhoneNo", supplier.PhoneNo, DbType.String);
            parameters.Add("p_Email", supplier.Email, DbType.String);
            try
            {
                string Ins_CD = Convert.ToString(conn.ExecuteScalar(sql, param: parameters));
                return true;
            }
            catch (Exception e)
            {
                error = "error to update Supplier" + e.Message;
            }
            return true;
        }
        public bool updateStock(Stock stock, out string error)
        {
            error = "";
            string sql = "exec update_inventorystock @p_StockCD,@p_ItemCD,@p_SaleQty," +
                "@p_PurQty,@p_ActualQty,@p_UnitCD,@p_AdjQty,@p_isactive";
            var parameters = new DynamicParameters();
            parameters.Add("p_StockCD", stock.StockCD, DbType.String);
            parameters.Add("p_ItemCD", stock.ItemCD, DbType.String);
            parameters.Add("p_SaleQty", stock.SaleQty, DbType.Decimal);
            parameters.Add("p_PurQty", stock.PurQty, DbType.Decimal);
            parameters.Add("p_ActualQty", stock.ActualQty, DbType.Decimal);
            parameters.Add("p_UnitCD", stock.UnitCD, DbType.String);
            parameters.Add("p_AdjQty", stock.AdjQty, DbType.Decimal);
            parameters.Add("p_isactive", stock.isactive, DbType.Boolean);
            try
            {
                string Ins_CD = Convert.ToString(conn.ExecuteScalar(sql, param: parameters));
                return true;
            }
            catch (Exception e)
            {
                error = "error to update Stock" + e.Message;
            }
            return true;
        }
        #endregion

    }
}
