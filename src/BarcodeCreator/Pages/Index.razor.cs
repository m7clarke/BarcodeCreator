using System.IO;
using System.Threading.Tasks;
using BarcodeCreator.Data;
using BarcodeCreator.Models;
using Microsoft.AspNetCore.Components;

namespace BarcodeCreator.Pages
{
    public partial class Index
    {
        private readonly BarcodeContentModel barcodeContent = new();

        [Inject]
        public PdfExportService ExportService { get; set; }

        [Inject]
        public Microsoft.JSInterop.IJSRuntime JavaScript { get; set; }

        protected Task HandleOnValidSubmitAsync()
        {
            return this.CreateBarcodePdfAsync(this.barcodeContent.Text);
        }

        private async Task CreateBarcodePdfAsync(string text)
        {
            await using MemoryStream pdfStream = this.ExportService.CreatePdf(text);
            await this.JavaScript.SaveAs($"{text}.pdf", pdfStream.ToArray()).ConfigureAwait(false);
            this.barcodeContent.Text = string.Empty;
        }
    }
}
