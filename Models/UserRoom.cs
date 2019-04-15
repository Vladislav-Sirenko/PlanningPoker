using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PlanningPoker.Models
{
    public class UserRoom
    {
        [Key]
        public string ConnectionId { get; set; }
        public string Name { get; set; }
    }
}
