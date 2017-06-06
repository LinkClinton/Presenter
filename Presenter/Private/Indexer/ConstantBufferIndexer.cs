using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presenter
{
    public class VertexShaderConstantBufferIndexer
    {
        private Dictionary<int, Buffer> vsshaderbuffer = new Dictionary<int, Buffer>();

        internal VertexShaderConstantBufferIndexer()
        {

        }

        public Buffer this[int index]
        {
            get => vsshaderbuffer[index];
            set
            {
                vsshaderbuffer[index] = value;
                Manager.ID3D11DeviceContext.VertexShader.SetConstantBuffer(index, value.ID3D11Buffer);
            }
        }
    }

    public class PixelShaderConstantBufferIndexer
    {
        private Dictionary<int, Buffer> psshaderbuffer = new Dictionary<int, Buffer>();

        internal PixelShaderConstantBufferIndexer()
        {

        }

        public Buffer this[int index]
        {
            get => psshaderbuffer[index];
            set
            {
                psshaderbuffer[index] = value;
                Manager.ID3D11DeviceContext.PixelShader.SetConstantBuffer(index, value.ID3D11Buffer);
            }
        }
    }

    public partial class VertexShader
    {
        private static VertexShaderConstantBufferIndexer constantbuffer = 
            new VertexShaderConstantBufferIndexer();

        public static VertexShaderConstantBufferIndexer ExConstantBuffer 
            => constantbuffer;
    }

    public partial class PixelShader
    {
        private static PixelShaderConstantBufferIndexer constantBuffer =
            new PixelShaderConstantBufferIndexer();

        public static PixelShaderConstantBufferIndexer ExConstantBuffer
           => constantBuffer;
    }

}
