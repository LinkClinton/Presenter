using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presenter
{
    public partial class ResourceLayout : IResourceLayout
    {
        public enum ResourceType
        {
            ConstantBufferView,
            ShaderResourceView,
            ResourceHeap
        }

        public class Element
        {
            public SubResource ConstantBufferView;
            public SubResource ShaderResourceView;

            public ResourceType Type;
            public int Register;

            public Element(SubResource constantBufferView = null,
                SubResource shaderResourceView = null)
            {
                ConstantBufferView = constantBufferView;
                ShaderResourceView = shaderResourceView;
                Type = ResourceType.ResourceHeap;
            }

            public Element(ResourceType type, int register)
            {
                Type = type;
                Register = register;
            }

        }

        public class SubResource
        {
            public int Start;
            public int Count;

            public SubResource(int start, int count)
            {
                Start = start;
                Count = count;
            }

            public static implicit operator SubResource((int Start, int Count) subResource)
                => new SubResource(subResource.Start, subResource.Count);
        }


        private Element[] layoutElements;

        private int staticSamplerCount = 0;

        public ResourceLayout(Element[] elements, int StaticSamplerCount = 0)
        {
            layoutElements = elements;

            staticSamplerCount = StaticSamplerCount;
        }

        public Element[] Elements => layoutElements;

        public int SlotCount => layoutElements.Length;

        public int StaticSamplerCount => staticSamplerCount;
    }
}
