using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaaAahwanam.Models
{
   public class wishlist
    {
        [Key]
        public long Id { get; set; }
        public long UserId { get; set; }
        public string Name { get; set; }
        public string Event { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string Description { get; set; }
        public DateTime UpdatedDate { get; set; }

    }
}
