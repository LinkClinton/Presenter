using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presenter
{
    public class FontfaceIndexer
    {
        private Dictionary<(string name, float size, int weight), Fontface> fontfaceindexer
            = new Dictionary<(string name, float size, int weight), Fontface>();

        public Fontface this[(string name, float size, int weight) index]
        {
            get
            {
                if (fontfaceindexer.ContainsKey(index) is false)
                    fontfaceindexer.Add(index, new Fontface(index.name, index.size, index.weight));
                return fontfaceindexer[index];
            }
        }

        public void Destory((string name, float size, int weight) index)
            => fontfaceindexer.Remove(index);
    }

    public class FontfaceContext
    {
        public Fontface this[(string name,float size,int weight) index]
        {
            get => Manager.Fontface[index];
        }
    }

    public static partial class Manager
    {
        private static FontfaceIndexer fontface = new FontfaceIndexer();

        public static FontfaceIndexer Fontface => fontface;
    }

    public partial class Fontface
    {
        private static FontfaceContext context = new FontfaceContext();

        public static FontfaceContext Context => context;
    }
}
