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
        public string MinPrice { get; set; }
        public string MaxPrice { get; set; }
        public string MinGuests { get; set; }
        public string MaxGuests { get; set; }
        public string images { get; set; }
        public string price1 { get; set; }
        public string price2 { get; set; }
        public string price3 { get; set; }
        public string price4 { get; set; }
        public string price5 { get; set; }
        public string price6 { get; set; }
        public string price7 { get; set; }
        public string price8 { get; set; }
        public string peakdays { get; set; }
        public string normaldays { get; set; }
        public string holidays { get; set; }
        public string choicedays { get; set; }
    }
}
