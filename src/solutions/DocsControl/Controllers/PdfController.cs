using Aspose.Finance.Ofx.Profile;
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
using _Aspose = Aspose.Pdf;

namespace DocsControl.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PdfController : CustomController
    {

        private readonly DocumentBL bl = new();

        [HttpGet("quotation")]
        public IActionResult SetQuotationDoc(dynamic obj)
        {
            return PreparePdf(() =>
            {
                JsonObject jObj = JsonObject.Parse(obj.ToString());
                List<QuotationItem> items = new List<QuotationItem>();
                var jItems = jObj["items"];

                string dateStr = ParseProperty<string>.GetValue("date", jObj, OnMissingProperty);
                string duoDtStr = ParseProperty<string>.GetValue("duoDate", jObj, OnMissingProperty);

                if(jItems != null)
                {
                    var jArr = jItems.AsArray();

                    if(jArr != null)
                    {
                        foreach(JsonObject? jItem in jArr)
                        {
                            items.Add(new()
                            {
                                Id = ParseProperty<int>.GetValue("id", jItem),
                                Name = ParseProperty<string>.GetValue("name", jItem),
                                Code = ParseProperty<string>.GetValue("code", jItem),
                                Description = ParseProperty<string>.GetValue("description", jItem),
                                Qty = ParseProperty<int>.GetValue("qty", jItem),
                                Value = ParseProperty<decimal>.GetValue("value", jItem)
                            });
                        }
                    }
                }

                var quotationDoc = Quotation.CreateQuotation(new QuotationParameters()
                {
                    Id = ParseProperty<int>.GetValue("id", jObj),
                    Name = ParseProperty<string>.GetValue("name", jObj, OnMissingProperty),
                    Client = ParseProperty<string>.GetValue("client", jObj, OnMissingProperty),
                    Contact = ParseProperty<string>.GetValue("contact", jObj, OnMissingProperty),
                    Date = DateTime.Parse(dateStr),
                    DuoDate = DateTime.Parse(duoDtStr),                    
                    Items = items,
                    Notes = ParseProperty<string>.GetValue("notes", jObj, OnMissingProperty),
                });

                return quotationDoc.Document;
            });            
        }

        [HttpGet("sale")]
        public IActionResult SetSaleDoc(dynamic obj)
        {
            return PreparePdf(() =>
            {
                JsonObject jObj = JsonObject.Parse(obj.ToString());
                string dtStr = ParseProperty<string>.GetValue("date", jObj, OnMissingProperty);

                var saleDoc = Sale.CreateSale(new()
                {
                    Id = ParseProperty<int>.GetValue("id", jObj),
                    Place = ParseProperty<string>.GetValue("place", jObj, OnMissingProperty),
                    Item = ParseProperty<string>.GetValue("item", jObj, OnMissingProperty),
                    Dt = DateTime.Parse(dtStr),

                    SalesAddress = ParseProperty<string>.GetValue("salesAddress", jObj, OnMissingProperty),
                    SalesNum = ParseProperty<int>.GetValue("salesNum", jObj, OnMissingProperty),
                    SalesPlace = ParseProperty<string>.GetValue("salesPlace", jObj, OnMissingProperty),
                    SalesSign = ParseProperty<string>.GetValue("salesSign", jObj, OnMissingProperty),

                    CustomerAddress = ParseProperty<string>.GetValue("customerAddress", jObj, OnMissingProperty),
                    CustomerNum = ParseProperty<int>.GetValue("customerNum", jObj, OnMissingProperty),
                    CustomerPlace = ParseProperty<string>.GetValue("customerPlace", jObj, OnMissingProperty),
                    CustomerSign = ParseProperty<string>.GetValue("customerSign", jObj, OnMissingProperty),

                    CustomerName = ParseProperty<string>.GetValue("customerName", jObj, OnMissingProperty),

                    Law = ParseProperty<string>.GetValue("law", jObj, OnMissingProperty),

                    Total = ParseProperty<decimal>.GetValue("total", jObj, OnMissingProperty)

                });

                return saleDoc.Document;
            });
        }

        [HttpGet("view/{id:int}")]
        public IActionResult ViewPdf(int id)
        {
            return PreparePdf(() =>
            {
                var doc = bl.GetDocument(id);
                return null;
            });
        }

        private static IActionResult PreparePdf(Func<_Aspose.Document> getDoc)
        {
            try
            {
                MemoryStream output = new();
                var doc = getDoc();
                doc.Save(output);
                return new FileStreamResult(output, "application/pdf");
            }
            catch (Exception e)
            {
                Result result = new Result()
                {
                    Status = C.ERROR,
                    Message = C.ERROR,
                    Data = e
                };

                return new JsonResult(result);
            }
        }
    }
}
