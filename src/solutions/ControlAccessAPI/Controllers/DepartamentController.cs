using Microsoft.AspNetCore.Mvc;
using System.Collections;
using System.Text.Json.Nodes;
using Engine.BL;
using Engine.BO;
using Classes;
using Engine.Constants;
using Engine.BO.AccessControl;
using Engine.BL.Actuators;
using System.Text.Json.Nodes;

namespace ControlAccess.Controllers;

[Route("api/[controller]")]
[ApiController]
public class DepartamentController : CustomController
{
    private readonly DepartmentBL bl = new();

    [HttpGet]
    public Result GetDepartaments() => RequestResponse(() => bl.GetDepartments());

    [HttpGet("{id:int}")]
    public Result GetDepartament(int id) => RequestResponse(() => bl.GetDepartment(id));

    [HttpPost]
    public Result SetDepartament(dynamic obj) => RequestResponse(() => {
        JsonObject jObj = JsonObject.Parse(obj.ToString());
        return bl.SetDepartament(new Department(){
            Id = JsonProperty<int?>.GetValue("id", jObj),
            Name = JsonProperty<string>.GetValue("name", jObj, OnMissingProperty),
            Code = JsonProperty<string>.GetValue("code", jObj, OnMissingProperty)
        }, C.GLOBAL_USER);
    });

}