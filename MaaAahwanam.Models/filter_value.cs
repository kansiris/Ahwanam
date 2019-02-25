using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaaAahwanam.Models
{
    public class filter_value
    {
        [Key]
        public int filter_id { get; set; }
        public int id { get; set; }
        public string name { get; set; }
    }
}
