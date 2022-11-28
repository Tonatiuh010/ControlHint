using Aspose.Diagram;
using Aspose.Email.Clients.Exchange.WebService.Schema_2016;
using Aspose.Imaging.CoreExceptions;
using Aspose.Pdf;
using Aspose.Pdf.Facades;
using Aspose.Pdf.Text;
using Engine.BO;
using System.Security.Cryptography.X509Certificates;

namespace DocsControl.Ocr.FlowDocs
{
    public class Quotation
    {
        #region CONSTANTS
        public const string Path = @".\Templates\COT.pdf";

        public const string NAME = "{{NAME}}";
        public const string NUM = "{{NUM}}";
        public const string DUO_DATE = "{{DUO_DATE}}";
        public const string DATE = "{{DATE}}";
        public const string CLIENT = "{{CLIENT}}";
        public const string CONTACT = "{{CONTACT}}";
        public const string TOTAL = "{{TOTAL}}";
        public const string NOTES = "{{NOTES}}";        
        #endregion

        private OcrScan Ocr { get; set; }
        public Document Document => Ocr.Document;

        private Quotation(QuotationParameters @params)
        {
            Ocr = new OcrScan(Path);
            ParseDocument(Ocr, @params);
        }
        private Quotation(QuotationParameters @params, byte[] bytes)
        {
            Ocr = new OcrScan(bytes);
            EditDocument(Ocr, @params);
        }

        public static Quotation CreateQuotation(QuotationParameters @params) => new Quotation(@params);
        public static Quotation EditQuotation(QuotationParameters @params, byte[] bytes) => new Quotation(@params, bytes);

        public static void ParseDocument(OcrScan ocr, QuotationParameters @params)
        {
            OcrScan.ParseField(ocr, NAME, @params.Name);
            OcrScan.ParseField(ocr, NUM, @params.Id);
            OcrScan.ParseField(ocr, DUO_DATE, @params.DuoDate.ToString("d"));
            OcrScan.ParseField(ocr, DATE, @params.Date.ToString("d"));
            OcrScan.ParseField(ocr, CLIENT, @params.Client);
            OcrScan.ParseField(ocr, CONTACT, @params.Contact);
            ParseItems(ocr, @params.Items);
            OcrScan.ParseField(ocr, TOTAL, $"{@params.Total}");
            OcrScan.ParseField(ocr, NOTES, @params.Notes);
            
        }

        public static void EditDocument(OcrScan oct, QuotationParameters @params)
        {

        }

        public static void ParseItems(OcrScan ocr, List<QuotationItem> items)
        {
            
            int itemsCount = items.Count;
            var tables = ocr.FindTable(ocr.Document.Pages[1]);
            var table = tables[2];

            if(table != null)
            {
                int rowsCount = table.RowList.Count;
                for(int i = 1; i < rowsCount - 1; i++)
                {
                    if ((itemsCount) < i) break;
                    AddItem(table.RowList[i], items[i - 1]);
                }

                //foreach (var row in table.RowList)
                //{
                //  ADD EXTRA ROWS IF POSSIBLE!
                //}
            }
        }

        public static void AddItem(AbsorbedRow row, QuotationItem item)
        {
            int cellsCount = row.CellList.Count;
            if (cellsCount >= 7)
            {
                PdfContentEditor editor = new PdfContentEditor();                

                var tf1 = row.CellList[0].TextFragments[1];
                var tf2 = row.CellList[1].TextFragments[1];
                var tf3 = row.CellList[2].TextFragments[1];
                var tf4 = row.CellList[3].TextFragments[1];
                var tf5 = row.CellList[4].TextFragments[1];
                var tf6 = row.CellList[5].TextFragments[1];
                var tf7 = row.CellList[6].TextFragments[1];

                tf1.Text = $"#{item.Id}";                
                tf2.Text = item.Code;                
                tf3.Text = item.Description;
                tf4.Text = item.Qty.ToString();
                tf5.Text = item.Name;
                tf6.Text = item.Value.ToString();
                tf7.Text = item.Total.ToString();
            }
        }

    }

    public class QuotationParameters : BaseBO
    {
        public string Name { get; set; }
        public DateTime DuoDate { get; set; }
        public DateTime Date { get; set; }
        public string Client { get; set; }
        public string Contact { get; set; }
        public decimal Total => Items.Sum(x => x.Total);

        public List<QuotationItem> Items { get; set; } = new();
        public string Notes { get; set; }

    }

    public class QuotationItem :  BaseBO
    {
        public string Code { get; set; }
        public string Description { get; set; }
        public decimal Qty { get; set; }
        public string Name { get; set; }
        public decimal Value { get; set; }
        public decimal Total => Qty * Value;

    }

}
