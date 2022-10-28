using Microsoft.AspNetCore.Mvc;
using BaseAPI.Classes;
using Classes;
using System.Text.Json;
using System.Text.Json.Nodes;
using Engine.BO;
using Engine.Constants;
using Engine.BL.Actuators2;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using FlowControl.Hubs;
using Microsoft.AspNetCore.SignalR;
using Engine.BO.FlowControl;

namespace FlowControl.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeviceController : CustomController
    {
        private readonly IHubContext<DeviceHub> _hub;
        private readonly DeviceBL bl = new ();

        public DeviceController(IHubContext<DeviceHub> hub) : base()
        {
            _hub = hub;
        }

        [HttpGet]
        public Result Get() => RequestResponse(() => bl.GetDevices());

        [HttpGet("{id}")]
        public Result Get(int id) => RequestResponse(() => bl.GetDevice(id));

        [HttpPost]
        public Result Post(dynamic obj) => RequestResponse(() =>
        {
            JsonObject jObj = JsonObject.Parse(obj.ToString());

            var result = bl.SetDevice(new()
            {
                Id = ParseProperty<int?>.GetValue("id", jObj),
                Name = ParseProperty<string?>.GetValue("deviceName", jObj, OnMissingProperty),
                Ip = ParseProperty<string?>.GetValue("ip", jObj, OnMissingProperty)
            });

            if (result.Message == C.OK)
                _hub.Clients.All.SendAsync("DeviceMonitor", result?.InsertDetails?.CastObject<Device>());

            return result;
        });
        
    }
}
