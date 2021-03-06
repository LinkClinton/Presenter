﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presenter
{
    public class Texture3D : ShaderResource
    {
        private int tWidth;
        private int tHeight;
        private int tDepth;
        private int mipLevels;
        private int rowPitch;
        private int depthPitch;

        public Texture3D(int width, int height, int depth, ResourceFormat format, int miplevels = 1)
        {
            tWidth = width;
            tHeight = height;
            tDepth = depth;
            pixelFormat = format;
            mipLevels = miplevels;

            rowPitch = ResourceFormatCounter.CountFormatSize(pixelFormat) * tWidth;
            depthPitch = rowPitch * tHeight;

            resource = Engine.ID3D12Device.CreateCommittedResource(
              new SharpDX.Direct3D12.HeapProperties(SharpDX.Direct3D12.HeapType.Default),
               SharpDX.Direct3D12.HeapFlags.None, SharpDX.Direct3D12.ResourceDescription.Texture3D(
                   (SharpDX.DXGI.Format)pixelFormat, tWidth, tHeight, (short)tDepth, (short)mipLevels),
                SharpDX.Direct3D12.ResourceStates.NonPixelShaderResource); 

            resourceview = new SharpDX.Direct3D12.ShaderResourceViewDescription()
            {
                Shader4ComponentMapping = DefaultComponentMapping(),
                Dimension = SharpDX.Direct3D12.ShaderResourceViewDimension.Texture3D,
                Format = resource.Description.Format,
                Texture3D = new SharpDX.Direct3D12.ShaderResourceViewDescription.Texture3DResource() { MipLevels = mipLevels }
            };

            size = ResourceFormatCounter.CountFormatSize(pixelFormat) * tWidth * tHeight * tDepth;
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
            Engine.ResourceCommandAllocator.Reset();

            using (var CommandList = Engine.ID3D12Device.CreateCommandList(SharpDX.Direct3D12.CommandListType.Direct,
                 Engine.ResourceCommandAllocator, null))
            {
                CommandList.ResourceBarrierTransition(resource, SharpDX.Direct3D12.ResourceStates.NonPixelShaderResource,
                     SharpDX.Direct3D12.ResourceStates.CopyDestination);

                using (var uploadBuffer = Engine.ID3D12Device.CreateCommittedResource(new SharpDX.Direct3D12.HeapProperties(
                 SharpDX.Direct3D12.CpuPageProperty.WriteBack, SharpDX.Direct3D12.MemoryPool.L0),
                  SharpDX.Direct3D12.HeapFlags.None, SharpDX.Direct3D12.ResourceDescription.Texture3D(
                       resource.Description.Format, tWidth, tHeight, (short)tDepth), SharpDX.Direct3D12.ResourceStates.GenericRead))
                {
                    uploadBuffer.WriteToSubresource(0, null, data, rowPitch, depthPitch);

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

        public int Width => tWidth;

        public int Height => tHeight;

        public int Depth => tDepth;

        public int MipLevels => mipLevels;
    }
}
