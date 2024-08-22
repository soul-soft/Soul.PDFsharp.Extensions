using Soul.PDFsharp.Extensions.Grids;
using System;
using System.Collections.Generic;

namespace Soul.PDFsharp.Extensions
{
    public class XGridRow
    {
        private double _margin = 0;

        private readonly List<XGridCell> _cells = new List<XGridCell>();

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

        public double MarginLeft { get; set; }

        public double MarginRight { get; set; }

        public double Height { get; set; }

        public XBorder Border { get; } = new XBorder();

        public void DrawTextCell(Action<XGridTextCell> configure)
        {
            var cell = new XGridTextCell();
            configure(cell);
            _cells.Add(cell);
        }

        internal XGrid Grid { get; }

        internal IReadOnlyList<XGridCell> Cells => _cells;
    }
}
