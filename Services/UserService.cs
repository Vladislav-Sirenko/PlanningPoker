using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PlanningPoker.Models;

namespace PlanningPoker.Services
{
    public class UserService : IUserService
    {
        private readonly Dictionary<string, string> _users;

        public UserService()
        {
            _users = new Dictionary<string, string>();
        }
        public User AddUser(User user)
        {
            _users.Add(user.Name, user.Password);
            return user;
        }

        public Dictionary<string, string> GetUsers()
        {
            return _users;
        }
    }
}
