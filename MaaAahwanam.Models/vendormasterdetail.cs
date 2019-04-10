using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaaAahwanam.Models
{
    public class vendormasterdetail
    {
        [Key]
        public long VendormasterId { get; set; }
        public long UserId { get; set; }
        public string BusinessName { get; set; }
        public string Address { get; set; }
        public string Landmark { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public string Description { get; set; }
        public string ContactPerson { get; set; }
        public string ContactNumber { get; set; }
        public string LandlineNumber { get; set; }
        public string EmailId { get; set; }
        public string Url { get; set; }
        public string Experience { get; set; }
        public string EstablishedYear { get; set; }
        public string ServiceType { get; set; }
        public string Status { get; set; }
        public long UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public bool Quotation { get; set; }
        public int tier { get; set; }
        public int priority { get; set; }
        public string GeoLocation { get; set; }
        public string BusinessType { get; set; }
        public decimal MinPrice { get; set; }
        public decimal MaxPrice { get; set; }
        public string Type_of_price { get; set; }
 
    }
}
