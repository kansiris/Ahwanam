using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace MaaAahwanam.Models
{
    public class Contest
    {
        [Key]
        public long ContestId { get; set; }
        public long ContentMasterID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string UploadedImage { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public int TermsAndConditions { get; set; }
        public long SharedCount { get; set; }
        public string Status { get; set; }
        public string IPAddress { get; set; }
    }
}
