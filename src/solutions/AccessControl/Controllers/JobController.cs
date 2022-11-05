using Microsoft.AspNetCore.Mvc;
using System.Collections;
using System.Text.Json.Nodes;
using Engine.BL;
using Engine.BO;
using Classes;
using Engine.Constants;
using Engine.BL.Actuators;
using Engine.BO.AccessControl;

namespace ControlAccess.Controllers;

[Route("api/[controller]")]
[ApiController]
public class JobController : CustomController
{
    private readonly JobBL bl = new ();
    private readonly PositionBL positionBl = new ();

    [HttpGet]
    public Result GetJobs() => RequestResponse(() => bl.GetJobs());

    [HttpGet("{id:int}")]
    public Result GetJob(int id) => RequestResponse(() => bl.GetJob(id));

    [HttpPost]
    public Result SetJob(dynamic obj) => RequestResponse(() => {
        JsonObject jObj = JsonObject.Parse(obj.ToString());

        return bl.SetJob(
            new Job() {
                Id = ParseProperty<int>.GetValue("id", jObj),
                Description = ParseProperty<string>.GetValue("description", jObj),
                Name = ParseProperty<string>.GetValue("name", jObj, OnMissingProperty)
            },
            C.GLOBAL_USER
        );
    });

}