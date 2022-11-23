using Classes;
using Engine.BL.Actuators3;
using Engine.BO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DocsControl.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FlowApproverController : CustomController
    {
        private readonly ApproverBL bl = new();

        [HttpGet]
        public Result GetDocsApprovers() => RequestResponse(() => bl.GetDocsApprovers());

        [HttpGet("{id:int}")]
        public Result GetDocApprover(int id) => RequestResponse(() => bl.GetDocApprover(id));
    }
}
