using Engine.BL.Actuators2;
using Engine.BO;
using Engine.Constants;
using Microsoft.Extensions.Logging.Abstractions;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading;

namespace FlowControl
{
    public static class WSClient
    {

        private readonly static DeviceBL bl = new();
        private readonly static List<DeviceClient> Clients = new ();

        public static async Task AddClient(string deviceName, WebSocket ws)
        {
            DeviceClient client = new (deviceName, ws);            
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

        public static Result ProcessAction(string response)
        {
            try
            {
                Result actionResult = new () { };
                JsonNode? jObj = JsonNode.Parse(response);

                if (jObj != null)
                {

                }

                return actionResult;
            } catch (Exception ex)
            {
                return new Result()
                {
                    Status = C.ERROR,
                    Message = ex.Message,
                    Data = ex.ToString()
                };
            }            
        }

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
    }

    public class DeviceClient
    {
        public string DeviceName { get; set; }
        private WebSocket Socket { get; set; }
        private CancellationTokenSource TokenSource { get; set; }

        public DeviceClient(string deviceName, WebSocket ws)
        {
            DeviceName = deviceName;
            Socket = ws;
            TokenSource = new CancellationTokenSource();
        }

        public async Task Start() => await LoopThread(Socket, TokenSource.Token);

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

        public void SetSocket(WebSocket ws) => Socket = ws;        

        public void Close()
        {
            TokenSource.Cancel();
            Socket.Abort();
            Socket.Dispose();
        }

        public static async Task LoopThread(WebSocket ws, CancellationToken token)
        {
            try
            {
                var buffer = new byte[1024 * 100];
                var receiveResult = await ws.ReceiveAsync(new ArraySegment<byte>(buffer), token);

                while (!receiveResult.CloseStatus.HasValue)
                {
                    string response = Encoding.Default.GetString(buffer);                    
                    
                    if (response != null)
                    {
                        string callbackPayload = JsonSerializer.Serialize(WSClient.ProcessAction(response));
                        byte[] callbackBytes = Encoding.Default.GetBytes(callbackPayload);

                        await ws.SendAsync(
                            new ArraySegment<byte>(callbackBytes),
                            WebSocketMessageType.Text,
                            true,
                            token
                        );
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
