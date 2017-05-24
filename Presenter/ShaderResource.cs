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

        ~ShaderResource() => resourceview?.Dispose();
    }


    public class ShaderResourceIndexer
    {
        private Dictionary<int, ShaderResource> vsshaderresource = new Dictionary<int, ShaderResource>();
        private Dictionary<int, ShaderResource> psshaderresource = new Dictionary<int, ShaderResource>();

        public ShaderResource this[(Shader target, int which) index]
        {
            get
            {
                switch (index.target)
                {
                    case VertexShader e:
                        return vsshaderresource[index.which];
                    case PixelShader e:
                        return psshaderresource[index.which];
                    default:
                        return null;
                }
            }
            set
            {
                switch (index.target)
                {
                    case VertexShader e:
                        vsshaderresource[index.which] = value;
                        Manager.ID3D11DeviceContext.VertexShader.SetShaderResource(index.which, value);
                        break;
                    case PixelShader e:
                        psshaderresource[index.which] = value;
                        Manager.ID3D11DeviceContext.PixelShader.SetShaderResource(index.which, value);
                        break;
                    default:
                        break;
                }
            }
        }
    }

    public static partial class Manager
    {
        private static ShaderResourceIndexer shaderresource = new ShaderResourceIndexer();

        public static ShaderResourceIndexer ShaderResource => shaderresource;
    }
}
