using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PlanningPoker.Models;

namespace PlanningPoker.Services
{
    public interface IUserService
    {
        User AddUser(User user);
        IOrderedEnumerable<KeyValuePair<string, string>> GetUsers();
        void AddVote(UserVote userVote);
        IOrderedEnumerable<KeyValuePair<string, int>> GetVotes();
        void ResetVote();
        void DeleteUser(string item);
        void AddUserConnection(string id, string name);
        string GetUserByConnection(string id);
    }
}
