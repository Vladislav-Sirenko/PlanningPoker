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
            var roomState = _userService.CheckSessionState(id);
            await Clients.Group(id).SendAsync("Join", roomState);
        }
        public async Task Send(string data)
        {
            var group = _userService.GetRoomByUserName(data.Split(":")[0]);
            await Clients.Group(group).SendAsync("Send", data);
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            var user = _userService.GetUserByConnectionId(Context.ConnectionId);
            if (user != null)
            {
                var roomId = user.RoomId;
                Groups.RemoveFromGroupAsync(roomId, Context.ConnectionId);
                _userService.DeleteUserFromRoom(user.Name);
            }
            return null;
        }

        public Task NotifyAdminRole()
        {
            var user = _userService.GetUserByConnectionId(Context.ConnectionId);
            if (user == null)
            {
                return null;
            }
            var roomId = user.RoomId;
            return Clients.Group(roomId).SendAsync("NotifyAdminRole", user.Name);

        }

    }
}

