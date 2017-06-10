using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presenter
{
    public class ResourceInputIndexer
    {
        private void ReportError(int index, ResourceType input,
            ResourceType target)
        {
#if DEBUG
            if (input != target)
                throw new NotImplementedException("Resource InputSlot " + index + " need: "
                    + target + ", but set: " + input);
#endif 
        }

        private void ReportError(int currentCount, int targetCount, ResourceType type)
        {
#if DEBUG
            if (currentCount > targetCount && targetCount != -1)
                throw new NotImplementedException("Resource InputSlot too many resource in heap, Type: "
                    + type);
#endif 
        }

        internal ResourceInputIndexer()
        {

        }

        public object this[int index]
        { 
            set
            {
#if DEBUG
                if (Manager.GraphicsPipelineState.ResourceLayout is null)
                    throw new NotImplementedException("Resource Layout is not set");
#endif
                ResourceLayout.Element element = Manager.GraphicsPipelineState.ResourceLayout.Elements[index];

                switch (value)
                {
                    case Buffer buffer:
                        ReportError(index, ResourceType.ConstantBufferView, element.Type);

                        Manager.ID3D11DeviceContext.VertexShader.SetConstantBuffer(element.Register, buffer.ID3D11Buffer);
                        Manager.ID3D11DeviceContext.PixelShader.SetConstantBuffer(element.Register, buffer.ID3D11Buffer);
                        break;
                    case ShaderResource shaderresource:
                        ReportError(index, ResourceType.ShaderResourceView, element.Type);

                        Manager.ID3D11DeviceContext.VertexShader.SetShaderResource(element.Register, shaderresource.ID3D11ShaderResourceView);
                        Manager.ID3D11DeviceContext.PixelShader.SetShaderResource(element.Register, shaderresource.ID3D11ShaderResourceView);
                        break;
                    case ResourceHeap heap:
                        ReportError(index, ResourceType.ResourceHeap, element.Type);

                        int bufferCount = 0;
                        int resourceCount = 0;
                        int register = 0;
                        
                        foreach (var item in heap.Elements)
                        {
                            switch (item)
                            {
                                case Buffer buffer:
                                    ReportError(++bufferCount, element.ConstantBufferView.Count, ResourceType.ConstantBufferView);

                                    register = element.ConstantBufferView.Start + bufferCount - 1;

                                    Manager.ID3D11DeviceContext.VertexShader.SetConstantBuffer(register, buffer.ID3D11Buffer);
                                    Manager.ID3D11DeviceContext.PixelShader.SetConstantBuffer(register, buffer.ID3D11Buffer);
                                    break;
                                case ShaderResource shaderresource:
                                    ReportError(++resourceCount, element.ShaderResourceView.Count, ResourceType.ShaderResourceView);

                                    register = element.ShaderResourceView.Start + resourceCount - 1;

                                    Manager.ID3D11DeviceContext.VertexShader.SetShaderResource(register, shaderresource.ID3D11ShaderResourceView);
                                    Manager.ID3D11DeviceContext.PixelShader.SetShaderResource(register, shaderresource.ID3D11ShaderResourceView);
                                    break;
                                default:
                                    break;
                            }
                        }
                        break;
                    default:
                        break;
                }
            } 
        }
        
        public void Reset()
        {

        }
    }

    public partial class ResourceLayout
    {
        private static ResourceInputIndexer resouceInput = new ResourceInputIndexer();

        public static ResourceInputIndexer InputSlot => resouceInput;
    }
}
