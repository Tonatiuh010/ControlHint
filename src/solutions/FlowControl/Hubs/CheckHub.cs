using Microsoft.AspNetCore.SignalR;
using System;
using Engine.BO;
using Engine.BO.AccessControl;

namespace FlowControl.Hubs
{
    public class CheckHub : Hub
    {
        public async Task BroadcastCheck(Check check)
            => await Clients.All.SendAsync("CheckMonitor", check);

    }
}
