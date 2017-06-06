using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presenter
{
    public class BitmapContext
    {
        private Dictionary<string, ExBitmap> bitmapindexer = new Dictionary<string, ExBitmap>();

        internal BitmapContext()
        {

        }

        public ExBitmap this[string index]
        {
            get
            {
                if (bitmapindexer.ContainsKey(index) is false)
                    bitmapindexer.Add(index, new ExBitmap(index));
                return bitmapindexer[index];
            }
        }

        public void Destory(string index)
            => bitmapindexer.Remove(index);
    }

    public partial class ExBitmap
    {
        private static BitmapContext context = new BitmapContext();

        public static BitmapContext ExContext => context;
    }
}
