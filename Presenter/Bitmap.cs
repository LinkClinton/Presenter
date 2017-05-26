﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presenter
{
    public partial class Bitmap
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

            converter?.Dispose();
            decoder?.Dispose();
        }

        public float Width => bitmap.Size.Width;
        public float Height => bitmap.Size.Height;


        internal SharpDX.Direct2D1.Bitmap ID2D1Bitmap => bitmap;

        ~Bitmap() => SharpDX.Utilities.Dispose(ref bitmap);
    }




}
