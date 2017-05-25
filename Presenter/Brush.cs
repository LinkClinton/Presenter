using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presenter
{
    public partial class Brush
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

        ~Brush() => brush?.Dispose();
    }




}
