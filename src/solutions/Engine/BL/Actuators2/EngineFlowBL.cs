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
using System.ComponentModel;
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

        public async Task<JsonObject?> ProcessTransaction(DeviceSignal signal)
        {
            Flow? flow = signal.HintConfig.Device.Flow;
            Device? device = signal.HintConfig.Device;
            DeviceHintConfig config = signal.HintConfig;

            if (flow != null && device != null)
            {
                return await ExecuteFlow(flow, new JsonObject()
                {
                    ["deviceId"] = device.Id,
                    ["hintKey"] = config.HintKey,
                    ["employeeId"] = config.Employee.Id,
                    ["statusFinger"] = signal.StatusFinger,
                    ["confidence"] = signal.Confidence
                });
            }
            else return null;
        }       
        

        private async Task<JsonObject> ExecuteFlow(Flow flow, JsonObject data)
        {
            // Add logs!!! flow.Name
            List<Step> steps = flow.Steps.OrderBy(x => x.Sequence).ToList();
            JsonObject result = new() {
                ["status"] = C.OK,
                ["message"] = C.COMPLETE
            };

            try
            {
                int counter = 1;
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

                        //foreach(var p in parameters)
                        //{
                        //    if(!string.IsNullOrEmpty(p.Name))
                        //    {
                        //        jObj.Remove(p.Name);
                        //        jObj.Add(p.Name, data[p.Name]?.ToString());
                        //    }
                        //}

                        RequestProperties requestProps = new ()
                        {
                            Method = RequestProperties.GetMethod(endpoint.RequestType),
                            EndPoint = route,
                            Params = new HttpContentService(data.ToJsonString())
                        };

                        requestProps.SetContent(requestProps.Params);

                        switch(endpoint.RequestType)
                        {
                            case "POST":
                                result["data" + counter] = await GetContentString( await request.PostRequest(requestProps) );
                                break;
                            case "GET":
                                result["data" + counter] = await GetContentString(await request.PostRequest(requestProps));
                                break;
                            default:
                                result["status"] = "NO_METHOD_DEFINED " + endpoint.RequestType;
                                result["message"] = "No method recognized!";
                                break;
                        }                   
                    }

                    counter++;
                }
            }
            catch (Exception ex)
            {
                result["status"] = C.ERROR;
                result["message"] = ex.Message;
            }
            // Add logs!!!

            return result;
        }

        private async Task<JsonObject?> GetContentString(HttpResponseMessage response)
        {

            var str = await response.Content.ReadAsStringAsync();
            return (JsonObject?)JsonNode.Parse(str);
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
