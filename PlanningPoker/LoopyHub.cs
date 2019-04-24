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

        public async Task Join(string id, string userName)
        {
            _userService.AddUserConnection(Context.ConnectionId, id, userName);
            await Groups.AddToGroupAsync(Context.ConnectionId, id);
            await Clients.Group(id).SendAsync("Join");
        }
        public async Task Send(string data)
        {
            var group = _userService.GetRoomByUserName(data.Split(":")[0]);
            await Clients.Group(group).SendAsync("Send", data);
        }
        //public Task Disconnect(string data)
        //{
        //    var group = _userService.GetRoomName(Context.ConnectionId);
        //    Groups.RemoveFromGroupAsync(group, Context.ConnectionId);
        //    Log.Information("Trying to delete user");
        //    Log.Information("User disconnected with connection name:" + data);
        //    _userService.DeleteUser(Context.ConnectionId);
        //    UserDisconnected();
        //    return Clients.Group(group).SendAsync("Disconnect", data);
        //}

        public override Task OnDisconnectedAsync(Exception exception)
        {
            var user = _userService.GetUserByConnectionId(Context.ConnectionId);
            var roomId = user.RoomId;
            Groups.RemoveFromGroupAsync(roomId, Context.ConnectionId);
            _userService.DeleteUserFromRoom(user.Name);
            return Clients.Group(roomId).SendAsync("Disconnect", user.Name);
        }

        //public Task UserDisconnected()
        //{
        //    _userService.DeleteUser(Context.ConnectionId);
        //    return Clients.Caller.SendAsync("UserDisconnect");
        //}

    }
}
