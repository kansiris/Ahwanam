using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaaAahwanam.Models
{
   public class Userwishlistdetails
    {
        [Key]
        public long wishlistserviceId { get; set; }
        public long wishlistId { get; set; }
        public long vendorId { get; set; }
        public long vendorsubId { get; set; }
        public string BusinessName { get; set; }
        public string servicetype { get; set; }
        public long UserId { get; set; }
        public DateTime WhishListedDate { get; set; }
        public string IPAddress { get; set; }
        public string Status { get; set; }
        public long? NotesId { get; set; }
        public string Ceremony { get; set; }
        public string Budjet { get; set; }
        public DateTime? EventStartDate { get; set; }
        public DateTime? EventEndDate { get; set; }
        public long? collabratorid { get; set; }
    }
}
