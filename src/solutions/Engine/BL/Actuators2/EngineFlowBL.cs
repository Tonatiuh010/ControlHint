using DataService.Services;
using DataService.WebClient;
using Engine.BO;
using Engine.BO.FlowControl;
using Engine.Constants;
using Engine.DAL;
using Engine.Services;
using Google.Protobuf.WellKnownTypes;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.Tsp;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using System.Web;
using WebRequest = DataService.WebClient.WebRequest;

namespace Engine.BL.Actuators2
{
    public class EngineFlowBL : BaseBL<FlowControlDAL>
    {
        private readonly FlowBL bl = new ();

        public EngineFlowBL()
        {

        }

        public Result ProcessTransaction(string deviceName, JsonObject data)
        {
            int flowId = GetDeviceFlowId(deviceName);
            Flow? flow = bl.GetFlow(flowId);

            if (flow != null)                            
                ExecuteFlow(flow, data);

            return null;
        }

        private int GetDeviceFlowId(string deviceName) => Dal.GetDeviceFlow(
            Dal.GetDeviceId(deviceName), 
            null
        );
        

        private void ExecuteFlow(Flow flow, JsonObject data)
        {
            // Add logs!!! flow.Name
            List<Step> steps = flow.Steps.OrderBy(x => x.Sequence).ToList();

            foreach(Step step in steps)
            {
                // Add logs!! step.                
                Endpoint endpoint = step.Endpoint;
                List<Parameter> parameters = endpoint.Params;
                string? resourceString = GetResourceUrl(parameters, data);
                string? queryString = GetQueryUrl(parameters, data);
                // Add logs!! endpoint                

                if(!string.IsNullOrEmpty(endpoint.RequestType) && !string.IsNullOrEmpty(endpoint.Api.Url) && !string.IsNullOrEmpty(endpoint.Route))
                {
                    WebRequest request = new (endpoint.Api.Url);
                    JsonObject jObj = new();

                    string route = endpoint.Route + 
                        (!string.IsNullOrEmpty(resourceString)? resourceString : string.Empty) +
                        (!string.IsNullOrEmpty(queryString)? ("?" + queryString) : string.Empty)
                    ;

                    List<Parameter> contentParams = parameters
                        .Where(x => !IsResourceType(x) && !IsURLType(x))
                        .ToList();

                    foreach(var p in parameters)
                    {
                        if(!string.IsNullOrEmpty(p.Name))
                        {
                            jObj[p.Name] = data[p.Name];
                        }
                    }

                    var param = jObj.Cast<object>();

                    RequestProperties requestProps = new ()
                    {
                        Method = RequestProperties.GetMethod(endpoint.RequestType),
                        EndPoint = route,
                        Params = new HttpContentService(param)
                    };
                }
            }
            // Add logs!!!
        }

        private static bool IsURLType(Parameter x) => x.ContentType == C.URL;
        private static bool IsResourceType(Parameter x) => x.ContentType == C.RESOURCE;

        private static KeyValuePair<string, string?> CreateParamValue(string? name, JsonObject data)
        {
            if (string.IsNullOrEmpty(name)) throw new Exception($"Value Name is null!! -> EngineFlowBL.CreateQueryURL");

            KeyValuePair<string, string?> queryParam;
            JsonNode? jsonNode = data[name];

            if (jsonNode != null)
            {
                string value = jsonNode.GetValue<string>();
                queryParam = new (name, value);
            } else
            {
                queryParam = new(name, null);
            }

            return queryParam;
        }

        private static string? GetQueryUrl(List<Parameter> parameters, JsonObject data)
        {
            string? queryString = string.Empty;

            if (parameters.Any(IsURLType))
            {
                NameValueCollection query = HttpUtility.ParseQueryString(string.Empty);
                var queryParams = parameters
                    .Where(IsURLType)
                    .Select(x => CreateParamValue(x.Name, data))
                    .ToList();

                foreach (var d in queryParams)
                {
                    query.Add(d.Key, d.Value);
                }

                queryString = query.ToString();
            }

            return queryString;
        }

        private static string? GetResourceUrl(List<Parameter> parameters, JsonObject data)
        {
            string? queryString = string.Empty;

            if (parameters.Any(IsResourceType))
            {                
                var queryParams = parameters
                    .Where(IsResourceType)
                    .Select(x => CreateParamValue(x.Name, data))
                    .ToList();

                foreach (var d in queryParams)
                {
                    queryString += string.IsNullOrEmpty(d.Value) ? $"{d.Key}/" : $"{d.Key}/{d.Value}/";
                }                
            }

            return queryString;
        }
    }
}
