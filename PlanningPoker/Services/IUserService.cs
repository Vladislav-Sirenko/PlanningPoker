using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PlanningPoker.Models;

namespace PlanningPoker.Services
{
    public interface IUserService
    {
        Task AddVoteAsync(string name, int vote);
        Task ResetVoteAsync(string name);
        Task<User> AddUserConnection(string id, string roomId, string userName);
        List<User> GetUsersByRoom(string id);
        Task GetVotesForRoomAsync(string id);
        bool CheckSessionState(string id);
        Task<string> GetRoomByUserName(string userName);
        User AddUser(User user);
        User GetUserByConnectionId(string id);
        Task DeleteUserFromRoomAsync(string userName);
    }
}
