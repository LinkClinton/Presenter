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

        private int size;
        private int weight;

        public Fontface(string fontname, int fontsize, int fontweight = 400)
        {
            size = fontsize;

            fontface = new SharpDX.DirectWrite.TextFormat(Manager.IDWriteFactory,
                fontname, (SharpDX.DirectWrite.FontWeight)(weight = fontweight),
                SharpDX.DirectWrite.FontStyle.Normal, fontsize);
        }

        public int Size => size;

        public int Weight => weight;

        public static implicit operator SharpDX.DirectWrite.TextFormat(Fontface fontface) 
            => fontface.fontface;

        ~Fontface() => fontface.Dispose();
    }
}
