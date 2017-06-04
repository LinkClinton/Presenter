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
            int imagesize = rowPitch * height;

            byte[] pixels = new byte[imagesize];

            converter.CopyPixels(pixels, rowPitch);

            resource = Manager.ID3D12Device.CreateCommittedResource(
                new SharpDX.Direct3D12.HeapProperties(SharpDX.Direct3D12.HeapType.Default),
                SharpDX.Direct3D12.HeapFlags.None, SharpDX.Direct3D12.ResourceDescription.
                Texture2D(WICHelper.Translate[converter.PixelFormat], width, height),
                SharpDX.Direct3D12.ResourceStates.CopyDestination);

            var uploadHeapBuffer = Manager.ID3D12Device.CreateCommittedResource(new SharpDX.Direct3D12.HeapProperties(
                 SharpDX.Direct3D12.CpuPageProperty.WriteBack, SharpDX.Direct3D12.MemoryPool.L0),
                  SharpDX.Direct3D12.HeapFlags.None, resource.Description, SharpDX.Direct3D12.ResourceStates.GenericRead);

            var handle = System.Runtime.InteropServices.GCHandle.Alloc(pixels, System.Runtime.InteropServices.GCHandleType.Pinned);
            var ptr = System.Runtime.InteropServices.Marshal.UnsafeAddrOfPinnedArrayElement(pixels, 0);

            uploadHeapBuffer.WriteToSubresource(0, null, ptr, rowPitch, pixels.Length);

            handle.Free();

            Manager.ID3D12GraphicsCommandList.CopyTextureRegion(
                new SharpDX.Direct3D12.TextureCopyLocation(resource, 0), 0, 0, 0,
                new SharpDX.Direct3D12.TextureCopyLocation(uploadHeapBuffer, 0), null);

            Manager.ID3D12GraphicsCommandList.ResourceBarrierTransition(resource,
                 SharpDX.Direct3D12.ResourceStates.CopyDestination, SharpDX.Direct3D12.ResourceStates.PixelShaderResource);


            size = imagesize;

            resourceview = new SharpDX.Direct3D12.ShaderResourceViewDescription()
            {
                Format = resource.Description.Format,
                Dimension = SharpDX.Direct3D12.ShaderResourceViewDimension.Texture2D,
                Texture2D = { MipLevels = 1 }
            };

            SharpDX.Utilities.Dispose(ref decoder);
            SharpDX.Utilities.Dispose(ref converter);
            SharpDX.Utilities.Dispose(ref frame);
            SharpDX.Utilities.Dispose(ref uploadHeapBuffer);
        }

        public int Width => width;
        public int Height => height;
    }
}
