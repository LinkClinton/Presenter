using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presenter
{
    public class Bitmap
    {
        private SharpDX.Direct2D1.Bitmap bitmap;

        public Bitmap(string filename)
        {
            SharpDX.WIC.BitmapDecoder decoder = new SharpDX.WIC.BitmapDecoder(Manager.ImagingFactory,
                filename, SharpDX.IO.NativeFileAccess.Read, SharpDX.WIC.DecodeOptions.CacheOnLoad);

            SharpDX.WIC.FormatConverter converter = new SharpDX.WIC.FormatConverter(Manager.ImagingFactory);

            converter.Initialize(decoder.GetFrame(0), SharpDX.WIC.PixelFormat.Format32bppPBGRA,
                 SharpDX.WIC.BitmapDitherType.None, null, 0, SharpDX.WIC.BitmapPaletteType.MedianCut);
            
            bitmap = SharpDX.Direct2D1.Bitmap.FromWicBitmap(Manager.ID2D1DeviceContext, converter);
        }

        public float Width => bitmap.Size.Width;
        public float Height => bitmap.Size.Height;


        public static implicit operator SharpDX.Direct2D1.Bitmap(Bitmap bitmap) => bitmap.bitmap;

        ~Bitmap() => bitmap?.Dispose();
    }

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

    public static partial class Manager
    {
        private static BitmapIndexer bitmap = new BitmapIndexer();

        public static BitmapIndexer Bitmap => bitmap;
    }

}
