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

    //[HttpGet("chartView")]
    //public Result GetCheckDetails() => RequestResponse(() => {
    //    return CheckDetails.GetChecksByDepto(bl.GetCheckDetails(DateTime.Today.AddDays(-1), DateTime.Now), DateTime.Today.AddDays(-1));
    //});

    [HttpGet("{id:int}")]
    public Result GetCheck(int id) => RequestResponse(() => bl.GetCheck(id));

    [HttpPost] // Add header token
    public Result Set(dynamic obj) => RequestResponse(() => {
        JsonObject jObj = JsonObject.Parse(obj.ToString());
        
        Check check = new ()
        {
            Employee = new Employee()
            {
                Id = ParseProperty<int>.GetValue("employeeId", jObj, OnMissingProperty)
            },
            Device = new Engine.BO.FlowControl.Device() 
            { 
                Id = ParseProperty<int>.GetValue("deviceId", jObj, OnMissingProperty)
            } 
        };

        var insertResult = bl.SetCheck(check);

        return bl.GetCheck((int)insertResult?.InsertDetails?.Id);
    });

    [HttpPost("checkEmployee")]
    public Result SetCheckEmployee(dynamic obj) => RequestResponse(() => {
        JsonObject jObj = JsonObject.Parse(obj.ToString());
        return bl.SetCheckEmployee(new Check()
        {
            Id = ParseProperty<int?>.GetValue("id", jObj),
            Employee = new Employee()
            {
                Id = ParseProperty<int>.GetValue("employeeId", jObj, OnMissingProperty)
            },
            CheckDt = ParseProperty<DateTime>.GetValue("checkDt", jObj),
            CheckType = ParseProperty<string>.GetValue("checkType", jObj),
            Device = new Engine.BO.FlowControl.Device()
            {
                Id = ParseProperty<int>.GetValue("deviceId", jObj)
            }
            
        });
    });

    [HttpGet("employee/{id:int}")]
    public Result GetWeeklyChecks(int id) => RequestResponse(() =>
    {
        var from = Utils.StartOfWeek(DateTime.Today, DayOfWeek.Sunday);
        var to = from.AddDays(6);
        
        return bl.GetWeeklyChecks(from, to, id);
    });

}