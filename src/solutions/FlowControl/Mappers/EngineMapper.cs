using Classes;
using Engine.BO.FlowControl;
using System.Text.Json.Nodes;
using D = Engine.BL.Delegates;
using Endpoint = Engine.BO.FlowControl.Endpoint;

namespace FlowControl.Mappers
{
    public static class EngineMapper
    {
        public static API PostApiMapper(JsonObject jObj, D.CallbackExceptionMsg? onMissingProp = null) => new()
        {
            Id = ParseProperty<int?>.GetValue("apiId", jObj),
            Description = ParseProperty<string?>.GetValue("description", jObj),
            Url = ParseProperty<string>.GetValue("apiUrl", jObj, onMissingProp)
        };

        public static List<Endpoint> PostEndpointsMapper(JsonArray jArray, API api, D.CallbackExceptionMsg? onMissingProp = null)
        {
            List<Endpoint> endpoints = new ();
            

            foreach(JsonObject jObj in jArray.Cast<JsonObject>())
            {                
                List<Parameter> parameters = new ();
                JsonArray? jArrayParams = jObj["params"]?.AsArray();

                if (jArrayParams != null) 
                {
                    foreach(JsonObject jParam in jArrayParams.Cast<JsonObject>())
                    {
                        if(jParam != null)
                        {
                            parameters.Add(new Parameter()
                            {
                                Id = ParseProperty<int?>.GetValue("parameterId", jParam),
                                Name = ParseProperty<string>.GetValue("parameterName", jParam, onMissingProp),
                                ContentType = ParseProperty<string?>.GetValue("parameterType", jParam, onMissingProp),
                                IsRequired = ParseProperty<bool>.GetValue("isRequired", jParam, onMissingProp),
                                Description = ParseProperty<string?>.GetValue("description", jParam),
                            });
                        }
                    }
                }

                endpoints.Add(new Endpoint()
                {
                    Api = api,
                    Id = ParseProperty<int?>.GetValue("actionId", jObj),
                    RequestType = ParseProperty<string>.GetValue("actionType", jObj, onMissingProp),
                    Route = ParseProperty<string>.GetValue("actionUrl", jObj, onMissingProp),
                    Params = parameters 
                });
            }

            return endpoints;
        }
    }
}
