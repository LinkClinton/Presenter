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

        internal SharpDX.Direct3D11.ShaderResourceView ID3D11ShaderResourceView => resourceview;

        ~ShaderResource() => resourceview?.Dispose();
    }


   
    
}
