using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaaAahwanam.Models
{
    
    public class VendorsGift
    {
        public long Id { get; set; }
        public long VendorMasterId { get; set; }
        public string GiftType { get; set; }
        public string MinOrder { get; set; }
        public string MaxOrder { get; set; }
        public decimal GiftCost { get; set; }
        public string Status { get; set; }
        public long UpdatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string discount { get; set; }
    }
}
