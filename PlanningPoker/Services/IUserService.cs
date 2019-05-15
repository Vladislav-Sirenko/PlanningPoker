using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PlanningPoker.Models;

namespace PlanningPoker.Services
{
    public interface IUserService
    {
        void AddVote(string name, int vote);
        void ResetVote(string name);
        User AddUserConnection(string id, string roomId, string userName);
        List<User> GetUsersByRoom(string id);
        void GetVotesForRoom(string id);
        bool CheckSessionState(string id);
        string GetRoomByUserName(string userName);
        User AddUser(User user);
        User GetUserByConnectionId(string id);
        void DeleteUserFromRoom(string userName);
    }
}
