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

        public Texture1D(int width, ResourceFormat format, int miplevels = 1)
        {
            tWidth = width;
            pixelFormat = format;
            mipLevels = miplevels;

            resource = Manager.ID3D12Device.CreateCommittedResource(
                new SharpDX.Direct3D12.HeapProperties(SharpDX.Direct3D12.HeapType.Default),
                 SharpDX.Direct3D12.HeapFlags.None, SharpDX.Direct3D12.ResourceDescription.Texture1D(
                     (SharpDX.DXGI.Format)pixelFormat, width, 1, (short)mipLevels),
                  SharpDX.Direct3D12.ResourceStates.NonPixelShaderResource);

            resourceview = new SharpDX.Direct3D12.ShaderResourceViewDescription()
            {
                Shader4ComponentMapping = DefaultComponentMapping(),
                Dimension = SharpDX.Direct3D12.ShaderResourceViewDimension.Texture1D,
                Format = resource.Description.Format,
                Texture1D = new SharpDX.Direct3D12.ShaderResourceViewDescription.Texture1DResource() { MipLevels = mipLevels }
            };

            size = ResourceFormatCounter.CountFormatSize(pixelFormat) * tWidth;
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
                  SharpDX.Direct3D12.HeapFlags.None, SharpDX.Direct3D12.ResourceDescription.Texture1D(
                       resource.Description.Format, tWidth), SharpDX.Direct3D12.ResourceStates.GenericRead))
                {
                    var handle = System.Runtime.InteropServices.GCHandle.Alloc(data, System.Runtime.InteropServices.GCHandleType.Pinned);
                    IntPtr ptr = IntPtr.Zero;
                    System.Runtime.InteropServices.Marshal.StructureToPtr(data, ptr, false);

                    int size = ResourceFormatCounter.CountFormatSize(pixelFormat) * tWidth;

                    uploadBuffer.WriteToSubresource(0, null, ptr, size, size);

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
                  SharpDX.Direct3D12.HeapFlags.None, SharpDX.Direct3D12.ResourceDescription.Texture1D(
                       resource.Description.Format, tWidth), SharpDX.Direct3D12.ResourceStates.GenericRead))
                {
                    var handle = System.Runtime.InteropServices.GCHandle.Alloc(data, System.Runtime.InteropServices.GCHandleType.Pinned);
                    IntPtr ptr = System.Runtime.InteropServices.Marshal.UnsafeAddrOfPinnedArrayElement(data, 0);

                    int size = ResourceFormatCounter.CountFormatSize(pixelFormat) * tWidth;

                    uploadBuffer.WriteToSubresource(0, null, ptr, size, size);

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
                  SharpDX.Direct3D12.HeapFlags.None, SharpDX.Direct3D12.ResourceDescription.Texture1D(
                       resource.Description.Format, tWidth), SharpDX.Direct3D12.ResourceStates.GenericRead))
                {
                    int size = ResourceFormatCounter.CountFormatSize(pixelFormat) * tWidth;

                    uploadBuffer.WriteToSubresource(0, null, data, size, size);

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

        public int Width => throw new NotImplementedException();

        public int MipLevels => throw new NotImplementedException();
    }
}
