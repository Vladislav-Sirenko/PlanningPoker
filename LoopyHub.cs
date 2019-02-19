﻿using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace PlanningPoker
{
    public class LoopyHub : Hub
    {
        public Task Send(string data)
        {
            return Clients.All.SendAsync("Send", data);
        }
    }
}
