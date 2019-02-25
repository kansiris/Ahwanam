using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaaAahwanam.Models
{
    public class filter
    {
        [Key]
        public int filter_id { get; set; }
        public int serviceType_id { get; set; }
        public string name { get; set; }
        public string display_name { get; set; }
        public bool is_multiple_selection { get; set; }
        public string status { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}
