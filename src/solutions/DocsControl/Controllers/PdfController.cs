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
using System.Text.Json;
using System.Text.Json.Nodes;
using _Aspose = Aspose.Pdf;

namespace DocsControl.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PdfController : CustomController
    {

        private readonly DocumentBL bl = new();

        [HttpPost("quotation")]
        public Result SetQuotationDoc(dynamic obj)
        {
            return RequestResponse(() =>
            {
                byte[]? bytesDoc = null;
                Quotation? quotationDoc = null;

                JsonObject jObj = JsonObject.Parse(obj.ToString());
                List<QuotationItem> items = new List<QuotationItem>();
                int typeDocId = bl.GetDocTypeId(C.QUO);
                int docId = ParseProperty<int>.GetValue("id", jObj);
                var jItems = jObj["items"];
                string name = ParseProperty<string>.GetValue("docName", jObj, OnMissingProperty);
                string dateStr = ParseProperty<string>.GetValue("date", jObj, OnMissingProperty);
                string duoDtStr = ParseProperty<string>.GetValue("duoDate", jObj, OnMissingProperty);

                var parameters = new QuotationParameters()
                {
                    Id = ParseProperty<int>.GetValue("id", jObj),
                    Name = ParseProperty<string>.GetValue("name", jObj, OnMissingProperty),
                    Client = ParseProperty<string>.GetValue("client", jObj, OnMissingProperty),
                    Contact = ParseProperty<string>.GetValue("contact", jObj, OnMissingProperty),
                    Date = DateTime.Parse(dateStr),
                    DuoDate = DateTime.Parse(duoDtStr),
                    Items = items,
                    Notes = ParseProperty<string>.GetValue("notes", jObj, OnMissingProperty),
                };

                if (jItems != null)
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

                var baseDoc = bl.GetDocument(docId);

                var _tmpDoc = bl.SetDocument(new Document()
                {
                    DocType = new DocType()
                    {
                        Id = typeDocId,
                    },
                    Name = name
                });

                docId = (int)_tmpDoc.InsertDetails.Id;
                parameters.Id = docId;
                quotationDoc = Quotation.CreateQuotation(parameters);

                if (quotationDoc != null)
                {
                    bytesDoc = GetDocBytes(quotationDoc.Document);

                    var _tmpFile = bl.SetExistingFile(new DocFileExt()
                    {
                        Document = new Document()
                        {
                            Id = docId
                        },
                        DocImg = bytesDoc,
                        Params = JsonSerializer.Serialize(parameters, options: C.CustomJsonOptions)
                    });

                    baseDoc = bl.GetDocument(docId);
                }

                return baseDoc;
            });            
        }

        [HttpPost("sale")]
        public Result SetSaleDoc(dynamic obj)
        {
            return RequestResponse(() =>
            {
                byte[]? bytesDoc = null;
                Sale? saleDoc = null;
                JsonObject jObj = JsonObject.Parse(obj.ToString());
                SaleParameters? parameters = null;

                int typeDocId = bl.GetDocTypeId(C.SALE);
                int docId = ParseProperty<int>.GetValue("id", jObj);
                string name = ParseProperty<string>.GetValue("docName", jObj, OnMissingProperty);
                string dtStr = ParseProperty<string>.GetValue("date", jObj, OnMissingProperty);

                parameters = new SaleParameters()
                {
                    Id = docId,
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
                };

                var baseDoc = bl.GetDocument(docId);

                var _tmpDoc = bl.SetDocument(new Document()
                {
                    DocType = new DocType()
                    {
                        Id = typeDocId,
                    },
                    Name = name,
                    Id = docId,
                });

                docId = (int)_tmpDoc.InsertDetails.Id;
                parameters.Id = docId;
                saleDoc = Sale.CreateSale(parameters);


                if ( saleDoc != null)
                {
                    bytesDoc = GetDocBytes(saleDoc.Document);

                    var _tmpFile = bl.SetExistingFile(new DocFileExt()
                    {
                        Document = new Document()
                        {
                            Id = docId
                        },
                        DocImg = bytesDoc,
                        Params = JsonSerializer.Serialize(parameters, options: C.CustomJsonOptions)
                    });

                    baseDoc = bl.GetDocument(docId);
                }                

                return baseDoc;
            });
        }

        [HttpGet("view/{id:int}")]
        public IActionResult ViewPdf(int id)
        {
            try
            {
                var doc = bl.GetDocument(id);
                return ReadBytes(doc.File.DocImg);
            }
            catch (Exception e)
            {
                Result result = new()
                {
                    Status = C.ERROR,
                    Message = C.ERROR,
                    Data = e
                };

                return new JsonResult(result);
            }
        }

        private static IActionResult ReadBytes(byte[] bytes)
        {
            try
            {
                MemoryStream output = new(bytes);                               
                return new FileStreamResult(output, "application/pdf");
            }
            catch (Exception e)
            {
                Result result = new()
                {
                    Status = C.ERROR,
                    Message = C.ERROR,
                    Data = e
                };

                return new JsonResult(result);
            }
        }

        private static byte[] GetDocBytes(_Aspose.Document doc)
        {
            byte[] bytes; 

            try
            {
                MemoryStream output = new();                
                doc.Save(output);
                bytes = output.ToArray();
            } catch  
            {
                bytes = Array.Empty<byte>();
            }

            return bytes;
        }

        //private static IActionResult PreparePdf(Func<_Aspose.Document> getDoc)
        //{
        //    try
        //    {
        //        MemoryStream output = new();
        //        var doc = getDoc();
        //        doc.Save(output);                
        //        return new FileStreamResult(output, "application/pdf");
        //    }
        //    catch (Exception e)
        //    {
        //        Result result = new()
        //        {
        //            Status = C.ERROR,
        //            Message = C.ERROR,
        //            Data = e
        //        };

        //        return new JsonResult(result);
        //    }
        //}

    }
}
