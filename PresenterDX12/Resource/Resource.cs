using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presenter
{
    public abstract class Resource : IResource
    {
        protected int size;

        protected SharpDX.Direct3D12.Resource resource;

        protected IntPtr resourceStart;

        public virtual void Update<T>(ref T data) where T : struct
        {
            SharpDX.Utilities.Write(resourceStart, ref data);
        }

        public virtual void Update<T>(T[] data) where T : struct
        {
            SharpDX.Utilities.Write(resourceStart, data, 0,
                data.Length);
        }

        protected long UploadRequiredSize(SharpDX.Direct3D12.Resource resource)
        {
            var desc = resource.Description;
            Manager.ID3D12Device.GetCopyableFootprints(ref desc, 0, 1, 0, null, null, null, out long result);
            return result;
        }

        public int Size => size;

        internal SharpDX.Direct3D12.Resource ID3D12Resource => resource;

        ~Resource() => SharpDX.Utilities.Dispose(ref resource);
    }
}
