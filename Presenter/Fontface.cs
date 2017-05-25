using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presenter
{
    public class Fontface
    {
        private SharpDX.DirectWrite.TextFormat fontface;

        private float size;
        private int weight;

        public Fontface(string fontname, float fontsize, int fontweight = 400)
        {
            size = fontsize;

            fontface = new SharpDX.DirectWrite.TextFormat(Manager.IDWriteFactory,
                fontname, (SharpDX.DirectWrite.FontWeight)(weight = fontweight),
                SharpDX.DirectWrite.FontStyle.Normal, fontsize);
        }

        public float Size => size;

        public int Weight => weight;

        public static implicit operator SharpDX.DirectWrite.TextFormat(Fontface fontface) 
            => fontface.fontface;

        ~Fontface() => fontface?.Dispose();
    }

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

    public static partial class Manager
    {
        private static FontfaceIndexer fontface = new FontfaceIndexer();

        public static FontfaceIndexer Fontface => fontface;
    }

}
