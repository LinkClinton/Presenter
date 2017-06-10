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

        private DepthStencilState depthStencilState;

        public GraphicsPipelineState(VertexShader vertexshader,
            PixelShader pixelshader, BufferLayout bufferlayout,
            ResourceLayout resourcelayout, DepthStencilState depthstencilstate)
        {
            vertexShader = vertexshader;
            pixelShader = pixelshader;

            bufferLayout = bufferlayout;
            resourceLayout = resourcelayout;

            depthStencilState = depthstencilstate;

            bufferlayout.ID3D11InputLayout = new SharpDX.Direct3D11.InputLayout(Manager.ID3D11Device,
               vertexshader.ByteCode, bufferlayout.Elements);
        }

        public VertexShader VertexShader => vertexShader;

        public PixelShader PixelShader => pixelShader;

        public BufferLayout BufferLayout => bufferLayout;

        public ResourceLayout ResourceLayout => resourceLayout;

        public DepthStencilState DepthStencilState => depthStencilState;
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

                ID3D11DeviceContext.VertexShader.SetShader(graphicsPipelineState.VertexShader.ID3D11VertexShader, null, 0);
                ID3D11DeviceContext.PixelShader.SetShader(graphicsPipelineState.PixelShader.ID3D11PixelShader, null, 0);
                ID3D11DeviceContext.InputAssembler.InputLayout = graphicsPipelineState.BufferLayout.ID3D11InputLayout;
                ID3D11DeviceContext.OutputMerger.DepthStencilState = graphicsPipelineState.DepthStencilState.ID3D11DepthStencilState;
            }
        }
    }
}
