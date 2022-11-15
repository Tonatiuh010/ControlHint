﻿using Classes;
using Engine.BL.Actuators3;
using Engine.BO;
using Engine.BO.AccessControl;
using Engine.BO.DocsControl;
using Engine.Constants;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Nodes;

namespace DocsControl.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApproverController : CustomController
    {
        private readonly ApproverBL bl = new();

        
        [HttpPost]
        public Result SetApprover(dynamic obj) => RequestResponse(() =>
        {
            JsonObject jObj = JsonObject.Parse(obj.ToString());
            return bl.SetApprover(new Approver()
            {
                Id = ParseProperty<int>.GetValue("id", jObj),
                FullName = ParseProperty<string>.GetValue("fullName", jObj, OnMissingProperty),
                Position = new Position() { PositionId = ParseProperty<int>.GetValue("positionId", jObj, OnMissingProperty) },
                Depto = new Department() { Id = ParseProperty<int>.GetValue("deptoId", jObj, OnMissingProperty) }
            });
        });
    }
}