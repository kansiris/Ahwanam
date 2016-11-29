using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaaAahwanam.Models
{
    public class AdminTestimonial
    {
        [Key]
        public long Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Email { get; set; }
        public long UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public long Orderid { get; set; }
        public string Status { get; set; }
        public long Ratings { get; set; }
    }
}
