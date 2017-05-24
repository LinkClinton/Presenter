using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presenter
{
    public abstract class Buffer : Resource
    {
        protected int size;

        protected int count;

        public void Update<T>(ref T data) where T : struct
        {
            Manager.ID3D11DeviceContext.UpdateSubresource(ref data, resource);
        }

        public void Update<T>(T[] data) where T : struct
        {
            Manager.ID3D11DeviceContext.UpdateSubresource(data, resource);
        }

        public static implicit operator SharpDX.Direct3D11.Buffer(Buffer buffer)
            => buffer.resource as SharpDX.Direct3D11.Buffer;

        public int Size => size;

        public int Count => count;

        ~Buffer() => (resource as SharpDX.Direct3D11.Buffer).Dispose();
    }
}
