using PdfSharp.Drawing;
using System;
using System.Collections.Generic;

namespace Soul.PDFsharp.Extensions
{
    public class XGrid
    {
        private readonly List<XGridRow> _rows = new List<XGridRow>();

        public void DrawRow(Action<XGridRow> configure)
        {
            var row = new XGridRow();
            configure(row);
            _rows.Add(row);
        }

        internal IReadOnlyCollection<XGridRow> Rows => _rows;
    }
}
