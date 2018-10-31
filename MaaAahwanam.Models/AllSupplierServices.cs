using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace MaaAahwanam.Models
{
    public class AllSupplierServices
    {
        [Key]
        public long ID { get; set; }
        public string VendorMasterID { get; set; }
        public string ServiceName { get; set; }
        public string Description { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string Status { get; set; }
    }
}
