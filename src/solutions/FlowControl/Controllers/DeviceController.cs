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
using Engine.BO.AccessControl;
using System.Xml.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace FlowControl.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeviceController : CustomController
    {
        private readonly IHubContext<DeviceHub> _hub;
        private readonly DeviceBL bl = new ();
        private readonly EngineFlowBL flowBl = new();

        public DeviceController(IHubContext<DeviceHub> hub) : base() => _hub = hub;        

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

        [HttpPost("setHint")]
        public Result SetHint(dynamic obj) => RequestResponse(() =>
        {
            JsonObject jObj = JsonObject.Parse(obj.ToString());
            
            string? deviceName = ParseProperty<string>.GetValue("deviceName", jObj, OnMissingProperty);
            int employeeId = ParseProperty<int>.GetValue("employeeId", jObj, OnMissingProperty);
            int hintKey = ParseProperty<int>.GetValue("hintKey", jObj, OnMissingProperty);

            if (string.IsNullOrEmpty(deviceName))
                throw new Exception("Device name Property is empty!");

            return bl.SetDeviceEmployeeHint(deviceName, employeeId, hintKey);
        });

        [HttpPost("signal")]
        public Result EmitHint(dynamic obj) => RequestResponse(() =>
        {
            JsonObject jObj = JsonObject.Parse(obj.ToString());

            string? deviceName = ParseProperty<string>.GetValue("deviceName", jObj, OnMissingProperty);
            string? b64 = ParseProperty<string>.GetValue("b64", jObj, OnMissingProperty);
            bool auth = ParseProperty<bool>.GetValue("auth", jObj, OnMissingProperty);

            return C.OK; //flowBl;
        });
        
    }
}
