using PdfSharp.Drawing;
using PdfSharp.Pdf;
using System;

namespace Soul.PDFsharp.Extensions
{
    public static class PdfDocumentExtensions
    {
        public static void DrawPage(this PdfDocument document, Action<PdfPage, XGraphics> configure)
        {
            var page = document.AddPage();
            using (var graphics = XGraphics.FromPdfPage(page))
            {
                configure(page, graphics);
            }
        }
    }
}
