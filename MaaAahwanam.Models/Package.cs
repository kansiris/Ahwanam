using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace MaaAahwanam.Models
{
    public class Package
    {
        [Key]
        public long PackageID { get; set; }
        public long VendorId { get; set; }
        public long VendorSubId { get; set; }
        public string VendorType { get; set; }
        public string VendorSubType { get; set; }
        public string Category { get; set; }
        public string PackageName { get; set; }
        public string PackagePrice { get; set; }
        public string PackageDescription { get; set; }
        public string Status { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
