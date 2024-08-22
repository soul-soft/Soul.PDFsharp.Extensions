using PdfSharp.Drawing;
using System.Collections.Generic;
using System;

namespace Soul.PDFsharp.Extensions
{
    public class XBorder
    {
        public double Size { get; set; }
        public XColor Color { get; set; } = XColors.Black;
        public bool Visible { get; set; }
    }
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
        
        internal void DebugBorders()
        {
            if (!_isDebug)
            {
                return;
            }
            foreach (var row in _rows)
            {
                row.Border.Visible = true; // 设置行边框可见
                foreach (var cell in row.Cells)
                {
                    cell.Border.Visible = true; // 设置单元格边框可见
                }
            }
        }

        internal List<XGridRow> Rows => _rows;
    }
    public enum XGridAlignment
    {
        Left,
        Right,
        Center,
        Top,
        Bottom,
    }
    public abstract class XGridCell
    {

        public double Width { get; set; }

        /// <summary>
        /// 控制内边距
        /// </summary>
        public XGridBox Margin { get; set; } = new XGridBox(0);
        /// <summary>
        /// 控制外边距
        /// </summary>
        public XGridBox Padding { get; set; } = new XGridBox(0);

        /// <summary>
        /// 水平对齐方式
        /// </summary>
        public XGridAlignment HorizontalAlignment { get; set; } = XGridAlignment.Left;
        /// <summary>
        /// 垂直对齐方式
        /// </summary>
        public XGridAlignment VerticalAlignment { get; set; } = XGridAlignment.Center;

        internal XGridRow Row { get; }
        /// <summary>
        /// 控制边框
        /// </summary>
        public XBorder Border { get; } = new XBorder();
        internal double Height { get;  set; }
    }
    public class XGridBox
    {
        public double Left { get; set; }
        public double Right { get; set; }
        public double Top { get; set; }
        public double Bottom { get; set; }

        public XGridBox(double size)
        {
            Left = size;
            Right = size;
            Top = size;
            Bottom = size;
        }

        public void SetHorizontal(double left)
        {
            Left = left;
            Right = left;
        }

        public void SetVertical(double top)
        {
            Top = top;
            Bottom = top;
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
    public class XGridRow
    {

        private readonly List<XGridCell> _cells = new List<XGridCell>();

        public XGridBox Margin { get; set; } = new XGridBox(0);

        public double Height { get; set; }
        public double Width { get; set; }

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
    public class XGridTextCell : XGridCell
    {
        /// <summary>
        /// 控制换行
        /// </summary>
        public bool Warp { get; set; } = true;
        /// <summary>
        /// 文本
        /// </summary>
        public string Text { get; set; }
        /// <summary>
        /// 多行文本的行间距
        /// </summary>
        public double LineSpacing { get; set; } = 0;
     
    }
}
