using Classes;
using Engine.BO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Nodes;
using Engine.BO.DocsControl;
using Engine.BL.Actuators3;
using Engine.Constants;

namespace DocsControl.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DocumentController : CustomController
    {
        private readonly DocumentBL bl = new();
        
        [HttpPost]
        public Result SetDocument(dynamic obj) => RequestResponse(() =>
        {
            JsonObject jObj = JsonObject.Parse(obj.ToString());
            return bl.SetDocument(new Document() {
                Id = ParseProperty<int>.GetValue("id", jObj),
                Name = ParseProperty<string>.GetValue("name", jObj, OnMissingProperty),
                DocType = new DocType()
                {
                    Id = ParseProperty<int>.GetValue("typeId", jObj)
                }                
            });
        });        
    }
}
