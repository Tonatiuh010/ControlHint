using System;
using Aspose.Pdf;
using Aspose.Pdf.Facades;
using Aspose.Pdf.Text;

namespace DocsControl.Ocr
{
    public class OcrScan
    {
        public Document Document { get; set; }

        public OcrScan(string path)
        {
            Document = GetDocument(path);
        }

        public TextFragmentCollection FindText(string pattern)
        {
            TextFragmentAbsorber patternText = new (pattern);
            Document.Pages.Accept(patternText);
            return patternText.TextFragments;
        }

        public List<AbsorbedTable> FindTable(Page page)
        {
            TableAbsorber tableAbsorber = new ();
            tableAbsorber.Visit(page);
            return tableAbsorber.TableList.ToList();
        }

        public static Document GetDocument(string path)
        {
            return new(path);
        }

        public static void SetLicense()
        {
            var lic = new License();

            try
            {
                lic.SetLicense("aspose.lic");
            } catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        public static void ParseField(OcrScan ocr, string pattern, object? value)
        {
            PdfContentEditor editor = new PdfContentEditor(ocr.Document);
            var fragmentCollection = ocr.FindText(pattern);
            var fragment = fragmentCollection.FirstOrDefault();

            if (fragment != null)
            {
                editor.ReplaceText(pattern, value?.ToString(), fragment.TextState);
            }
        }
    }
}
