using PdfSharp.Drawing;
using PdfSharp.Drawing.Layout;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Soul.PDFsharp.Extensions
{
    public static class XGridExtensions
    {
        public static void DrawGrid(this XGraphics graphics, double y, XFont font, XBrush brush, Action<XGrid> configure)
        {
            var grid = new XGrid();
            configure(grid);

            double pageWidth = graphics.PdfPage.Width; // 获取页面宽度
            double currentY = y; // 当前y坐标

            foreach (var row in grid.Rows)
            {
                double rowHeight = 0;
                double totalWidth = 0;
                double availableWidth = pageWidth - row.Margin.Left - row.Margin.Right; // 可用宽度

                // 计算每个单元格的宽度和高度
                foreach (var cell in row.Cells)
                {
                    // 计算单元格的宽度
                    double cellWidth = cell.Width > 0 ? cell.Width : MeasureCellWidth(graphics, cell, font);
                    totalWidth += cellWidth;

                    // 计算单元格的高度
                    double cellHeight = MeasureCellHeight(graphics, cell, font);
                    rowHeight = Math.Max(rowHeight, cellHeight);
                }

                // 处理总宽度超出页面宽度的情况
                if (totalWidth > availableWidth)
                {
                    double scale = availableWidth / totalWidth;
                    foreach (var cell in row.Cells)
                    {
                        cell.Width *= scale; // 根据比例调整单元格宽度
                    }
                }

                // 如果行高未指定，则使用行内单元格的最大高度
                if (row.Height <= 0)
                {
                    row.Height = rowHeight;
                }

                // 统一设置所有单元格的高度为行高
                foreach (var cell in row.Cells)
                {
                    cell.Height = row.Height; // 假设XGridCell有Height属性
                }

                // 绘制行边框
                if (row.Border.Visible)
                {
                    DrawBorder(graphics, row.Border, row.Margin.Left, currentY, totalWidth, row.Height);
                }

                // 绘制单元格
                double currentX = row.Margin.Left;
                foreach (var cell in row.Cells)
                {
                    DrawCell(graphics, cell, font, brush, currentX, currentY, row.Height);
                    currentX += cell.Width; // 移动到下一个单元格的X坐标
                }

                // 移动Y坐标到下一行的顶部
                currentY += row.Height + row.Margin.Top + row.Margin.Bottom;
            }
        }

        private static double MeasureCellWidth(XGraphics graphics, XGridCell cell, XFont font)
        {
            if (cell is XGridTextCell textCell)
            {
                var size = graphics.MeasureString(textCell.Text, font);
                return size.Width + cell.Padding.Left + cell.Padding.Right; // 添加内边距
            }
            return 0; // 其他类型的单元格
        }

        private static double MeasureCellHeight(XGraphics graphics, XGridCell cell, XFont font)
        {
            if (cell is XGridTextCell textCell)
            {
                var size = graphics.MeasureString(textCell.Text, font);
                // 假设行间距为字体高度的一定倍数
                double lineHeight = font.Height;
                int lineCount = (int)Math.Ceiling(size.Height / lineHeight);
                return lineCount * lineHeight + cell.Padding.Top + cell.Padding.Bottom; // 添加内边距
            }
            return 0; // 其他类型的单元格
        }

        private static void DrawCell(XGraphics graphics, XGridCell cell, XFont font, XBrush brush, double x, double y, double height)
        {
            if (cell is XGridTextCell textCell)
            {
                // 绘制文本
                var layout = new XTextFormatter(graphics)
                {
                    Alignment = textCell.HorizontalAlignment == XGridAlignment.Center ? XParagraphAlignment.Center :
                                textCell.HorizontalAlignment == XGridAlignment.Right ? XParagraphAlignment.Right :
                                XParagraphAlignment.Left
                };
                var rect = new XRect(x + cell.Margin.Left, y + cell.Margin.Top, cell.Width - cell.Margin.Left - cell.Margin.Right, height - cell.Margin.Top - cell.Margin.Bottom);
                layout.DrawString(textCell.Text, font, brush, rect, XStringFormats.TopLeft);
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
