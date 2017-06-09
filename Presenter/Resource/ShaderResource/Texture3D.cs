using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presenter
{
    public class Texture3D : ShaderResource, ITexture3D
    {
        private int tWidth;
        private int tHeight;
        private int tDepth;
        private int mipLevels;

        public Texture3D(int width, int height, int depth, ResourceFormat format, int miplevels = 1)
        {
            tWidth = width;
            tHeight = height;
            tDepth = depth;
            pixelFormat = format;
            mipLevels = miplevels;

            resource = new SharpDX.Direct3D11.Texture3D(Manager.ID3D11Device,
                new SharpDX.Direct3D11.Texture3DDescription()
                {
                    BindFlags = SharpDX.Direct3D11.BindFlags.ShaderResource,
                    CpuAccessFlags = SharpDX.Direct3D11.CpuAccessFlags.None,
                    Width = tWidth,
                    Height = tHeight,
                    Depth = tDepth,
                    Format = (SharpDX.DXGI.Format)pixelFormat,
                    MipLevels = mipLevels,
                    OptionFlags = SharpDX.Direct3D11.ResourceOptionFlags.None,
                    Usage = SharpDX.Direct3D11.ResourceUsage.Default
                });

            resourceview = new SharpDX.Direct3D11.ShaderResourceView(Manager.ID3D11Device,
                resource);

            size = ResourceFormatCounter.CountFormatSize(pixelFormat) * tWidth * tHeight * tDepth;
        }

        public override void Update<T>(ref T data)
        {
            int rowPitch = ResourceFormatCounter.CountFormatSize(pixelFormat) * tWidth;
            int depthPitch = rowPitch * tHeight;

            Manager.ID3D11DeviceContext.UpdateSubresource(ref data, resource, 0, rowPitch, depthPitch);
        }

        public override void Update<T>(T[] data)
        {
            int rowPitch = ResourceFormatCounter.CountFormatSize(pixelFormat) * tWidth;
            int depthPitch = rowPitch * tHeight;

            Manager.ID3D11DeviceContext.UpdateSubresource(data, resource, 0, rowPitch, depthPitch);
        }

        public int Width => tWidth;

        public int Height => tHeight;

        public int Depth => tDepth;

        public int MipLevels => mipLevels;
    }
}
