using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace MaaAahwanam.Models
{
    public class SupplierServices
    {
        [Key]
        public long ID { get; set; }
        public long VendorID { get; set; }
        public long VendorMasterID { get; set; }
        public string ServiceName { get; set; }
        public string ServiceDescription { get; set; }
        public string Images { get; set; }
        public string Status { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
