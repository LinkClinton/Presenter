using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presenter
{
    public class Resource : IResource
    {
        protected int size;

        protected SharpDX.Direct3D11.Resource resource;

        public virtual void Update<T>(ref T data) where T : struct
        {
            Manager.ID3D11DeviceContext.UpdateSubresource(ref data, resource);
        }

        public virtual void Update<T>(T[] data) where T : struct
        {
            Manager.ID3D11DeviceContext.UpdateSubresource(data, resource);
        }

        public virtual void Update(IntPtr data)
        {
            Manager.ID3D11DeviceContext.UpdateSubresource(resource, 0, null,
                data, size, size);
        }

        public int Size => size;

        internal SharpDX.Direct3D11.Resource ID3D11Resource => resource;

        ~Resource() => SharpDX.Utilities.Dispose(ref resource);
    }
}
