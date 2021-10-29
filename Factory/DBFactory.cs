using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WWT_Inventory.Factory
{
    public class DBFactory
    {
        public static IDbConnection InvDB()
        {
            SqlConnection conn = new SqlConnection(WWT_Inventory.Properties.Settings.Default.DBConnection);
            return conn;
        }
    }
}
