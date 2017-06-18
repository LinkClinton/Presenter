using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presenter
{
    public class Texture2D : ShaderResource
    {
        private int tWidth;
        private int tHeight;
        private int mipLevels;
        private int rowPitch;

        public Texture2D(int width, int height, ResourceFormat format, int miplevels = 1)
        {
            tWidth = width;
            tHeight = height;
            pixelFormat = format;
            mipLevels = miplevels;

            rowPitch = ResourceFormatCounter.CountFormatSize(pixelFormat) * tWidth;

            resource = Engine.ID3D12Device.CreateCommittedResource(
              new SharpDX.Direct3D12.HeapProperties(SharpDX.Direct3D12.HeapType.Default),
               SharpDX.Direct3D12.HeapFlags.None, SharpDX.Direct3D12.ResourceDescription.Texture2D(
                   (SharpDX.DXGI.Format)pixelFormat, width, height, 1, (short)mipLevels),
                SharpDX.Direct3D12.ResourceStates.NonPixelShaderResource);

            resourceview = new SharpDX.Direct3D12.ShaderResourceViewDescription()
            {
                Shader4ComponentMapping = DefaultComponentMapping(),
                Dimension = SharpDX.Direct3D12.ShaderResourceViewDimension.Texture2D,
                Format = resource.Description.Format,
                Texture2D = new SharpDX.Direct3D12.ShaderResourceViewDescription.Texture2DResource() { MipLevels = miplevels }
            };

            size = ResourceFormatCounter.CountFormatSize(pixelFormat) * tWidth * tHeight;
        }

        public override void Update<T>(ref T data)
        {
            var handle = System.Runtime.InteropServices.GCHandle.Alloc(data, System.Runtime.InteropServices.GCHandleType.Pinned);
            IntPtr ptr = IntPtr.Zero;
            System.Runtime.InteropServices.Marshal.StructureToPtr(data, ptr, false);

            Update(ptr);

            handle.Free();
        }

        public override void Update<T>(T[] data)
        {
            var handle = System.Runtime.InteropServices.GCHandle.Alloc(data, System.Runtime.InteropServices.GCHandleType.Pinned);
            IntPtr ptr = System.Runtime.InteropServices.Marshal.UnsafeAddrOfPinnedArrayElement(data, 0);

            Update(ptr);

            handle.Free();
        }

        public override void Update(IntPtr data)
        {
            using (var CommandList = Engine.ID3D12Device.CreateCommandList(SharpDX.Direct3D12.CommandListType.Direct,
                Engine.ID3D12CommandAllocator, null))
            {
                CommandList.ResourceBarrierTransition(resource, SharpDX.Direct3D12.ResourceStates.NonPixelShaderResource,
                     SharpDX.Direct3D12.ResourceStates.CopyDestination);

                using (var uploadBuffer = Engine.ID3D12Device.CreateCommittedResource(new SharpDX.Direct3D12.HeapProperties(
                 SharpDX.Direct3D12.CpuPageProperty.WriteBack, SharpDX.Direct3D12.MemoryPool.L0),
                  SharpDX.Direct3D12.HeapFlags.None, SharpDX.Direct3D12.ResourceDescription.Texture2D(
                       resource.Description.Format, tWidth, tHeight), SharpDX.Direct3D12.ResourceStates.GenericRead))
                {
                    uploadBuffer.WriteToSubresource(0, null, data, rowPitch, size);

                    CommandList.CopyTextureRegion(
                    new SharpDX.Direct3D12.TextureCopyLocation(resource, 0), 0, 0, 0,
                    new SharpDX.Direct3D12.TextureCopyLocation(uploadBuffer, 0), null);

                    CommandList.ResourceBarrierTransition(resource,
                         SharpDX.Direct3D12.ResourceStates.CopyDestination, SharpDX.Direct3D12.ResourceStates.NonPixelShaderResource);

                    CommandList.Close();

                    Engine.ID3D12CommandQueue.ExecuteCommandList(CommandList);

                    Engine.Wait();
                }
            }
        }

        public static Texture2D FromFile(string filename, int miplevels = 1)
        {   
            using (var decoder = new SharpDX.WIC.BitmapDecoder(Engine.ImagingFactory,
                filename, SharpDX.IO.NativeFileAccess.Read, SharpDX.WIC.DecodeOptions.CacheOnLoad))
            {

                using (var converter = new SharpDX.WIC.FormatConverter(Engine.ImagingFactory))
                {

                    using (var frame = decoder.GetFrame(0))
                    {

                        converter.Initialize(frame, frame.PixelFormat,
                             SharpDX.WIC.BitmapDitherType.None, null, 0, SharpDX.WIC.BitmapPaletteType.MedianCut);

                        int width = converter.Size.Width;
                        int height = converter.Size.Height;

                        int bpp = SharpDX.WIC.PixelFormat.GetBitsPerPixel(converter.PixelFormat);

                        int rowPitch = (width * bpp + 7) / 8;
                        int imagesize = rowPitch * height;

                        byte[] pixels = new byte[imagesize];

                        converter.CopyPixels(pixels, rowPitch);

                        Texture2D texture = new Texture2D(width, height, (ResourceFormat)WICHelper.Translate[converter.PixelFormat],
                            miplevels);

                        texture.Update(pixels);

                        return texture;
                    }
                }
            }
        }

        public int Width => tWidth;

        public int Height => tHeight;

        public int MipLevels => mipLevels;
    }
}
