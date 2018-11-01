using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace MaaAahwanam.Models
{
    public class StaffAccess
    {
        [Key]
        public long ID { get; set; }
        public long UserLoginId { get; set; }
        public long VendorMasterID { get; set; }
        public string fname { get; set; }
        public string lname { get; set; }
        public string emailid { get; set; }
        public string phoneno { get; set; }
        public string designation { get; set; }
        public int order { get; set; }
        public int book { get; set; }
        public int quote { get; set; }
        public int service { get; set; }
        public int revenuemodel { get; set; }
        public int invoice { get; set; }
        public int payment { get; set; }
        public int customer { get; set; }
        public int supplier { get; set; }
        public int addstaff { get; set; }
        public DateTime UpdatedDate { get; set; }
        public DateTime RegisteredDate { get; set; }
        public string Status { get; set; }
    }
}
