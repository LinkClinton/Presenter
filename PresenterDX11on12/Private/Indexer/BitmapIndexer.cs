using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presenter
{
    public class BitmapIndexer
    {
        private Dictionary<string, Bitmap> bitmapindexer = new Dictionary<string, Bitmap>();

        public Bitmap this[string index]
        {
            get
            {
                if (bitmapindexer.ContainsKey(index) is false)
                    bitmapindexer.Add(index, new Bitmap(index));
                return bitmapindexer[index];
            }
        }

        public void Destory(string index)
            => bitmapindexer.Remove(index);
    }

    public class BitmapContext
    {
        public Bitmap this[string index]
        {
            get => Manager.Bitmap[index];
        }
    }

    public static partial class Manager
    {
        private static BitmapIndexer bitmap = new BitmapIndexer();

        public static BitmapIndexer Bitmap => bitmap;
    }

    public partial class Bitmap
    {
        private static BitmapContext context = new BitmapContext();

        public BitmapContext Context => context;
    }
}
