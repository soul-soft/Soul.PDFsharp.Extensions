using PdfSharp.Drawing;

namespace Soul.PDFsharp.Extensions
{
    public class XGridBox
    {
        public double Left { get; set; }
        public double Right { get; set; }
        public double Top { get; set; }
        public double Bottom { get; set; }

        public XGridBox(double size) : this(size, size, size, size) { }

        public XGridBox(double left, double right, double top, double bottom)
        {
            Left = left;
            Right = right;
            Top = top;
            Bottom = bottom;
        }

        public XGridBox SetHorizontal(double left, double right = 0)
        {
            Left = left;
            Right = right == 0 ? left : right;
            return this;
        }

        public XGridBox SetVertical(double top, double bottom = 0)
        {
            Top = top;
            Bottom = bottom == 0 ? top : bottom;
            return this;
        }

        public static implicit operator XGridBox(int size)
        {
            return new XGridBox(size);
        }

        public static implicit operator XGridBox(double size)
        {
            return new XGridBox(size);
        }
    }
}
