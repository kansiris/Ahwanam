using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaaAahwanam.Models
{
    public class collabratornotes
    {
        [Key]
        public long collabratornotesId { get; set; }
        public long wishlist_id { get; set; }
        public long vendor_id { get; set; }
        public long Userid { get; set; }
        public string collabratorNote { get; set; }
        public string Name { get; set; }
        public DateTime AddedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}
