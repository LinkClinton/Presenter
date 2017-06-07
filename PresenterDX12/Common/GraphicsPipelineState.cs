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

        private static SharpDX.Direct3D12.DepthStencilStateDescription DepthStencilStateDefault
        {
            get
            {
                return new SharpDX.Direct3D12.DepthStencilStateDescription()
                {
                    IsDepthEnabled = true,
                    DepthWriteMask = SharpDX.Direct3D12.DepthWriteMask.All,
                    DepthComparison = SharpDX.Direct3D12.Comparison.Less,
                    IsStencilEnabled = false,
                    StencilReadMask = 0xff,
                    StencilWriteMask = 0xff,
                    FrontFace = new SharpDX.Direct3D12.DepthStencilOperationDescription()
                    {
                        FailOperation = SharpDX.Direct3D12.StencilOperation.Keep,
                        DepthFailOperation = SharpDX.Direct3D12.StencilOperation.Keep,
                        PassOperation = SharpDX.Direct3D12.StencilOperation.Keep,
                        Comparison = SharpDX.Direct3D12.Comparison.Always
                    },
                    BackFace = new SharpDX.Direct3D12.DepthStencilOperationDescription()
                    {
                        FailOperation = SharpDX.Direct3D12.StencilOperation.Keep,
                        DepthFailOperation = SharpDX.Direct3D12.StencilOperation.Keep,
                        PassOperation = SharpDX.Direct3D12.StencilOperation.Keep,
                        Comparison = SharpDX.Direct3D12.Comparison.Always
                    }
                };
            }
        }

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
                DepthStencilFormat = Surface.DepthStencilFormat,
                DepthStencilState = DepthStencilStateDefault,
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

        ~GraphicsPipelineState() => SharpDX.Utilities.Dispose(ref pipelineState);
        
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
