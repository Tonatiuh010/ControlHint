using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Classes;
using BaseAPI.Classes;
using Engine.BL.Actuators3;
using Engine.BO;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using Engine.BO.DocsControl;

namespace DocsControl.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionController : CustomController
    {
        private readonly TxnBL bl = new();

        [HttpGet("{id:int}")]
        public Result GetTransaction(int id) => RequestResponse(() => bl.GetTransaction(id));

        [HttpPost("document")]
        public Result CreateDocument(dynamic obj) => RequestResponse(() =>
        {            
            JsonObject jObj = JsonObject.Parse(obj.ToString());

            int documentId = ParseProperty<int>.GetValue("documentId", jObj, OnMissingProperty);
            string key = ParseProperty<string>.GetValue("key", jObj, OnMissingProperty);

            return bl.SetDocumentTxn(documentId, key);
        });

        [HttpPost("approve")]
        public Result SetApproverTxn(dynamic obj)
        {
            JsonObject jObj = JsonObject.Parse(obj.ToString());
            int documentId = ParseProperty<int>.GetValue("documentId", jObj, OnMissingProperty);
            


            return bl.SetApproverTxn(new ApproverStep() { 
                DocumentDetail = new ()
                {
                    Id = ParseProperty<int>.GetValue("approverDocId", jObj, OnMissingProperty),
                    Approver = new Approver()
                    {
                        Id = ParseProperty<int>.GetValue("approverId", jObj, OnMissingProperty),
                    }
                },
                Status = ParseProperty<string>.GetValue("status", jObj, OnMissingProperty),
                Comments = ParseProperty<string>.GetValue("comments", jObj)
            }, documentId);
        }

    }
}