using Engine.BO.AccessControl;
using Engine.BO.FlowControl;
using Microsoft.AspNetCore.SignalR;

namespace FlowControl.Hubs
{
    public class DeviceHub : Hub
    {
        public async Task BroadcastDevice(Device device)
            => await Clients.All.SendAsync("DeviceMonitor", device);

    }
}
