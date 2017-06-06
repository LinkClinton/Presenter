using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presenter
{
    public partial class ExBitmap : IBitmap
    {
        private SharpDX.Direct2D1.Bitmap bitmap;

        public ExBitmap(string filename)
        {
            SharpDX.WIC.BitmapDecoder decoder = new SharpDX.WIC.BitmapDecoder(Manager.ImagingFactory,
                filename, SharpDX.IO.NativeFileAccess.Read, SharpDX.WIC.DecodeOptions.CacheOnLoad);

            SharpDX.WIC.FormatConverter converter = new SharpDX.WIC.FormatConverter(Manager.ImagingFactory);

            SharpDX.WIC.BitmapFrameDecode frame = decoder.GetFrame(0);

            converter.Initialize(frame, SharpDX.WIC.PixelFormat.Format32bppPBGRA,
                 SharpDX.WIC.BitmapDitherType.None, null, 0, SharpDX.WIC.BitmapPaletteType.MedianCut);
            
            bitmap = SharpDX.Direct2D1.Bitmap.FromWicBitmap(Manager.ID2D1DeviceContext, converter);

            SharpDX.Utilities.Dispose(ref frame);
            SharpDX.Utilities.Dispose(ref converter);
            SharpDX.Utilities.Dispose(ref decoder);
        }

        public float Width => bitmap.Size.Width;
        public float Height => bitmap.Size.Height;


        internal SharpDX.Direct2D1.Bitmap ID2D1Bitmap => bitmap;

        ~ExBitmap() => SharpDX.Utilities.Dispose(ref bitmap);
    }

    public static partial class Manager
    {
        public static void ExPutBitmap((float left, float top, float right, float bottom) rect,
          ExBitmap bitmap)
        {
            ID2D1DeviceContext.DrawBitmap(bitmap.ID2D1Bitmap, new SharpDX.Mathematics.Interop.RawRectangleF(
                rect.left, rect.top, rect.right, rect.bottom), 1f, SharpDX.Direct2D1.BitmapInterpolationMode.Linear);
        }
    }



}
