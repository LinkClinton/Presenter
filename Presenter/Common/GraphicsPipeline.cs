using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presenter
{
    public static partial class GraphicsPipeline
    {
        static GraphicsPipeline()
        { 
        }

        public static void Open(GraphicsPipelineState GraphicsPipelineState, Surface target)
        {
            surface = target;

            Reset(GraphicsPipelineState);

            Engine.ID3D11DeviceContext.Rasterizer.SetViewport(new SharpDX.Mathematics.Interop.RawViewportF()
            {
                Height = surface.Height,
                Width = surface.Width,
                MaxDepth = 1.0f,
                MinDepth = 0.0f,
                X = 0f,
                Y = 0f
            });

            Engine.ID3D11DeviceContext.Rasterizer.SetScissorRectangle(0, 0, surface.Width, surface.Height);

            Engine.ID3D11DeviceContext.OutputMerger.SetTargets(surface.ID3D11DepthStencilView,
                surface.ID3D11RenderTargetView);

            Engine.ID3D11DeviceContext.ClearRenderTargetView(surface.ID3D11RenderTargetView,
                new SharpDX.Mathematics.Interop.RawColor4(surface.BackGround.red,
                    surface.BackGround.green, surface.BackGround.blue, surface.BackGround.alpha));

            Engine.ID3D11DeviceContext.ClearDepthStencilView(surface.ID3D11DepthStencilView,
                 SharpDX.Direct3D11.DepthStencilClearFlags.Depth | SharpDX.Direct3D11.DepthStencilClearFlags.Stencil, 1f, 0);
        }

        public static void Close()
        {
            surface.IDXGISwapChain.Present(0, SharpDX.DXGI.PresentFlags.None);

            InputAssemblerStage.Reset();
            VertexShaderStage.Reset();
            PixelShaderStage.Reset();
            OutputMergerStage.Reset();

            surface = null;
            graphicsPipelineState = null;
        }
    }
}
