using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using PlanningPoker.Models;

namespace PlanningPoker.Context
{
    public class PokerContext : DbContext
    {
        public PokerContext(DbContextOptions<PokerContext> options)
            : base(options)
        {
        }

        public DbSet<Room> Rooms{ get; set; }
        public DbSet<User> Users{ get; set; }




    }
}
