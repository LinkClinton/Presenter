using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presenter
{
    public class ResourceTable
    {
        private SharpDX.Direct3D12.GpuDescriptorHandle gpuHandle;

        private int startPos;

        public ResourceTable(ResourceHeap heap, int start)
        {
            gpuHandle = heap.ID3D12DescriptorHeap.GPUDescriptorHandleForHeapStart + start * ResourceHeap.ResourceHeapSize;
            startPos = start;
        }

        public int Start => startPos;

        internal SharpDX.Direct3D12.GpuDescriptorHandle ID3D12GpuDescriptorHandle
            => gpuHandle;
    }
}
