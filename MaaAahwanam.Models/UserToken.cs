using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace MaaAahwanam.Models
{
    public class UserToken
    {
        [Key]
        public long Id { get; set; }
        public long UserLoginID { get; set; }
        public string IPAddress { get; set; }
        public string Token { get; set; }
        public DateTime UpdatedDate { get; set; }
        public DateTime LastLogin { get; set; }
        public string Status { get; set; }
    }
}
