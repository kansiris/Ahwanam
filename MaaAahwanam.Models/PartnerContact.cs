using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace MaaAahwanam.Models
{
    public class PartnerContact
    {
        [Key]
        public long ID { get; set; }
        public string PartnerID { get; set; }
        public string VendorID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Designation { get; set; }
        public string PhoneNo { get; set; }
        public string EmailID { get; set; }
        public DateTime RegisteredDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string Status { get; set; }
    }
}
