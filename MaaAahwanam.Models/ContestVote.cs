using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace MaaAahwanam.Models
{
    public class ContestVote
    {
        [Key]
        public long ContestVoteID { get; set; }
        public long ContestId { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public DateTime VotedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string Status { get; set; }
        public string IPAddress { get; set; }
    }
}
