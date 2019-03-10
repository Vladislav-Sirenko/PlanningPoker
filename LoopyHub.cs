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
        public static List<UserConnection> userConnections;
        IUserService _userService;
        public LoopyHub(IUserService userService)
        {
            _userService = userService;
            userConnections = new List<UserConnection>();
        }


        public Task Send(string data)
        {
            return Clients.All.SendAsync("Send", data);
        }
        public Task Vote(string data)
        {
            return Clients.All.SendAsync("Vote", data);
        }

        public Task ResetVotes()
        {
            return Clients.All.SendAsync("ResetVotes");
        }

        public Task GetVotes()
        {
            return Clients.All.SendAsync("GetVotes");
        }

        public Task Connect(string data)
        {
            _userService.AddUserConnection(Context.ConnectionId, data);
            Log.Information(data + "connected to application with connection id:" + Context.ConnectionId);
            return Clients.All.SendAsync("Connect", data);
        }
        public Task Disconnect(string data)
        {
            Log.Information("Trying to delete user");
            var name = _userService.GetUserByConnection(Context.ConnectionId);
            Log.Information("User disconnected with connection name:" + name);
            _userService.DeleteUser(name);
            return Clients.All.SendAsync("Disconnect", name);           
        }

    }
}
