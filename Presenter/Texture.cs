using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presenter
{
    public class Texture : ShaderResource
    {
        int width;
        int height;

        public Texture(string filename)
        {
            SharpDX.WIC.BitmapDecoder decoder = new SharpDX.WIC.BitmapDecoder(Manager.ImagingFactory,
              filename, SharpDX.IO.NativeFileAccess.Read, SharpDX.WIC.DecodeOptions.CacheOnLoad);

            SharpDX.WIC.FormatConverter converter = new SharpDX.WIC.FormatConverter(Manager.ImagingFactory);

            SharpDX.WIC.BitmapFrameDecode frame = decoder.GetFrame(0);

            int maxsize = 0;

            int Width = frame.Size.Width;
            int Height = frame.Size.Height;

            switch (Manager.ID3D11Device.FeatureLevel)
            {
                case SharpDX.Direct3D.FeatureLevel.Level_9_1:
                case SharpDX.Direct3D.FeatureLevel.Level_9_2:
                    maxsize = 2048;
                    break;
                case SharpDX.Direct3D.FeatureLevel.Level_9_3:
                    maxsize = 4096;
                    break;
                case SharpDX.Direct3D.FeatureLevel.Level_10_0:
                case SharpDX.Direct3D.FeatureLevel.Level_10_1:
                    maxsize = 8192;
                    break;
                default:
                    maxsize = 16384;
                    break;
            }

            if (frame.Size.Width > maxsize || frame.Size.Height > maxsize)
            {
                float scale = (float)Height / Width;
                if (Width > Height)
                {
                    width = maxsize;
                    height = (int)Math.Max(1f, maxsize * scale);
                }else
                {
                    width = maxsize;
                    height = (int)Math.Max(1f, maxsize / scale);
                }
            }else
            {
                width = Width;
                height = Height;
            }

            

        }

        public int Width => width;
        public int Height => height;
    }
}
