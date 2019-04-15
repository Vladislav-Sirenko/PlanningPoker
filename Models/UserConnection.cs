using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PlanningPoker.Models
{
    public class UserConnection
    {
        [Key]
        public string ConnectionId { get; set; }
        public string Name { get; set; } 
    }
}
