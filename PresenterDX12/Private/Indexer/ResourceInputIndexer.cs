using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presenter
{
    public class ResourceInputIndexer
    {
        public object this[int index]
        {
            set
            {
                switch (value)
                {
                    case Buffer buffer:
                        GraphicsPipeline.ID3D12GraphicsCommandList.SetGraphicsRootConstantBufferView(index,
                            buffer.ID3D12Resource.GPUVirtualAddress);
                        break;
                    case ShaderResource shaderResource:
                        GraphicsPipeline.ID3D12GraphicsCommandList.SetGraphicsRootShaderResourceView(index,
                            shaderResource.ID3D12Resource.GPUVirtualAddress);
                        break;
                    case ResourceTable resourceTable:
                        GraphicsPipeline.ID3D12GraphicsCommandList.SetGraphicsRootDescriptorTable(index,
                            resourceTable.ID3D12GpuDescriptorHandle);
                        break;
                    default:
                        break;
                }
            }
        }
    }

    public static partial class GraphicsPipeline
    {
        private static ResourceInputIndexer resouceInput = new ResourceInputIndexer();

        public static ResourceInputIndexer InputSlot => resouceInput;

        public static void SetHeaps(ResourceHeap[] heaps)
        {
            SharpDX.Direct3D12.DescriptorHeap[] dx12heaps = new SharpDX.Direct3D12.DescriptorHeap[heaps.Length];

            for (int i = 0; i < heaps.Length; i++)
            {
                dx12heaps[i] = heaps[i].ID3D12DescriptorHeap;
            }

            ID3D12GraphicsCommandList.SetDescriptorHeaps(dx12heaps);
        }
    }

}
