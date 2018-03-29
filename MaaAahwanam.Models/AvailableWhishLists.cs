using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace MaaAahwanam.Models
{
    public class AvailableWhishLists
    {
        [Key]
        public int WhishListID { get; set; }
        public string VendorID { get; set; }
        public string VendorSubID { get; set; }
        public string BusinessName { get; set; }
        public string ServiceType { get; set; }
        public string UserID { get; set; }
        public string WhishListedDate { get; set; }
        public string IPAddress { get; set; }
        public string Status { get; set; }
    }
}
