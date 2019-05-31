using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PlanningPoker.Models;

namespace PlanningPoker.Context
{
    public class PokerContext : DbContext
    {
        public PokerContext(IDbContextOptionsProvider<PokerContext> optionsProvider)
            : base(optionsProvider.Options)
        {
        }

        public DbSet<Room> Rooms{ get; set; }
        public DbSet<User> Users{ get; set; }




    }
}
