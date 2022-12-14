using Classes;
using Engine.BL.Actuators2;
using Engine.BO;
using Engine.Constants;
using FlowControl.Hubs;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging.Abstractions;
using NuGet.Common;
using System.ComponentModel.Design.Serialization;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading;
using static Org.BouncyCastle.Math.EC.ECCurve;

namespace FlowControl
{
    public static class WSClient
    {

        private readonly static DeviceBL bl = new();
        private readonly static List<DeviceClient> Clients = new ();

        public static async Task AddClient(string deviceName, WebSocket ws, IHubContext<DeviceHub> hub)
        {
            DeviceClient client = new (deviceName, ws, hub);
            int index = Clients.FindIndex(x => x.DeviceName == deviceName);

            if (index != -1)
            {
                Clients[index].Close();
                Clients[index] = client;
            } else
            {
                Clients.Add(client);                
            }

            await client.Start();
        }

        public static DeviceClient? FindClient(string deviceName) => Clients.Find(x => x.DeviceName == deviceName);

        public static JsonObject RegisterFingerRequest(string deviceName, int employeeId)
        {
            JsonObject result = new() {
                ["action"] = C.REGISTER_FINGER,
                ["deviceName"] = deviceName,
                ["employeeId"] = employeeId,
            };

            var config = bl.GetDeviceHintConfig(deviceName, employeeId);

            if (config != null)
            {
                result["hintKey"] = config.HintKey;
            } else
            {
                result["hintKey"] = 0;
            }

            return result;
        }

        public static JsonObject DeleteFingersRequest(string deviceName)
        {
            JsonObject result = new()
            {
                ["action"] = C.DELETE_HINTS,
                ["deviceName"] = deviceName,
            };

            return result;
        }

        public static JsonObject DeleteFingerRequest(string deviceName, int hintKey)
        {
            JsonObject result = new()
            {
                ["action"] = C.DELETE_HINT,
                ["deviceName"] = deviceName,
                ["hintKey"] = hintKey
            };

            return result;
        }

        internal static async Task<Result> ProcessAction(string response, IHubContext<DeviceHub> hub)
        {
            try
            {
                Result actionResult = new();
                JsonNode? jObj = JsonNode.Parse(response);

                if (jObj != null)
                {
                    string? deviceName = ParseProperty<string>.GetValue("deviceName", (JsonObject)jObj);
                    JsonNode? data = jObj["data"];

                    if (data != null && !string.IsNullOrEmpty(deviceName))
                    {
                        string? action = ParseProperty<string>.GetValue("action", (JsonObject)data);

                        if(!string.IsNullOrEmpty(action))                        
                            actionResult = await SelectAction(deviceName, action, (JsonObject)data, hub);
                        else
                        {
                            actionResult.Status = C.ERROR;
                            actionResult.Message = "No action property founded!";
                        }

                    } else
                    {
                        actionResult.Status = C.ERROR;
                        actionResult.Message = "No data or deviceName property founded!";
                    }
                } else
                {
                    actionResult.Status = C.ERROR;
                    actionResult.Message = "Request is Empty...";
                }

                return actionResult;
            }
            catch (Exception ex)
            {
                return new Result()
                {
                    Status = C.ERROR,
                    Message = ex.Message,
                    Data = ex.ToString()
                };
            }
        }

        private static async Task<Result> SelectAction(string deviceName, string action, JsonObject data, IHubContext<DeviceHub> hub)
        {
            switch(action)
            {
                case C.INFO:

                    string? msg = ParseProperty<string>.GetValue("msg", data);
                    string? type = ParseProperty<string>.GetValue("type", data);

                    await hub.Clients.Group(deviceName).SendAsync(C.HUB_DEVICE_INFO, type, msg);

                    return new Result()
                    {
                        Status = C.NO_CALLBACK,
                        Message = C.COMPLETE
                    };
                    
                default:
                    return new()
                    {
                        Status = C.NO_CALLBACK,
                        Message = "Do Nothing"                        
                    };                    
            }
        }

    }

    public class DeviceClient
    {
        public string DeviceName { get; set; }
        private WebSocket Socket { get; set; }
        private IHubContext<DeviceHub> Hub { get; set; }
        private CancellationTokenSource TokenSource { get; set; }

        public DeviceClient(string deviceName, WebSocket ws, IHubContext<DeviceHub> hub)
        {
            DeviceName = deviceName;
            Socket = ws;
            Hub = hub;
            TokenSource = new CancellationTokenSource();
        }

        public async Task Start() => await LoopThread(this);

        public async Task SendMessage(string msg)
        {
            if (Socket.State == WebSocketState.Open)
            {
                byte[] buffer = Encoding.Default.GetBytes(msg);

                await Socket.SendAsync(
                    new ArraySegment<byte>(buffer),
                    WebSocketMessageType.Text,
                    true,
                    TokenSource.Token
                );
            } else
            {
                throw new Exception("The socket client connection is closed!!");
            }
        }

        // public void SetSocket(WebSocket ws) => Socket = ws;

        public void Close()
        {
            TokenSource.Cancel();
            Socket.Abort();
            Socket.Dispose();
        }

        public static async Task LoopThread(DeviceClient client)
        {
            try
            {
                WebSocket ws = client.Socket;
                CancellationToken token = client.TokenSource.Token;
                IHubContext<DeviceHub> hub = client.Hub;

                var buffer = new byte[1024 * 100];
                var receiveResult = await ws.ReceiveAsync(new ArraySegment<byte>(buffer), token);

                while (!receiveResult.CloseStatus.HasValue)
                {
                    string response = Encoding.Default.GetString(buffer);                    
                    
                    if (response != null)
                    {
                        response = response.Replace("\0", string.Empty);
                        var result = await WSClient.ProcessAction(response, hub);                        

                        if(result.Status != C.NO_CALLBACK && result.Status != C.ERROR)
                        {
                            string callbackPayload = JsonSerializer.Serialize(result, options: C.CustomJsonOptions);
                            byte[] callbackBytes = Encoding.Default.GetBytes(callbackPayload);

                            await ws.SendAsync(
                                new ArraySegment<byte>(callbackBytes),
                                WebSocketMessageType.Text,
                                true,
                                token
                            );
                        }

                    }

                    receiveResult = await ws.ReceiveAsync(new ArraySegment<byte>(buffer), token);
                }

                await ws.CloseAsync(receiveResult.CloseStatus.Value, receiveResult.CloseStatusDescription, token);

            } catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
