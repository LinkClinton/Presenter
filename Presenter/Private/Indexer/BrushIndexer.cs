using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presenter
{
    public class BrushContext
    {
        private Dictionary<(float red, float green, float blue, float alpha), ExBrush> brushindexer
             = new Dictionary<(float red, float green, float blue, float alpha), ExBrush>();

        internal BrushContext()
        {

        }

        public ExBrush this[(float red, float green, float blue, float alpha) index]
        {
            get
            {
                if (brushindexer.ContainsKey(index) is false)
                    brushindexer.Add(index, new ExBrush(index));
                return brushindexer[index];
            }
        }

        public void Destory((float red, float green, float blue, float alpha) index)
            => brushindexer.Remove(index);
    }

    public partial class ExBrush
    {
        private static BrushContext context = new BrushContext();

        public static BrushContext ExContext => context;
    }


}
