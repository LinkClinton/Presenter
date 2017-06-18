using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presenter
{
    public abstract class InputAssemblerStage
    {
        private Buffer vertexBuffer;
        private Buffer indexBuffer;

        private PrimitiveType primitiveType = PrimitiveType.TriangleList;

        public void Reset()
        {
            vertexBuffer = null;
            indexBuffer = null;

            PrimitiveType = PrimitiveType.TriangleList;
        }

        public Buffer VertexBuffer
        {
            get => vertexBuffer;
            set
            {
                vertexBuffer = value;

                GraphicsPipeline.ID3D12GraphicsCommandList.SetVertexBuffer(0, new SharpDX.Direct3D12.VertexBufferView()
                {
                    BufferLocation = vertexBuffer.ID3D12Resource.GPUVirtualAddress,
                    SizeInBytes = vertexBuffer.Size,
                    StrideInBytes = vertexBuffer.Size / vertexBuffer.Count
                });
            }
        }

        public Buffer IndexBuffer
        {
            get => indexBuffer;
            set
            {
                indexBuffer = value;

                GraphicsPipeline.ID3D12GraphicsCommandList.SetIndexBuffer(new SharpDX.Direct3D12.IndexBufferView()
                {
                    BufferLocation = indexBuffer.ID3D12Resource.GPUVirtualAddress,
                    Format =  SharpDX.DXGI.Format.R32_UInt,
                    SizeInBytes = indexBuffer.Size
                });
            }
        }

        public PrimitiveType PrimitiveType
        {
            get => primitiveType;
            set
            {
                primitiveType = value;

                GraphicsPipeline.ID3D12GraphicsCommandList.PrimitiveTopology = (SharpDX.Direct3D.PrimitiveTopology)primitiveType;
            }
        }

        public InputLayout InputLayout
        {
            get => GraphicsPipeline.State.InputLayout;
        }

    }

    class StaticInputAssemblerStage : InputAssemblerStage { }

    public static partial class GraphicsPipeline
    {
        private static InputAssemblerStage inputAssemblerStage = new StaticInputAssemblerStage();

        public static InputAssemblerStage InputAssemblerStage => inputAssemblerStage;

    }

}
