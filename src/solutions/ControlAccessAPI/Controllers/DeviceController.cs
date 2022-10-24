using System;
using Microsoft.AspNetCore.Mvc;
using System.Collections;
using System.Text.Json.Nodes;
using Engine.BL;
using Engine.BO;
using Classes;
using Engine.Constants;
namespace ControlAccess.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeviceController : CustomController
    {
        //[HttpGet]
        //public Result GetDevice() => RequestResponse(() => bl.GetDevices());

        //[HttpGet("{id:int?}")]
        //public Result GetDevice(int? id) => RequestResponse(() =>
        //    GetItem(bl.GetDevices(id))
        //);
    }
}
