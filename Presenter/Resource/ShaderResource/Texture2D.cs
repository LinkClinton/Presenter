using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presenter
{
    public class Texture2D : ShaderResource, ITexture2D
    {
        private int tWidth;
        private int tHeight;
        private int mipLevels;

        public Texture2D(int width,int height,ResourceFormat format,int miplevels = 0)
        {
            tWidth = width;
            tHeight = height;
            pixelFormat = format;
            mipLevels = miplevels;

            resource = new SharpDX.Direct3D11.Texture2D(Manager.ID3D11Device,
                new SharpDX.Direct3D11.Texture2DDescription()
                {
                    ArraySize = 1,
                    BindFlags = SharpDX.Direct3D11.BindFlags.ShaderResource,
                    CpuAccessFlags = SharpDX.Direct3D11.CpuAccessFlags.None,
                    Format = (SharpDX.DXGI.Format)pixelFormat,
                    Height = tHeight,
                    Width = tWidth,
                    MipLevels = mipLevels,
                    OptionFlags = SharpDX.Direct3D11.ResourceOptionFlags.None,
                    SampleDescription = new SharpDX.DXGI.SampleDescription(1, 0),
                    Usage = SharpDX.Direct3D11.ResourceUsage.Default
                });

            resourceview = new SharpDX.Direct3D11.ShaderResourceView(Manager.ID3D11Device,
                resource);

            size = ResourceFormatCounter.CountFormatSize(pixelFormat) * tWidth * tHeight;
        }

        public int Width => tWidth;

        public int Height => tHeight;

        public int MipLevels => mipLevels;
    }
}
