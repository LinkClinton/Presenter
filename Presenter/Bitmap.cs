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
    }
}
