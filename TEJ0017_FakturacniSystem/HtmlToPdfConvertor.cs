using SelectPdf;
using TEJ0017_FakturacniSystem.Models.Document.DocumentTypes;

namespace TEJ0017_FakturacniSystem
{
    public class HtmlToPdfConvertor
    {
        private string OutputHtml { get; set; }
        public HtmlToPdfConvertor(string outputHtml)
        {
            this.OutputHtml = outputHtml;
        }

        public MemoryStream getDocumentPdf()
        {
            HtmlToPdf converter = new HtmlToPdf();

            //staticky nastaveno na rozmer A4
            converter.Options.PdfPageSize = PdfPageSize.A4;
            converter.Options.PdfPageOrientation = PdfPageOrientation.Portrait;
            converter.Options.WebPageWidth = 1240;
            converter.Options.MarginTop = 25;
            converter.Options.MarginBottom = 25;

            //prevod pohledu Detail (prevedeneho na HTML string)
            PdfDocument pdfDocument = converter.ConvertHtmlString(this.OutputHtml);
            MemoryStream output = new MemoryStream();
            pdfDocument.Save(output);
            pdfDocument.Close();

            return output;
        }
    }
}
