using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WWT_Inventory.Model.Inventory
{
    public class Unit
    {
        public string UnitCD { get; set; }
        public string UnitName { get; set; }
        public int BaseQty { get; set; }
        public bool isactive { get; set; }
    }
}
