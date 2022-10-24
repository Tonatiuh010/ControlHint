﻿using Microsoft.AspNetCore.SignalR;
using System;
using Engine.BO;
using Engine.BO.AccessControl;

namespace ControlAccess.Hubs
{
    public class CheckHub : Hub
    {
        public async Task BroadcastCheck(Check check)
            => await Clients.All.SendAsync("CheckMonitor", check);

        public async Task BroadcastGreeting(string urName) =>
            await Clients.All.SendAsync("test", $"Hello, {urName}");

    }
}
