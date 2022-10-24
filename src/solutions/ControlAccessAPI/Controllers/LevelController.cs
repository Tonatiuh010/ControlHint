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
public class LevelController : CustomController
{

    private readonly AccessLevelBL bl = new();
    private readonly EmployeeBL employeeBl = new ();

    [HttpGet]
    public Result GetAccessLevels() => RequestResponse(() => bl.GetAccessLevels());

    [HttpGet("EmployeeLevel/{id:int}")]
    public Result GetEmployeeAccessLevel(int id) => RequestResponse(() => employeeBl.GetEmployeeAccessLevels(id));

    [HttpPost]
    public Result SetLevel(dynamic obj) => RequestResponse(() => {
        JsonObject jObj = JsonObject.Parse(obj);
        return bl.SetAccessLevel(
            new () 
            { 
                Id = JsonProperty<int?>.GetValue("id", jObj), 
                Name = JsonProperty<string>.GetValue("name", jObj, OnMissingProperty) 
            }, 
            C.GLOBAL_USER
        );
    });

}