using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MaaAahwanam.Models
{
    public class UserRequest
    {
        [Key]
        public long UserLoginId { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string UserType { get; set; }
        public DateTime? RegDate { get; set; }

        public long UserDetailId { get; set; }        
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string AlternativeEmailID { get; set; }
        public string Country { get; set; }
        public string Gender { get; set; }
        public string Landmark { get; set; }
        public string UserPhone { get; set; }
        public string Url { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public string UserImgId { get; set; }
        public string UserImgName { get; set; }
    }
}
