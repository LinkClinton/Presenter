using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presenter
{
    public class VertexShaderResourceIndexer 
    {
        private Dictionary<int, ShaderResource> vsshaderresource = new Dictionary<int, ShaderResource>();

        internal VertexShaderResourceIndexer()
        {

        }

        public ShaderResource this[int index]
        {
            get => vsshaderresource[index];
            set
            {
                vsshaderresource[index] = value;
                Manager.ID3D11DeviceContext.VertexShader.SetShaderResource(index, value.ID3D11ShaderResourceView);
            }
        }
    }

    public class PixelShaderResourceIndexer
    {
        private Dictionary<int, ShaderResource> psshaderresource = new Dictionary<int, ShaderResource>();

        internal PixelShaderResourceIndexer()
        {

        }

        public ShaderResource this[int index]
        {
            get => psshaderresource[index];
            set
            {
                psshaderresource[index] = value;
                Manager.ID3D11DeviceContext.PixelShader.SetShaderResource(index, value.ID3D11ShaderResourceView);
            }
        }
    }

    public partial class VertexShader
    {
        private static VertexShaderResourceIndexer resource =
            new VertexShaderResourceIndexer();

        public static VertexShaderResourceIndexer ExResource => resource;
    }

    public partial class PixelShader
    {
        private static PixelShaderResourceIndexer resource = 
            new PixelShaderResourceIndexer();

        public static PixelShaderResourceIndexer ExResource => resource;
    }

}
