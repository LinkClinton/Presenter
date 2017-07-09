using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presenter
{
    public class ResourceHeap
    {
        private static int resourceHeapSize;
        private static int renderTargetHeapSize;
        private static int depthStencilHeapSize;

        private SharpDX.Direct3D12.DescriptorHeap heap;
        private int heapCount;

        static ResourceHeap()
        {
            resourceHeapSize = Engine.ID3D12Device.GetDescriptorHandleIncrementSize(
                SharpDX.Direct3D12.DescriptorHeapType.ConstantBufferViewShaderResourceViewUnorderedAccessView);

            renderTargetHeapSize = Engine.ID3D12Device.GetDescriptorHandleIncrementSize(SharpDX.Direct3D12.DescriptorHeapType.RenderTargetView);
            depthStencilHeapSize = Engine.ID3D12Device.GetDescriptorHandleIncrementSize(SharpDX.Direct3D12.DescriptorHeapType.DepthStencilView);
        }

        public ResourceHeap(int count)
        {
            heap = Engine.ID3D12Device.CreateDescriptorHeap(
                new SharpDX.Direct3D12.DescriptorHeapDescription()
                {
                    DescriptorCount = count,
                    Flags = SharpDX.Direct3D12.DescriptorHeapFlags.ShaderVisible,
                    Type = SharpDX.Direct3D12.DescriptorHeapType.ConstantBufferViewShaderResourceViewUnorderedAccessView
                });

            heapCount = 0;
        }

        public void AddResource<T>(ConstantBuffer<T> constantBuffer) where T : struct
        {
            Engine.ID3D12Device.CreateConstantBufferView(constantBuffer.BufferView,
                        heap.CPUDescriptorHandleForHeapStart + heapCount * ResourceHeapSize);
            heapCount++;
        }

        public void AddResource(ShaderResource resource)
        {
            Engine.ID3D12Device.CreateShaderResourceView(resource.ID3D12Resource, resource.ShaderResourceView,
                heap.CPUDescriptorHandleForHeapStart + heapCount * ResourceHeapSize);
            heapCount++;
        }

        internal SharpDX.Direct3D12.DescriptorHeap ID3D12DescriptorHeap => heap;

        internal static int ResourceHeapSize => resourceHeapSize;
        internal static int RenderTargetHeapSize => renderTargetHeapSize;
        internal static int DepthStencilHeapSize => depthStencilHeapSize;

        public int Count => heapCount;

        public int MaxCount => heap.Description.DescriptorCount;
        
        ~ResourceHeap() => SharpDX.Utilities.Dispose(ref heap);
    }
}
