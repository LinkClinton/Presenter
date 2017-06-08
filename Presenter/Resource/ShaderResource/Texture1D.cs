using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presenter
{
    public class Texture1D : ShaderResource, ITexture1D
    {
        private int tWidth;
        private int mipLevels;

        public Texture1D(int width, ResourceFormat format, int miplevels = 0)
        {
            tWidth = width;
            pixelFormat = format;
            mipLevels = miplevels;

            resource = new SharpDX.Direct3D11.Texture1D(Manager.ID3D11Device,
                new SharpDX.Direct3D11.Texture1DDescription()
                {
                    ArraySize = 1,
                    BindFlags = SharpDX.Direct3D11.BindFlags.ShaderResource,
                    CpuAccessFlags = SharpDX.Direct3D11.CpuAccessFlags.None,
                    Format = (SharpDX.DXGI.Format)pixelFormat,
                    MipLevels = mipLevels,
                    OptionFlags = SharpDX.Direct3D11.ResourceOptionFlags.None,
                    Usage = SharpDX.Direct3D11.ResourceUsage.Default,
                    Width = tWidth
                });

            resourceview = new SharpDX.Direct3D11.ShaderResourceView(Manager.ID3D11Device,
                resource);

            size = ResourceFormatCounter.CountFormatSize(pixelFormat) * tWidth;
        }

        public int Width => tWidth;

        public int MipLevels => mipLevels;
    }
}
