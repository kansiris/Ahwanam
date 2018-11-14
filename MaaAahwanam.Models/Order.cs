using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaaAahwanam.Models
{
    
    public class Order
    {
        [Key]
        public long OrderId { get; set; }
        public long OrderedBy { get; set; }
        public long OrderNumber { get; set; }
        public long PaymentId { get; set; }
        public DateTime? OrderDate { get; set; }
        public decimal TotalPrice { get; set; }
        public long UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string Status { get; set; }
        public string type { get; set; }
        public string bookingtype { get; set; }
    }
}
