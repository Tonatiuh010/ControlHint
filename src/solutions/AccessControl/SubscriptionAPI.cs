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
        public SubscriptionAPI()
        {
        }

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
                new Endpoint()
                {
                    Id = 1,
                    Api = GetAPI(),
                    Params = new List<Parameter>()
                    {
                        new Parameter()
                        {
                            Id = 1, 
                            Name = "employeeId",
                            IsRequired = true,
                            ContentType = "application/json",
                            Description = "ID of Employee",
                        },
                        new Parameter()
                        {
                            Id = 2, 
                            Name = "deviceHintId",
                            IsRequired = true,
                            ContentType = "application/json",
                            Description = "ID Hint in Local MEMORY of Device",
                        },
                        new Parameter()
                        {
                            Id = 3,
                            Name = "isValid",
                            IsRequired = true,
                            ContentType = "application/json",
                            Description = "Is Authorized to continue",
                        }
                    }, 
                    RequestType = "POST",
                    Route = "Check"
                }                
            };
        }

        public void SendAPI()
        {
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
        }

    }
}
