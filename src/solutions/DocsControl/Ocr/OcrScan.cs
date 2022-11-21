using System;
using Aspose.Pdf;
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
    }
}
