using System;
using Microsoft.AspNetCore.Mvc;
using System.Collections;
using System.Text.Json.Nodes;
using Engine.BL;
using Engine.BO;
using Classes;
using Engine.Constants;
using Engine.BL.Actuators;

namespace ControlAccess.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CatalogController : CustomController
    {

        private ShiftBL bl = new ();
        private PositionBL positionBl = new ();
        private AccessLevelBL levelBl = new ();
        

        [HttpGet]
        [Route("Assets")]
        public Result GetEmployeeAssets() => RequestResponse(
            () => bl.GetShifts(),
            () => positionBl.GetPositions(),            
            () => levelBl.GetAccessLevels()
        );

    }
}
