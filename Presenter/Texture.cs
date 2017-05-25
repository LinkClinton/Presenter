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

            converter.Initialize(frame, SharpDX.WIC.PixelFormat.Format32bppRGBA,
                 SharpDX.WIC.BitmapDitherType.None, null, 0, SharpDX.WIC.BitmapPaletteType.MedianCut);

            width = converter.Size.Width;
            height = converter.Size.Height;

            int bpp = SharpDX.WIC.PixelFormat.GetBitsPerPixel(converter.PixelFormat);

            int rowPitch = (width * bpp + 7) / 8;
            int imagesize= rowPitch * height;

            byte[] pixels = new byte[imagesize];

            converter.CopyPixels(pixels, rowPitch);

            resource = new SharpDX.Direct3D11.Texture2D(Manager.ID3D11Device,
                new SharpDX.Direct3D11.Texture2DDescription()
                {
                    ArraySize = 1,
                    BindFlags = SharpDX.Direct3D11.BindFlags.ShaderResource,
                    CpuAccessFlags = SharpDX.Direct3D11.CpuAccessFlags.None,
                    Format = WICHelper.Translate[converter.PixelFormat],
                    Height = height,
                    Width = width,
                    MipLevels = 1,
                    OptionFlags = SharpDX.Direct3D11.ResourceOptionFlags.None,
                    Usage = SharpDX.Direct3D11.ResourceUsage.Default,
                    SampleDescription = new SharpDX.DXGI.SampleDescription(1, 0)
                });

            Manager.ID3D11DeviceContext.UpdateSubresource(pixels, resource, 0, rowPitch);

            resourceview = new SharpDX.Direct3D11.ShaderResourceView(Manager.ID3D11Device,
                resource);

            size = imagesize;
        }

        public int Width => width;
        public int Height => height;

        public static implicit operator SharpDX.Direct3D11.Texture2D(Texture texture)
            => (texture.resource as SharpDX.Direct3D11.Texture2D);

        ~Texture() => (resource as SharpDX.Direct3D11.Texture2D)?.Dispose();
    }


}
