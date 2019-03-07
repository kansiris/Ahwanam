using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaaAahwanam.Models
{
    public class CeremonyCategory
    {
        [Key]
        public long Id { get; set; }
        public long CeremonyId { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public string image { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string Status { get; set; }
        public string page_name { get; set; }
    }
}
