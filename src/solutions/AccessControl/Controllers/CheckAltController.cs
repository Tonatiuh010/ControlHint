using Classes;
using Engine.BL.Actuators;
using Engine.BO;
using Engine.BO.AccessControl;
using Engine.BO.DocsControl;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Nodes;

namespace DocsControl.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CheckAltController : CustomController
    {
        private readonly CheckAltBL bl = new();

        [HttpPost]
        public Result SetCheckAlt(dynamic obj) => RequestResponse(() =>
        {
            JsonObject jObj = JsonObject.Parse(obj.ToString());
            return bl.SetCheckAlt(new CheckAlt()
            {
                Id = ParseProperty<int>.GetValue("id", jObj),

                Device = new Engine.BO.FlowControl.Device()
                {
                    Id = ParseProperty<int>.GetValue("deviceId", jObj, OnMissingProperty)
                },
                Employee = new Employee()
                {
                    Id = ParseProperty<int>.GetValue("employeeId", jObj, OnMissingProperty)
                }
            });
        });
    }
}
