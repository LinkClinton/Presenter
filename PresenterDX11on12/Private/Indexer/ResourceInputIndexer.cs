﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presenter
{
    public class ResourceInputIndexer
    {
        private void ReportError(int index, ResourceLayout.ResourceType input,
            ResourceLayout.ResourceType target)
        {
#if DEBUG
            if (input != target)
                throw new NotImplementedException("Resource InputSlot " + index + " need: "
                    + target + ", but set: " + input);
#endif 
        }

        private void ReportError(int currentCount, int targetCount, ResourceLayout.ResourceType type)
        {
#if DEBUG
            if (currentCount > targetCount && targetCount != -1)
                throw new NotImplementedException("Resource InputSlot too many resource in heap, Type: "
                    + type);
#endif 
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
                        ReportError(index, ResourceLayout.ResourceType.ConstantBufferView, element.Type);

                        VertexShader.ConstantBuffer[element.Register] = buffer;
                        PixelShader.ConstantBuffer[element.Register] = buffer;
                        break;
                    case ShaderResource shaderresource:
                        ReportError(index, ResourceLayout.ResourceType.ShaderResourceView, element.Type);

                        VertexShader.Resource[element.Register] = shaderresource;
                        PixelShader.Resource[element.Register] = shaderresource;
                        break;
                    case ResourceHeap heap:
                        ReportError(index, ResourceLayout.ResourceType.ResourceHeap, element.Type);

                        int bufferCount = 0;
                        int resourceCount = 0;
                        int register = 0;
                        
                        foreach (var item in heap.Elements)
                        {
                            switch (item)
                            {
                                case Buffer buffer:
                                    ReportError(++bufferCount, element.ConstantBufferView.Count, ResourceLayout.ResourceType.ConstantBufferView);

                                    register = element.ConstantBufferView.Start + bufferCount - 1;

                                    VertexShader.ConstantBuffer[register] = buffer;
                                    PixelShader.ConstantBuffer[register] = buffer;
                                    break;
                                case ShaderResource shaderresource:
                                    ReportError(++resourceCount, element.ShaderResourceView.Count, ResourceLayout.ResourceType.ShaderResourceView);

                                    register = element.ShaderResourceView.Start + resourceCount - 1;

                                    VertexShader.Resource[register] = shaderresource;
                                    PixelShader.Resource[register] = shaderresource;
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

    public static partial class Manager
    {
        private static ResourceInputIndexer resouceInput = new ResourceInputIndexer();

        public static ResourceInputIndexer ResourceInput => resouceInput;
    }

}