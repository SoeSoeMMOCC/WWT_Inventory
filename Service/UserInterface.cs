using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WWT_Inventory.Model;

namespace WWT_Inventory.Service
{
    public interface UserInterface
    {
        User GetLoginUser(string userName, string password, out string error);
    }
}
