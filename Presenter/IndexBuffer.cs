using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace Presenter
{
    class IndexBuffer<T> : Buffer where T : struct
    {
        private int indexCount;

        public IndexBuffer(T[] indices)
        {
            buffer = new SharpDX.Direct3D11.Buffer(Manager.ID3D11Device,
               size = Marshal.SizeOf<T>() * indices.Length, SharpDX.Direct3D11.ResourceUsage.Default,
               SharpDX.Direct3D11.BindFlags.ConstantBuffer, SharpDX.Direct3D11.CpuAccessFlags.None,
               SharpDX.Direct3D11.ResourceOptionFlags.None, 0);

            Update(indices);

            indexCount = indices.Length;
        }

        public int IndexCount => indexCount;
    }
}
