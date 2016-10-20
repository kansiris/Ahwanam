using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace MaaAahwanam.Models
{
    public class Deal
    {
        [Key]
        public long DealID { get; set; }
        public string VendorType { get; set; }
        public string VendorCategory { get; set; }
        public DateTime? DealStartDate { get; set; }
        public DateTime? DealEndDate { get; set; }
        public string TermsConditions { get; set; }
        public decimal DealVegLunchCost { get; set; }
        public decimal DealNonVegLunchCost { get; set; }
        public decimal DealVegDinnerCost { get; set; }
        public decimal DealNonVegDinnerCost { get; set; }
        public decimal DealServicePrice { get; set; }
        public long VendorId { get; set; }
        public long VendorSubId { get; set; }
        public decimal VegPricePerPlate { get; set; }
        public decimal NonVegPricePerPlate { get; set; }
        public decimal BridalMakeupStartsFrom { get; set; }
        public decimal PartyMakeupStartsFrom { get; set; }
        public decimal CardCost { get; set; }
        public decimal CardCostWithPrint { get; set; }
    }
}
