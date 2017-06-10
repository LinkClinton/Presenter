using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presenter
{
    public abstract class Buffer : Resource, IBuffer
    {
        protected int count;

        public int Count => count;

        protected void UpdateDefaultBuffer<T>(ref T data) where T : struct
        {
            using (var CommandList = Manager.ID3D12Device.CreateCommandList(SharpDX.Direct3D12.CommandListType.Direct,
                Manager.ID3D12CommandAllocator, null))
            {
                CommandList.ResourceBarrierTransition(resource, SharpDX.Direct3D12.ResourceStates.GenericRead,
                     SharpDX.Direct3D12.ResourceStates.CopyDestination);

                using (var uploadBuffer = Manager.ID3D12Device.CreateCommittedResource(new SharpDX.Direct3D12.HeapProperties(
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

                    Manager.ID3D12CommandQueue.ExecuteCommandList(CommandList);

                    Manager.WaitForFrame();
                }
            }
        }

        protected void UpdateDefaultBuffer<T>(T[] data) where T : struct
        {
            using (var CommandList = Manager.ID3D12Device.CreateCommandList(SharpDX.Direct3D12.CommandListType.Direct,
              Manager.ID3D12CommandAllocator, null))
            {
                CommandList.ResourceBarrierTransition(resource, SharpDX.Direct3D12.ResourceStates.GenericRead,
                     SharpDX.Direct3D12.ResourceStates.CopyDestination);

                using (var uploadBuffer = Manager.ID3D12Device.CreateCommittedResource(new SharpDX.Direct3D12.HeapProperties(
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

                    Manager.ID3D12CommandQueue.ExecuteCommandList(CommandList);

                    Manager.WaitForFrame();
                }
            }
        }
    }

    public static partial class Manager
    {
        public static void DrawObject(int vertexCount, int startLocation = 0,
           PrimitiveType type = PrimitiveType.TriangleList)
        {
            ID3D12GraphicsCommandList.PrimitiveTopology = (SharpDX.Direct3D.PrimitiveTopology)type;

            ID3D12GraphicsCommandList.DrawInstanced(vertexCount, 1, startLocation, 0);
        }

        public static void DrawObjectIndexed(int indexCount, int startLocation = 0,
            int baseVertexLocation = 0, PrimitiveType type = PrimitiveType.TriangleList)
        {
            ID3D12GraphicsCommandList.PrimitiveTopology = (SharpDX.Direct3D.PrimitiveTopology)type;

            ID3D12GraphicsCommandList.DrawIndexedInstanced(indexCount, 1, startLocation, baseVertexLocation, 0);
        }
    }
}
