using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presenter
{
    public class ResourceInputIndexer
    {
        private List<SharpDX.Direct3D12.DescriptorHeap> heapSet = new List<SharpDX.Direct3D12.DescriptorHeap>();

        internal ResourceInputIndexer()
        {

        }

        public object this[int index]
        { 
            set
            {
                switch (value)
                {
                    case Buffer buffer:
                        Manager.ID3D12GraphicsCommandList.SetGraphicsRootConstantBufferView(index,
                            buffer.ID3D12Resource.GPUVirtualAddress);
                        break;
                    case ShaderResource shaderresource:
                        Manager.ID3D12GraphicsCommandList.SetGraphicsRootConstantBufferView(index,
                            shaderresource.ID3D12Resource.GPUVirtualAddress);
                        break;
                    case ResourceHeap heap:
                        if (heapSet.Contains(heap.ID3D12DescriptorHeap) is false)
                        {
                            heapSet.Add(heap.ID3D12DescriptorHeap);
                            Manager.ID3D12GraphicsCommandList.SetDescriptorHeaps(heapSet.ToArray());
                        }
                        Manager.ID3D12GraphicsCommandList.SetGraphicsRootDescriptorTable(index,
                            heap.ID3D12DescriptorHeap.GPUDescriptorHandleForHeapStart);
                        break;
                    default:
                        break;
                }
            } 
        }
        
        public void Reset()
        {
            heapSet.Clear();
        }
    }

    public partial class ResourceLayout
    {
        private static ResourceInputIndexer resouceInput = new ResourceInputIndexer();

        public static ResourceInputIndexer InputSlot => resouceInput;
    }

}
