using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaaAahwanam.Models
{
    public class Category
    {
        [Key]
        public int servicType_id { get; set; }
        public string name { get; set; }
        public string display_name { get; set; }
        public string image { get; set; }
        public string status { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}
