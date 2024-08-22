namespace Soul.PDFsharp.Extensions
{
    public class XGridTextCell : XGridCell
    {
        public bool Warp { get; set; }
       
        public string Text { get; set; }
       
        public double LineSpacing { get; set; } = 0;
       
        public XGridAlignment Alignment { get; set; } = XGridAlignment.Left;
      
    }
}
