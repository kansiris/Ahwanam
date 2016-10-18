using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaaAahwanam.Models
{
    
    public class ServiceRequest
    {
        [Key]
        public long RequestId { get; set; }
        public string Name { get; set; }
        public string EmailId { get; set; }
        public decimal Budget { get; set; }
        public string ServiceType { get; set; }
        public string Preferences { get; set; }
        public string EventName { get; set; }
        public string EventAddress { get; set; }
        public string EventLocation { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
        public string PhoneNo { get; set; }
        public DateTime EventStartDate { get; set; }
        public DateTime EventStartTime { get; set; }
        public DateTime EventEnddate { get; set; }
        public DateTime EventEndtime { get; set; }
        public long  VendorId { get; set; }
        public long UpdatedBy { get; set; }
        public DateTime? UpdatedTime { get; set; }
        public string Status { get; set; }
        public string Type { get; set; }
        public string SubserviceType { get; set; }
    }
}
