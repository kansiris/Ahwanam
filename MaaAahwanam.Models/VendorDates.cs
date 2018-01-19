using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace MaaAahwanam.Models
{
    public class VendorDates
    {
        [Key]
        public long Id { get; set; }
        public long VendorId { get; set; }
        public long Vendorsubid { get; set; }
        public string Servicetype { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Color { get; set; }
        public string IsFullDay { get; set; }
        public string Type { get; set; }
    }
}
