using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaaAahwanam.Models
{
    public class VendorAvailableArea
    {
        [Key]
        public long AreaId { get; set; }
        public long VendorId { get; set; }
        public string AreaDescription { get; set; }
        public string AreaIcon { get; set; }
        public string FeatureName { get; set; }
        public string Area { get; set; }
        public int SeatingCapacity { get; set; }
        public int Minimumseatingcapacity { get; set; }
        public int Maximumseatingcapacity { get; set; }
        public int Diningcapacity { get; set; }
        public decimal RentalPrice { get; set; }
    }
}
