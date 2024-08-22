using PdfSharp.Drawing;

namespace Soul.PDFsharp.Extensions.Grids
{
    public class XBorder
    {
        public double Size { get; set; }
        public XColor Color { get; set; } = XColors.Black;
        public bool Visible { get; set; } 
    }
}
