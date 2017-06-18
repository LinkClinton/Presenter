using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presenter
{
    public abstract class Resource
    {
        protected int size;

        protected SharpDX.Direct3D12.Resource resource;

        public abstract void Update<T>(ref T data) where T : struct;

        public abstract void Update<T>(T[] data) where T : struct;

        public abstract void Update(IntPtr data);



        public int Size => size;

        internal SharpDX.Direct3D12.Resource ID3D12Resource => resource;

        ~Resource() => SharpDX.Utilities.Dispose(ref resource);
    }
}
