using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PlanningPoker.Models;

namespace PlanningPoker.Services
{
    public interface IUserService
    {
        void AddVote(UserVote userVote);
        void ResetVote(string name);
        void DeleteUser(string id);
        void AddUserConnection(string id, string name);
        string GetUserByConnection(string id);
        string GetConnectionByUserName(string name);
        void AddRoom(Room room);
        void DeleteRoom(string id);
        List<Room> GetRooms();
        void AddUserToGroup(UserRoom userConnection);
        string GetRoomName(string id);
        List<string> GetUsersByRoom(string id);
        Dictionary<string, int> GetVotesForRoom(string id);

        string GetRoleForRoom(string userId,string roomId);
        IEnumerable<string> GetRoles(string[] users, string id);
    }
}
