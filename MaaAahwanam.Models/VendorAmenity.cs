using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaaAahwanam.Models
{
    public class VendorAmenity
    {
        [Key]
        public long AmenityId { get; set; }
        public long VendorId { get; set; }
        public string Amenity { get; set; }
        public string AmenityIcon { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
