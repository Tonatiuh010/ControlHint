using Classes;
using Engine.BL.Actuators3;
using Engine.BO;
using Engine.BO.DocsControl;
using Engine.Constants;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Nodes;

namespace DocsControl.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DocFlowController : CustomController
    {
        private readonly DocFlowBL bl = new();
        
        [HttpPost]
        public Result SetDocument(dynamic obj) => RequestResponse(() =>
        {
            JsonObject jObj = JsonObject.Parse(obj.ToString());
            return bl.SetDocFlow(new DocFlow()
            {
                Id = ParseProperty<int>.GetValue("id", jObj),
                DocType = new DocType() { 
                    Id = ParseProperty<int>.GetValue("typeId", jObj, OnMissingProperty), 
                },
                Key1 = ParseProperty<string>.GetValue("key1", jObj, OnMissingProperty),
                Key2 = ParseProperty<string>.GetValue("key2", jObj),
                Key3 = ParseProperty<string>.GetValue("key3", jObj),
                Key4 = ParseProperty<string>.GetValue("key4", jObj)
            });
        });
    }
}
