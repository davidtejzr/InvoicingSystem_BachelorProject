using SelectPdf;
using TEJ0017_FakturacniSystem.Models.Document.DocumentTypes;

namespace TEJ0017_FakturacniSystem
{
    public class HtmlToPdfConvertor
    {
        public HtmlToPdfConvertor() { }

        public MemoryStream getBasicInvoicePdf(BasicInvoice basicInvoice)
        {
            HtmlToPdf converter = new HtmlToPdf();

            converter.Options.PdfPageSize = PdfPageSize.A4;
            converter.Options.PdfPageOrientation = PdfPageOrientation.Portrait;
            converter.Options.WebPageWidth = 1024;

            string html = "<html><body><h1>"+ basicInvoice.DocumentId +"</h1></body></html>";
            PdfDocument pdfDocument = converter.ConvertHtmlString(html);

            var tt = basicInvoice.BankDetailId;

            MemoryStream output = new MemoryStream();
            //pdfDocument.Save("test.pdf");
            pdfDocument.Save(output);
            pdfDocument.Close();

            return output;
        }
    }
}
