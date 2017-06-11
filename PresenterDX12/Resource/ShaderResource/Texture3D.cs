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

            resource = Manager.ID3D12Device.CreateCommittedResource(
              new SharpDX.Direct3D12.HeapProperties(SharpDX.Direct3D12.HeapType.Default),
               SharpDX.Direct3D12.HeapFlags.None, SharpDX.Direct3D12.ResourceDescription.Texture3D(
                   (SharpDX.DXGI.Format)pixelFormat, tWidth, tHeight, (short)tDepth, (short)mipLevels),
                SharpDX.Direct3D12.ResourceStates.NonPixelShaderResource); 

            resourceview = new SharpDX.Direct3D12.ShaderResourceViewDescription()
            {
                Shader4ComponentMapping = DefaultComponentMapping(),
                Dimension = SharpDX.Direct3D12.ShaderResourceViewDimension.Texture3D,
                Format = resource.Description.Format,
                Texture1D = new SharpDX.Direct3D12.ShaderResourceViewDescription.Texture1DResource() { MipLevels = mipLevels }
            };

            size = ResourceFormatCounter.CountFormatSize(pixelFormat) * tWidth * tHeight * tDepth;
        }

        public override void Update<T>(ref T data)
        {
            using (var CommandList = Manager.ID3D12Device.CreateCommandList(SharpDX.Direct3D12.CommandListType.Direct,
                Manager.ID3D12CommandAllocator, null))
            {
                CommandList.ResourceBarrierTransition(resource, SharpDX.Direct3D12.ResourceStates.NonPixelShaderResource,
                     SharpDX.Direct3D12.ResourceStates.CopyDestination);

                using (var uploadBuffer = Manager.ID3D12Device.CreateCommittedResource(new SharpDX.Direct3D12.HeapProperties(
                 SharpDX.Direct3D12.CpuPageProperty.WriteBack, SharpDX.Direct3D12.MemoryPool.L0),
                  SharpDX.Direct3D12.HeapFlags.None, SharpDX.Direct3D12.ResourceDescription.Texture3D(
                       resource.Description.Format, tWidth, tHeight,(short)tDepth), SharpDX.Direct3D12.ResourceStates.GenericRead))
                {
                    var handle = System.Runtime.InteropServices.GCHandle.Alloc(data, System.Runtime.InteropServices.GCHandleType.Pinned);
                    IntPtr ptr = IntPtr.Zero;
                    System.Runtime.InteropServices.Marshal.StructureToPtr(data, ptr, false);

                    int rowPitch = ResourceFormatCounter.CountFormatSize(pixelFormat) * tWidth;
                    int depthPitch = rowPitch * Height;
                    int size = depthPitch * depthPitch;

                    uploadBuffer.WriteToSubresource(0, null, ptr, rowPitch, depthPitch);

                    handle.Free();

                    CommandList.CopyTextureRegion(
                    new SharpDX.Direct3D12.TextureCopyLocation(resource, 0), 0, 0, 0,
                    new SharpDX.Direct3D12.TextureCopyLocation(uploadBuffer, 0), null);

                    CommandList.ResourceBarrierTransition(resource,
                         SharpDX.Direct3D12.ResourceStates.CopyDestination, SharpDX.Direct3D12.ResourceStates.NonPixelShaderResource);

                    CommandList.Close();

                    Manager.ID3D12CommandQueue.ExecuteCommandList(CommandList);

                    Manager.WaitForFrame();
                }
            }
        }

        public override void Update<T>(T[] data)
        {
            using (var CommandList = Manager.ID3D12Device.CreateCommandList(SharpDX.Direct3D12.CommandListType.Direct,
               Manager.ID3D12CommandAllocator, null))
            {
                CommandList.ResourceBarrierTransition(resource, SharpDX.Direct3D12.ResourceStates.NonPixelShaderResource,
                     SharpDX.Direct3D12.ResourceStates.CopyDestination);

                using (var uploadBuffer = Manager.ID3D12Device.CreateCommittedResource(new SharpDX.Direct3D12.HeapProperties(
                 SharpDX.Direct3D12.CpuPageProperty.WriteBack, SharpDX.Direct3D12.MemoryPool.L0),
                  SharpDX.Direct3D12.HeapFlags.None, SharpDX.Direct3D12.ResourceDescription.Texture3D(
                       resource.Description.Format, tWidth, tHeight, (short)tDepth), SharpDX.Direct3D12.ResourceStates.GenericRead))
                {
                    var handle = System.Runtime.InteropServices.GCHandle.Alloc(data, System.Runtime.InteropServices.GCHandleType.Pinned);
                    IntPtr ptr = System.Runtime.InteropServices.Marshal.UnsafeAddrOfPinnedArrayElement(data, 0);

                    int rowPitch = ResourceFormatCounter.CountFormatSize(pixelFormat) * tWidth;
                    int depthPitch = rowPitch * tHeight;
                    int size = depthPitch * tDepth;

                    uploadBuffer.WriteToSubresource(0, null, ptr, rowPitch, depthPitch);

                    handle.Free();

                    CommandList.CopyTextureRegion(
                    new SharpDX.Direct3D12.TextureCopyLocation(resource, 0), 0, 0, 0,
                    new SharpDX.Direct3D12.TextureCopyLocation(uploadBuffer, 0), null);

                    CommandList.ResourceBarrierTransition(resource,
                         SharpDX.Direct3D12.ResourceStates.CopyDestination, SharpDX.Direct3D12.ResourceStates.NonPixelShaderResource);

                    CommandList.Close();

                    Manager.ID3D12CommandQueue.ExecuteCommandList(CommandList);

                    Manager.WaitForFrame();
                }
            }
        }

        public override void Update(IntPtr data)
        {
            using (var CommandList = Manager.ID3D12Device.CreateCommandList(SharpDX.Direct3D12.CommandListType.Direct,
                 Manager.ID3D12CommandAllocator, null))
            {
                CommandList.ResourceBarrierTransition(resource, SharpDX.Direct3D12.ResourceStates.NonPixelShaderResource,
                     SharpDX.Direct3D12.ResourceStates.CopyDestination);

                using (var uploadBuffer = Manager.ID3D12Device.CreateCommittedResource(new SharpDX.Direct3D12.HeapProperties(
                 SharpDX.Direct3D12.CpuPageProperty.WriteBack, SharpDX.Direct3D12.MemoryPool.L0),
                  SharpDX.Direct3D12.HeapFlags.None, SharpDX.Direct3D12.ResourceDescription.Texture3D(
                       resource.Description.Format, tWidth, tHeight, (short)tDepth), SharpDX.Direct3D12.ResourceStates.GenericRead))
                {
                    int rowPitch = ResourceFormatCounter.CountFormatSize(pixelFormat) * tWidth;
                    int depthPitch = rowPitch * tHeight;
                    int size = depthPitch * tDepth;

                    uploadBuffer.WriteToSubresource(0, null, data, rowPitch, depthPitch);

                    CommandList.CopyTextureRegion(
                    new SharpDX.Direct3D12.TextureCopyLocation(resource, 0), 0, 0, 0,
                    new SharpDX.Direct3D12.TextureCopyLocation(uploadBuffer, 0), null);

                    CommandList.ResourceBarrierTransition(resource,
                         SharpDX.Direct3D12.ResourceStates.CopyDestination, SharpDX.Direct3D12.ResourceStates.NonPixelShaderResource);

                    CommandList.Close();

                    Manager.ID3D12CommandQueue.ExecuteCommandList(CommandList);

                    Manager.WaitForFrame();
                }
            }
        }

        public int Width => tWidth;

        public int Height => tHeight;

        public int Depth => tDepth;

        public int MipLevels => mipLevels;
    }
}
