using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaaAahwanam.Models
{
    public class Note
    {
        [Key]
        public long NotesId { get; set; }
        public long wishlistId { get; set; }
        public long VendorId { get; set; }
        public long VendorSubId { get; set; }
        public long UserId { get; set; }
        public string Notes { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}
