using PdfSharp.Drawing;
using System.Collections.Generic;
using System;

namespace Soul.PDFsharp.Extensions
{
    public class XGridRow
    {

        private readonly List<XGridCell> _cells = new List<XGridCell>();

        public XGridBox Margin { get; set; } = new XGridBox(0);

        public double Height { get; set; }
        public double Width { get; set; }

        public XGridBorder Border { get; } = new XGridBorder();

        public void DrawTextCell(Action<XGridTextCell> configure)
        {
            var cell = new XGridTextCell();
            configure(cell);
            _cells.Add(cell);
        }

        public void DrawImageCell(Action<XGridImageCell> configure)
        {
            var cell = new XGridImageCell();
            configure(cell);
            _cells.Add(cell);
        }

        internal XGrid Grid { get; }

        internal IReadOnlyList<XGridCell> Cells => _cells;
    }
}
