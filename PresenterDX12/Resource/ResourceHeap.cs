﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presenter
{
    public class ResourceHeap : IResourceHeap
    {
        private SharpDX.Direct3D12.DescriptorHeap heap;
        private int heapSize;
        private int heapCount;

        public ResourceHeap(int count)
        {
            heap = Manager.ID3D12Device.CreateDescriptorHeap(
                new SharpDX.Direct3D12.DescriptorHeapDescription()
                {
                    DescriptorCount = count,
                    Flags = SharpDX.Direct3D12.DescriptorHeapFlags.ShaderVisible,
                    Type = SharpDX.Direct3D12.DescriptorHeapType.ConstantBufferViewShaderResourceViewUnorderedAccessView
                });

            heapSize = Manager.ID3D12Device.GetDescriptorHandleIncrementSize(
                SharpDX.Direct3D12.DescriptorHeapType.ConstantBufferViewShaderResourceViewUnorderedAccessView);

            heapCount = 0;
        }

        public void AddResource<T>(ConstantBuffer<T> constantBuffer) where T : struct
        {
            Manager.ID3D12Device.CreateConstantBufferView(constantBuffer.BufferView,
                        heap.CPUDescriptorHandleForHeapStart + heapCount * heapSize);
            heapCount++;
        }

        public void AddResource(ShaderResource resource)
        {
            Manager.ID3D12Device.CreateShaderResourceView(resource.ID3D12Resource, resource.ShaderResourceView,
                heap.CPUDescriptorHandleForHeapStart + heapCount * heapSize);
            heapCount++;
        }

        public int Count => heapCount;

        public int MaxCount => heap.Description.DescriptorCount;

        internal SharpDX.Direct3D12.DescriptorHeap ID3D12DescriptorHeap => heap;

        ~ResourceHeap() => SharpDX.Utilities.Dispose(ref heap);
    }
}
