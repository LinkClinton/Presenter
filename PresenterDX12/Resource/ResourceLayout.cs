using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presenter
{
    public class ResourceLayout : IResourceLayout
    {
        public class Element
        {
            public SubResource ConstantBufferView;
            public SubResource ShaderResourceView;

            public Element(SubResource constantBufferView = null,
                SubResource shaderResourceView = null)
            {
                ConstantBufferView = constantBufferView;
                ShaderResourceView = shaderResourceView;
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

            public static implicit operator SubResource((int Start,int Count) subResource)
                => new SubResource(subResource.Start, subResource.Count);
        }


        private Element[] layoutElements;

        private SharpDX.Direct3D12.RootSignature rootSignature;

        public ResourceLayout(Element[] elements)
        {
            SharpDX.Direct3D12.RootParameter[] rootParameter = new SharpDX.Direct3D12.RootParameter[elements.Length];

            for (int i = 0; i < elements.Length; i++)
            {
                int rootCount = 0;

                if (elements[i].ConstantBufferView != null) rootCount++;
                if (elements[i].ShaderResourceView != null) rootCount++;

                SharpDX.Direct3D12.DescriptorRange[] range = new SharpDX.Direct3D12.DescriptorRange[rootCount];

                rootCount = 0;

                if (elements[i].ConstantBufferView != null)
                    range[rootCount++] = new SharpDX.Direct3D12.DescriptorRange(SharpDX.Direct3D12.DescriptorRangeType.ConstantBufferView,
                        elements[i].ConstantBufferView.Count, elements[i].ConstantBufferView.Start, 0, int.MinValue);

                if (elements[i].ShaderResourceView != null)
                    range[rootCount++] = new SharpDX.Direct3D12.DescriptorRange(SharpDX.Direct3D12.DescriptorRangeType.ShaderResourceView,
                        elements[i].ShaderResourceView.Count, elements[i].ShaderResourceView.Start, 0, int.MinValue);


                rootParameter[i] = new SharpDX.Direct3D12.RootParameter(SharpDX.Direct3D12.ShaderVisibility.All,
                    range);
            }

            SharpDX.Direct3D12.RootSignatureDescription rootDesc = new SharpDX.Direct3D12.RootSignatureDescription(
                SharpDX.Direct3D12.RootSignatureFlags.AllowInputAssemblerInputLayout, rootParameter);

            rootSignature = Manager.ID3D12Device.CreateRootSignature(rootDesc.Serialize());

            layoutElements = elements;
        }

        public Element[] Elements => layoutElements;

        public int SlotCount => layoutElements.Length;

        internal SharpDX.Direct3D12.RootSignature ID3D12RootSignature => rootSignature;
    }
}
