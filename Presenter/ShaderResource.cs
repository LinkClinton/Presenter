using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presenter
{
    public class ShaderResource : Resource
    {
        protected SharpDX.Direct3D11.ShaderResourceView resourceview;

        public static implicit operator SharpDX.Direct3D11.ShaderResourceView(ShaderResource resource)
            => resource.resourceview;
    }
}
