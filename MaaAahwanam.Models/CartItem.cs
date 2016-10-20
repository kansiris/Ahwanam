using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaaAahwanam.Models
{

    public class CartItem
    {
        [Key]
        public long CartId { get; set; }
        public long VendorId { get; set; }
        public string ServiceType { get; set; }
        public decimal ServicePrice { get; set; }
        public decimal TotalPrice { get; set; }
        public decimal Perunitprice { get; set; }
        public string attribute { get; set; }
        public int Quantity { get; set; }
        public decimal Discount { get; set; }
        public decimal DiscountPrice { get; set; }
        public long Orderedby { get; set; }
        public string Status { get; set; }
        public long UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public long subid { get; set; }
        public bool Isdeal { get; set; }
    }
}
