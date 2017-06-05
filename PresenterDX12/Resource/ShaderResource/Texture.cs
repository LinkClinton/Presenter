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

        private const int ComponentMappingMask = 0x7;
        private const int ComponentMappingShift = 3;
        private const int ComponentMappingAlwaysSetBitAvoidingZeromemMistakes = (1 << (ComponentMappingShift * 4));

        private static int ComponentMapping(int src0, int src1, int src2, int src3)
        {
            return ((((src0) & ComponentMappingMask) |
            (((src1) & ComponentMappingMask) << ComponentMappingShift) |
            (((src2) & ComponentMappingMask) << (ComponentMappingShift * 2)) |
            (((src3) & ComponentMappingMask) << (ComponentMappingShift * 3)) |
            ComponentMappingAlwaysSetBitAvoidingZeromemMistakes));
        }

        private static int DefaultComponentMapping()
        {
            return ComponentMapping(0, 1, 2, 3);
        }

        private static int ComponentMapping(int ComponentToExtract, int Mapping)
        {
            return ((Mapping >> (ComponentMappingShift * ComponentToExtract) & ComponentMappingMask));
        }

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

            SharpDX.Direct3D12.ResourceDescription resourceDesc = SharpDX.Direct3D12.ResourceDescription.
                Texture2D(WICHelper.Translate[converter.PixelFormat], width, height);

            resource = Manager.ID3D12Device.CreateCommittedResource(
                new SharpDX.Direct3D12.HeapProperties(SharpDX.Direct3D12.HeapType.Default),
                SharpDX.Direct3D12.HeapFlags.None, resourceDesc,
                SharpDX.Direct3D12.ResourceStates.CopyDestination);

            var uploadHeapBuffer = Manager.ID3D12Device.CreateCommittedResource(new SharpDX.Direct3D12.HeapProperties(
                 SharpDX.Direct3D12.CpuPageProperty.WriteBack, SharpDX.Direct3D12.MemoryPool.L0),
                  SharpDX.Direct3D12.HeapFlags.None, SharpDX.Direct3D12.ResourceDescription.Texture2D(
                       resource.Description.Format, width, height), SharpDX.Direct3D12.ResourceStates.GenericRead);

            var handle = System.Runtime.InteropServices.GCHandle.Alloc(pixels, System.Runtime.InteropServices.GCHandleType.Pinned);
            var ptr = System.Runtime.InteropServices.Marshal.UnsafeAddrOfPinnedArrayElement(pixels, 0);

            uploadHeapBuffer.WriteToSubresource(0, null, ptr, rowPitch, pixels.Length);

            handle.Free();

            using (var CommandList = Manager.ID3D12Device.CreateCommandList(SharpDX.Direct3D12.CommandListType.Direct,
                 Manager.ID3D12CommandAllocator, null))
            {
                CommandList.CopyTextureRegion(
                    new SharpDX.Direct3D12.TextureCopyLocation(resource, 0), 0, 0, 0,
                    new SharpDX.Direct3D12.TextureCopyLocation(uploadHeapBuffer, 0), null);

                CommandList.ResourceBarrierTransition(resource,
                     SharpDX.Direct3D12.ResourceStates.CopyDestination, SharpDX.Direct3D12.ResourceStates.PixelShaderResource);

                CommandList.Close();
                
                Manager.ID3D12CommandQueue.ExecuteCommandList(CommandList);
            }
            
            size = imagesize;

            resourceview = new SharpDX.Direct3D12.ShaderResourceViewDescription()
            {
                Shader4ComponentMapping = DefaultComponentMapping(),
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
