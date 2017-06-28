using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presenter
{
    public partial class ResourceLayout 
    {
        private Element[] layoutElements;

        private StaticSampler[] layoutStaticSamplers;

        private int staticSamplerCount = 0;

        private SharpDX.Direct3D12.RootSignature rootSignature;

        public ResourceLayout(Element[] elements = null, StaticSampler[] staticSamplers = null)
        {
            SharpDX.Direct3D12.RootParameter[] rootParameter = null;

            if (elements != null)
            {
                rootParameter = new SharpDX.Direct3D12.RootParameter[elements.Length];

                for (int i = 0; i < elements.Length; i++)
                {
                    switch (elements[i].Type)
                    {
                        case ResourceType.ConstantBufferView:
                            rootParameter[i] = new SharpDX.Direct3D12.RootParameter(SharpDX.Direct3D12.ShaderVisibility.All,
                                new SharpDX.Direct3D12.RootDescriptor(elements[i].Register, 0), SharpDX.Direct3D12.RootParameterType.ConstantBufferView);
                            break;
                        case ResourceType.ShaderResourceView:
                            rootParameter[i] = new SharpDX.Direct3D12.RootParameter(SharpDX.Direct3D12.ShaderVisibility.All,
                                new SharpDX.Direct3D12.RootDescriptor(elements[i].Register, 0), SharpDX.Direct3D12.RootParameterType.ShaderResourceView);
                            break;
                        case ResourceType.ConstantBufferTable:
                            rootParameter[i] = new SharpDX.Direct3D12.RootParameter(SharpDX.Direct3D12.ShaderVisibility.All,
                                new SharpDX.Direct3D12.DescriptorRange(SharpDX.Direct3D12.DescriptorRangeType.ConstantBufferView,
                                elements[i].Count, elements[i].Register));
                            break;
                        case ResourceType.ShaderResourceTable:
                            rootParameter[i] = new SharpDX.Direct3D12.RootParameter(SharpDX.Direct3D12.ShaderVisibility.All,
                                new SharpDX.Direct3D12.DescriptorRange(SharpDX.Direct3D12.DescriptorRangeType.ShaderResourceView,
                                elements[i].Count, elements[i].Register));
                            break;
                        default:
                            break;
                    }
                }
            }

            SharpDX.Direct3D12.StaticSamplerDescription[] sampleState = null;

            if (staticSamplers != null)
            {
                sampleState = new SharpDX.Direct3D12.StaticSamplerDescription[staticSamplerCount = staticSamplers.Length];

                for (int i = 0; i < sampleState.Length; i++)
                {
                    sampleState[i] = new SharpDX.Direct3D12.StaticSamplerDescription(SharpDX.Direct3D12.ShaderVisibility.All, i, 0)
                    {
                        AddressU = (SharpDX.Direct3D12.TextureAddressMode)staticSamplers[i].AddressU,
                        AddressV = (SharpDX.Direct3D12.TextureAddressMode)staticSamplers[i].AddressV,
                        AddressW = (SharpDX.Direct3D12.TextureAddressMode)staticSamplers[i].AddressW,
                        Filter = (SharpDX.Direct3D12.Filter)staticSamplers[i].Filter
                    };
                }
            }

            SharpDX.Direct3D12.RootSignatureDescription rootDesc = new SharpDX.Direct3D12.RootSignatureDescription(
                SharpDX.Direct3D12.RootSignatureFlags.AllowInputAssemblerInputLayout, rootParameter, sampleState);

            rootSignature = Engine.ID3D12Device.CreateRootSignature(0, rootDesc.Serialize());

            layoutElements = elements;

            layoutStaticSamplers = staticSamplers;
        }

        public Element[] Elements => layoutElements;

        public StaticSampler[] StaticSamplers => layoutStaticSamplers;

        public int SlotCount => layoutElements.Length;

        public int StaticSamplerCount => staticSamplerCount;

        internal SharpDX.Direct3D12.RootSignature ID3D12RootSignature => rootSignature;

        ~ResourceLayout() => SharpDX.Utilities.Dispose(ref rootSignature);
    }
}
