using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaaAahwanam.Models
{
    public class VendorsWeddingCollection
    {
        [Key]
        public long Id { get; set; }
        public long VendorMasterId { get; set; }
        public string OnlineStore { get; set; }
        public string InStore { get; set; }
        public string Status { get; set; }
        public long UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
