using PdfSharp.Drawing;
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
            graphics.CalcGridElements(font, brush, grid);
            var yOffset = y;
            foreach (var row in grid.Rows)
            {
                var xOffset = row.MarginLeft;
                foreach (var cell in row.Cells)
                {
                    xOffset += cell.MarginLeft;
                    if (cell is XGridTextCell textCell)
                    {
                        var lines = new List<string>()
                        {
                            textCell.Text
                        };
                        if (textCell.Warp)
                        {
                            lines = graphics.GetLines(font, textCell.Text, cell.Width - 4);
                        }
                        var yLineOffset = 0d;
                        foreach (var item in lines)
                        {
                            if (string.IsNullOrEmpty(item))
                            {
                                continue;
                            }
                            var textSize = graphics.MeasureString(textCell.Text, font);
                            var cellHeight = cell.AutoHeight ? row.Height : textSize.Height;
                            var centerOffset = (cellHeight - (textSize.Height * lines.Count + textCell.LineSpacing * lines.Count-1)) / 2;
                            if (textCell.Alignment == XGridAlignment.Center)
                            {
                                graphics.DrawString(item, font, brush, new XRect(xOffset, yOffset + centerOffset + yLineOffset, cell.Width, 0), XStringFormats.TopCenter);
                            }
                            else if (textCell.Alignment == XGridAlignment.Left)
                            {
                                graphics.DrawString(item, font, brush, new XRect(xOffset, yOffset + centerOffset + yLineOffset, cell.Width, 0), XStringFormats.TopLeft);
                            }
                            else if (textCell.Alignment == XGridAlignment.Right)
                            {
                                graphics.DrawString(item, font, brush, new XRect(xOffset, yOffset + centerOffset + yLineOffset, cell.Width, 0), XStringFormats.TopRight);
                            }
                            yLineOffset += textSize.Height + textCell.LineSpacing;
                        }
                        if (cell.Border.Visible)
                        {
                            graphics.DrawRectangle(new XPen(row.Border.Color, row.Border.Size), new XRect(xOffset, yOffset, cell.Width, row.Height));
                        }
                    }
                    xOffset += cell.Width + cell.MarginRight;
                }

                if (row.Border.Visible)
                {
                    graphics.DrawRectangle(new XPen(row.Border.Color, row.Border.Size), new XRect(row.MarginLeft, yOffset, graphics.PageSize.Width - (row.MarginLeft + row.MarginRight), row.Height));
                }

                yOffset += row.Height;
            }
        }

        private static void CalcGridElements(this XGraphics graphics, XFont font, XBrush brush, XGrid grid)
        {
            foreach (var row in grid.Rows)
            {
                var xOffset = row.MarginLeft;
                foreach (var cell in row.Cells)
                {
                    xOffset += cell.MarginLeft;
                    if (cell is XGridTextCell textCell)
                    {
                        var textSize = graphics.MeasureString(textCell.Text, font);
                        if (textSize.Width > cell.Width && cell.Width == 0)
                        {
                            cell.Width = textSize.Width;
                        }
                        //溢出检查
                        if (cell.Width + xOffset > graphics.PageSize.Width)
                        {
                            cell.Width = graphics.PageSize.Width - xOffset - row.MarginRight;
                        }
                        var lines = new List<string>()
                        {
                            textCell.Text
                        };
                        if (textCell.Warp)
                        {
                            lines = graphics.GetLines(font, textCell.Text, cell.Width - 4);
                        }
                        //最后一列如果没有设置宽度，那么等于页面宽度，减掉当前偏移，减掉右边距
                        if (cell == row.Cells.Last() && cell.Width == 0)
                        {
                            cell.Width = graphics.PageSize.Width - xOffset - row.MarginRight;
                        }
                        if (textSize.Height * lines.Count + textCell.LineSpacing * lines.Count - 1 > row.Height)
                        {
                            row.Height = textSize.Height * lines.Count + textCell.LineSpacing * lines.Count - 1;
                        }
                    }
                    xOffset += cell.Width + cell.MarginRight;
                }
            }
        }

        private static List<string> GetLines(this XGraphics graphics, XFont font, string text, double lineWidth)
        {
            var lines = new List<string>();
            var sb = new StringBuilder();
            foreach (var item in text)
            {
                var size = graphics.MeasureString(sb.ToString(), font);
                if ((int)size.Width >= (int)lineWidth)
                {
                    lines.Add(sb.ToString());
                    sb.Clear();
                }
                sb.Append(item);
            }
            lines.Add(sb.ToString());
            return lines;
        }
    }
}
