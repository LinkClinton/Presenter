using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presenter
{
    class IndexBuffer<T> : Buffer, IIndexBuffer where T : struct
    {
        public IndexBuffer(T[] indices)
        {
            resource = Manager.ID3D12Device.CreateCommittedResource(
                   new SharpDX.Direct3D12.HeapProperties(SharpDX.Direct3D12.HeapType.Upload),
                   SharpDX.Direct3D12.HeapFlags.None, SharpDX.Direct3D12.ResourceDescription.
                   Buffer(size = (SharpDX.Utilities.SizeOf<T>() * indices.Length)),
                   SharpDX.Direct3D12.ResourceStates.GenericRead);

            Update(indices);

            count = indices.Length;
        }
    }
}
