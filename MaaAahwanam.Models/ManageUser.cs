using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaaAahwanam.Models
{
    public class ManageUser
    {
        public long id { get; set; }
        public string vendorId { get; set; }
        public string type { get; set; }
        public string Businessname { get; set; }
        public string firstname { get; set; }
        public string lastname { get; set; }
        public string middlename { get; set; }
        public string email { get; set; }
        public string phoneno { get; set; }
        public string adress1 { get; set; }
        public string adress2 { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string country { get; set; }
        public string services { get; set; }
        public string bankname { get; set; }
        public string ifsccode { get; set; }
        public string accountnumber { get; set; }
        public string branch { get; set; }
        public string accountholdername { get; set; }
        public string GSTtin { get; set; }
        public DateTime registereddate { get; set; }
        public DateTime updateddate { get; set; }
        public string Status { get; set; }
        public string updatedby { get; set; }
    }
}
