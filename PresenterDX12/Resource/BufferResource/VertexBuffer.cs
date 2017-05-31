using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presenter
{
    public class VertexBuffer<T> : Buffer, IVertexBuffer where T : struct
    {

        public VertexBuffer(T[] vertices)
        {
            resource = Manager.ID3D12Device.CreateCommittedResource(
                  new SharpDX.Direct3D12.HeapProperties(SharpDX.Direct3D12.HeapType.Upload),
                  SharpDX.Direct3D12.HeapFlags.None, SharpDX.Direct3D12.ResourceDescription.
                  Buffer(size = (SharpDX.Utilities.SizeOf<T>() * vertices.Length)),
                  SharpDX.Direct3D12.ResourceStates.GenericRead);

            resourceStart = resource.Map(0);

            Update(vertices);

            count = vertices.Length;
        }
    }
}
