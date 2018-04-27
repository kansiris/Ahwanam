using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace MaaAahwanam.Models
{
    public class NDeals
    {
        [Key]
        public long DealID { get; set; }
        public long VendorId { get; set; }
        public long VendorSubId { get; set; }
        public string VendorType { get; set; }
        public string VendorSubType { get; set; }
        public string Category { get; set; }
        public string DealName { get; set; }
        public DateTime? DealStartDate { get; set; }
        public DateTime? DealEndDate { get; set; }
        public decimal DealPrice { get; set; }
        public decimal OriginalPrice { get; set; }
        public string DealDescription { get; set; }
        public string FoodType { get; set; }
        public string MinMemberCount { get; set; }
        public string MaxMemberCount { get; set; }
        public string TermsConditions { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string TimeSlot { get; set; }
    }
}
