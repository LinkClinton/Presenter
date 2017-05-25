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

        public int Count => count;

        internal SharpDX.Direct3D11.Buffer ID3D11Buffer => resource as SharpDX.Direct3D11.Buffer;

        ~Buffer() => (resource as SharpDX.Direct3D11.Buffer)?.Dispose();
    }
}
