using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PlanningPoker.Models
{
    public class Room
    {
        [Key]
        public string id { get; set; }
        public string name { get; set; }
        public string CreatorId { get; set; }
    }
}
    