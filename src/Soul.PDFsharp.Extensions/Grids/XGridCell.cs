using Soul.PDFsharp.Extensions.Grids;

namespace Soul.PDFsharp.Extensions
{
    public abstract class XGridCell
    {
        private double _margin = 0;

        public double Width { get; set; }
        
        public bool AutoHeight { get; set; } = true;

        public double Margin
        {
            get
            {
                return _margin;
            }
            set
            {
                MarginLeft = value;
                MarginRight = value;
                _margin = value;
            }
        }

        public double Padding { get; set; }

        public double MarginLeft { get; set; }

        public double MarginRight { get; set; }

        internal XGridRow Row { get; }

        public XBorder Border { get; } = new XBorder();
    }
}
