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
        void DeleteUser(string id, string item);
        void AddUserConnection(string id, string name);
        string GetUserByConnection(string id);
        void AddRoom(Room room);
        void DeleteRoom(string id);
        List<Room> GetRooms();
        void AddUserToGroup(UserConnection userConnection);
        string GetRoomName(string id);
        List<string> GetUsersByRoom(string id);
        Dictionary<string, int> GetVotesForRoom(string id);
    }
}
