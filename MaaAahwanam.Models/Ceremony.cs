using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaaAahwanam.Models
{
    public class Ceremony
    {  
        [Key]
        public long Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string Status { get; set; }
        public string name { get; set; }
        public string ceremonyImage { get; set; }
        public string page_name { get; set; }
        public string default_category { get; set; }
        public int type { get; set; }
        public string location { get; set; }
        public string MetatagTitle { get; set; }
        public string MetatagDesicription { get; set; }
        public string MetatagKeywords { get; set; }

    }
}
