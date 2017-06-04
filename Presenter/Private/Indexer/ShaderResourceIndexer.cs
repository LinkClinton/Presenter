using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presenter
{
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
                        Manager.ID3D11DeviceContext.VertexShader.SetShaderResource(index.which, value.ID3D11ShaderResourceView);
                        break;
                    case PixelShader e:
                        psshaderresource[index.which] = value;
                        Manager.ID3D11DeviceContext.PixelShader.SetShaderResource(index.which, value.ID3D11ShaderResourceView);
                        break;
                    default:
                        break;
                }
            }
        }
    }

    public class VertexShaderResourceIndexer
    {
        public ShaderResource this[int index]
        {
            get => Manager.ShaderResource[(Manager.GraphicsPipelineState?.VertexShader, index)];
            set => Manager.ShaderResource[(Manager.GraphicsPipelineState?.VertexShader, index)] = value;
        }
    }

    public class PixelShaderResourceIndexer
    {
        public ShaderResource this[int index]
        {
            get => Manager.ShaderResource[(Manager.GraphicsPipelineState?.PixelShader, index)];
            set => Manager.ShaderResource[(Manager.GraphicsPipelineState?.PixelShader, index)] = value;
        }
    }

    public static partial class Manager
    {
        private static ShaderResourceIndexer shaderresource = new ShaderResourceIndexer();

        public static ShaderResourceIndexer ShaderResource => shaderresource;
    }

    public partial class VertexShader
    {
        private static VertexShaderResourceIndexer resource =
            new VertexShaderResourceIndexer();

        public static VertexShaderResourceIndexer Resource => resource;
    }

    public partial class PixelShader
    {
        private static PixelShaderResourceIndexer resource = 
            new PixelShaderResourceIndexer();

        public static PixelShaderResourceIndexer Resource => resource;
    }

}
