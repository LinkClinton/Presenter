using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presenter
{
    public class IndexBuffer<T> : Buffer, IIndexBuffer where T : struct
    {
        private SharpDX.Direct3D12.IndexBufferView bufferview;

        public IndexBuffer(T[] indices)
        {
            resource = Manager.ID3D12Device.CreateCommittedResource(
                   new SharpDX.Direct3D12.HeapProperties(SharpDX.Direct3D12.HeapType.Upload),
                   SharpDX.Direct3D12.HeapFlags.None, SharpDX.Direct3D12.ResourceDescription.
                   Buffer(size = (SharpDX.Utilities.SizeOf<T>() * indices.Length)),
                   SharpDX.Direct3D12.ResourceStates.GenericRead);

            Update(indices);

            count = indices.Length;

            bufferview = new SharpDX.Direct3D12.IndexBufferView()
            {
                BufferLocation = resource.GPUVirtualAddress,
                SizeInBytes = Size,
                Format = SharpDX.DXGI.Format.R32_UInt
            };
        }

        internal SharpDX.Direct3D12.IndexBufferView IndexBufferView => bufferview;
    }
}
