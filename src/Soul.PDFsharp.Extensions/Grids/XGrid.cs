using System;
using System.Collections.Generic;
using System.Text;

namespace Soul.PDFsharp.Extensions
{
    public class XGrid
    {
        private readonly bool _isDebug;
      
        private readonly List<XGridRow> _rows = new List<XGridRow>();
      
        public XGrid()
        {
            _isDebug = true;
        }
      
        public XGrid(bool isDebug)
        {
            _isDebug = isDebug;
        }

        public void DrawRow(Action<XGridRow> configure)
        {
            var row = new XGridRow();
            configure(row);
            _rows.Add(row);
        }

        internal void EnableDebugBorders()
        {
            if (!_isDebug) return;
            foreach (var row in _rows)
            {
                row.Border.Visible = true;
                foreach (var cell in row.Cells)
                {
                    cell.Border.Visible = true;
                }
            }
        }

        internal IReadOnlyList<XGridRow> Rows => _rows;
    }
}
