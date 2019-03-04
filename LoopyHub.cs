using Microsoft.AspNetCore.SignalR;
using PlanningPoker.Models;
using PlanningPoker.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

        public Task Connect(string data)
        {
            userConnections.Add(new UserConnection { Name = data, ConnectionId = Context.UserIdentifier });
          return Clients.All.SendAsync("Connect", data);
        }
        public override Task OnDisconnectedAsync(Exception exception)
        {
           var name =  userConnections.First(x => x.ConnectionId == Context.UserIdentifier).Name;
            return Clients.All.SendAsync("Disconnect",name);
        }

    }
}
