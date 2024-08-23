namespace Soul.PDFsharp.Extensions
{
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
