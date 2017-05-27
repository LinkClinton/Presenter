using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presenter
{
    public class ShaderResource : Resource, IShaderResource
    {
        protected SharpDX.Direct3D11.ShaderResourceView resourceview;

        public override void Update<T>(ref T data)
        {
            base.Update(ref data);

            SharpDX.Utilities.Dispose(ref resourceview);
            resourceview = new SharpDX.Direct3D11.ShaderResourceView(Manager.ID3D11Device, resource);
        }

        public override void Update<T>(T[] data)
        {
            base.Update(data);

            SharpDX.Utilities.Dispose(ref resourceview);
            resourceview = new SharpDX.Direct3D11.ShaderResourceView(Manager.ID3D11Device, resource);
        }

        internal SharpDX.Direct3D11.ShaderResourceView ID3D11ShaderResourceView => resourceview;

        ~ShaderResource() => SharpDX.Utilities.Dispose(ref resourceview);
    }


   
    
}
