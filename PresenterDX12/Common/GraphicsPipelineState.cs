using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presenter
{
    public class GraphicsPipelineState 
    {
        private VertexShader vertexShader;
        private PixelShader pixelShader;

        private InputLayout inputLayout;
        private ResourceLayout resourceLayout;

        private DepthStencilState depthStencilState;
        private BlendState blendState;
        private RasterizerState rasterizerState;

        private SharpDX.Direct3D12.GraphicsPipelineStateDescription graphicsDesc;
        private SharpDX.Direct3D12.PipelineState pipelineState;

        public GraphicsPipelineState(VertexShader vertexshader,
            PixelShader pixelshader, InputLayout inputlayout,
            ResourceLayout resourcelayout, RasterizerState rasterizerstate,
            DepthStencilState depthstencilstate, BlendState blendstate)
        {
            vertexShader = vertexshader;
            pixelShader = pixelshader;

            inputLayout = inputlayout;

            resourceLayout = resourcelayout is null ? new ResourceLayout() : resourcelayout;

            rasterizerState = rasterizerstate is null ? new RasterizerState() : rasterizerstate;
            depthStencilState = depthstencilstate is null ? new DepthStencilState(false, false) : depthstencilstate;
            blendState = blendstate is null ? new BlendState(false) : blendstate;

            graphicsDesc = new SharpDX.Direct3D12.GraphicsPipelineStateDescription()
            {
                InputLayout = inputLayout.ID3D12InputLayoutDescription,
                RootSignature = resourceLayout.ID3D12RootSignature,
                VertexShader = vertexShader.ByteCode,
                PixelShader = pixelShader.ByteCode,
                RasterizerState = rasterizerState.ID3D12RasterizerState,
                BlendState = blendState.ID3D12BlendState,
                DepthStencilFormat = Present.DepthStencilFormat,
                DepthStencilState = depthStencilState.ID3D12DepthStencilState,
                SampleMask = int.MaxValue,
                PrimitiveTopologyType = SharpDX.Direct3D12.PrimitiveTopologyType.Triangle,
                RenderTargetCount = 1,
                Flags = SharpDX.Direct3D12.PipelineStateFlags.None,
                SampleDescription = new SharpDX.DXGI.SampleDescription(1, 0),
                StreamOutput = new SharpDX.Direct3D12.StreamOutputDescription()
            };

            graphicsDesc.RenderTargetFormats[0] = SharpDX.DXGI.Format.R8G8B8A8_UNorm;

            pipelineState = Engine.ID3D12Device.CreateGraphicsPipelineState(graphicsDesc);
        }

        internal SharpDX.Direct3D12.PipelineState ID3D12GraphicsPipelineState
            => pipelineState;

        public VertexShader VertexShader => vertexShader;

        public PixelShader PixelShader => pixelShader;

        public InputLayout InputLayout => inputLayout;

        public ResourceLayout ResourceLayout => resourceLayout;

        public DepthStencilState DepthStencilState => depthStencilState;

        public BlendState BlendState => blendState;

        ~GraphicsPipelineState() => SharpDX.Utilities.Dispose(ref pipelineState);
    }

    public static partial class GraphicsPipeline
    {
        private static GraphicsPipelineState graphicsPipelineState;


        public static GraphicsPipelineState State
        {
            get => graphicsPipelineState;
        }

        public static void Reset(GraphicsPipelineState GraphicsPipelineState)
        {
            graphicsPipelineState = GraphicsPipelineState;

            graphicsCommandList.PipelineState = State.ID3D12GraphicsPipelineState;
            graphicsCommandList.SetGraphicsRootSignature(State.ResourceLayout.ID3D12RootSignature);
        }
    }
}
