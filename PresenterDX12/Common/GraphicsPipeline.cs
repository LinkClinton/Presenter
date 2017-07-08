using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presenter
{
    public static partial class GraphicsPipeline
    {
        private static SharpDX.Direct3D12.GraphicsCommandList graphicsCommandList;

        static GraphicsPipeline()
        {
            ID3D12GraphicsCommandList = Engine.ID3D12Device.CreateCommandList(SharpDX.Direct3D12.CommandListType.Direct,
                Engine.ID3D12CommandAllocator, null);

            ID3D12GraphicsCommandList.Close();
        }

        public static void Open(GraphicsPipelineState GraphicsPipelineState, Surface target)
        {
            Engine.ID3D12CommandAllocator.Reset();

            surface = target;

            graphicsPipelineState = GraphicsPipelineState;

            ID3D12GraphicsCommandList.Reset(Engine.ID3D12CommandAllocator,
                graphicsPipelineState.ID3D12GraphicsPipelineState);

            ID3D12GraphicsCommandList.SetGraphicsRootSignature(
                graphicsPipelineState.ResourceLayout.ID3D12RootSignature);

            ID3D12GraphicsCommandList.SetViewport(new SharpDX.Mathematics.Interop.RawViewportF()
            {
                Height = surface.Height,
                Width = surface.Width,
                MaxDepth = 1.0f,
                MinDepth = 0.0f,
                X = 0f,
                Y = 0f
            });

            ID3D12GraphicsCommandList.SetScissorRectangles(new SharpDX.Mathematics.Interop.RawRectangle()
            {
                Left = 0,
                Right = surface.Width,
                Top = 0,
                Bottom = surface.Height
            });

            ID3D12GraphicsCommandList.ResourceBarrierTransition(surface.RenderTargetView[surface.IDXGISwapChain.CurrentBackBufferIndex],
                  SharpDX.Direct3D12.ResourceStates.Present, SharpDX.Direct3D12.ResourceStates.RenderTarget);

            var RTVHandle = surface.ID3D12RenderTargetViewHeap.CPUDescriptorHandleForHeapStart;
            var DSVHandle = surface.ID3D12DepthStencilViewHeap.CPUDescriptorHandleForHeapStart;

            RTVHandle += surface.IDXGISwapChain.CurrentBackBufferIndex * surface.ID3D12RenderTargetViewHeapSize;

            ID3D12GraphicsCommandList.SetRenderTargets(RTVHandle, DSVHandle);

            ID3D12GraphicsCommandList.ClearRenderTargetView(RTVHandle, new SharpDX.Mathematics.Interop.RawColor4(
                surface.BackGround.X, surface.BackGround.Y, surface.BackGround.Z, surface.BackGround.W));

            ID3D12GraphicsCommandList.ClearDepthStencilView(DSVHandle,
                SharpDX.Direct3D12.ClearFlags.FlagsDepth | SharpDX.Direct3D12.ClearFlags.FlagsStencil, 1.0f, 0);
        }

        public static void Close()
        {
            ID3D12GraphicsCommandList.ResourceBarrierTransition(surface.RenderTargetView[surface.IDXGISwapChain.CurrentBackBufferIndex],
                 SharpDX.Direct3D12.ResourceStates.RenderTarget, SharpDX.Direct3D12.ResourceStates.Present);

            ID3D12GraphicsCommandList.Close();

            Engine.ID3D12CommandQueue.ExecuteCommandList(ID3D12GraphicsCommandList);

            surface.IDXGISwapChain.Present(0, SharpDX.DXGI.PresentFlags.None);

            Engine.Wait();

            InputAssemblerStage.Reset();
            VertexShaderStage.Reset();
            PixelShaderStage.Reset();
            OutputMergerStage.Reset();

            surface = null;
            graphicsPipelineState = null;
        }

        internal static SharpDX.Direct3D12.GraphicsCommandList ID3D12GraphicsCommandList
        {
            private set => graphicsCommandList = value;
            get => graphicsCommandList;
        }
    }
}
