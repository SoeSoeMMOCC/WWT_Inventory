using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WWT_Inventory.Model
{
    public class User
    {
        public string UserCD { get; set; }
        public string UserName { get; set; }
        public string UserPassword { get; set; }
        public bool isactive { get; set; }
        public DateTime ts { get; set; }
        public string role { get; set; }
    }
}
