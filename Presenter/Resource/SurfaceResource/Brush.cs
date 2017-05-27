using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presenter
{
    public partial class Brush : IBrush
    {
        private SharpDX.Direct2D1.Brush brush;

        private (float red, float green, float blue, float alpha) brushcolor;

        public Brush((float red, float green, float blue, float alpha) color)
        {
            brush = new SharpDX.Direct2D1.SolidColorBrush(Manager.ID2D1DeviceContext,
                new SharpDX.Mathematics.Interop.RawColor4(color.red, color.green, color.blue, color.alpha));

            brushcolor = color;
        }

        public float Red => brushcolor.red;
        public float Green => brushcolor.green;
        public float Blue => brushcolor.blue;
        public float Alpha => brushcolor.alpha;

        internal SharpDX.Direct2D1.Brush ID2D1Brush => brush;

        ~Brush() => SharpDX.Utilities.Dispose(ref brush);
    }

    public static partial class Manager
    {
        public static void PutLine((float x, float y) start, (float x, float y) end,
          Brush brush, float width = 1.0f)
        {
            ID2D1DeviceContext.DrawLine(new SharpDX.Mathematics.Interop.RawVector2(start.x, start.y),
                new SharpDX.Mathematics.Interop.RawVector2(end.x, end.y), brush.ID2D1Brush,
                width);
        }

        public static void PutRectangle((float left, float top, float right, float bottom) rect,
            Brush brush, float width = 1.0f)
        {
            ID2D1DeviceContext.DrawRectangle(new SharpDX.Mathematics.Interop.RawRectangleF(rect.left, rect.top,
                rect.right, rect.bottom), brush.ID2D1Brush, width);
        }

        public static void PutEllipse((float x, float y) center, float radiusx, float radiusy,
            Brush brush, float width = 1.0f)
        {
            ID2D1DeviceContext.DrawEllipse(new SharpDX.Direct2D1.Ellipse(
                new SharpDX.Mathematics.Interop.RawVector2(center.x, center.y), radiusx, radiusy),
                brush.ID2D1Brush, width);
        }

        public static void FillRectangle((float left, float top, float right, float bottom) rect,
        Brush brush)
        {
            ID2D1DeviceContext.FillRectangle(new SharpDX.Mathematics.Interop.RawRectangleF(rect.left, rect.top,
             rect.right, rect.bottom), brush.ID2D1Brush);
        }

        public static void FillEllipse((float x, float y) center, float radiusx, float radiusy,
            Brush brush)
        {
            ID2D1DeviceContext.FillEllipse(new SharpDX.Direct2D1.Ellipse(
                new SharpDX.Mathematics.Interop.RawVector2(center.x, center.y), radiusx, radiusy),
                brush.ID2D1Brush);
        }
    }


}
