using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presenter
{
    public class GraphicsPipelineState : IGraphicsPipelineState
    {
        private VertexShader vertexShader;
        private PixelShader pixelShader;

        private BufferLayout bufferLayout;
        private ResourceLayout resourceLayout;


        private SharpDX.Direct3D12.GraphicsPipelineStateDescription graphicsDesc;
        private SharpDX.Direct3D12.PipelineState pipelineState;

        public GraphicsPipelineState(VertexShader vertexshader,
            PixelShader pixelshader, BufferLayout bufferlayout,
            ResourceLayout resourcelayout)
        {
            vertexShader = vertexshader;
            pixelShader = pixelshader;

            bufferLayout = bufferlayout;
            resourceLayout = resourcelayout;

            graphicsDesc = new SharpDX.Direct3D12.GraphicsPipelineStateDescription()
            {
                InputLayout = bufferLayout.ID3D12InputLayoutDescription,
                RootSignature = resourceLayout.ID3D12RootSignature,
                VertexShader = vertexShader.ByteCode,
                PixelShader = pixelShader.ByteCode,
                RasterizerState = SharpDX.Direct3D12.RasterizerStateDescription.Default(),
                BlendState = SharpDX.Direct3D12.BlendStateDescription.Default(),
                DepthStencilFormat = SharpDX.DXGI.Format.D32_Float,
                DepthStencilState = new SharpDX.Direct3D12.DepthStencilStateDescription()
                {
                    IsDepthEnabled = false,
                    IsStencilEnabled = false
                },
                SampleMask = int.MaxValue,
                PrimitiveTopologyType = SharpDX.Direct3D12.PrimitiveTopologyType.Triangle,
                RenderTargetCount = 1,
                Flags = SharpDX.Direct3D12.PipelineStateFlags.None,
                SampleDescription = new SharpDX.DXGI.SampleDescription(1, 0),
                StreamOutput = new SharpDX.Direct3D12.StreamOutputDescription()
            };

            graphicsDesc.RenderTargetFormats[0] = SharpDX.DXGI.Format.R8G8B8A8_UNorm;

            pipelineState = Manager.ID3D12Device.CreateGraphicsPipelineState(graphicsDesc);
        }

        internal SharpDX.Direct3D12.PipelineState ID3D12GraphicsPipelineState
            => pipelineState;

        public VertexShader VertexShader => vertexShader;

        public PixelShader PixelShader => pixelShader;

        public BufferLayout BufferLayout => bufferLayout;

        public ResourceLayout ResourceLayout => resourceLayout;
    }

    public partial class Manager
    {
        private static GraphicsPipelineState graphicsPipelineState;

        public static GraphicsPipelineState GraphicsPipelineState
        {
            get => graphicsPipelineState;
            set
            {
                graphicsPipelineState = value;

                ID3D12GraphicsCommandList.SetGraphicsRootSignature(graphicsPipelineState.ResourceLayout.ID3D12RootSignature);
                ID3D12GraphicsCommandList.PipelineState = graphicsPipelineState.ID3D12GraphicsPipelineState;
            }
        }
    }
}
