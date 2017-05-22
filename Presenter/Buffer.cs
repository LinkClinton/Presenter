using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presenter
{
    public abstract class Buffer
    {
        protected SharpDX.Direct3D11.Buffer buffer;

        protected int size;

        protected int count;

        public void Update<T>(ref T data) where T : struct
        {
            Manager.ID3D11DeviceContext.UpdateSubresource(ref data, buffer);
        }

        public void Update<T>(T[] data) where T : struct
        {
            Manager.ID3D11DeviceContext.UpdateSubresource(data, buffer);
        }

        public static implicit operator SharpDX.Direct3D11.Buffer(Buffer buffer)
            => buffer.buffer;

        public int Size => size;

        public int Count => count;
    }
}
