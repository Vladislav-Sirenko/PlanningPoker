using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PlanningPoker.Context;
using PlanningPoker.Models;

namespace PlanningPoker.Repostitories
{
    public class RoomRepository:IRoomsRepository
    {
        private readonly PokerContext _context;

        public RoomRepository(PokerContext context)
        {
            _context = context;
        }
        public async Task<Room> AddAsync(Room entity)
        {
            _context.Rooms.Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public void DeleteAsync(string id)
        {
            var entity = _context.Rooms.First(x => x.Id == id);
            _context.Rooms.Remove(entity);
             _context.SaveChanges();
        }

        public Room GetByIdAsync(string id)
        {
            return _context.Rooms.AsNoTracking().FirstOrDefault(x => x.Id == id);
        }

        public Room GetByNameAsync(string name)
        {
            return _context.Rooms.AsNoTracking().FirstOrDefault(x => x.Name == name);
        }

        public Task UpdateAsync(Room entity)
        {
            _context.Rooms.Update(entity);
            return _context.SaveChangesAsync();
        }

        public Task<List<Room>> GetRoomsAsync()
        {
            return _context.Rooms.AsNoTracking().ToListAsync();
        }
        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
