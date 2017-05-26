using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presenter
{
    public partial class Fontface
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

        internal SharpDX.DirectWrite.TextFormat IDWriteTextFormat => fontface;

        ~Fontface() => SharpDX.Utilities.Dispose(ref fontface);
    }

   

   

}
