using Classes;
using Engine.BL.Actuators2;
using Engine.BO;
using FlowControl.Hubs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json.Serialization;
using NuGet.Common;
using System.Net.WebSockets;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace FlowControl.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FlowDeviceController : CustomController
    {
        private readonly EngineFlowBL bl = new();        
        private readonly IHubContext<DeviceHub> _hub;

        public FlowDeviceController(IHubContext<DeviceHub> hub) => _hub = hub;

        [HttpGet("/webSocket/{deviceName}")]
        public async Task WebSocketAction(string deviceName)
        {
            if (HttpContext.WebSockets.IsWebSocketRequest)
            {
                using var webSocket = await HttpContext.WebSockets.AcceptWebSocketAsync();
                await WSClient.AddClient(deviceName, webSocket, _hub);
            }
            else
            {
                HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
            }
        }

        [HttpPost("actionRegisterHint")]
        public async Task<Result> RegisterHint(dynamic obj)
        {
            DeviceClient? client = null;

            var msg = RequestResponse(() =>
            {
                JsonObject requestBody;
                JsonObject jObj = JsonObject.Parse(obj.ToString());
                string? deviceName = ParseProperty<string>.GetValue("deviceName", jObj, OnMissingProperty);
                int employeeId = ParseProperty<int>.GetValue("employeeId", jObj, OnMissingProperty);

                if (deviceName != null) { 
                    client = WSClient.FindClient(deviceName);
                    requestBody = WSClient.RegisterFingerRequest(deviceName, employeeId);
                } else
                {
                    requestBody = new()
                    {
                        ["info"] = "No Device Name Property Attached"
                    };
                }

                return requestBody;
            });

            var str = JsonSerializer.Serialize<Result>(msg, options: new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase                
            });

            if (client != null)
                await client.SendMessage(str);

            return msg;
        }

        [HttpGet("send")]
        public async Task SendHello(string deviceName)
        {
            var client = WSClient.FindClient(deviceName);

            if (client != null)
                await client.SendMessage("Testing this crazy outcome msg");
            
        }
    }
}
