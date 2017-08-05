using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace Presenter
{
    public class TextureFace : Texture2D
    {
        private SharpDX.Direct3D12.Resource depthStencil;

        private SharpDX.Direct3D12.DescriptorHeap renderTargetViewHeap;
        private SharpDX.Direct3D12.DescriptorHeap depthStencilViewHeap;

        private bool enableDepth;

        private Vector4 backGround = Vector4.One;

        private void CreateDepthStencil()
        {
            SharpDX.Utilities.Dispose(ref depthStencil);

            depthStencil = Engine.ID3D12Device.CreateCommittedResource(
                new SharpDX.Direct3D12.HeapProperties(SharpDX.Direct3D12.HeapType.Default),
                 SharpDX.Direct3D12.HeapFlags.None, new SharpDX.Direct3D12.ResourceDescription()
                 {
                     Dimension = SharpDX.Direct3D12.ResourceDimension.Texture2D,
                     Alignment = 0,
                     Width = Width,
                     Height = Height,
                     DepthOrArraySize = 1,
                     MipLevels = 1,
                     Format = SharpDX.DXGI.Format.R24G8_Typeless,
                     SampleDescription = new SharpDX.DXGI.SampleDescription(1, 0),
                     Layout = SharpDX.Direct3D12.TextureLayout.Unknown,
                     Flags = SharpDX.Direct3D12.ResourceFlags.AllowDepthStencil
                 }, SharpDX.Direct3D12.ResourceStates.Common,
                 new SharpDX.Direct3D12.ClearValue()
                 {
                     Format = Present.DepthStencilFormat,
                     DepthStencil = new SharpDX.Direct3D12.DepthStencilValue()
                     {
                         Depth = 1.0f,
                         Stencil = 0
                     }
                 });
        }

        private void CreateDescriptorHeap()
        {
            renderTargetViewHeap = Engine.ID3D12Device.CreateDescriptorHeap(
                new SharpDX.Direct3D12.DescriptorHeapDescription()
                {
                    DescriptorCount = 1,
                    Flags = SharpDX.Direct3D12.DescriptorHeapFlags.None,
                    Type = SharpDX.Direct3D12.DescriptorHeapType.RenderTargetView,
                    NodeMask = 0
                });

            depthStencilViewHeap = Engine.ID3D12Device.CreateDescriptorHeap(
                new SharpDX.Direct3D12.DescriptorHeapDescription()
                {
                    DescriptorCount = 1,
                    Flags = SharpDX.Direct3D12.DescriptorHeapFlags.None,
                    Type = SharpDX.Direct3D12.DescriptorHeapType.DepthStencilView,
                    NodeMask = 0
                });
        }

        private void CreateResourceView(SharpDX.Direct3D12.Resource RenderTarget)
        {
            var rtvHandle = renderTargetViewHeap.CPUDescriptorHandleForHeapStart;
            var dsvHandle = depthStencilViewHeap.CPUDescriptorHandleForHeapStart;

            Engine.ID3D12Device.CreateRenderTargetView(RenderTarget, null, rtvHandle);
            rtvHandle += ResourceHeap.RenderTargetHeapSize;

            if (enableDepth is true)
            {

                Engine.ID3D12Device.CreateDepthStencilView(depthStencil, new SharpDX.Direct3D12.DepthStencilViewDescription()
                {
                    Dimension = SharpDX.Direct3D12.DepthStencilViewDimension.Texture2D,
                    Flags = SharpDX.Direct3D12.DepthStencilViewFlags.None,
                    Format = Present.DepthStencilFormat,
                    Texture2D = new SharpDX.Direct3D12.DepthStencilViewDescription.Texture2DResource() { MipSlice = 0 }
                }, dsvHandle);

                Engine.ResourceCommandAllocator.Reset();

                using (var CommandList = Engine.ID3D12Device.CreateCommandList(SharpDX.Direct3D12.CommandListType.Direct,
                   Engine.ResourceCommandAllocator, null))
                {
                    CommandList.ResourceBarrierTransition(depthStencil, SharpDX.Direct3D12.ResourceStates.Common,
                         SharpDX.Direct3D12.ResourceStates.DepthWrite);

                    CommandList.Close();

                    Engine.ID3D12CommandQueue.ExecuteCommandList(CommandList);

                    Engine.Wait();
                }
            }
        }

        internal void ResetViewport()
        {
            GraphicsPipeline.ID3D12GraphicsCommandList.SetViewport(new SharpDX.Mathematics.Interop.RawViewportF()
            {
                Height = Height,
                Width = Width,
                MaxDepth = 1.0f,
                MinDepth = 0.0f,
                X = 0f,
                Y = 0f
            });

            GraphicsPipeline.ID3D12GraphicsCommandList.SetScissorRectangles(new SharpDX.Mathematics.Interop.RawRectangle()
            {
                Left = 0,
                Right = Width,
                Top = 0,
                Bottom = Height
            });
        }

        internal void ResetResourceView()
        {
            if (enableDepth is true)
            {
                GraphicsPipeline.ID3D12GraphicsCommandList.ResourceBarrierTransition(ID3D12Resource,
                   SharpDX.Direct3D12.ResourceStates.NonPixelShaderResource, SharpDX.Direct3D12.ResourceStates.RenderTarget);

                var rtvHandle = renderTargetViewHeap.CPUDescriptorHandleForHeapStart;
                var dsvHandle = depthStencilViewHeap.CPUDescriptorHandleForHeapStart;

                GraphicsPipeline.ID3D12GraphicsCommandList.SetRenderTargets(rtvHandle, dsvHandle);

                GraphicsPipeline.ID3D12GraphicsCommandList.ClearRenderTargetView(rtvHandle, new SharpDX.Mathematics.Interop.RawColor4(
                    backGround.X, backGround.Y, backGround.Z, backGround.W));

                GraphicsPipeline.ID3D12GraphicsCommandList.ClearDepthStencilView(dsvHandle,
                    SharpDX.Direct3D12.ClearFlags.FlagsDepth | SharpDX.Direct3D12.ClearFlags.FlagsStencil, 1.0f, 0);
            }
            else
            {
                GraphicsPipeline.ID3D12GraphicsCommandList.ResourceBarrierTransition(ID3D12Resource,
                 SharpDX.Direct3D12.ResourceStates.NonPixelShaderResource, SharpDX.Direct3D12.ResourceStates.RenderTarget);

                var rtvHandle = renderTargetViewHeap.CPUDescriptorHandleForHeapStart;

                GraphicsPipeline.ID3D12GraphicsCommandList.SetRenderTargets(rtvHandle, null);

                GraphicsPipeline.ID3D12GraphicsCommandList.ClearRenderTargetView(rtvHandle, new SharpDX.Mathematics.Interop.RawColor4(
                    backGround.X, backGround.Y, backGround.Z, backGround.W));
            }
        }

        internal void ClearState()
        {
            GraphicsPipeline.ID3D12GraphicsCommandList.ResourceBarrierTransition(ID3D12Resource,
               SharpDX.Direct3D12.ResourceStates.RenderTarget, SharpDX.Direct3D12.ResourceStates.NonPixelShaderResource);
        }

        internal void Presented()
        {

        }

        public TextureFace(int width, int height, bool enableDepthBuffer = false)
           : base(width, height, (ResourceFormat)Present.RenderTargetFormat, 1)
        {
            enableDepth = enableDepthBuffer;

            if (enableDepth is true)
            {
                CreateDepthStencil();
            }

            CreateDescriptorHeap();
            CreateResourceView(ID3D12Resource);
        }

        public bool EnableDepthTest => enableDepth;

        public Vector4 BackGround
        {
            set => backGround = value;
            get => backGround;
        }
    }

    public static partial class GraphicsPipeline
    {
        private static TextureFace textureFace;
    }
}
