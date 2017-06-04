using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presenter
{
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

    public class BrushContext
    {
        public Brush this[(float red,float green, float blue,float alpha) index]
        {
            get => Manager.Brush[index];
        }

        public void Destory((float red, float green, float blue, float alpha) index)
        {
            Manager.Brush.Destory(index);
        }
    }

    public static partial class Manager
    {
        private static BrushIndexer brush = new BrushIndexer();

        public static BrushIndexer Brush => brush;
    }

    public partial class Brush
    {
        private static BrushContext context = new BrushContext();

        public static BrushContext Context => context;
    }


}
