using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaaAahwanam.Models
{
    public class VendorPolicies
    {
        [Key]
        public long PolicyId { get; set; }
        public long VendorId { get; set; }
        public string Policy { get; set; }
        public string PolicyIcon { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
