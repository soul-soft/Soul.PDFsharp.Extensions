# Soul.PDFsharp.Extensions
基于Grid布局，支持内外边距，多行文本

# 使用方式

```` C#
document.DrawPage((page, gfx) =>
{
    var footerFont = new XFont("STSONG.TTF", 18);
    gfx.DrawGrid(100, footerFont, XBrushes.Black, grid =>
    {
        grid.DrawRow(row =>
        {
            row.Margin.SetHorizontal(100, 100);
            row.DrawTextCell(cell =>
            {
                cell.Text = "你好";
                cell.Border.Visible = true;
                cell.Width = 100;
                cell.VerticalAlignment = XGridAlignment.Top;
                cell.HorizontalAlignment = XGridAlignment.Right;
            });
            row.DrawTextCell(cell =>
            {
                cell.Text = "工程造价咨询报告书工程造价咨询报告书工程造价咨询报告书工程造价咨询报告书工程造价咨询报告书工程造价咨询报告书";
                cell.Border.Visible = true;
                cell.Width = 200;
                cell.Padding.Left = 10;//设置内边距
            });
            row.DrawImageCell(cell =>
            {
                cell.Image = XImage.FromFile("./images/hjd.jpg");
                cell.Border.Visible = true;
                cell.Width = 200;
                cell.ImageWidth = 40;
                cell.ImageHeight = 40;
                cell.HorizontalAlignment = XGridAlignment.Center;
                cell.VerticalAlignment = XGridAlignment.Center;
            });
        });
    });
});
````

