using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presenter
{
    public class ResourceInputIndexer
    {
        public object this[int index]
        { 
            set
            {
                switch (value)
                {
                    case Buffer buffer:
                        Manager.ID3D12GraphicsCommandList.SetComputeRootConstantBufferView(index,
                            buffer.ID3D12Resource.GPUVirtualAddress);
                        break;
                    case ShaderResource shaderresource:
                        Manager.ID3D12GraphicsCommandList.SetComputeRootShaderResourceView(index,
                            shaderresource.ID3D12Resource.GPUVirtualAddress);
                        break;
                    default:
                        break;
                }
            } 
        }   
    }

    public static partial class Manager
    {
        private static ResourceInputIndexer resouceInput = new ResourceInputIndexer();

        public static ResourceInputIndexer ResourceInput => resouceInput;
    }

}
