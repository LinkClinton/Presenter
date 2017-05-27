using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace Presenter
{
    public enum PrimitiveType
    {
        UNK,
        PointList,
        LineList,
        LineStrip,
        TriangleList,
        TriangleStrip,
    };

    public class BufferLayout : IBufferLayout
    {
        public enum ElementSize
        {
            eFloat1,
            eFloat2,
            eFloat3,
            eFlaot4
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct Element
        {
            public ElementSize Size;
            public string Tag;
        }

        private SharpDX.Direct3D11.InputLayout layout;

        public BufferLayout(Element[] elements)
        {
#if DEBUG
            if (Manager.VertexShader is null) throw new Exception("Vertex Shader is null");
#endif

            SharpDX.Direct3D11.InputElement[] desc = new SharpDX.Direct3D11.InputElement[elements.Length];

            int bit_off = 0;

            for (int i = 0; i < elements.Length; i++)
            {
                desc[i].SemanticName = elements[i].Tag;

                int add_off = 0;

                switch (elements[i].Size)
                {
                    case ElementSize.eFloat1:
                        add_off = 4;
                        desc[i].Format = SharpDX.DXGI.Format.R8G8B8A8_UInt;
                        break;
                    case ElementSize.eFloat2:
                        add_off = 8;
                        desc[i].Format = SharpDX.DXGI.Format.R32G32_Float;
                        break;
                    case ElementSize.eFloat3:
                        add_off = 12;
                        desc[i].Format = SharpDX.DXGI.Format.R32G32B32_Float;
                        break;
                    case ElementSize.eFlaot4:
                        add_off = 16;
                        desc[i].Format = SharpDX.DXGI.Format.R32G32B32A32_Float;
                        break;
                    default:
                        break;
                }

                desc[i].Classification = SharpDX.Direct3D11.InputClassification.PerVertexData;

                desc[i].AlignedByteOffset = bit_off;
                bit_off += add_off;

            }

            layout = new SharpDX.Direct3D11.InputLayout(Manager.ID3D11Device, Manager.VertexShader.ByteCode, desc);
        }

        internal SharpDX.Direct3D11.InputLayout ID3D11InputLayout => layout;

        ~BufferLayout() => SharpDX.Utilities.Dispose(ref layout);
    }

    public static partial class Manager
    {
        private static BufferLayout bufferlayout;

        public static BufferLayout BufferLayout
        {
            get => bufferlayout;
            set
            {
                bufferlayout = value;

                ID3D11DeviceContext.InputAssembler.InputLayout = bufferlayout.ID3D11InputLayout;
            }
        }
    }
}
