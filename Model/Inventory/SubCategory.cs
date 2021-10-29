using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WWT_Inventory.Model.Inventory
{
    public class SubCategory
    {
        public string SubCategoryCD { get; set; }
        public string SubCategoryName { get; set; }
        public string CategoryCD { get; set; }
        public string CategoryName { get; set; }
        public bool isactive { get; set; }
    }
}
