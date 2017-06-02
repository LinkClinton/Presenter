using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presenter
{
    public class ResourceLayout : IResourceLayout
    {

        public struct Element
        {
            public SubResource ConstantBufferView;
            public SubResource ShaderResourceView;
        }

        public struct SubResource
        {
            public int Start;
            public int Count;

            static int None => -1;
        }

        private int slotCount = 0;

        private SharpDX.Direct3D12.RootSignature rootSignature;

        public ResourceLayout(Element[] elements)
        {
            
            slotCount = elements.Length;
        }

        public int SlotCount => slotCount;
    }
}
