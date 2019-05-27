using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PlanningPoker.Context;
using PlanningPoker.Models;

namespace PlanningPoker.Repostitories
{
    public class RoomRepository : IRoomsRepository
    {
        private readonly PokerContext _context;

        public RoomRepository(PokerContext context)
        {
            _context = context;
        }
        public Task AddAsync(Room entity)
        {
           return  _context.Rooms.AddAsync(entity);
        }

        public void Delete(string id)
        {
            var entity = _context.Rooms.AsNoTracking().FirstOrDefault(x => x.Id == id);
            if (entity != null) _context.Rooms.Remove(entity);
        }

        public Room GetById(string id)
        {
            return _context.Rooms.AsNoTracking().FirstOrDefault(x => x.Id == id);
        }

        public Room GetByName(string name)
        {
            return _context.Rooms.AsNoTracking().FirstOrDefault(x => x.Name == name);
        }

        public void Update(Room entity)
        {
            _context.Rooms.Update(entity);
        }
        
        public Task<List<Room>> GetRoomsAsync()
        {
            return _context.Rooms.AsNoTracking().ToListAsync();
        }
    }
}
