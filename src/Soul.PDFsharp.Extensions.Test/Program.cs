using PdfSharp.Fonts;
using PdfSharp.Pdf;
using Soul.PDFsharp.Extensions.Test;
using Soul.PDFsharp.Extensions;
using PdfSharp.Drawing;
using System.Diagnostics;

GlobalFontSettings.FontResolver = new WindowsFontResolver();
GlobalFontSettings.DefaultFontEncoding = PdfFontEncoding.Unicode;
var showBorder = true;
var document = new PdfDocument();
document.Info.Title = "工程造价咨询报告书";
//document.DrawPage((page, gfx) =>
//{
//    page.Size = PdfSharp.PageSize.A4;
//    // 设置字体
//    var bodyFont = new XFont("STSONG.TTF", 12, XFontStyleEx.Bold);
//    var titleFont = new XFont("STSONG.TTF", 30, XFontStyleEx.Bold);
//    var footerFont = new XFont("STSONG.TTF", 18, XFontStyleEx.Bold);
//    gfx.DrawGrid(100, titleFont, XBrushes.Black, grid =>
//    {
//        grid.DrawRow(row =>
//        {
//            row.Margin = 50;
//            row.Height = 50;
//            row.Border.Visible = showBorder;
//            row.DrawTextCell(cell =>
//            {
//                cell.Text = "工程造价咨询报告书";
//                cell.Alignment = XGridAlignment.Center;
//            });
//        });
//    });
//    gfx.DrawGrid(350, bodyFont, XBrushes.Black, grid =>
//    {
//        grid.DrawRow(row =>
//        {
//            row.Margin = 100;
//            row.Border.Visible = showBorder;
//            row.DrawTextCell(cell =>
//            {
//                cell.Text = "咨询项目全称: ";
//                //cell.AutoHeight = false;
//            });
//            row.DrawTextCell(cell =>
//            {
//                cell.Text = "伊犁新天煤化工有限责任公司年产20亿Nm3煤制天然气项目2020年煤气水膨胀入气柜施工合同";
//                cell.Warp = true;
//                cell.LineSpacing = 5;
//            });
//        });
//        grid.DrawRow(row =>
//        {
//            row.Height = 40;
//            row.Margin = 100;
//            row.Border.Visible = showBorder;
//            row.DrawTextCell(cell =>
//            {
//                cell.Text = "咨询业务类别: ";
//            });
//            row.DrawTextCell(cell =>
//            {
//                cell.Text = "工程结算审核";
//            });
//        });
//        grid.DrawRow(row =>
//        {
//            row.Height = 40;
//            row.Margin = 100;
//            row.Border.Visible = showBorder;
//            row.DrawTextCell(cell =>
//            {
//                cell.Text = "咨询报告日期: ";
//            });
//            row.DrawTextCell(cell =>
//            {
//                cell.Text = "二〇二三年四月二十四日";
//            });
//        });
//    });
//    gfx.DrawGrid(page.Height.Value - 100, footerFont, XBrushes.Black, grid =>
//    {
//        grid.DrawRow(row =>
//        {
//            row.Margin = 50;
//            row.Height = 50;
//            row.Border.Visible = showBorder;
//            row.DrawTextCell(cell =>
//            {
//                cell.Text = "万邦工程管理咨询有限公司";
//                cell.Alignment = XGridAlignment.Center;
//            });
//        });
//    });
//});
//document.DrawPage((page, gfx) =>
//{
//    var bodyFont = new XFont("STSONG.TTF", 12);
//    gfx.DrawRectangle(new XPen(XColors.Black, 0.5), new XRect(50, 50, page.Width.Value - 100, page.Height.Value - 100));
//    gfx.DrawGrid(100, bodyFont, XBrushes.Black, grid =>
//    {
//        grid.DrawRow(row =>
//        {
//            row.Height = 50;
//            row.MarginLeft = 100;
//            row.MarginRight = 50;
//            row.Border.Visible = showBorder;
//            row.DrawTextCell(cell =>
//            {
//                cell.Text = "咨询报告书编号：万工咨结审H[2023]507号";
//            });
//        });
//        grid.DrawRow(row =>
//        {
//            row.Height = 50;
//            row.MarginLeft = 100;
//            row.MarginRight = 50;
//            row.Border.Visible = showBorder;
//            row.DrawTextCell(cell =>
//            {
//                cell.Text = "咨询项目委托方全称：杭州之江城市建设投资集团有限公司";
//            });
//        });
//        grid.DrawRow(row =>
//        {
//            row.Height = 50;
//            row.MarginLeft = 100;
//            row.MarginRight = 50;
//            row.Border.Visible = showBorder;
//            row.DrawTextCell(cell =>
//            {
//                cell.Text = "咨询企业法定住所： 宁波市海曙区布政巷16号19楼";
//            });
//        });
//    });
//    gfx.DrawGrid(250, bodyFont, XBrushes.Black, grid =>
//    {
//        grid.DrawRow(row =>
//        {
//            row.Height = 30;
//            row.MarginLeft = 100;
//            row.MarginRight = 50;
//            row.DrawTextCell(cell =>
//            {
//                cell.Text = "邮            编：";
//                cell.Width = 100;
//                cell.Border.Visible = showBorder;
//            });
//            row.DrawTextCell(cell =>
//            {
//                cell.Text = "315010";
//                cell.Width = 100;
//                cell.Border.Visible = showBorder;
//            });
//            row.DrawTextCell(cell =>
//            {
//                cell.Text = "联系电话：";
//                cell.Width = 100;
//                cell.Border.Visible = showBorder;
//            });
//            row.DrawTextCell(cell =>
//            {
//                cell.Text = "0574-83890360";
//                cell.Width = 100;
//                cell.Border.Visible = showBorder;
//            });
//        });
//        grid.DrawRow(row =>
//        {
//            row.Height = 50;
//            row.MarginLeft = 100;
//            row.MarginRight = 50;
//            row.DrawTextCell(cell =>
//            {
//                cell.Text = "咨询作业期：";
//                cell.Width = 100;
//                cell.Border.Visible = showBorder;
//            });
//            row.DrawTextCell(cell =>
//            {
//                cell.Text = "2022年11月至2023年4月";
//                cell.Width = 100;
//                cell.Border.Visible = showBorder;
//            });
//        });
//        grid.DrawRow(row =>
//        {
//            row.Height = 80;
//            row.MarginLeft = 100;
//            row.MarginRight = 50;
//            row.DrawTextCell(cell =>
//            {
//                cell.Text = "法定代表人：";
//                cell.Width = 100;
//                cell.Border.Visible = showBorder;
//            });
//            row.DrawTextCell(cell =>
//            {
//                cell.Text = "邵铭法（盖章）";
//                cell.Width = 100;
//                cell.Border.Visible = showBorder;
//            });
//            row.DrawTextCell(cell =>
//            {
//                cell.Text = "技术负责人：";
//                cell.Width = 100;
//                cell.Border.Visible = showBorder;
//            });
//            row.DrawTextCell(cell =>
//            {
//                cell.Text = "陈建华(盖章)";
//                cell.Width = 100;
//                cell.Border.Visible = showBorder;
//            });
//        });
//        grid.DrawRow(row =>
//        {
//            row.Height = 80;
//            row.MarginLeft = 100;
//            row.MarginRight = 50;
//            row.DrawTextCell(cell =>
//            {
//                cell.Text = "项目负责人：";
//                cell.Width = 100;
//                cell.Border.Visible = showBorder;
//            });
//            row.DrawTextCell(cell =>
//            {
//                cell.Text = "";
//                cell.Width = 60;
//                cell.Border.Visible = showBorder;
//            });
//            row.DrawTextCell(cell =>
//            {
//                cell.Text = "执(从)业资格(章)：";
//                cell.Width = 50;
//                cell.Border.Visible = showBorder;
//            });
//            row.DrawTextCell(cell =>
//            {
//                cell.Text = "";
//                cell.Width = 60;
//                cell.Border.Visible = showBorder;
//            });
//            row.DrawTextCell(cell =>
//            {
//                cell.Text = "从事专业：";
//                cell.Width = 50;
//                cell.Border.Visible = showBorder;
//            });
//            row.DrawTextCell(cell =>
//            {
//                cell.Text = "";
//                cell.Width = 50;
//                cell.Border.Visible = showBorder;
//            });
//        });
//        grid.DrawRow(row =>
//        {
//            row.Height = 80;
//            row.MarginLeft = 100;
//            row.MarginRight = 50;
//            row.DrawTextCell(cell =>
//            {
//                cell.Text = "专业咨询人员：";
//                cell.Width = 100;
//                cell.Border.Visible = showBorder;
//            });
//            row.DrawTextCell(cell =>
//            {
//                cell.Text = "";
//                cell.Width = 60;
//                cell.Border.Visible = showBorder;
//            });
//            row.DrawTextCell(cell =>
//            {
//                cell.Text = "执(从)业资格(章)：";
//                cell.Width = 50;
//                cell.Border.Visible = showBorder;
//            });
//            row.DrawTextCell(cell =>
//            {
//                cell.Text = "";
//                cell.Width = 60;
//                cell.Border.Visible = showBorder;
//            });
//            row.DrawTextCell(cell =>
//            {
//                cell.Text = "从事专业：";
//                cell.Width = 50;
//                cell.Border.Visible = showBorder;
//            });
//            row.DrawTextCell(cell =>
//            {
//                cell.Text = "";
//                cell.Width = 50;
//                cell.Border.Visible = showBorder;
//            });
//        });
//        grid.DrawRow(row =>
//        {
//            row.Height = 80;
//            row.MarginLeft = 100;
//            row.MarginRight = 50;
//            row.DrawTextCell(cell =>
//            {
//                cell.Text = "专业咨询人员：";
//                cell.Width = 100;
//                cell.Border.Visible = showBorder;
//            });
//            row.DrawTextCell(cell =>
//            {
//                cell.Text = "";
//                cell.Width = 60;
//                cell.Border.Visible = showBorder;
//            });
//            row.DrawTextCell(cell =>
//            {
//                cell.Text = "执(从)业资格(章)：";
//                cell.Width = 50;
//                cell.Border.Visible = showBorder;
//            });
//            row.DrawTextCell(cell =>
//            {
//                cell.Text = "";
//                cell.Width = 60;
//                cell.Border.Visible = showBorder;
//            });
//            row.DrawTextCell(cell =>
//            {
//                cell.Text = "从事专业：";
//                cell.Width = 50;
//                cell.Border.Visible = showBorder;
//            });
//            row.DrawTextCell(cell =>
//            {
//                cell.Text = "";
//                cell.Width = 50;
//                cell.Border.Visible = showBorder;
//            });
//        });
//        grid.DrawRow(row =>
//        {
//            row.Height = 80;
//            row.MarginLeft = 100;
//            row.MarginRight = 50;
//            row.DrawTextCell(cell =>
//            {
//                cell.Text = "专业咨询人员：";
//                cell.Width = 100;
//                cell.Border.Visible = showBorder;
//            });
//            row.DrawTextCell(cell =>
//            {
//                cell.Text = "";
//                cell.Width = 60;
//                cell.Border.Visible = showBorder;
//            });
//            row.DrawTextCell(cell =>
//            {
//                cell.Text = "执(从)业资格(章)：";
//                cell.Width = 50;
//                cell.Border.Visible = showBorder;
//            });
//            row.DrawTextCell(cell =>
//            {
//                cell.Text = "";
//                cell.Width = 60;
//                cell.Border.Visible = showBorder;
//            });
//            row.DrawTextCell(cell =>
//            {
//                cell.Text = "从事专业：";
//                cell.Width = 50;
//                cell.Border.Visible = showBorder;
//            });
//            row.DrawTextCell(cell =>
//            {
//                cell.Text = "";
//                cell.Width = 50;
//                cell.Border.Visible = showBorder;
//            });
//        });
//    });
//});
//document.DrawPage((page, gfx) => 
//{
//    var bodyFont = new XFont("STSONG.TTF", 12, XFontStyleEx.Bold);
//    var titleFont = new XFont("STSONG.TTF", 24, XFontStyleEx.Bold);
//    gfx.DrawGrid(50, titleFont, XBrushes.Black, grid =>
//    {
//        grid.DrawRow(row =>
//        {
//            row.Height = 50;
//            row.Margin = 100;
//            row.Border.Visible = showBorder;
//            row.DrawTextCell(cell =>
//            {
//                cell.Text = "咨 询 报 告 书 目 录";
//                cell.Alignment = XGridAlignment.Center;
//            });
//        });
//    });
//    gfx.DrawGrid(120, bodyFont, XBrushes.Black, grid =>
//    {
//        grid.DrawRow(row =>
//        {
//            row.Height = 15;
//            row.Margin = 50;
//            row.Border.Visible = showBorder;
//            row.DrawTextCell(cell =>
//            {
//                cell.Text = "序号";
//                cell.Border.Visible = true;
//                cell.Width = 30;
//                cell.Alignment = XGridAlignment.Center;
//            });
//            row.DrawTextCell(cell =>
//            {
//                cell.Text = "文件内容";
//                cell.Border.Visible = true;
//                cell.Width = 80;
//                cell.Alignment = XGridAlignment.Center;
//            });
//            row.DrawTextCell(cell =>
//            {
//                cell.Text = "文件作者";
//                cell.Border.Visible = true;
//                cell.Width = 80;
//                cell.Alignment = XGridAlignment.Center;
//            });
//            row.DrawTextCell(cell =>
//            {
//                cell.Text = "页数";
//                cell.Border.Visible = true;
//                cell.Width = 80;
//                cell.Alignment = XGridAlignment.Center;
//            });
//            row.DrawTextCell(cell =>
//            {
//                cell.Text = "备注";
//                cell.Border.Visible = true;
//                cell.Alignment = XGridAlignment.Center;
//            });
//        });

//        for (int i = 0; i < 10; i++)
//        {
//            grid.DrawRow(row =>
//            {
//                row.Height = 15;
//                row.Margin = 50;
//                row.Border.Visible = showBorder;
//                row.DrawTextCell(cell =>
//                {
//                    cell.Text = $"{i + 1}";
//                    cell.Border.Visible = true;
//                    cell.Width = 30;
//                    cell.Alignment = XGridAlignment.Center;
//                });
//                var content = $"报告正文{i}";
//                if (i == 2)
//                {
//                    content = $"报告正文{i}报告正文{i}报告正文{i}报告正文{i}报告正文{i}报告正文{i}报告正文{i}";
//                }
//                row.DrawTextCell(cell =>
//                {
//                    cell.Text = content;
//                    cell.Border.Visible = true;
//                    cell.Alignment = XGridAlignment.Center;
//                    cell.Width = 80;
//                    cell.Warp = true ;
//                });
//                row.DrawTextCell(cell =>
//                {
//                    cell.Text = "xx";
//                    cell.Border.Visible = true;
//                    cell.Alignment = XGridAlignment.Center;
//                    cell.Width = 80;
//                });
//                row.DrawTextCell(cell =>
//                {
//                    cell.Text = "1-2";
//                    cell.Border.Visible = true;
//                    cell.Alignment = XGridAlignment.Center;
//                    cell.Width = 80;
//                });
//                row.DrawTextCell(cell =>
//                {
//                    cell.Text = "***";
//                    cell.Border.Visible = true;
//                    cell.Alignment = XGridAlignment.Center;
//                });
//            });
//        }
//    });
//});

document.DrawPage((page, gfx) =>
{
    var bodyFont = new XFont("STSONG.TTF", 12, XFontStyleEx.Bold);
    gfx.DrawGrid(100, bodyFont, XBrushes.Black, grid =>
    {
        grid.DrawRow(row => 
        {
            row.Height = 100;
            row.Margin = 100;
            //row.Border.Visible = true;
            row.DrawTextCell(cell => 
            {
                cell.Width = 100;
                cell.Text = "PdfSharp 是一个用于创建和处理 PDF 文件的 .NET 库，但在其标准功能中并没有直接提供文字底部对齐的选项。通常情况下，文本的对齐方式（如左对齐、居中对齐和右对齐）是比较常见的，但底部对齐通常需要一些额外的处理。";
                cell.Warp = true;
                cell.Border.Visible = true;
            });
        });
    });
});
// 保存文档
string filename = "工程造价咨询报告书.pdf";
document.Save(filename);
// 打开生成的 PDF 文件
Process.Start(new ProcessStartInfo(filename) { UseShellExecute = true });