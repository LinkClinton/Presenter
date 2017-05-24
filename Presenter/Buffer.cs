using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presenter
{
    public abstract class Buffer : Resource
    {
        protected int count;

        public static implicit operator SharpDX.Direct3D11.Buffer(Buffer buffer)
            => buffer.resource as SharpDX.Direct3D11.Buffer;

        public int Count => count;

        ~Buffer() => (resource as SharpDX.Direct3D11.Buffer)?.Dispose();
    }
}
