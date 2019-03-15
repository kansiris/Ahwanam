using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaaAahwanam.Models
{
    public class Snapshot
    {
        [Key]
        public long snapshotId { get; set; }
        public string snapshotimage { get; set; }
    }
}
