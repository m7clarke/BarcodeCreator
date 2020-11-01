namespace BarcodeCreator.Data
{
    using System;
    using System.IO;
    using iText.Barcodes;
    using iText.IO.Font;
    using iText.Kernel.Colors;
    using iText.Kernel.Font;
    using iText.Kernel.Pdf;
    using iText.Kernel.Pdf.Canvas;
    using iText.Kernel.Pdf.Extgstate;
    using Microsoft.Extensions.Logging;

    public class PdfExportService
    {
        private readonly ILogger<PdfExportService> _logger;

        public PdfExportService(ILogger<PdfExportService> _logger)
        {
            this._logger = _logger ?? throw new ArgumentNullException(nameof(_logger));
        }

        public MemoryStream CreatePdf(string text)
        {
            if (text == null)
            {
                throw new ArgumentNullException(nameof(text));
            }

            _logger.LogInformation("Creating barcode for '{Text}'", text);

            var properties = new WriterProperties();
            properties.SetPdfVersion(PdfVersion.PDF_1_4);

            var stream = new MemoryStream();
            using var writer = new PdfWriter(stream, properties);
            var pdfDoc = new PdfDocument(writer);

            PdfFont fontOcrb = PdfFontFactory.CreateFont("wwwroot/OCRB.otf", PdfEncodings.WINANSI, true);
            Color cmykBlack = new DeviceCmyk(0, 0, 0, 100);
            var gState = new PdfExtGState().SetFillOverPrintFlag(true).SetStrokeOverPrintFlag(true).SetOverprintMode(1);

            var code128 = new Barcode128(pdfDoc);
            code128.SetBaseline(7.67f);
            code128.SetSize(9f);
            code128.SetFont(fontOcrb);
            code128.SetX(0.72f);
            code128.SetBarHeight(14.17f);
            code128.SetCode(text);
            code128.SetCodeType(Barcode128.CODE128);

            var xObject = code128.CreateFormXObject(cmykBlack, cmykBlack, pdfDoc);

            pdfDoc.AddNewPage(new iText.Kernel.Geom.PageSize(xObject.GetWidth(), xObject.GetHeight()));
            PdfCanvas canvas = new PdfCanvas(pdfDoc.GetFirstPage());
            canvas.SaveState();
            canvas.SetExtGState(gState);
            canvas.AddXObjectAt(xObject, 0f, 0f);
            canvas.RestoreState();

            pdfDoc.Close();

            return stream;
        }
    }
}
