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
    //    void DeleteUser(string id);
        User AddUserConnection(string id, string roomId, string userName);
        Task<Room> AddRoom(Room room);
        void DeleteRoom(string id);
        Task<List<Room>> GetRooms();
        //   void AddUserToGroup(string connectionId, string roomName);
        List<User> GetUsersByRoom(string id);
        void GetVotesForRoom(string id);

        string GetRoleForRoom(string userId, string roomId);
        IEnumerable<string> GetRoles(string[] users, string id);
        string GetRoomByUserName(string userName);
        User AddUser(User user);
        User GetUserByConnectionId(string id);
        void DeleteUserFromRoom(string userName);
    }
}
