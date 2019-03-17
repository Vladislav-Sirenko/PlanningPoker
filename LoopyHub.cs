using Microsoft.AspNetCore.SignalR;
using PlanningPoker.Models;
using PlanningPoker.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Serilog;

namespace PlanningPoker
{
    public class LoopyHub : Hub
    {
       
        IUserService _userService;
        public LoopyHub(IUserService userService)
        {
            _userService = userService;
        }

         public Task AddRoom(Room room)
        {
            _userService.AddRoom(room);
            return Clients.All.SendAsync("AddRoom");
        }

        public Task Join(string id)
        {
            _userService.AddUserToGroup(new UserConnection(){Name = id,ConnectionId = Context.ConnectionId});
             Groups.AddToGroupAsync(Context.ConnectionId, id);
             var group = _userService.GetRoomName(Context.ConnectionId);
              return Clients.Group(group).SendAsync("Join");
        }
        public Task Send(string data)
        {
            var group = _userService.GetRoomName(Context.ConnectionId);
            return Clients.Group(group).SendAsync("Send", data);
        }
        public Task Vote(string data)
        {
          var group = _userService.GetRoomName(Context.ConnectionId);
            return Clients.Group(group).SendAsync("Vote", data);
        }

        public Task ResetVotes()
        {
          var group = _userService.GetRoomName(Context.ConnectionId);
            return Clients.Group(group).SendAsync("ResetVotes");
        }

        public Task GetVotes()
        {
          var group = _userService.GetRoomName(Context.ConnectionId);
            return Clients.Group(group).SendAsync("GetVotes");
        }

        public Task Connect(string data)
        {
            _userService.AddUserConnection(Context.ConnectionId, data);
            Log.Information(data + "connected to application with connection id:" + Context.ConnectionId);
            return Clients.Group("lol").SendAsync("Connect", data);
        }
        public Task Disconnect(string data)
        {
            var group = _userService.GetRoomName(Context.ConnectionId);
            Groups.RemoveFromGroupAsync(group, Context.ConnectionId);
            Log.Information("Trying to delete user");
          //  var name = _userService.GetUserByConnection(Context.ConnectionId);
            Log.Information("User disconnected with connection name:" + data);
            _userService.DeleteUser(Context.ConnectionId,data);
            return Clients.All.SendAsync("Disconnect", data);
        }

    }
}
