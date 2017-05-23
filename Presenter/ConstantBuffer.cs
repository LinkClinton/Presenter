using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;


namespace Presenter
{
    public class ConstantBuffer<T> : Buffer where T : struct
    {
        public ConstantBuffer(int datasize, int data_count = 1)
        {
            buffer = new SharpDX.Direct3D11.Buffer(Manager.ID3D11Device,
                size = datasize, SharpDX.Direct3D11.ResourceUsage.Default,
                SharpDX.Direct3D11.BindFlags.ConstantBuffer, SharpDX.Direct3D11.CpuAccessFlags.None,
                SharpDX.Direct3D11.ResourceOptionFlags.None, 0);

            count = data_count;
        }

        public ConstantBuffer(T data)
        {
            buffer = new SharpDX.Direct3D11.Buffer(Manager.ID3D11Device,
                size = Marshal.SizeOf<T>(), SharpDX.Direct3D11.ResourceUsage.Default,
                SharpDX.Direct3D11.BindFlags.ConstantBuffer, SharpDX.Direct3D11.CpuAccessFlags.None,
                SharpDX.Direct3D11.ResourceOptionFlags.None, 0);

            Update(ref data);

            count = 1;
        }

        public ConstantBuffer(T[] data)
        {
            buffer = new SharpDX.Direct3D11.Buffer(Manager.ID3D11Device,
                size = Marshal.SizeOf<T>() * data.Length, SharpDX.Direct3D11.ResourceUsage.Default,
                SharpDX.Direct3D11.BindFlags.ConstantBuffer, SharpDX.Direct3D11.CpuAccessFlags.None,
                SharpDX.Direct3D11.ResourceOptionFlags.None, 0);

            Update(data);

            count = 1;
        }
    }

    public class VertexShaderConstantBufferIndexer
    {
        public Buffer this[int index]
        {
            get => Manager.ConstantBuffer[(Manager.VertexShader, index)];
            set => Manager.ConstantBuffer[(Manager.VertexShader, index)] = value;
        }
    }

    public class PixelShaderConstantBufferIndexer
    {
        public Buffer this[int index]
        {
            get => Manager.ConstantBuffer[(Manager.PixelShader, index)];
            set => Manager.ConstantBuffer[(Manager.PixelShader, index)] = value;
        }
    }

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
                        Manager.ID3D11DeviceContext.VertexShader.SetConstantBuffer(index.which, value);
                        break;
                    case PixelShader e:
                        psshaderbuffer[index.which] = value;
                        Manager.ID3D11DeviceContext.PixelShader.SetConstantBuffer(index.which, value);
                        break;
                    default:
                        break;
                }
            }
        }

    }



    public static partial class Manager
    {
        private static ConstantBufferIndexer constantbuffer = new ConstantBufferIndexer();

        public static ConstantBufferIndexer ConstantBuffer
        {
            get => constantbuffer;
        }
    }
}
