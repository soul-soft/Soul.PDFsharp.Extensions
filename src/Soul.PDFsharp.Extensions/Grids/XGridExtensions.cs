using PdfSharp.Drawing;
using PdfSharp.Drawing.Layout;
using System;
using System.Collections.Generic;
using System.Linq; 

namespace Soul.PDFsharp.Extensions
{
    public static class XGridExtensions
    {
        public static void DrawGrid(this XGraphics graphics, double y, XFont font, XBrush brush, Action<XGrid> configure)
        {
            var grid = new XGrid(true);
            configure(grid);

            double pageWidth = graphics.PageSize.Width; // 获取页面宽度
            double currentY = y; // 当前y坐标

            // 先计算每个单元格的宽度和高度
            CalculateGridDimensions(graphics, grid, font, pageWidth);

            // 然后绘制网格
            foreach (var row in grid.Rows)
            {
                double currentX = row.Margin.Left; // 考虑行的左外边距

                // 绘制行边框
                if (row.Border.Visible)
                {
                    DrawBorder(graphics, row.Border, currentX, currentY, row.Width, row.Height);
                }

                // 绘制单元格
                foreach (var cell in row.Cells)
                {
                    DrawCell(graphics, cell, font, brush, currentX, currentY, row.Height);
                    currentX += cell.Width + cell.Margin.Right; // 移动到下一个单元格的X坐标，考虑右外边距
                }

                // 移动Y坐标到下一行的顶部，考虑行的上下外边距
                currentY += row.Height + row.Margin.Top + row.Margin.Bottom;
            }
        }

        private static void CalculateGridDimensions(XGraphics graphics, XGrid grid, XFont font, double pageWidth)
        {
            foreach (var row in grid.Rows)
            {
                double availableWidth = pageWidth - row.Margin.Left - row.Margin.Right; // 可用宽度
                double totalWidth = 0; // 当前行的总宽度

                // 计算每个单元格的宽度
                for (int i = 0; i < row.Cells.Count; i++)
                {
                    var cell = row.Cells[i];
                    if (cell.Width <= 0) // 如果单元格宽度未指定
                    {
                        // 计算文本宽度并赋值给单元格宽度
                        cell.Width = MeasureCellWidth(graphics, cell, font);
                    }
                    totalWidth += cell.Width; // 更新总宽度
                }

                // 如果只有一个单元格，填满可用宽度
                if (row.Cells.Count == 1)
                {
                    row.Cells[0].Width = availableWidth;
                    totalWidth = availableWidth; // 更新总宽度
                }

                // 处理总宽度超出页面宽度的情况
                if (totalWidth > availableWidth && row.Cells.Count > 0)
                {
                    // 将可用宽度赋值给最后一个单元格
                    row.Cells.Last().Width = availableWidth - (totalWidth - row.Cells.Last().Width);
                }

                // 计算每个单元格的高度
                double rowHeight = 0;
                foreach (var cell in row.Cells)
                {
                    double cellHeight = MeasureCellHeight(graphics, cell, font, cell.Width);
                    rowHeight = Math.Max(rowHeight, cellHeight);
                    cell.Height = cellHeight; // 更新单元格的高度
                }

                // 如果行高未指定，则使用行内单元格的最大高度
                if (row.Height <= 0)
                {
                    row.Height = rowHeight; // 行高为最大单元格高度
                }

                // 统一设置所有单元格的高度为行高
                foreach (var cell in row.Cells)
                {
                    cell.Height = row.Height; // 假设XGridCell有Height属性
                }
                row.Width = row.Cells.Sum(s=>s.Width);
            }
        }

        private static double MeasureCellWidth(XGraphics graphics, XGridCell cell, XFont font)
        {
            if (cell is XGridTextCell textCell)
            {
                // 计算文本宽度并加上内边距
                return graphics.MeasureString(textCell.Text, font).Width + cell.Padding.Left + cell.Padding.Right;
            }
            return 0; // 其他类型的单元格
        }

        private static double MeasureCellHeight(XGraphics graphics, XGridCell cell, XFont font, double width)
        {
            if (cell is XGridTextCell textCell)
            {
                // 计算可用宽度，减去内边距
                double availableWidth = width - cell.Padding.Left - cell.Padding.Right;
                int lineCount = GetLines(graphics, textCell, availableWidth, font).Count; // 获取行数，减去内边距
                double lineHeight = graphics.MeasureString("A", font).Height; // 使用 MeasureString 获取行高
                // 计算总高度时考虑内边距
                double totalHeight = lineCount * lineHeight + (lineCount - 1) * textCell.LineSpacing + cell.Padding.Top + cell.Padding.Bottom; // 添加内边距
                return totalHeight; // 返回总高度
            }
            return 0; // 其他类型的单元格
        }

        private static List<string> GetLines(XGraphics graphics, XGridTextCell textCell, double width, XFont font)
        {
            var lines = new List<string>();
            if (textCell.Warp)
            {
                var sb = new System.Text.StringBuilder();
                for (int i = 0; i < textCell.Text.Length; i++)
                {
                    char c = textCell.Text[i];
                    sb.Append(c); // 添加字符到当前行

                    // 测量当前行的宽度
                    var size = graphics.MeasureString(sb.ToString(), font);

                    if (size.Width > width) // 如果超出宽度，则换行
                    {
                        if (sb.Length > 1)
                        {
                            // 添加当前行到结果
                            lines.Add(sb.ToString(0, sb.Length - 1)); // 去掉最后一个字符
                        }
                        sb.Clear(); // 清空 StringBuilder
                        sb.Append(c); // 将超出的字符添加到新行
                    }
                }

                // 添加最后一行
                if (sb.Length > 0)
                {
                    lines.Add(sb.ToString());
                }
            }
            else
            {
                // 不换行，直接添加整个文本
                lines.Add(textCell.Text);
            }

            return lines;
        }

        private static void DrawCell(XGraphics graphics, XGridCell cell, XFont font, XBrush brush, double x, double y, double height)
        {
            if (cell is XGridTextCell textCell)
            {
                // 获取文本行，考虑内边距
                var lines = GetLines(graphics, textCell, cell.Width - cell.Padding.Left - cell.Padding.Right, font);
                double lineHeight = graphics.MeasureString("A", font).Height; // 使用 MeasureString 获取行高

                // 计算文本的起始 Y 坐标
                double totalTextHeight = lines.Count * lineHeight + (lines.Count - 1) * textCell.LineSpacing;
                double startY;

                // 根据垂直对齐方式调整 startY
                switch (textCell.VerticalAlignment)
                {
                    case XGridAlignment.Top:
                        startY = y + cell.Margin.Top + cell.Padding.Top; // 顶部对齐
                        break;
                    case XGridAlignment.Bottom:
                        startY = y + cell.Margin.Top + cell.Padding.Top + height - totalTextHeight; // 底部对齐
                        break;
                    case XGridAlignment.Center:
                    default:
                        startY = y + cell.Margin.Top + cell.Padding.Top + (height - totalTextHeight) / 2; // 垂直居中
                        break;
                }

                // 绘制每一行文本，考虑内边距
                for (int i = 0; i < lines.Count; i++)
                {
                    var lineY = startY + i * (lineHeight + textCell.LineSpacing);
                    var layout = new XTextFormatter(graphics)
                    {
                        // 根据水平对齐方式设置文本对齐
                        Alignment = textCell.HorizontalAlignment == XGridAlignment.Center ? XParagraphAlignment.Center :
                                    textCell.HorizontalAlignment == XGridAlignment.Right ? XParagraphAlignment.Right :
                                    XParagraphAlignment.Left
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
