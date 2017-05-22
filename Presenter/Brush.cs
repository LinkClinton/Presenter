using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presenter
{
    public class Brush
    {
        private SharpDX.Direct2D1.Brush brush;

        public Brush((float red, float green, float blue, float alpha) color)
        {
            brush = new SharpDX.Direct2D1.SolidColorBrush(Manager.ID2D1DeviceContext,
                new SharpDX.Mathematics.Interop.RawColor4(color.red, color.green, color.blue, color.alpha));
        }

        public static implicit operator SharpDX.Direct2D1.Brush(Brush brush) => brush.brush;
    }
}
