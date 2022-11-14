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

        // POST: DocumentController/Create
        [HttpPost]
        public Result SetDocument(dynamic obj) => RequestResponse(() =>
        {
            JsonObject jObj = JsonObject.Parse(obj.ToString());
            return bl.SetDocFlow(new DocFlow()
            {
                Id = ParseProperty<int>.GetValue("id", jObj),
                TypeID = ParseProperty<int>.GetValue("name", jObj),
                Key1 = ParseProperty<int>.GetValue("typeID", jObj),
                Key2 = ParseProperty<int>.GetValue("typeID", jObj),
                Key3 = ParseProperty<int>.GetValue("typeID", jObj),
                Key4 = ParseProperty<int>.GetValue("typeID", jObj)
            }, C.GLOBAL_USER);
        });
    }
}
