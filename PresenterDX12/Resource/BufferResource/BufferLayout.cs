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
        public class Element
        {
            public ElementSize Size;
            public string Tag;
        }

        private SharpDX.Direct3D12.InputLayoutDescription layout;

        public BufferLayout(Element[] elements)
        {
            SharpDX.Direct3D12.InputElement[] desc = new SharpDX.Direct3D12.InputElement[elements.Length];

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

                desc[i].Classification = SharpDX.Direct3D12.InputClassification.PerVertexData;

                desc[i].AlignedByteOffset = bit_off;
                bit_off += add_off;

            }

            layout = new SharpDX.Direct3D12.InputLayoutDescription(desc);
        }

        internal SharpDX.Direct3D12.InputLayoutDescription ID3D12InputLayoutDescription
            => layout;
    }

    
}
