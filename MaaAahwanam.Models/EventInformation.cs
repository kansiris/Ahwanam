using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaaAahwanam.Models
{
    public class EventInformation
    {
        [Key]
        public long EventId { get; set; }
        public long OrderId { get; set; }
        public string EventName { get; set; }
        public string Address { get; set; }
        public string Location { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public long UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string Status { get; set; }
        public long vendorid { get; set; }
        public long CartId { get; set; }
        public long OrderDetailsid { get; set; }
        public long subid { get; set; }
    }
}
