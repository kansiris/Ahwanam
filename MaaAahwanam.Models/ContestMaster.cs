using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace MaaAahwanam.Models
{
    public class ContestMaster
    {
        public long ContentMasterID { get; set; }
        public string ContestName { get; set; }
        public string CreatedDate { get; set; }
        public string Status { get; set; }
    }
}
