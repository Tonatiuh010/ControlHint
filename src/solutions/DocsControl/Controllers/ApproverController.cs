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
    public class ApproverController : CustomController
    {
        private readonly ApproverBL bl = new();

        // POST: DocumentController/Create
        [HttpPost]
        public Result SetApprover(dynamic obj) => RequestResponse(() =>
        {
            JsonObject jObj = JsonObject.Parse(obj.ToString());
            return bl.SetApprover(new Approver()
            {
                Id = ParseProperty<int>.GetValue("id", jObj),
                FullName = ParseProperty<string>.GetValue("name", jObj, OnMissingProperty),
                PositionID = ParseProperty<int>.GetValue("typeID", jObj),
                DeptoID = ParseProperty<int>.GetValue("typeID", jObj)
            }, C.GLOBAL_USER);
        });
    }
}
