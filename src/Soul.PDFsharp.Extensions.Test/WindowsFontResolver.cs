using PdfSharp.Fonts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Soul.PDFsharp.Extensions.Test
{
    internal class WindowsFontResolver : IFontResolver
    {
        public byte[] GetFont(string faceName)
        {
            return File.ReadAllBytes($@".\Fonts\{faceName}");
        }

        public FontResolverInfo ResolveTypeface(string familyName, bool isBold, bool isItalic)
        {
            return new FontResolverInfo(familyName, isBold, isItalic);
        }
    }
}
