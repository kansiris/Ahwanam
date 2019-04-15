using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaaAahwanam.Models
{
    
    public class Enquiry
    {
        [Key]
        public long EnquiryId { get; set; }
        public string PersonName { get; set; }
        public string SenderEmailId { get; set; }
        public string SenderPhone { get; set; }
        public string EnquiryTitle { get; set; }
        public string EnquiryDetails { get; set; }
        public DateTime? EnquiryDate { get; set; }
        public string EnquiryStatus { get; set; }
        public string Country { get; set; }
        public string PostalCode { get; set; }
        public string CompanyName { get; set; }
        public long UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string Status { get; set; }
        public string originfromurl { get; set; }
         public string city { get; set; }
    }
}
