using Engine.BL.Actuators2;
using Engine.BO.AccessControl;
using Engine.BO.FlowControl;
using Engine.Constants;
using D = Engine.BL.Delegates;
using Engine.Services;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json.Serialization;

namespace FlowControl.Hubs
{
    public class DeviceHub : Hub
    {        
        private static readonly DeviceBL bl = new ();

        private ExceptionManager ExceptionManager { get; set; } = new(OnError);

        public async Task BroadcastDevice(Device device)
            => await Clients.All.SendAsync(C.HUB_DEVICE_MONITOR, device);

        public async Task AddToGroup(string groupName)
        {
            var device = bl.GetDevice(groupName);

            if (device != null && !string.IsNullOrEmpty(device.Name)) 
                await Groups.AddToGroupAsync(Context.ConnectionId, device.Name);            

            ExceptionManager.Dispose();
        }

        public async Task RemoveFromGroup(string groupName)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);

            ExceptionManager.Dispose();
        }

        private static void OnError(Exception ex, string msg)
        {
            Console.WriteLine(ex.Message);
        }

    }
}
