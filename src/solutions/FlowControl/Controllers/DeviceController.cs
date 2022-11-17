using Microsoft.AspNetCore.Mvc;
using BaseAPI.Classes;
using Classes;
using System.Text.Json;
using System.Text.Json.Nodes;
using Engine.BO;
using Engine.Constants;
using Engine.BL.Actuators2;
using FlowControl.Hubs;
using Microsoft.AspNetCore.SignalR;
using Engine.BO.FlowControl;
using Engine.BO.AccessControl;

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
                Model = ParseProperty<string?>.GetValue("deviceModel", jObj, OnMissingProperty),
                Ip = ParseProperty<string?>.GetValue("ip", jObj, OnMissingProperty),
                IsActive = true,
                LastUpdate = DateTime.Now
            });

            if (result != null && result.Status == C.OK) 
            {
                var device = result?.InsertDetails?.CastObject<Device>();

                if (device != null && device.Name != null)
                {
                    device = bl.GetDevice(device.Name);
                    _hub.Clients.All.SendAsync(C.HUB_DEVICE_MONITOR, device);
                }
            }
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
            Result result = new ()
            {
                Status = C.OK,
                Message = C.COMPLETE
            };

            JsonObject jObj = JsonObject.Parse(obj.ToString());

            string? deviceName = ParseProperty<string>.GetValue("deviceName", jObj, OnMissingProperty);
            string? status = ParseProperty<string>.GetValue("status", jObj, OnMissingProperty);
            int? confidence = ParseProperty<int?>.GetValue("confidence", jObj);
            int? hintKey = ParseProperty<int?>.GetValue("hintKey", jObj);

            if (!string.IsNullOrEmpty(deviceName)) { 

                if (status == C.NOT_MATCH)
                {
                    result = new()
                    {
                        Status = C.NOT_MATCH,
                        Message = "Finger print did not match!",
                        Data = new object()
                    };

                } else if (!string.IsNullOrEmpty(status) && confidence != null && hintKey != null)
                {
                    var config = bl.GetDeviceHintConfigByHint(deviceName, (int)hintKey);

                    if(config != null)
                    {
                        var signal = new DeviceSignal(config, status, (int)confidence);
                        flowBl.ProcessTransaction(signal);
                        _hub.Clients.Group(deviceName).SendAsync(C.HUB_DEVICE_SIGNAL, signal);
                    }
                }                           

            } else throw new Exception("Device name Property is empty!");

            return  result;
        });
        
    }
}
