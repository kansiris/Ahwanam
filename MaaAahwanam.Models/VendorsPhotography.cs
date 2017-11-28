using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaaAahwanam.Models
{
    public class VendorsPhotography
    {
        [Key]
        public long Id { get; set; }
        public long VendorMasterId { get; set; }
        public string PhotographyType { get; set; }
        public string PreWeddingShoot { get; set; }
        public string DestinationPhotography { get; set; }
        public decimal StartingPrice { get; set; }
        public string PriorBookingsDays { get; set; }
        public string Status { get; set; }
        public long UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string discount { get; set; }
    }
}
