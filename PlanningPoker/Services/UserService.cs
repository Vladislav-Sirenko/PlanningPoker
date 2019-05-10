using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using PlanningPoker.Context;
using PlanningPoker.Models;
using PlanningPoker.Repostitories;
using Serilog;

namespace PlanningPoker.Services
{
    public class UserService : IUserService
    {
        private readonly IHubContext<LoopyHub> _hubContext;
        private readonly IRoomsRepository _roomsRepository;
        private readonly IUserRepository _userRepository;

        public UserService(IHubContext<LoopyHub> hubContext, IRoomsRepository repository, IUserRepository userRepository)
        {
            _hubContext = hubContext;
            _roomsRepository = repository;
            _userRepository = userRepository;
        }

        public async void AddVote(string name, int vote)
        {
            var user = _userRepository.GetByNameAsync(name);
            user.Vote = vote;
            _userRepository.UpdateAsync(user);
            var room = _roomsRepository.GetByIdAsync(user.RoomId);
            await _hubContext.Clients.Group(room.Id).SendAsync("Vote", user.Name);
        }

        public List<User> GetUsersByRoom(string id)
        {
            return _userRepository.GetUsersByRoomId(id).ToList();
        }

        public void GetVotesForRoom(string id)
        {
            if (id == null)
                return;
            var room = _roomsRepository.GetByIdAsync(id);
            var users = _userRepository.GetUsersByRoomId(id);
            Dictionary<string, int> votes = new Dictionary<string, int>();
            foreach (var user in users)
            {
                if (user.Vote != null) votes.Add(user.Name, user.Vote.Value);
            }
            _hubContext.Clients.Group(room.Id).SendAsync("GetVotes", votes);
        }

        public string GetRoleForRoom(string userName, string id)
        {
            var room = _roomsRepository.GetByIdAsync(id);
            if (room != null)
            {
                if (userName == room.CreatorName)
                    return "Admin";
                return "Guest";
            }
            return null;
        }

        public IEnumerable<string> GetRoles(string[] users, string id)
        {

            List<string> roles = new List<string>();
            foreach (var user in users)
            {
                roles.Add(GetRoleForRoom(user, id));
            }
            _hubContext.Clients.Group(id).SendAsync("GetRolesForRoom");
            return roles;

        }

        public void ResetVote(string id)
        {
            var userlist = _userRepository.GetUsersByRoomId(id).ToList();
            foreach (var user in userlist)
            {
                user.Vote = null;
            }
            _userRepository.UpdateRangeAsync(userlist);
            _hubContext.Clients.Group(id).SendAsync("ResetVotes");
        }


        //public void DeleteUser(string id)
        //{
        //    try
        //    {
        //        var removedUserConnection = _userConnections.First(x => x.ConnectionId == id);
        //        _userConnections.Remove(removedUserConnection);
        //        var removedUser = _userRooms.First(x => x.ConnectionId == id);
        //        if (removedUser != null)
        //            _userRooms.Remove(removedUser);

        //        Log.Information(removedUserConnection.Name + "successfully removed from local database");
        //    }
        //    catch (Exception ex)
        //    {
        //        Log.Information("Disconnected user doesn`t exists in local database" + ex.Data);
        //    }
        //}

        public User AddUserConnection(string id, string roomId, string userName)
        {
            var user = _userRepository.GetByNameAsync(userName);
            if (user != null) 
            {
                user.ConnectionId = id;
                user.RoomId = roomId;
                _userRepository.UpdateAsync(user);
            }
            return user;

        }
        //public string GetUserByConnection(string id)
        //{
        //    var name = _userConnections.First(x => x.ConnectionId == id).Name;
        //    return name;
        //}
        //public string GetConnectionByUserName(string name)
        //{
        //    var id = _userConnections.First(x => x.Name == name).ConnectionId;
        //    return id;
        //}
        public async Task<Room> AddRoom(Room room)
        {
            var entity = await _roomsRepository.AddAsync(room);
            await _hubContext.Clients.All.SendAsync("AddRoom");
            return entity;
        }
        public void DeleteRoom(string id)
        {
            var users = _userRepository.GetUsersByRoomId(id);
            foreach (var user in users)
            {
                user.RoomId = null;
            }
            _userRepository.UpdateRangeAsync(users.ToList());
            _roomsRepository.DeleteAsync(id);
            _hubContext.Clients.All.SendAsync("DeleteRoom");
        }
        public async Task<List<Room>> GetRooms()
        {
            return await _roomsRepository.GetRoomsAsync();
        }

        public string GetRoomByUserName(string userName)
        {
            var user = _userRepository.GetByNameAsync(userName);
            var room = _roomsRepository.GetByIdAsync(user.RoomId);
            return room.Name;
        }

        public User AddUser(User user)
        {
            return _userRepository.AddAsync(user);
        }

        public User GetUserByConnectionId(string id)
        {
            return _userRepository.GetUserByConnectionId(id);
        }
        public void DeleteUserFromRoom(string userName)
        {
            var roomName = GetRoomByUserName(userName);
            _userRepository.DeleteUserFromRoom(userName);
            _hubContext.Clients.Group(roomName).SendAsync("Disconnect", userName);


        }
    }
}
