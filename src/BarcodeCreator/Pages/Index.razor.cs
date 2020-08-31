﻿namespace BarcodeCreator.Pages
{
    using System.IO;
    using System.Threading.Tasks;
    using BarcodeCreator.Data;
    using BarcodeCreator.Models;
    using Microsoft.AspNetCore.Components;

    public partial class Index
    {
        private readonly BarcodeContentModel barcodeContent = new BarcodeContentModel();

        [Inject]
        public PdfExportService ExportService { get; set; }

        [Inject]
        public Microsoft.JSInterop.IJSRuntime JavaScript { get; set; }

        protected async Task HandleOnValidSubmit()
        {
            await this.CreateBarcodePdf(this.barcodeContent.Text).ConfigureAwait(false);
        }

        private async Task CreateBarcodePdf(string text)
        {
            await using MemoryStream pdfStream = this.ExportService.CreatePdf(text);
            await this.JavaScript.SaveAs($"{text}.pdf", pdfStream.ToArray()).ConfigureAwait(false);
            this.barcodeContent.Text = string.Empty;
        }
    }
}