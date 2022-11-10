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

    //[HttpGet("employee/{id:int}")]
    //public Result GetWeeklyChecks(int id) => RequestResponse(() => $"Getting checks of employee ({id})");

    [HttpGet("{id:int}")]
    public Result GetCheck(int id) => RequestResponse(() => bl.GetCheck(id));

    [HttpPost] // Add header token
    public Result Set(dynamic obj) => RequestResponse(() => {
        JsonObject jObj = JsonObject.Parse(obj.ToString());
        ResultInsert result = new ();

        //var serial = JsonProperty<string>.GetValue("serial", jObj, 
        //    OnMissingProperty
        //);
        //var device = JsonProperty<string>.GetValue("device", jObj, 
        //    OnMissingProperty
        //);

        //var deviceObj = bl.GetDevice(device);

        //if (deviceObj != null && deviceObj.IsValid())
        //{
        //    AccessCheck checkStats;

        //    result = bl.SetCheck(
        //        serial,
        //        deviceObj.Id,
        //        C.GLOBAL_USER
        //    );

        //    if (result != null && result.Status == C.OK)
        //    {
        //        var check = GetItem(bl.GetChecks(result.InsertDetails.Id));
        //        checkStats = new AccessCheck()
        //        {
        //            Check = check,
        //            IsValid = true,
        //            Status = C.OK,
        //            Message = C.COMPLETE
        //        };
        //        result.Data = check;

        //    } else
        //    {
        //        CardEmployee? card = bl.GetCards(assigned: true).Find(x => x.Key == serial);
        //        var ch = new Check()
        //        {
        //            Id = null,
        //            CheckDt = DateTime.Now,
        //            Device = deviceObj.Id,
        //            Type = C.ERROR,
        //            Card = card
        //        };

        //        ch.SetDeviceFinder(id => {
        //            var devices = bl.GetDevices(id);
        //            return devices != null && devices.Count > 0 ? devices[0] : null;
        //        });

        //        string? msg = string.Empty;

        //        try
        //        {
        //            if (!string.IsNullOrEmpty(result?.Message))
        //            {
        //                int index = result.Message.IndexOf("Says:");
        //                msg = result.Message.Substring(index, (result.Message.Length - 1) - index);
        //            }
        //        } catch
        //        {
        //            msg = result.Message;
        //        }                

        //        checkStats = new AccessCheck()
        //        {
        //            IsValid = false,
        //            Message = msg,
        //            Status = C.ERROR,
        //            Check = ch
        //        };
        //    }

        //    _hub.Clients.All.SendAsync("CheckMonitor", checkStats);

        //} else
        //{
        //    result.Status = C.ERROR;
        //    result.Message = "Device no existe";
        //}
            
        return result;
    });

}