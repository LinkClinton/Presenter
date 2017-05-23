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

        ~Brush() => brush.Dispose();
    }

    public class BrushIndexer
    {
        private Dictionary<(float red, float green, float blue, float alpha), Brush> brushindexer 
            = new Dictionary<(float red, float green, float blue, float alpha), Brush>();

        public Brush this[(float red, float green, float blue, float alpha) index]
        {
            get
            {
                if (brushindexer.ContainsKey(index) is false)
                    brushindexer.Add(index, new Brush(index));
                return brushindexer[index];
            }
        }

        public void Destory((float red, float green, float blue, float alpha) index)
            => brushindexer.Remove(index);
    }

    public static partial class Manager
    {
        private static BrushIndexer brush = new BrushIndexer();

        public static BrushIndexer Brush => brush;
    }

}
