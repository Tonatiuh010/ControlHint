using Engine.Constants;
using Engine.BO.FlowControl;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Classes;
using Engine.BL.Actuators2;
using Engine.BO;
using System.Text.Json.Nodes;

namespace FlowControl.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : CustomController
    {
        private readonly UserBL bl = new UserBL();

        [HttpGet]
        public Result GetUsers() => RequestResponse( () => bl.GetUsers());

        [HttpPost]
        public Result Auth(dynamic obj)
        {
            User? _user = null;

            return RequestResponse(() =>
            {
                JsonObject jObj = JsonObject.Parse(obj.ToString());

                string? userName = ParseProperty<string>.GetValue("userName", jObj, OnMissingProperty);
                string? password = ParseProperty<string>.GetValue("password", jObj, OnMissingProperty);

                if (userName != null && password != null)
                {
                    var user = bl.GetUser(userName);

                    if (user != null && user.Password == password)
                    {
                        _user = user;
                        return C.OK;
                    }
                    else
                    {
                        return C.NOT_AUTH;
                    }

                }
                else
                {
                    return C.NOT_AUTH;
                }
            }, () => _user);
        }

        [HttpPost("new")]
        public Result SetUser(dynamic obj) => RequestResponse(() =>
        {
            JsonObject jObj = JsonObject.Parse(obj.ToString());
            User user = new User()
            {
                Id =  ParseProperty<int?>.GetValue("employeeId", jObj, OnMissingProperty),
                UserName = ParseProperty<string>.GetValue("userName", jObj, OnMissingProperty),
                Password = ParseProperty<string>.GetValue("password", jObj, OnMissingProperty),
                UserType = ParseProperty<string>.GetValue("type", jObj, OnMissingProperty),
            };            

            return bl.SetUser(user);
        });
    }
}
