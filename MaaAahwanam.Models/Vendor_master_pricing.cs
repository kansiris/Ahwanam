using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaaAahwanam.Models
{
    public class Vendor_master_pricing
    {
        [Key]
        public long RecNo { get; set; }
        public long Id { get; set; }
        public long VendormasterId { get; set; }
        public decimal MinPrice { get; set; }
        public decimal MaxPrice { get; set; }
        public string Type_Of_Price { get; set; }
        public string comments { get; set; }
        public decimal? Ratings { get; set; }
        public long? Reviews { get; set; }
    }
}
