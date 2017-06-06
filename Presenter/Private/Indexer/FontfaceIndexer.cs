using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presenter
{
    public class FontfaceContext
    {
        private Dictionary<(string name, float size, int weight), ExFontface> fontfaceindexer
            = new Dictionary<(string name, float size, int weight), ExFontface>();

        internal FontfaceContext()
        {

        }

        public ExFontface this[(string name,float size,int weight) index]
        {
            get
            {
                if (fontfaceindexer.ContainsKey(index) is false)
                    fontfaceindexer.Add(index, new ExFontface(index.name, index.size, index.weight));
                return fontfaceindexer[index];
            }
        }

        public void Destory((string name, float size, int weight) index)
            => fontfaceindexer.Remove(index);
    }

    public partial class ExFontface
    {
        private static FontfaceContext context = new FontfaceContext();

        public static FontfaceContext ExContext => context;
    }
}
