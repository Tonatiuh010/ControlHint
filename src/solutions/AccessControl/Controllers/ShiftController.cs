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
        shift = new (ParseProperty<string>.GetValue("inTime", jObj, OnMissingProperty), ParseProperty<string>.GetValue("outTime", jObj, OnMissingProperty)) 
        {
            Id = ParseProperty<int?>.GetValue("id", jObj),
            Name = ParseProperty<string>.GetValue("name", jObj, OnMissingProperty),
            LunchTime = Shift.ConvertTime( ParseProperty<string>.GetValue("lunchTime", jObj, OnMissingProperty) ),
            DayCount = ParseProperty<int?>.GetValue("dayCount", jObj),
        };        

        return bl.SetShift(shift, C.GLOBAL_USER);
    });
}