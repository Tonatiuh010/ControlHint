using System;
using Microsoft.AspNetCore.Mvc;
using System.Collections;
using System.Text.Json.Nodes;
using Engine.BL;
using Engine.BO;
using Classes;
using Engine.Constants;
using Engine.BL.Actuators;
using Engine.BO.AccessControl;

namespace ControlAccess.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PositionController : CustomController
    {
        private readonly PositionBL bl = new ();

        [HttpGet]
        public Result GetPostitions() => RequestResponse(() => bl.GetPositions());

        [HttpGet("{id:int}")]
        public Result GetPosition(int id) => RequestResponse(() => bl.GetPosition(id));
        
        [HttpPost]
        public Result SetPostition(dynamic obj) =>  RequestResponse(() => {
            JsonObject jObj = JsonObject.Parse(obj.ToString());

            return bl.SetPosition(
                new Position()
                {
                    PositionId = ParseProperty<int?>.GetValue("position", jObj),
                    Id = ParseProperty<int?>.GetValue("job", jObj, OnMissingProperty),
                    Department =new Department() { 
                        Id = ParseProperty<int?>.GetValue("departament", jObj, OnMissingProperty) 
                    },
                    Alias = ParseProperty<string?>.GetValue("alias", jObj, OnMissingProperty)                    
                },
                C.GLOBAL_USER
            );
        });
    }
}
