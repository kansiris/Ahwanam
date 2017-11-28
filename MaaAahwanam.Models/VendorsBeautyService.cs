using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaaAahwanam.Models
{
    
    public class VendorsBeautyService
    {
        [Key]
        public long Id { get; set; }
        public long VendorMasterId { get; set; }
        public string Category { get; set; }
        public decimal BridalMakeupStartsFrom { get; set; }
        public decimal PartyMakeupStartsFrom { get; set; }
        public string HomeServices { get; set; }
        public string Status { get; set; }
        public long UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string discount { get; set; }
    }
}
