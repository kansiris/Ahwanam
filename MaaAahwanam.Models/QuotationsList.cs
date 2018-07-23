using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaaAahwanam.Models
{
    public class QuotationsList
    {
        [Key]
        public long Id { get; set; }
        public string Name { get; set; }
        public string EmailId { get; set; }
        public string ServiceType { get; set; }
        public string PhoneNo { get; set; }
        public DateTime EventStartDate { get; set; }
        public DateTime EventStartTime { get; set; }
        public DateTime EventEnddate { get; set; }
        public DateTime EventEndtime { get; set; }
        public string VendorId { get; set; }
        public string VendorMasterId { get; set; }
        public string Persons { get; set; }
        public string ExtraPersons { get; set; }
        public long UpdatedBy { get; set; }
        public DateTime? UpdatedTime { get; set; }
        public string Status { get; set; }
        public string IPaddress { get; set; }
        public string Description { get; set; }
        public string TimeSlot { get; set; }
        public string FirstTime { get; set; }
        public string SecondTime { get; set; }
        public string ThirdTime { get; set; }
        public string FirstTimeQuoteDate { get; set; }
        public string SecondTimeQuoteDate { get; set; }
        public string ThirdTimeQuoteDate { get; set; }
    }
}
