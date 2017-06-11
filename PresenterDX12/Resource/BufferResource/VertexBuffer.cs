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
                  new SharpDX.Direct3D12.HeapProperties(SharpDX.Direct3D12.HeapType.Default),
                  SharpDX.Direct3D12.HeapFlags.None, SharpDX.Direct3D12.ResourceDescription.
                  Buffer(size = (SharpDX.Utilities.SizeOf<T>() * vertices.Length)),
                  SharpDX.Direct3D12.ResourceStates.GenericRead);

            Update(vertices);

            count = vertices.Length;
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

    public static partial class Manager
    {
        private static Buffer vertexBuffer;

        public static Buffer VertexBuffer
        {
            get => vertexBuffer;
            set
            {
                vertexBuffer = value;

                SharpDX.Direct3D12.VertexBufferView bufferview = new SharpDX.Direct3D12.VertexBufferView()
                {
                    BufferLocation = vertexBuffer.ID3D12Resource.GPUVirtualAddress,
                    SizeInBytes = vertexBuffer.Size,
                    StrideInBytes = vertexBuffer.Size / vertexBuffer.Count
                };

                ID3D12GraphicsCommandList.SetVertexBuffer(0, bufferview);
            }
        }
    }

}
