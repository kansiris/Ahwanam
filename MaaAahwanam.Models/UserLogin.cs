using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaaAahwanam.Models
{
    public class UserLogin
    {
          [Key]
        public long UserLoginId { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string UserType { get; set; }
        public DateTime? RegDate { get; set; }
        public string Status { get; set; }
        public long UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string ActivationCode { get; set; }
        public string IPAddress { get; set; }
        public string businesstype { get; set; }
        public string EmailOTP { get; set; }
        public string MobileOTP { get; set; }
    }
}
