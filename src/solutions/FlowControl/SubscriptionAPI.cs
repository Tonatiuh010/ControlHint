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
            };
        }

        public void SendAPI()
        {
            try
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

            } catch
            {

            }
        }        

    }
}
