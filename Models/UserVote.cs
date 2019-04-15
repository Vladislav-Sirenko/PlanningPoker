 using System;
using System.Collections.Generic;
 using System.ComponentModel.DataAnnotations;
 using System.Linq;
using System.Threading.Tasks;

namespace PlanningPoker.Models
{
    public class UserVote
    {
        [Key]
        public string UserName { get; set; }
        public int Vote { get; set; }
    }
}
