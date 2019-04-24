﻿using PlanningPoker.Context;
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
            _context.SaveChanges();
            return entity;
        }

        public async Task DeleteAsync(string name)
        {
            var entity = _context.Users.FirstOrDefault(x => x.Name == name);
            if (entity != null) _context.Users.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public IEnumerable<User> GetUsersByRoomId(string id)
        {
            var users = _context.Users.Where(x => x.RoomId == id);
            return users;
        }

        public User GetUserByConnectionId(string id)
        {
            var user = _context.Users.FirstOrDefault(x => x.ConnectionId == id);
            return user;
        }

        public User GetByNameAsync(string name)
        {
            return _context.Users.AsNoTracking().FirstOrDefault(x => x.Name == name);
        }

        public void UpdateAsync(User entity)
        {
            _context.Users.Update(entity);
            _context.SaveChanges();
        }

        public void UpdateRangeAsync(List<User> entity)
        {
            _context.Users.UpdateRange(entity);
            _context.SaveChanges();
        }

        public void DeleteUserFromRoom(string userName)
        {
            var user = _context.Users.FirstOrDefault(x => x.Name == userName);
            if (user != null)
            {
                user.RoomId = null;
                user.ConnectionId = null;
                _context.Users.Update(user);
                _context.SaveChanges();
            }
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}