using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaaAahwanam.Models
{
    public class Payments_Requests
    {
        [Key]
        public long PaymentID { get; set; }
        public long RequestID { get; set; }
        public decimal paidamount { get; set; }
        public string cardnumber { get; set; }
        public string CVV { get; set; }
        public DateTime Paiddate { get; set; }
        public string TotalAmount { get; set; }
        public string Currency { get; set; }
        public string International { get; set; }
        public string Payment_Method { get; set; }
        public string Refunded_Amount { get; set; }
        public string Refund_Status { get; set; }
        public string Payment_Captured { get; set; }
        public string Card_ID { get; set; }
        public string Bank { get; set; }
        public string Wallet { get; set; }
        public string Customer_Email { get; set; }
        public string Customer_Contact { get; set; }
        public string Fee { get; set; }
        public string Tax { get; set; }
        public string Error_Code { get; set; }
        public string Error_Description { get; set; }
        public string Payment_Status { get; set; }
    }
}
