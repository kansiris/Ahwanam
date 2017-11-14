using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaaAahwanam.Models
{
    public class VendorImage
    {
        [Key]
        public long ImageId { get; set; }
        public long VendorId { get; set; }
        public long VendorMasterId { get; set; }
        public string ImageName { get; set; }
        public string Imagedescription { get; set; }
        public string ImageType { get; set; }
        public string ImageLimit { get; set; }
        public long UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string Status { get; set; }
    }
}
