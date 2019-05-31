using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PlanningPoker.Models
{
    public class Room
    {
        public Room()
        {
            Users = new Collection<User>();
        }

        [Key]
        public string Id { get; set; }
        public string Name { get; set; }
        public string CreatorName { get; set; }
        public bool SessionEnded { get; set; }
        public ICollection<User> Users { get; set; }
    }
}
