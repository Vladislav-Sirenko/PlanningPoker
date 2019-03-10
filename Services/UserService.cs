using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using PlanningPoker.Models;
using Serilog;

namespace PlanningPoker.Services
{
    public class UserService : IUserService
    {
        private readonly Dictionary<string, string> _users;
        private Dictionary<string, int> _usersVotes;
        private Dictionary<string, string> _userConnections;

        public UserService()
        {
            _users = new Dictionary<string, string>();
            _usersVotes = new Dictionary<string, int>();
           _userConnections = new Dictionary<string, string>();
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
        public IOrderedEnumerable<KeyValuePair<string, int>> GetVotes()
        {
            return _usersVotes.OrderBy(key => key.Key);
        }

        public IOrderedEnumerable<KeyValuePair<string, string>> GetUsers()
        {
            return _users.OrderBy(key=>key.Key);
        }

        public void ResetVote() => _usersVotes.Clear();


        public void DeleteUser(string item)
        {
            try
            {
                _users.Remove(item);
                _userConnections.Remove(item);
                Log.Information(item + "successfully removed from local database");
            }
            catch (Exception ex)
            {
                Log.Information(item + " doesn`t exists in local database");
            }
        }
        public void AddUserConnection(string id, string name)
        {
            _userConnections.TryAdd(id, name);
        }
        public string GetUserByConnection(string id)
        {
          _userConnections.TryGetValue(id,out string name);
            return name;
        }
    }
}
