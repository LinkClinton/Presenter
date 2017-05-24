using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presenter
{
    public class Resource
    {
        protected int size;

        protected SharpDX.Direct3D11.Resource resource;

        public void Update<T>(ref T data) where T : struct
        {
            Manager.ID3D11DeviceContext.UpdateSubresource(ref data, resource);
        }

        public void Update<T>(T[] data) where T : struct
        {
            Manager.ID3D11DeviceContext.UpdateSubresource(data, resource);
        }

        public int Size => size;

        public static implicit operator SharpDX.Direct3D11.Resource(Resource resource)
            => resource.resource;
    }
}
