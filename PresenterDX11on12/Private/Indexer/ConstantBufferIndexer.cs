using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presenter
{
    public class ConstantBufferIndexer
    {
        private Dictionary<int, Buffer> vsshaderbuffer = new Dictionary<int, Buffer>();
        private Dictionary<int, Buffer> psshaderbuffer = new Dictionary<int, Buffer>();

        public Buffer this[(Shader target, int which) index]
        {
            get
            {
                switch (index.target)
                {
                    case VertexShader e:
                        return vsshaderbuffer[index.which];
                    case PixelShader e:
                        return psshaderbuffer[index.which];
                    default:
                        return null;
                }
            }
            set
            {
                switch (index.target)
                {
                    case VertexShader e:
                        vsshaderbuffer[index.which] = value;
                        Manager.ID3D11DeviceContext.VertexShader.SetConstantBuffer(index.which, value.ID3D11Buffer);
                        break;
                    case PixelShader e:
                        psshaderbuffer[index.which] = value;
                        Manager.ID3D11DeviceContext.PixelShader.SetConstantBuffer(index.which, value.ID3D11Buffer);
                        break;
                    default:
                        break;
                }
            }
        }
    }

    public class VertexShaderConstantBufferIndexer
    {
        public Buffer this[int index]
        {
            get => Manager.ConstantBuffer[(Manager.GraphicsPipelineState?.VertexShader, index)];
            set => Manager.ConstantBuffer[(Manager.GraphicsPipelineState?.VertexShader, index)] = value;
        }
    }

    public class PixelShaderConstantBufferIndexer
    {
        public Buffer this[int index]
        {
            get => Manager.ConstantBuffer[(Manager.GraphicsPipelineState?.PixelShader, index)];
            set => Manager.ConstantBuffer[(Manager.GraphicsPipelineState?.PixelShader, index)] = value;
        }
    }

    public static partial class Manager
    {
        private static ConstantBufferIndexer constantbuffer = new ConstantBufferIndexer();

        public static ConstantBufferIndexer ConstantBuffer => constantbuffer;
    }

    public partial class VertexShader
    {
        private static VertexShaderConstantBufferIndexer constantbuffer = 
            new VertexShaderConstantBufferIndexer();

        public static VertexShaderConstantBufferIndexer ConstantBuffer => constantbuffer;
    }

    public partial class PixelShader
    {
        private static PixelShaderConstantBufferIndexer constantBuffer =
            new PixelShaderConstantBufferIndexer();

        public static PixelShaderConstantBufferIndexer ConstantBuffer
           => constantBuffer;
    }

}
