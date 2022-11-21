using Classes;
using DocsControl.Ocr;
using DocsControl.Ocr.FlowDocs;
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
    public class PdfController : CustomController
    {

        [HttpGet]
        public IActionResult GetPdf()
        {
            MemoryStream output = new ();
            var doc = Quotation.CreateQuotation(new QuotationParameters()
            {
                Name = "test",
                Client = "CLIENT TEST",
                Contact = "Contact test",
                Date = DateTime.Now,
                DuoDate = DateTime.Now.AddDays(14),
                Id = 1,
                Items = new ()
                {
                    new ()
                    {
                        Id = 1,
                        Name = "Item 1",
                        Code = "ITM 1",
                        Description = "This is a brief description",
                        Qty = 2,
                        Value = 300 
                    },
                    new ()
                    {
                        Id = 2,
                        Name = "Item 2",
                        Code = "ITM 2",
                        Description = "This is a brief description",
                        Qty = 1,
                        Value = 400
                    },
                },
                Notes = "Crazy Notes",                
            });
            doc.Document.Save(output);
            return new FileStreamResult(output, "application/pdf");
        }

        //[HttpGet("test")]
        //public Result doStuff()
        //{
        //    return RequestResponse(() =>
        //    {
        //        OcrScan.TestPdf();  
        //        return C.OK;
        //    });
        //}
    }


}
