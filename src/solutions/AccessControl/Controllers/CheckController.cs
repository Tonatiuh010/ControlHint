using Microsoft.AspNetCore.Mvc;
using System.Collections;
using System.Text.Json.Nodes;
using Engine.BO;
using Engine.BO.AccessControl;
using Classes;
using Engine.Constants;
//using ControlAccess.Hubs;
using Microsoft.AspNetCore.SignalR;
using Engine.BL.Actuators;

namespace ControlAccess.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CheckController : CustomController
{    
    private readonly CheckBL bl = new ();    

    [HttpGet]
    public Result Get() => RequestResponse(() => bl.GetChecks());

    [HttpGet("chartView")]
    public Result GetCheckDetails() => RequestResponse(() => {
        return CheckDetails.GetChecksByDepto(bl.GetCheckDetails(DateTime.Today.AddDays(-1), DateTime.Now), DateTime.Today.AddDays(-1));
    });

    [HttpGet("employee/{id:int}")]
    public Result GetWeeklyChecks(int id) => RequestResponse(() => $"Getting checks of employee ({id})");

    [HttpGet("{id:int}")]
    public Result GetCheck(int id) => RequestResponse(() => bl.GetCheck(id));

    [HttpPost] // Add header token
    public Result Set(dynamic obj) => RequestResponse(() => {
        JsonObject jObj = JsonObject.Parse(obj.ToString());

        int employeeId = ParseProperty<int>.GetValue("employeeId", jObj, OnMissingProperty);
        Check check = new ()
        {
            Device = ParseProperty<int>.GetValue("deviceId", jObj, OnMissingProperty)
        };


        return bl.SetCheck(check, employeeId);
    });

}