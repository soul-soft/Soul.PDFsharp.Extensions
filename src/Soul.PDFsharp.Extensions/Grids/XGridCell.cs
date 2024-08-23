namespace Soul.PDFsharp.Extensions
{
    public abstract class XGridCell
    {
        /// <summary>
        /// 高度
        /// </summary>
        public double Height { get; set; }
        /// <summary>
        /// 宽度
        /// </summary>
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
        public XGridBorder Border { get; } = new XGridBorder();
    }
}
