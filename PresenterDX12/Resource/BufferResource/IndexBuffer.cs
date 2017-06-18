using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presenter
{
    public class IndexBuffer<T> : Buffer where T : struct
    {
        public IndexBuffer(T[] indices)
        {
            resource = Engine.ID3D12Device.CreateCommittedResource(
                   new SharpDX.Direct3D12.HeapProperties(SharpDX.Direct3D12.HeapType.Default),
                   SharpDX.Direct3D12.HeapFlags.None, SharpDX.Direct3D12.ResourceDescription.
                   Buffer(size = (SharpDX.Utilities.SizeOf<T>() * indices.Length)),
                   SharpDX.Direct3D12.ResourceStates.GenericRead);

            Update(indices);

            count = indices.Length;
        }

#pragma warning disable CS0693 // Type parameter has the same name as the type parameter from outer type
        public override void Update<T>(ref T data)
#pragma warning restore CS0693 // Type parameter has the same name as the type parameter from outer type
        {
            UpdateDefaultBuffer(ref data);
        }

#pragma warning disable CS0693 // Type parameter has the same name as the type parameter from outer type
        public override void Update<T>(T[] data)
#pragma warning restore CS0693 // Type parameter has the same name as the type parameter from outer type
        {
            UpdateDefaultBuffer(data);
        }

        public override void Update(IntPtr data)
        {
            UpdateDefaultBuffer(data);
        }
    }

}
