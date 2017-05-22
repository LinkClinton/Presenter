using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace Presenter
{
    public class VertexBuffer<T> : Buffer where T : struct
    {
        public VertexBuffer(T[] vertices)
        {
            buffer = new SharpDX.Direct3D11.Buffer(Manager.ID3D11Device,
               size = Marshal.SizeOf<T>() * vertices.Length, SharpDX.Direct3D11.ResourceUsage.Default,
               SharpDX.Direct3D11.BindFlags.ConstantBuffer, SharpDX.Direct3D11.CpuAccessFlags.None,
               SharpDX.Direct3D11.ResourceOptionFlags.None, 0);

            Update(vertices);

            count = vertices.Length;
        }
    }

    public static partial class Manager 
    {
        private static Buffer vertexbuffer;

        public static Buffer VertexBuffer
        {
            get => vertexbuffer;
            set
            {
                vertexbuffer = value;

                ID3D11DeviceContext.InputAssembler.SetVertexBuffers(0,
                    new SharpDX.Direct3D11.VertexBufferBinding(vertexbuffer, vertexbuffer.Size / vertexbuffer.Count, 0));
            }
        }
    }
}
