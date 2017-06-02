using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presenter
{
    public abstract class ShaderResource : Resource, IShaderResource
    {
        private SharpDX.Direct3D12.ShaderResourceViewDescription resourceview =
            new SharpDX.Direct3D12.ShaderResourceViewDescription();


        internal SharpDX.Direct3D12.ShaderResourceViewDescription ShaderResourceView => resourceview;
    }
}
