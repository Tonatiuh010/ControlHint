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

        public static Quotation CreateQuotation(QuotationParameters @params) => new Quotation(@params);

        public static void ParseDocument(OcrScan ocr, QuotationParameters @params)
        {
            ParseField(ocr, NAME, @params.Name);
            ParseField(ocr, NUM, @params.Id);
            ParseField(ocr, DUO_DATE, @params.DuoDate.ToString("d"));
            ParseField(ocr, DATE, @params.Date.ToString("d"));
            ParseField(ocr, CLIENT, @params.Client);
            ParseField(ocr, CONTACT, @params.Contact);
            ParseItems(ocr, @params.Items);
            ParseField(ocr, TOTAL, $"{@params.Total}");
            ParseField(ocr, NOTES, @params.Notes);

            /*Do something for the items*/
        }

        public static void ParseField(OcrScan ocr, string pattern, object? value)
        {
            PdfContentEditor editor = new PdfContentEditor(ocr.Document);
            var fragmentCollection = ocr.FindText(pattern);
            var fragment = fragmentCollection.FirstOrDefault();            

            if (fragment != null) 
            {
                var x = editor.ReplaceText(pattern, value?.ToString(), fragment.TextState);

                //Console.WriteLine("Text : {0} ", fragment.Text);
                //Console.WriteLine("Position : {0} ", fragment.Position);
                //Console.WriteLine("XIndent : {0} ", fragment.Position.XIndent);
                //Console.WriteLine("YIndent : {0} ", fragment.Position.YIndent);
                //Console.WriteLine("Font - Name : {0}", fragment.TextState.Font.FontName);
                //Console.WriteLine("Font - IsAccessible : {0} ", fragment.TextState.Font.IsAccessible);
                //Console.WriteLine("Font - IsEmbedded : {0} ", fragment.TextState.Font.IsEmbedded);
                //Console.WriteLine("Font - IsSubset : {0} ", fragment.TextState.Font.IsSubset);
                //Console.WriteLine("Font Size : {0} ", fragment.TextState.FontSize);
                //Console.WriteLine("Foreground Color : {0} ", fragment.TextState.ForegroundColor);
            }

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

        public static TextState FormatText()
        {
            return new TextState()
            {
                FontSize = 10,
                ForegroundColor = Color.Black,
                RenderingMode = TextRenderingMode.FillText,
                Font = FontRepository.FindFont("Arial"),
                CharacterSpacing = 10,
                WordSpacing = 10,
            };
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
