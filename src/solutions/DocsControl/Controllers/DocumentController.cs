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

        [HttpGet]
        public Result GetDocuments() => RequestResponse(() => bl.GetDocuments());

        [HttpGet("{id:int}")]
        public Result GetDocument(int id) => RequestResponse(() => bl.GetDocument(id));

        [HttpPost("file")]
        public Result SetFile(dynamic obj) => RequestResponse(() =>
        {
            JsonObject jObj = JsonObject.Parse(obj.ToString());
            string? b64 = ParseProperty<string>.GetValue("b64", jObj, OnMissingProperty);

            return bl.SetFile(new()
            {
                Id = ParseProperty<int>.GetValue("id", jObj),
                Document = new()
                {
                    Id = ParseProperty<int>.GetValue("documentId", jObj, OnMissingProperty),
                },                
                DocImg = string.IsNullOrEmpty(b64) ? null : Convert.FromBase64String( DocFile.RemoveHeaderB64(b64) )
            });
        });

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
