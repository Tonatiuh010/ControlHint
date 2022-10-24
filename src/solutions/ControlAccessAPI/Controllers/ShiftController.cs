using Microsoft.AspNetCore.Mvc;
using System.Collections;
using Engine.BL;
using Engine.BO;
using Classes;
using Engine.Constants;
using Engine.BL.Actuators;
using System.Text.Json.Nodes;

namespace ControlAccess.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ShiftController : CustomController
{
    private readonly ShiftBL bl = new ();

    [HttpGet]
    public Result GetShifts() => RequestResponse(() => bl.GetShifts());

    [HttpGet("{id:int}")]
    public Result GetShift(int id) => RequestResponse(() => bl.GetShift(id));

    [HttpPost]
    public Result SetShift(dynamic obj) => RequestResponse(() => {
        Shift shift;
        
        JsonObject jObj = JsonObject.Parse(obj.ToString());        
        shift = new (JsonProperty<string>.GetValue("inTime", jObj, OnMissingProperty), JsonProperty<string>.GetValue("outTime", jObj, OnMissingProperty)) 
        {
            Id = JsonProperty<int?>.GetValue("id", jObj),
            Name = JsonProperty<string>.GetValue("name", jObj, OnMissingProperty),
            LunchTime = Shift.ConvertTime( JsonProperty<string>.GetValue("lunchTime", jObj, OnMissingProperty) ),
            DayCount = JsonProperty<int?>.GetValue("dayCount", jObj),
        };        

        return bl.SetShift(shift, C.GLOBAL_USER);
    });
}