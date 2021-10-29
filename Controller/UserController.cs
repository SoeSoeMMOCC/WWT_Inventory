using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WWT_Inventory.Factory;
using WWT_Inventory.Model;
using WWT_Inventory.Service;

namespace WWT_Inventory.Controller
{
    public class UserController : UserInterface
    {
        IDbConnection conn;
        public UserController()
        {
            conn = DBFactory.InvDB();
        }
        /*
         Getting Login User Information, to Check Authorize or Not
         */
        public User GetLoginUser(string userName, string password, out string error)
        {
            error = "";
            User user = new User();

            string sql = "exec get_userinfo @UserName, @UserPassword;";
            var parameters = new DynamicParameters();
            parameters.Add("UserName", userName, DbType.String);
            parameters.Add("UserPassword", password, DbType.String);
            try
            {
                user = conn.Query<User>(sql, param: parameters).FirstOrDefault();
            }
            catch (Exception e)
            {
                error = "Error to Retrieve Login User Information.\n"+e.Message;
            }
            return user;
        }
    }
}
