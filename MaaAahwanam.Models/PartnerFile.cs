using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace MaaAahwanam.Models
{
    public class PartnerFile
    {
        [Key]
        public long ID { get; set; }
        public string PartnerID { get; set; }
        public string FileName { get; set; }
        public string VendorID { get; set; }
        public DateTime UpdatedDate { get; set; }
        public DateTime RegisteredDate { get; set; }
        public string Status { get; set; }
    }
}
