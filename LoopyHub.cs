using Microsoft.AspNetCore.SignalR;
using PlanningPoker.Models;
using PlanningPoker.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Serilog;
using Microsoft.AspNetCore.Mvc;

namespace PlanningPoker
{
    public class LoopyHub : Hub
    {
        readonly IUserService _userService;
        public LoopyHub(IUserService userService)
        {
            _userService = userService;
        }

        public Task Join(string id)
        {
            _userService.AddUserToGroup(new UserConnection() { Name = id, ConnectionId = Context.ConnectionId });
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
            Log.Information("User disconnected with connection name:" + data);
            _userService.DeleteUser(Context.ConnectionId);
            UserDisconnected();
            return Clients.Group(group).SendAsync("Disconnect", data);
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            var group = _userService.GetRoomName(Context.ConnectionId);
            Groups.RemoveFromGroupAsync(group, Context.ConnectionId);
            Log.Information("Trying to delete user");
            Log.Information("User disconnected with connection name:" + exception);
            var name = _userService.GetUserByConnection(Context.ConnectionId);
            _userService.DeleteUser(Context.ConnectionId);
            UserDisconnected();
            return Clients.Group(group).SendAsync("Disconnect", name);

        }

        public Task UserDisconnected()
        {
            _userService.DeleteUser(Context.ConnectionId);
            return Clients.Caller.SendAsync("UserDisconnect");
        }

        public Task GetRole(string id)
        {
            var role = _userService.GetRoleForRoom(Context.ConnectionId, id);
            return Clients.Caller.SendAsync("GetRoles", role);
        }
    }
}
