using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using PlanningPoker.Models;
using Serilog;

namespace PlanningPoker.Services
{
    public class UserService : IUserService
    {
        private readonly List<Room> _rooms;
        private readonly List<UserConnection> userRooms;
        private List<UserVote> _usersVotes;
        private List<UserConnection> _userConnections;

        public UserService()
        {
            _rooms = new List<Room>();
            _usersVotes = new List<UserVote>();
            _userConnections = new List<UserConnection>();
            userRooms = new List<UserConnection>();

        }

        public void AddVote(UserVote userVote)
        {
            _usersVotes.Add( new UserVote(){UserName = userVote.UserName, Vote = userVote.Vote});
        }
        public Dictionary<string, int> GetVotesForRoom(string name)
        {
            var users = GetUsersByRoom(name);
            Dictionary<string,int> votes = new Dictionary<string, int>();
            foreach (var user in users)
            {
                votes.Add(user,_usersVotes.First(x=>x.UserName ==user).Vote);
            }
            return votes;
        }

        public void ResetVote(string name)
        {
            var userlist = GetUsersByRoom(name);
            foreach (var uservote in _usersVotes)
            {
                foreach (var user in userlist)
                {
                    if (user == uservote.UserName)
                    {
                        _usersVotes.Remove(uservote);
                    }
                }
            }
        }


        public void DeleteUser(string id, string item)
        {
            try
            {
                _userConnections.Remove(new UserConnection(){ConnectionId = id ,Name = item});
                Log.Information(item + "successfully removed from local database");
            }
            catch (Exception ex)
            {
                Log.Information(item + " doesn`t exists in local database");
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
        public void AddRoom(Room room)
        {
            _rooms.Add(room);
        }
        public List<Room> GetRooms()
        {
            return _rooms;
        }

        public void AddUserToGroup(UserConnection userConnection)
        {
            userRooms.Add(userConnection);
        }

        public string GetRoomName(string id)
        {
            return userRooms.First(x => x.ConnectionId == id).Name;
        }

        public List<string> GetUsersByRoom(string name)
        {
            var userlist = userRooms.Where(x => x.Name == name);
            List<string> userNames = new List<string>();
            foreach (var user in userlist)
            {
                userNames.Add(GetUserByConnection(user.ConnectionId));
            }
            return userNames;

        }
    }
}
