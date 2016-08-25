using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaaAahwanam.Models
{
    
    public class EventDate
    {
        [Key]
        public long EventDateId { get; set; }
        public long EventId { get; set; }
        public DateTime? StartDate { get; set; }
        public string StartTime { get; set; }
        public DateTime? EndDate { get; set; }
        public string EndTime { get; set; }
        public long UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string Status { get; set; }
        public long vendorid { get; set; }
    }
}
