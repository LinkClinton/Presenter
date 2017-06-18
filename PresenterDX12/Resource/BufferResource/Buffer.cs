using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presenter
{
    public abstract class Buffer : Resource
    {
        protected int count;

        public int Count => count;

        protected void UpdateDefaultBuffer<T>(ref T data) where T : struct
        {
            using (var CommandList = Engine.ID3D12Device.CreateCommandList(SharpDX.Direct3D12.CommandListType.Direct,
                Engine.ID3D12CommandAllocator, null))
            {
                CommandList.ResourceBarrierTransition(resource, SharpDX.Direct3D12.ResourceStates.GenericRead,
                     SharpDX.Direct3D12.ResourceStates.CopyDestination);

                using (var uploadBuffer = Engine.ID3D12Device.CreateCommittedResource(new SharpDX.Direct3D12.HeapProperties(
                     SharpDX.Direct3D12.HeapType.Upload), SharpDX.Direct3D12.HeapFlags.None,
                     SharpDX.Direct3D12.ResourceDescription.Buffer(size),
                   SharpDX.Direct3D12.ResourceStates.GenericRead))
                {
                    var ptr = uploadBuffer.Map(0);

                    SharpDX.Utilities.Write(ptr, ref data);

                    uploadBuffer.Unmap(0);

                    CommandList.CopyResource(resource, uploadBuffer);

                    CommandList.ResourceBarrierTransition(resource,
                         SharpDX.Direct3D12.ResourceStates.CopyDestination, SharpDX.Direct3D12.ResourceStates.GenericRead);

                    CommandList.Close();

                    Engine.ID3D12CommandQueue.ExecuteCommandList(CommandList);

                    Engine.Wait();
                }
            }
        }

        protected void UpdateDefaultBuffer<T>(T[] data) where T : struct
        {
            using (var CommandList = Engine.ID3D12Device.CreateCommandList(SharpDX.Direct3D12.CommandListType.Direct,
              Engine.ID3D12CommandAllocator, null))
            {
                CommandList.ResourceBarrierTransition(resource, SharpDX.Direct3D12.ResourceStates.GenericRead,
                     SharpDX.Direct3D12.ResourceStates.CopyDestination);

                using (var uploadBuffer = Engine.ID3D12Device.CreateCommittedResource(new SharpDX.Direct3D12.HeapProperties(
                     SharpDX.Direct3D12.HeapType.Upload), SharpDX.Direct3D12.HeapFlags.None,
                     SharpDX.Direct3D12.ResourceDescription.Buffer(size), SharpDX.Direct3D12.ResourceStates.GenericRead))
                {
                    var ptr = uploadBuffer.Map(0);

                    SharpDX.Utilities.Write(ptr, data, 0, data.Length);

                    uploadBuffer.Unmap(0);

                    CommandList.CopyResource(resource, uploadBuffer);

                    CommandList.ResourceBarrierTransition(resource,
                         SharpDX.Direct3D12.ResourceStates.CopyDestination, SharpDX.Direct3D12.ResourceStates.GenericRead);

                    CommandList.Close();

                    Engine.ID3D12CommandQueue.ExecuteCommandList(CommandList);

                    Engine.Wait();
                }
            }
        }

        protected void UpdateDefaultBuffer(IntPtr data)
        {
            using (var CommandList = Engine.ID3D12Device.CreateCommandList(SharpDX.Direct3D12.CommandListType.Direct,
                Engine.ID3D12CommandAllocator, null))
            {
                CommandList.ResourceBarrierTransition(resource, SharpDX.Direct3D12.ResourceStates.GenericRead,
                     SharpDX.Direct3D12.ResourceStates.CopyDestination);

                using (var uploadBuffer = Engine.ID3D12Device.CreateCommittedResource(new SharpDX.Direct3D12.HeapProperties(
                     SharpDX.Direct3D12.HeapType.Upload), SharpDX.Direct3D12.HeapFlags.None,
                     SharpDX.Direct3D12.ResourceDescription.Buffer(size), SharpDX.Direct3D12.ResourceStates.GenericRead))
                {
                    var ptr = uploadBuffer.Map(0);

                    SharpDX.Utilities.CopyMemory(ptr, data, size);

                    uploadBuffer.Unmap(0);

                    CommandList.CopyResource(resource, uploadBuffer);

                    CommandList.ResourceBarrierTransition(resource,
                         SharpDX.Direct3D12.ResourceStates.CopyDestination, SharpDX.Direct3D12.ResourceStates.GenericRead);

                    CommandList.Close();

                    Engine.ID3D12CommandQueue.ExecuteCommandList(CommandList);

                    Engine.Wait();
                }
            }
        }
    }

    public static partial class GraphicsPipeline
    {
        public static void PutObject(int vertexCount, int startLocation = 0)
        {
            ID3D12GraphicsCommandList.DrawInstanced(vertexCount, 1, startLocation, 0);
        }

        public static void PutObjectIndexed(int indexCount, int startLocation = 0,
            int baseVertexLocation = 0)
        {
            ID3D12GraphicsCommandList.DrawIndexedInstanced(indexCount, 1, startLocation, baseVertexLocation, 0);
        }
    }
}
