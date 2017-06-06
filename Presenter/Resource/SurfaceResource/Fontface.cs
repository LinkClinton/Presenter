using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presenter
{
    public partial class ExFontface : IFontface
    {
        private SharpDX.DirectWrite.TextFormat fontface;

        private float size;
        private int weight;

        public ExFontface(string fontname, float fontsize, int fontweight = 400)
        {
            size = fontsize;
            
            fontface = new SharpDX.DirectWrite.TextFormat(Manager.IDWriteFactory,
                fontname, (SharpDX.DirectWrite.FontWeight)(weight = fontweight),
                SharpDX.DirectWrite.FontStyle.Normal, fontsize);
        }

        public float Size => size;

        public int Weight => weight;

        internal SharpDX.DirectWrite.TextFormat IDWriteTextFormat => fontface;

        ~ExFontface() => SharpDX.Utilities.Dispose(ref fontface);
    }

   public static partial class Manager
    {
        public static void ExPutText(string text, (float x, float y) pos,
          ExBrush brush, ExFontface fontface)
        {
            SharpDX.DirectWrite.TextLayout layout = new SharpDX.DirectWrite.TextLayout(
                IDWriteFactory, text, fontface.IDWriteTextFormat, float.MaxValue, float.MaxValue);

            ID2D1DeviceContext.DrawTextLayout(new SharpDX.Mathematics.Interop.RawVector2(pos.x, pos.y),
                layout, brush.ID2D1Brush);

            SharpDX.Utilities.Dispose(ref layout);
        }

        public static void ExPutText(string text, (float left, float top, float right, float bottom) rect,
            ExBrush brush, ExFontface fontface)
        {
            ID2D1DeviceContext.DrawText(text, fontface.IDWriteTextFormat, new SharpDX.Mathematics.Interop.
                RawRectangleF(rect.left, rect.top, rect.right, rect.bottom), brush.ID2D1Brush);
        }
    }

   

}
