using Classes;
using Engine.BL.Actuators2;
using Engine.BO;
using Engine.Constants;
using Engine.BO.FlowControl;
using FlowControl.Mappers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Nodes;
using Endpoint = Engine.BO.FlowControl.Endpoint;
using Engine.BO.AccessControl;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace FlowControl.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EngineController : CustomController
    {
        private readonly FlowBL bl = new FlowBL();


        [HttpGet("flow")]
        public Result GetFlows() => RequestResponse(() => bl.GetFlows());

        [HttpGet("flow/{id:int}")]
        public Result GetFlow(int id) => RequestResponse(() => bl.GetFlow(id));


        [HttpGet("step")]
        public Result GetSteps() => RequestResponse(() => bl.GetSteps());

        [HttpGet("step/{id:int}")]
        public Result GetStep(int id) => RequestResponse(() => bl.GetStep(id));

        [HttpPost]
        public Result SetAPI(dynamic obj)
        {
            API api = new ();
            List<Endpoint> endpoints = new();           

            try
            {
                JsonObject jObj = JsonObject.Parse(obj.ToString());
                JsonArray? jActions = jObj["actions"]?.AsArray();

                api = EngineMapper.PostApiMapper(jObj, OnMissingProperty);                

                if (jActions != null)
                {
                    endpoints = EngineMapper.PostEndpointsMapper(jActions, api, OnMissingProperty);                                        
                }
            } catch
            {
                return new Result()
                {
                    Status = C.ERROR,
                    Message = "Error reading Json -> " + obj.ToString()
                };
            }            

            return RequestResponse(
                () => bl.SetAPI(api).InsertDetails, 
                () =>
                {
                    List<object?> results = new();

                    foreach (Endpoint endpoint in endpoints)
                        results.Add(bl.SetEndpoint(endpoint)?.InsertDetails);

                    return results;
                },
                () =>
                {
                    List<object?> results = new();

                    foreach (Endpoint endpoint in endpoints)
                    {
                        foreach (var param in endpoint.Params)
                        {
                            results.Add(bl.SetParameter(param, (int)endpoint.Id).InsertDetails);
                        }
                    }

                    return results;
                }
            );
        }

    }
}
