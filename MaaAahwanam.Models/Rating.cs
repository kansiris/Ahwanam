using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace MaaAahwanam.Models
{
    public class Rating
    {
        [Key]
        public int ID { get; set; }
        public long VendorID { get; set; }
        public long VendorSubID { get; set; }
        public string BussinessName { get; set; }
        public string Category { get; set; }
        public string SubCategory { get; set; }
        public decimal FbRating { get; set; }
        public decimal JDRating { get; set; }
        public decimal GoogleRating { get; set; }
    }
}
