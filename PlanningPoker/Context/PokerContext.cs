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
        public PokerContext(DbContextOptions<PokerContext> options)
            : base(options)
        {
        }

        public DbSet<Room> Rooms{ get; set; }
        public DbSet<UserVote> UserVotes{ get; set; }
        public DbSet<UserRoom> UserRooms { get; set; }
        public DbSet<UserConnection> Connections{ get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Room>().HasData(new Room(){id = "123",CreatorId = "213",name = "Room1"} );
        }

    }
}
