using BaseAPI;
using Engine.BL.Actuators2;
using Engine.BO.FlowControl;
using Endpoint = Engine.BO.FlowControl.Endpoint;

namespace FlowControl
{
    public class SubscriptionAPI : ISubscriptionAPI
    {

        public API GetAPI()
        {
            return new API()
            {
                Id = 2,
                Url = Builder.URL,
                Description = "Flow Control API Handle Configuration, Signals & Devices"
            };
        }

        public List<Endpoint> GetEndpoints()
        {
            return new()
            {
                new Endpoint()
                {
                    Id = 2,
                    Api = GetAPI(),
                    Params = new List<Parameter>()
                    {
                        new Parameter()
                        {
                            Id = 4,
                            Name = "deviceName",
                            IsRequired = true,
                            ContentType = "application/json",
                            Description = "Name of Device where comes the signal",
                        },
                        new Parameter()
                        {
                            Id = 5,
                            Name = "deviceHintId",
                            IsRequired = true,
                            ContentType = "application/json",
                            Description = "ID Hint in Local MEMORY of Device",
                        },                        
                    },
                    RequestType = "HUB",
                    Route = "showHint"
                },
                //new Endpoint()
                //{
                //    Id = 3,
                //    Api = GetAPI(),
                //    Params = new List<Parameter>()
                //    {                        
                //    },
                //    RequestType = "HUB",
                //    Route = "showHint"
                //}
            };
        }

        public void SendAPI()
        {
            FlowBL bl = new FlowBL();

            bl.SetAPI(GetAPI());

            foreach (var endpoint in GetEndpoints())
            {
                bl.SetEndpoint(endpoint);
                foreach (var param in endpoint.Params)
                {
                    bl.SetParameter(param, (int)endpoint.Id);
                }
            }
        }
    }
}
