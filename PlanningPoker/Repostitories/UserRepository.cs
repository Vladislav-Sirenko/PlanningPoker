using PlanningPoker.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PlanningPoker.Models;

namespace PlanningPoker.Repostitories
{
    public class UserRepository : IUserRepository
    {
        private readonly PokerContext _context;

        public UserRepository(PokerContext context)
        {
            _context = context;
        }
        public User AddAsync(User entity)
        {
            _context.Users.Add(entity);
            return entity;
        }

        public void DeleteAsync(string name)
        {
            var entity = _context.Users.AsNoTracking().FirstOrDefault(x => x.Name == name);
            if (entity != null) _context.Users.Remove(entity);
        }

        public IReadOnlyCollection<User> GetUsersByRoomId(string id)
        {
            var users = _context.Users.AsNoTracking().Where(x => x.RoomId == id).ToList().AsReadOnly();
            return users;
        }

        public User GetUserByConnectionId(string id)
        {
            var user = _context.Users.AsNoTracking().FirstOrDefault(x => x.ConnectionId == id);
            return user;
        }

        public Task<User> GetByNameAsync(string name)
        {
            return  _context.Users.AsNoTracking().FirstOrDefaultAsync(x => x.Name == name);
        }

        public void Update(User entity)
        {
            _context.Users.Update(entity);
        }

        public void UpdateRange(IEnumerable<User> entity)
        {
            _context.Users.UpdateRange(entity);
        }

        public void DeleteUserFromRoom(string userName)
        {
            var user = _context.Users.FirstOrDefault(x => x.Name == userName);
            if (user != null)
            {
                user.Vote = null;
                user.RoomId = null;
                user.ConnectionId = null;
                _context.Users.Update(user);
            }
        }
    }
}
