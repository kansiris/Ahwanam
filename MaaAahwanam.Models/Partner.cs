using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace MaaAahwanam.Models
{
    public class Partner
    {
        [Key]
        public long PartnerID { get; set; }
        public long VendorId { get; set; }
        public string VendorType { get; set; }
        public string PartnerName { get; set; }
        public string url { get; set; }
        public string phoneno { get; set; }
        public string emailid { get; set; }
        public string address1 { get; set; }
        public string address2 { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string country { get; set; }
        public string cpfname { get; set; }
        public string cplname { get; set; }
        public string cpdesig { get; set; }
        public string cpphoneno { get; set; }
        public string cpemailid { get; set; }
        public string PAN { get; set; }
        public string bankname { get; set; }
        public string bankbranch { get; set; }
        public string IFSC { get; set; }
        public string AccountNumber { get; set; }
        public string AccountHolderName { get; set; }
        public string GST { get; set; }
        public string Status { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public DateTime? RegisteredDate { get; set; }
        public string fax { get; set; }
    }
}
