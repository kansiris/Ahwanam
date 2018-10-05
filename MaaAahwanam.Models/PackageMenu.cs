using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaaAahwanam.Models
{
    public class PackageMenu
    {
        [Key]
        public long MenuID { get; set; }
        public string VendorID { get; set; }
        public string VendorMasterID { get; set; }
        public string Category { get; set; }
        public string Welcome_Drinks { get; set; }
        public string Starters { get; set; }
        public string Rice { get; set; }
        public string Bread { get; set; }
        public string Curries { get; set; }
        public string Fry_Dry { get; set; }
        public string Salads { get; set; }
        public string Soups { get; set; }
        public string Tandoori_Kababs { get; set; }
        public string Deserts { get; set; }
        public string Beverages { get; set; }
        public string Fruits { get; set; }
        public string Extra_Menu_Items { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}
