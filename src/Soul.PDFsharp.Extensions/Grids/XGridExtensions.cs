using PdfSharp.Drawing;
using PdfSharp.Drawing.Layout;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Soul.PDFsharp.Extensions
{
    public static class XGridExtensions
    {
        public static void DrawCoordinateSystem(this XGraphics graphics, XFont font, double size = 20)
        {
            // 获取页面的宽度和高度
            double pageWidth = graphics.PageSize.Width;
            double pageHeight = graphics.PageSize.Height;

            // 绘制 Y 轴
            graphics.DrawLine(XPens.Black, 0, 0, 0, pageHeight); // Y 轴
            graphics.DrawString("Y", font, XBrushes.Black, 5, 5); // 在 Y 轴末尾标注 Y

            // 绘制 X 轴
            graphics.DrawLine(XPens.Black, 0, pageHeight, pageWidth, pageHeight); // X 轴
            graphics.DrawString("X", font, XBrushes.Black, pageWidth - 20, pageHeight + 5); // 在 X 轴末尾标注 X

            // 绘制 X 轴刻度
            for (double i = 0; i <= pageWidth; i += size)
            {
                graphics.DrawLine(XPens.Black, i, pageHeight - 5, i, pageHeight + 5); // 刻度线
                graphics.DrawString(i.ToString(), font, XBrushes.Black, i - (font.Size / 2), pageHeight + 10); // 刻度数字
            }

            // 绘制 Y 轴刻度
            for (double i = 0; i <= pageHeight; i += size)
            {
                graphics.DrawLine(XPens.Black, -5, i, 5, i); // 刻度线
                graphics.DrawString((pageHeight - i).ToString(), font, XBrushes.Black, 10, i - (font.Size / 2)); // 刻度数字
            }
        }


        /// <summary>
        /// 绘制网格，指定起始位置、字体、画刷和配置操作。
        /// </summary>
        public static void DrawGrid(this XGraphics graphics, double y, XFont font, XBrush brush, Action<XGrid> configure)
        {
            var grid = new XGrid(true);
            configure(grid);

            double pageWidth = graphics.PageSize.Width;

            // 计算网格的宽度和高度
            CalculateGridDimensions(graphics, grid, font, pageWidth);

            // 绘制网格单元格
            DrawGridCells(graphics, grid, font, brush, y);
        }

        /// <summary>
        /// 计算网格的宽度和高度，不包括绘制。
        /// </summary>
        private static void CalculateGridDimensions(XGraphics graphics, XGrid grid, XFont font, double pageWidth)
        {
            foreach (var row in grid.Rows)
            {
                CalculateRowDimensions(graphics, row, font, pageWidth);
            }
        }

        /// <summary>
        /// 计算单个行的宽度和高度。
        /// </summary>
        private static void CalculateRowDimensions(XGraphics graphics, XGridRow row, XFont font, double pageWidth)
        {
            double availableWidth = pageWidth - row.Margin.Left - row.Margin.Right;
            double totalWidth = 0;

            for (int i = 0; i < row.Cells.Count; i++)
            {
                var cell = row.Cells[i];
                if (cell.Width <= 0)
                {
                    cell.Width = MeasureCellWidth(graphics, cell, font);

                    // 单元格只有一个时，将宽度设置为可用宽度
                    if (row.Cells.Count == 1)
                    {
                        cell.Width = availableWidth;
                    }
                }

                // 限制单元格宽度不能超过剩余的可用宽度
                if (totalWidth + cell.Width > availableWidth)
                {
                    cell.Width = availableWidth - totalWidth;
                }

                totalWidth += cell.Width;
            }

            // 处理行宽度溢出
            if (totalWidth > availableWidth && row.Cells.Count > 0)
            {
                row.Cells.Last().Width = availableWidth - (totalWidth - row.Cells.Last().Width);
            }

            // 计算行高
            if (row.Height <= 0)
            {
                row.Height = CalculateRowHeight(graphics, row, font);
            }

            // 设置行宽度
            row.Width = totalWidth;
        }

        /// <summary>
        /// 计算行高，通过获取行内最高单元格的高度。
        /// </summary>
        private static double CalculateRowHeight(XGraphics graphics, XGridRow row, XFont font)
        {
            double rowHeight = 0;
            foreach (var cell in row.Cells)
            {
                double cellHeight = MeasureCellHeight(graphics, cell, font, cell.Width);
                rowHeight = Math.Max(rowHeight, cellHeight);
                cell.Height = cellHeight;
            }
            return rowHeight;
        }

        /// <summary>
        /// 绘制网格中的每一个单元格。
        /// </summary>
        private static void DrawGridCells(XGraphics graphics, XGrid grid, XFont font, XBrush brush, double startY)
        {
            double currentY = startY;

            foreach (var row in grid.Rows)
            {
                DrawGridRow(graphics, row, font, brush, ref currentY);
            }
        }

        /// <summary>
        /// 绘制单行网格中的所有单元格。
        /// </summary>
        private static void DrawGridRow(XGraphics graphics, XGridRow row, XFont font, XBrush brush, ref double currentY)
        {
            // 在绘制之前，将 currentY 加上当前行的 Margin.Top
            currentY += row.Margin.Top;

            double currentX = row.Margin.Left;

            if (row.Border.Visible)
            {
                DrawBorder(graphics, row.Border, currentX, currentY, row.Width, row.Height);
            }

            foreach (var cell in row.Cells)
            {
                DrawCell(graphics, cell, font, brush, currentX, currentY, row.Height);
                currentX += cell.Width + cell.Margin.Right;
            }

            // 在绘制之后，将 currentY 加上当前行的高度和 Margin.Bottom
            currentY += row.Height + row.Margin.Bottom;
        }


        /// <summary>
        /// 测量单元格的宽度，如果未指定宽度则返回测量值。
        /// </summary>
        private static double MeasureCellWidth(XGraphics graphics, XGridCell cell, XFont font)
        {
            if (cell is XGridTextCell textCell)
            {
                double measuredWidth = graphics.MeasureString(textCell.Text, font).Width + cell.Padding.Left + cell.Padding.Right;
                return Math.Min(measuredWidth, graphics.PageSize.Width);
            }
            return 0;
        }

        /// <summary>
        /// 测量单元格的高度，基于给定宽度和字体。
        /// </summary>
        private static double MeasureCellHeight(XGraphics graphics, XGridCell cell, XFont font, double width)
        {
            if (cell is XGridTextCell textCell)
            {
                double availableWidth = width - cell.Padding.Left - cell.Padding.Right;
                int lineCount = GetLines(graphics, textCell, availableWidth, font).Count;
                double lineHeight = graphics.MeasureString("A", font).Height;
                double totalHeight = lineCount * lineHeight + (lineCount - 1) * textCell.LineSpacing + cell.Padding.Top + cell.Padding.Bottom;
                return totalHeight;
            }
            return 0;
        }

        /// <summary>
        /// 根据宽度分割文本，获取多行内容。
        /// </summary>
        private static List<string> GetLines(XGraphics graphics, XGridTextCell textCell, double width, XFont font)
        {
            var lines = new List<string>();
            if (textCell.Warp)
            {
                var sb = new System.Text.StringBuilder();
                for (int i = 0; i < textCell.Text.Length; i++)
                {
                    char c = textCell.Text[i];
                    sb.Append(c);

                    var size = graphics.MeasureString(sb.ToString(), font);

                    if (size.Width > width)
                    {
                        if (sb.Length > 1)
                        {
                            lines.Add(sb.ToString(0, sb.Length - 1));
                        }
                        sb.Clear();
                        sb.Append(c);
                    }
                }

                if (sb.Length > 0)
                {
                    lines.Add(sb.ToString());
                }
            }
            else
            {
                lines.Add(textCell.Text);
            }

            return lines;
        }

        /// <summary>
        /// 绘制单元格内容和边框。
        /// </summary>
        private static void DrawCell(XGraphics graphics, XGridCell cell, XFont font, XBrush brush, double x, double y, double height)
        {
            if (cell is XGridTextCell textCell)
            {
                var lines = GetLines(graphics, textCell, cell.Width - cell.Padding.Left - cell.Padding.Right, font);
                double lineHeight = graphics.MeasureString("A", font).Height;

                // 计算文本总高度
                double totalTextHeight = lines.Count * lineHeight + (lines.Count - 1) * textCell.LineSpacing;

                // 根据垂直对齐方式调整起始Y坐标
                double startY = CalculateTextStartY(y, height, cell, totalTextHeight);

                // 绘制每一行文本，考虑内边距和水平对齐方式
                for (int i = 0; i < lines.Count; i++)
                {
                    var lineY = startY + i * (lineHeight + textCell.LineSpacing);
                    var layout = new XTextFormatter(graphics)
                    {
                        Alignment = GetParagraphAlignment(textCell.HorizontalAlignment)
                    };
                    var rect = new XRect(x + cell.Margin.Left + cell.Padding.Left, lineY,
                                          cell.Width - cell.Margin.Left - cell.Margin.Right - cell.Padding.Left - cell.Padding.Right,
                                          lineHeight);
                    layout.DrawString(lines[i], font, brush, rect, XStringFormats.TopLeft);
                }
            }

            // 绘制单元格边框
            if (cell.Border.Visible)
            {
                DrawBorder(graphics, cell.Border, x, y, cell.Width, height);
            }
        }

        /// <summary>
        /// 根据垂直对齐方式计算文本的起始Y坐标。
        /// </summary>
        private static double CalculateTextStartY(double y, double height, XGridCell cell, double totalTextHeight)
        {
            switch (cell.VerticalAlignment)
            {
                case XGridAlignment.Top:
                    return y + cell.Margin.Top + cell.Padding.Top;
                case XGridAlignment.Bottom:
                    return y + height - cell.Margin.Bottom - cell.Padding.Bottom - totalTextHeight;
                case XGridAlignment.Center:
                default:
                    return y + cell.Margin.Top + cell.Padding.Top + (height - totalTextHeight) / 2;
            }
        }

        /// <summary>
        /// 获取文本的水平对齐方式。
        /// </summary>
        private static XParagraphAlignment GetParagraphAlignment(XGridAlignment alignment)
        {
            return alignment == XGridAlignment.Center ? XParagraphAlignment.Center :
                   alignment == XGridAlignment.Right ? XParagraphAlignment.Right :
                   XParagraphAlignment.Left;
        }

        /// <summary>
        /// 绘制单元格或行的边框。
        /// </summary>
        private static void DrawBorder(XGraphics graphics, XBorder border, double x, double y, double width, double height)
        {
            if (border.Visible)
            {
                XPen pen = new XPen(border.Color, border.Size);
                graphics.DrawRectangle(pen, x, y, width, height);
            }
        }
    }
}
