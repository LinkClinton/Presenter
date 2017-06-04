using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presenter
{
    public abstract class ShaderResource : Resource, IShaderResource
    {
        protected SharpDX.Direct3D12.ShaderResourceViewDescription resourceview =
            new SharpDX.Direct3D12.ShaderResourceViewDescription();

        public override void Update<T>(ref T data)
        {
            throw new NotImplementedException("not Supported");
        }

        public override void Update<T>(T[] data)
        {
            throw new NotImplementedException("not Supported");
        }

        internal SharpDX.Direct3D12.ShaderResourceViewDescription ShaderResourceView => resourceview;
    }
}
