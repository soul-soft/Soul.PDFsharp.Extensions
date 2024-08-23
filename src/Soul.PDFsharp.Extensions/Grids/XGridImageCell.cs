using PdfSharp.Drawing;
using System.Collections.Generic;
using System;

namespace Soul.PDFsharp.Extensions
{
   
    public class XGridImageCell : XGridCell
    {
        /// <summary>
        /// 要在此单元格中渲染的图片。
        /// </summary>
        public XImage Image { get; set; }
        
        public double ImageWidth { get; set; }

        public double ImageHeight { get; set; }

        public bool DisableImage { get; set; } = true;
      
    }
}
