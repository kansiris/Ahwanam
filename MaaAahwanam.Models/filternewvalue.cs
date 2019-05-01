using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaaAahwanam.Models
{
    public class filternewvalue
    {
        [Key]
        public int id { get; set; }
        public string name { get; set; }
        public int filter_id { get; set; }
        public int city_id { get; set; }
    }
}
