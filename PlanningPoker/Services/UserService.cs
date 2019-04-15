using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using PlanningPoker.Models;
using Serilog;

namespace PlanningPoker.Services
{
    public class UserService : IUserService
    {
        private readonly List<Room> _rooms;
        private readonly List<UserRoom> userRooms;
        private List<UserVote> _usersVotes;
        private List<UserConnection> _userConnections;
        private readonly IHubContext<LoopyHub> _hubContext;

        public UserService(IHubContext<LoopyHub> hubContext)
        {
            _rooms = new List<Room>();
            _usersVotes = new List<UserVote>();
            _userConnections = new List<UserConnection>();
            userRooms = new List<UserRoom>();
            _hubContext = hubContext;
        }

        public void AddVote(UserVote userVote)
        {
            Log.Information("User:" + userVote.UserName + " voted " + userVote.Vote);
            _usersVotes.Add(new UserVote() { UserName = userVote.UserName, Vote = userVote.Vote });
        }
        public Dictionary<string, int> GetVotesForRoom(string id)
        {
            var users = GetUsersByRoom(id);
            Dictionary<string, int> votes = new Dictionary<string, int>();
            foreach (var user in users)
            {
                votes.Add(user, _usersVotes.First(x => x.UserName == user).Vote);
            }
            return votes;
        }

        public string GetRoleForRoom(string userId, string roomId)
        {
            var room = _rooms.First(x => x.id == roomId);
            if (userId == room.CreatorId)
                return "Admin";
            return "Guest";
        }

        public IEnumerable<string> GetRoles(string[] users, string id)
        {
            var group = GetRoomName(GetConnectionByUserName(users[0]));
            List<string> roles = new List<string>();
            foreach (var user in users)
            {
                roles.Add(GetRoleForRoom(GetConnectionByUserName(user), id));
            }
            _hubContext.Clients.Group(group).SendAsync("GetRolesForRoom");
            return roles;

        }

        public void ResetVote(string id)
        {
            var userlist = GetUsersByRoom(id);
            var removedUserVotes = new List<UserVote>();
            foreach (var uservote in _usersVotes)
            {
                foreach (var user in userlist)
                {
                    if (user == uservote.UserName)
                    {
                        removedUserVotes.Add(uservote);
                    }
                }
            }
            foreach (var userVote in removedUserVotes)
            {
                _usersVotes.Remove(userVote);
            }
        }


        public void DeleteUser(string id)
        {
            try
            {
                var removedUserConnection = _userConnections.First(x => x.ConnectionId == id);
                _userConnections.Remove(removedUserConnection);
                var removedUser = userRooms.First(x => x.ConnectionId == id);
                if (removedUser != null)
                    userRooms.Remove(removedUser);

                Log.Information(removedUserConnection.Name + "successfully removed from local database");
            }
            catch (Exception ex)
            {
                Log.Information("Disconnected user doesn`t exists in local database" + ex.Data);
            }
        }
        public void AddUserConnection(string id, string name)
        {
            _userConnections.Add(new UserConnection() { Name = name, ConnectionId = id });
        }
        public string GetUserByConnection(string id)
        {
            var name = _userConnections.First(x => x.ConnectionId == id).Name;
            return name;
        }
        public string GetConnectionByUserName(string name)
        {
            var id = _userConnections.First(x => x.Name == name).ConnectionId;
            return id;
        }
        public void AddRoom(Room room)
        {
            _rooms.Add(room);
            _hubContext.Clients.All.SendAsync("AddRoom");

        }
        public void DeleteRoom(string id)
        {
            var remroom = _rooms.First(x => x.id == id);
            _rooms.Remove(remroom);
            _hubContext.Clients.All.SendAsync("DeleteRoom");
        }
        public List<Room> GetRooms()
        {
            return _rooms;
        }

        public void AddUserToGroup(UserRoom userConnection)
        {
            userRooms.Add(userConnection);
        }

        public string GetRoomName(string id)
        {
            return userRooms.First(x => x.ConnectionId == id).Name;
        }

        public List<string> GetUsersByRoom(string id)
        {
            var roomName = _rooms.First(x => x.id == id).name;
            var userlist = userRooms.Where(x => x.Name == roomName);
            List<string> userNames = new List<string>();
            foreach (var user in userlist)
            {
                userNames.Add(GetUserByConnection(user.ConnectionId));
            }
            return userNames;

        }
    }
}
