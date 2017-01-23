using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaaAahwanam.Models
{
    public class Availabledates
    {
        [Key]
        public long Id { get; set; }
        public long vendorId { get; set; }
        public string servicetype { get; set; }
        public DateTime availabledate { get; set; }
        public long vendorsubid { get; set; }
    }
}
