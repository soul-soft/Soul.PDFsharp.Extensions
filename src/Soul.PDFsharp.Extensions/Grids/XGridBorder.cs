using PdfSharp.Drawing;

namespace Soul.PDFsharp.Extensions
{
    public class XGridBorder
    {
        public double Size { get; set; }
        public XColor Color { get; set; } = XColors.Black;
        public bool Visible { get; set; }
    }
}
