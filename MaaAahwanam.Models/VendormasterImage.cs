using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaaAahwanam.Models
{
   public class VendormasterImage
    {
        [Key]
        public long ImageId { get; set; }
        public long VendorId { get; set; }
        public string Title { get; set; }
        public string ImageDescription { get; set; }
        public string ThumbnailUrl { get; set; }
        public string MainImageUrl { get; set; }
        public DateTime? updatedDate { get; set; }
    }
}
