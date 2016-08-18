using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaaAahwanam.Models
{
    public class Notification
    {
        [Key]
        public long id { get; set; }
        public long NotificationTo { get; set; }
        public string type { get; set; }
        public string description { get; set; }
        public DateTime? DateandTime { get; set; }
        public string Subject { get; set; }
        public string Status { get; set; }
        public string EmailID { get; set; }
    }
}
