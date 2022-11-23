using Engine.BL.Actuators2;
using Engine.BO;
using Engine.BO.FlowControl;
using BaseAPI;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using Endpoint = Engine.BO.FlowControl.Endpoint;

namespace AccessControl
{
    public class SubscriptionAPI : ISubscriptionAPI
    {
        public SubscriptionAPI() { }

        public API GetAPI()
        {
            return new API()
            {
                Id = 1,
                Url = Builder.URL,
                Description = "Access Control API for Employee and System status"
            };
        }

        public List<Endpoint> GetEndpoints()
        {
            return new()
            {
               CheckEndpoint()            
            };
        }

        public void SendAPI()
        {
            try {  

                FlowBL bl = new FlowBL();

                bl.SetAPI(GetAPI());

                foreach(var endpoint in GetEndpoints())
                {
                    bl.SetEndpoint(endpoint);
                    foreach(var param in endpoint.Params)
                    {
                        bl.SetParameter(param, (int)endpoint.Id);
                    }
                }

            } catch
            {

            }
        }

        private Endpoint CheckEndpoint() => new Endpoint()
        {
            Id = 1,
            Api = GetAPI(),
            RequestType = "POST",
            Route = "Check",
            Params = new List<Parameter>()
            {
                new Parameter()
                {
                    Id = 1,
                    ContentType = "application/json",
                    IsRequired = true,
                    Name = "employeeId",
                    Description = "Employee ID, from the check",
                },
                new Parameter()
                {
                    Id = 2,
                    ContentType = "application/json",
                    IsRequired = true,
                    Name = "deviceId",
                    Description = "Device ID, from the check",
                }
            }
        };

    }
}
