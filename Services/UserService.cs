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
        private Dictionary<string, int> _usersVotes;

        public UserService()
        {
            _users = new Dictionary<string, string>();
            _usersVotes = new Dictionary<string, int>();
        }
        public User AddUser(User user)
        {
            _users.Add(user.Name, user.Password);
            return user;
        }

        public void AddVote(UserVote userVote)
        {

            _usersVotes.Add(userVote.UserName, userVote.Vote);

        }
        public Dictionary<string, int> GetVotes()
        {
            return _usersVotes;
        }

        public Dictionary<string, string> GetUsers()
        {
            return _users;
        }

        public void ResetVote() => _usersVotes.Clear();

        public void DeleteUser(string item) => _users.Remove(item);
    }
}
