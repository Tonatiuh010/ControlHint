using Microsoft.AspNetCore.Mvc;
using System.Collections;
using System.Text.Json.Nodes;
using Engine.BL;
using Engine.BO;
using Engine.BO.AccessControl;
using Classes;
using Engine.Constants;
using Engine.BL.Actuators;

namespace ControlAccess.Controllers;

[Route("api/[controller]")]
[ApiController]
public class EmployeeController : CustomController
{
    private readonly EmployeeBL bl = new ();
    private readonly AccessLevelBL levelBl = new ();

    [HttpGet]
    public Result GetEmployees() => RequestResponse(() => bl.GetEmployees());
    
    [HttpGet("{id:int?}")]
    public Result GetEmployee(int? id) => RequestResponse(() => bl.GetEmployee(id));

    [HttpPost]  
    public Result SetEmployee(dynamic obj) => RequestResponse(() => {       
        JsonObject jObj = JsonObject.Parse(obj.ToString());

        ImageData? img = null;

        var imgString = ParseProperty<string>.GetValue("image", jObj);

        if (!string.IsNullOrEmpty(imgString))
        {
            if (Uri.TryCreate(imgString, UriKind.Absolute, out Uri uri))
            {
                img = new ImageData(ImageData.GetBytesFromUrl(uri.AbsoluteUri));
            }
            else
            {
                img = imgString.Contains("base64") ? new ImageData(imgString) : new ImageData();
            }
        }

        Employee employee = new () {
            Id = ParseProperty<int?>.GetValue("id", jObj),
            Name = ParseProperty<string>.GetValue("name", jObj, OnMissingProperty),
            LastName = ParseProperty<string>.GetValue("lastName", jObj, OnMissingProperty),
            Job = new Position() {                    
                PositionId = ParseProperty<int?>.GetValue("position", jObj),
            },
            Shift = new Shift() {
                Id = ParseProperty<int>.GetValue("shift", jObj),
            },
            Image = img,            
        };

        var levels = ParseProperty<JsonArray>.GetValue("accessLevels", jObj);

        var resultInsert = bl.SetEmployee(employee, C.GLOBAL_USER);

        if ( resultInsert.Status == C.OK )
        {
            int? employeeId = resultInsert.InsertDetails.Id;

            //if (employee.Card != null && employee.Card.IsValid())
            //{

            //    var employeeCard = bl.GetCards(assigned: true).Find(x => x.Employee == employeeId);

            //    if (employeeCard != null && employeeCard.IsValid())
            //        bl.SetDownCard((int)employeeCard.Id, C.GLOBAL_USER);

            //    var chResult = bl.SetCard(new CardEmployee(employee.Card, employeeId), C.GLOBAL_USER);
                
            //}

            if (levels != null)
            {                
                var employeeLevels = bl.GetEmployeeAccessLevels(employeeId);
                var incomingLevels = levels.Select(x => levelBl.GetAccessLevel(x.ToString())).ToList();

                foreach (var level in incomingLevels)
                {
                    if (level != null && level.IsValid() && !employeeLevels.Any(x => x.Id == level.Id))
                    {
                        bl.SetEmployeeAccessLevel((int)employeeId, (int)level.Id, true, C.GLOBAL_USER);
                    }
                }

                foreach (var empLevel in employeeLevels)
                {
                    if (!incomingLevels.Any(x => x != null && x.IsValid() && x.Id == empLevel.Id))
                    {
                        bl.SetEmployeeAccessLevel((int)employeeId, (int)empLevel.Id, false, C.GLOBAL_USER);
                    }
                }

            }
        }        

        return resultInsert;
    });

    [HttpGet("image/{id:int?}")]    
    public IActionResult GetEmployeeImage(int? id)
    {
        byte[] bytes = Array.Empty<byte>();
        var obj = GetItem(bl.GetEmployees(id));

        if(obj != null)
        {
            var emp = obj;
            var imgBytes = emp.Image?.Bytes;

            if(imgBytes != null)
            {
                bytes = imgBytes;
            }
        }

        return File(bytes, "image/jpeg");
    }

    [HttpPost]
    [Route("DownEmployee")]
    public Result SetDownEmployee(dynamic obj) => RequestResponse(
        () => (Result)bl.SetDownEmployee(
            ParseProperty<int>.GetValue(
                "id", 
                JsonObject.Parse(obj.ToString()), 
                OnMissingProperty
            ),
            C.GLOBAL_USER
        )
    );

    [HttpPost]
    [Route("SetLevel")]
    public Result SetEmployeeAccessLevel(dynamic obj) => RequestResponse(() =>
    {
        JsonObject jObj = JsonObject.Parse(obj.ToString());
        return bl.SetEmployeeAccessLevel(
            ParseProperty<int>.GetValue("employee", jObj, OnMissingProperty),
            ParseProperty<int>.GetValue("level", jObj, OnMissingProperty),
            ParseProperty<bool>.GetValue("status", jObj, OnMissingProperty),
            C.GLOBAL_USER
        );
    });
}