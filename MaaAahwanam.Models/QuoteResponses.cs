using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace MaaAahwanam.Models
{
    public class QuoteResponse
    {
        [Key]
        public long ID { get; set; }
        public long QuoteID { get; set; }
        public string BusinessName { get; set; }
        public string ServiceType { get; set; }
        public string SubServiceType { get; set; }
        public string VendorID { get; set; }
        public string VendorSubID { get; set; }
        public long Installments { get; set; }
        public string FirstInstallment { get; set; }
        public string SecondInstallment { get; set; }
        public string ThirdInstallment { get; set; }
        public string FourthInstallment { get; set; }
        public string FifthInstallment { get; set; }
        public decimal TokenAmount { get; set; }
        public decimal TotalAmount { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string Status { get; set; }
    }
}
