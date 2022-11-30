using Aspose.Cells;
using Aspose.Pdf;
using Aspose.Pdf.Facades;
using Engine.BO;
using Microsoft.VisualBasic;

namespace DocsControl.Ocr.FlowDocs
{
    public class Sale
    {
        #region CONSTANTS
        public const string Path = @".\Templates\SALE.pdf";

        public const string PLACE = "{{PLACE}}";
        public const string DATE = "{{DATE}}";
        public const string STUFF_SALE = "{{STUFF_SALE}}";

        public const string SALES_PLACE = "{{SALES_PLACE}}";
        public const string SALES_NUM = "{{SALES_NUM}}";
        public const string SALES_ADDRESS = "{{SALES_ADDRESS}}";
        public const string SALES_SIGN = "{{SALES_SIGN}}";

        public const string CUSTOMER_PLACE = "{{CUSTOMER_PLACE}}";
        public const string CUSTOMER_NUM = "{{CUSTOMER_NUM}}";
        public const string CUSTOMER_ADDRESS = "{{CUSTOMER_ADDRESS}}";
        public const string CUSTOMER_NAME = "{{CUSTOMER_NAME}}";
        public const string CUSTOMER_SIGN = "{{CUSTOMER_SIGN}}";

        public const string LAW_NAME = "{{LAW_NAME}}";

        public const string TOTAL = "{{TOTAL}}";
        #endregion

        private OcrScan Ocr { get; set; }
        public Document Document => Ocr.Document;

        public Sale(SaleParameters @params)
        {
            Ocr = new OcrScan(Path);
            ParseDocument(Ocr, @params);
        }

        public Sale(SaleParameters @params, byte[] bytes)
        {
            Ocr = new OcrScan(bytes);
            ParseDocument(Ocr, @params);
        }

        public static Sale CreateSale(SaleParameters @params) => new (@params);
        public static Sale EditSale(SaleParameters @params, byte[] bytes) => new(@params, bytes);

        public static void ParseDocument(OcrScan ocr, SaleParameters @params)
        {
            OcrScan.ParseField(ocr, PLACE, @params.Place);
            OcrScan.ParseField(ocr, DATE, @params.Date.ToString("d"));
            OcrScan.ParseField(ocr, STUFF_SALE, @params.Item);

            OcrScan.ParseField(ocr, SALES_PLACE, @params.SalesPlace);
            OcrScan.ParseField(ocr, SALES_NUM, @params.SalesNum);
            OcrScan.ParseField(ocr, SALES_ADDRESS, @params.SalesAddress);
            OcrScan.ParseField(ocr, SALES_SIGN, @params.SalesSign);

            OcrScan.ParseField(ocr, CUSTOMER_PLACE, @params.CustomerPlace);
            OcrScan.ParseField(ocr, CUSTOMER_NUM, @params.CustomerNum);
            OcrScan.ParseField(ocr, CUSTOMER_ADDRESS, @params.CustomerAddress);
            OcrScan.ParseField(ocr, CUSTOMER_SIGN, @params.CustomerSign);
            OcrScan.ParseField(ocr, CUSTOMER_NAME, @params.CustomerName);

            OcrScan.ParseField(ocr, LAW_NAME, @params.Law);

            OcrScan.ParseField(ocr, TOTAL, @params.Total);

        }

        public static void EditDocument(OcrScan ocr, SaleParameters @params)
        {

        }
        
    }

    public class SaleParameters : BaseBO
    {
        public string Place { get; set; }
        public DateTime Date { get; set; }
        public string Item { get; set; }

        public string Name { get; set; }

        public string SalesPlace { get; set; }
        public int SalesNum { get; set; }
        public string SalesAddress { get; set; }
        public string SalesSign { get; set; }

        public string CustomerPlace { get; set; }
        public int CustomerNum { get; set; }
        public string CustomerAddress { get; set; }
        public string CustomerSign { get; set; }

        public string CustomerName { get; set; }
        
        public string Law { get; set; }        

        public decimal Total { get; set; }

    }

}
