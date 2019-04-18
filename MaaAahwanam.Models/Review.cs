using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaaAahwanam.Models
{
    public class Review
    {
        [Key]
        public long ReviewId { get; set; }
        public string FirstName { get; set; }
        public string EmailId { get; set; }
        public string Service { get; set; }
        public long ServiceId { get; set; }
        public string Comments { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string Status { get; set; }
        public long? Sid { get; set; }
        public string rating { get; set; }
        public int categoryid { get; set; }
        public string servicetype { get; set; }
        public string reviewsfrom { get; set; }
        public DateTime? reviewaddedDate { get; set; }
    }
}
