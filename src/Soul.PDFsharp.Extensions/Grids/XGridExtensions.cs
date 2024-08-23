using PdfSharp.Drawing;
using PdfSharp.Drawing.Layout;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Soul.PDFsharp.Extensions
{
    public static class XGridExtensions
    {
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

            //同步行高
            foreach (var item in row.Cells)
            {
                item.Height = row.Height;
            }

            // 设置行宽度
            row.Width = totalWidth;
        }

        /// <summary>
        /// 计算行高，通过获取行内最高单元格的高度，考虑用户指定的行高和单元格内边距。
        /// </summary>
        private static double CalculateRowHeight(XGraphics graphics, XGridRow row, XFont font)
        {
            double maxCellHeight = 0;

            foreach (var cell in row.Cells)
            {
                // 计算单元格的高度，考虑内边距
                double cellHeight = MeasureCellHeight(graphics, cell, font, cell.Width);
                cellHeight += cell.Padding.Top + cell.Padding.Bottom;

                // 记录最大的单元格高度
                maxCellHeight = Math.Max(maxCellHeight, cellHeight);
            }

            // 如果用户指定了行高，行高取用户指定的行高和所有单元格高度最大值之间的较大值
            if (row.Height > 0)
            {
                return Math.Max(row.Height, maxCellHeight);
            }

            // 如果用户未指定行高，行高取所有单元格高度中的最大值
            return maxCellHeight;
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
            else if (cell is XGridImageCell imageCell)
            {
                // 优先使用用户设置的宽度
                return cell.Width > 0 ? cell.Width : imageCell.ImageWidth;
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
            else if (cell is XGridImageCell imageCell)
            {
                // 优先使用用户设置的高度
                return cell.Height > 0 ? cell.Height : imageCell.ImageHeight;
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
        private static void DrawCell(XGraphics graphics, XGridCell cell, XFont font, XBrush brush, double x, double y, double rowHeight)
        {
            if (cell is XGridTextCell textCell)
            {
                var lines = GetLines(graphics, textCell, cell.Width - cell.Padding.Left - cell.Padding.Right, font);
                double lineHeight = graphics.MeasureString("A", font).Height;
                double totalTextHeight = lines.Count * lineHeight + (lines.Count - 1) * textCell.LineSpacing;
                double startY = CalculateTextStartY(y, rowHeight, cell, totalTextHeight);

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
            else if (cell is XGridImageCell imageCell)
            {
                // 计算图片的水平对齐位置
                double imageX = x + cell.Margin.Left;
                switch (cell.HorizontalAlignment)
                {
                    case XGridAlignment.Center:
                        imageX += (cell.Width - imageCell.ImageWidth) / 2;
                        break;
                    case XGridAlignment.Right:
                        imageX += cell.Width - imageCell.ImageWidth;
                        break;
                    case XGridAlignment.Left:
                    default:
                        // 默认是左对齐
                        break;
                }

                // 计算图片的垂直对齐位置
                double imageY = y + cell.Margin.Top;
                switch (cell.VerticalAlignment)
                {
                    case XGridAlignment.Center:
                        imageY += (rowHeight - imageCell.ImageHeight) / 2;
                        break;
                    case XGridAlignment.Bottom:
                        imageY += rowHeight - imageCell.ImageHeight;
                        break;
                    case XGridAlignment.Top:
                    default:
                        // 默认是顶部对齐
                        break;
                }

                // 绘制图片
                graphics.DrawImage(imageCell.Image, imageX, imageY, imageCell.ImageWidth, imageCell.ImageHeight);
            }

            // 绘制单元格边框
            if (cell.Border.Visible)
            {
                DrawBorder(graphics, cell.Border, x, y, cell.Width, rowHeight);
            }
        }

        /// <summary>
        /// 根据垂直对齐方式计算文本的起始Y坐标。
        /// </summary>
        private static double CalculateTextStartY(double y, double height, XGridCell cell, double totalTextHeight)
        {
            // Adjust Y coordinate based on vertical alignment without recalculating padding
            switch (cell.VerticalAlignment)
            {
                case XGridAlignment.Top:
                    return y + cell.Margin.Top; // Only consider margin, padding is already included in height calculation
                case XGridAlignment.Bottom:
                    return y + height - cell.Margin.Bottom - totalTextHeight;
                case XGridAlignment.Center:
                default:
                    return y + cell.Margin.Top + (height - totalTextHeight) / 2;
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
        private static void DrawBorder(XGraphics graphics, XGridBorder border, double x, double y, double width, double height)
        {
            if (border.Visible)
            {
                XPen pen = new XPen(border.Color, border.Size);
                graphics.DrawRectangle(pen, x, y, width, height);
            }
        }
    }
}
