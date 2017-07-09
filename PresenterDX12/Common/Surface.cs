using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace Presenter
{
    public partial class Surface
    {
        private SharpDX.Direct3D12.DescriptorHeap renderTargetViewHeap;
        private SharpDX.Direct3D12.DescriptorHeap depthStencilViewHeap;

        private SharpDX.Direct3D12.Resource renderTarget;
        private SharpDX.Direct3D12.Resource depthStencil;

        protected int width;
        protected int height;

        private Vector4 backGround = Vector4.One;

        protected virtual void CreateRenderTarget()
        {
            SharpDX.Utilities.Dispose(ref renderTarget);

            renderTarget = Engine.ID3D12Device.CreateCommittedResource(
                new SharpDX.Direct3D12.HeapProperties(SharpDX.Direct3D12.HeapType.Default),
                 SharpDX.Direct3D12.HeapFlags.None, new SharpDX.Direct3D12.ResourceDescription()
                 {
                     Dimension = SharpDX.Direct3D12.ResourceDimension.Texture2D,
                     Alignment = 0,
                     Width = width,
                     Height = height,
                     DepthOrArraySize = 1,
                     MipLevels = 1,
                     Format = RenderTargetFormat,
                     SampleDescription = new SharpDX.DXGI.SampleDescription(1, 0),
                     Layout = SharpDX.Direct3D12.TextureLayout.Unknown,
                     Flags = SharpDX.Direct3D12.ResourceFlags.AllowRenderTarget
                 }, SharpDX.Direct3D12.ResourceStates.Common);
        }

        protected virtual void CreateDepthStencil()
        {
            SharpDX.Utilities.Dispose(ref depthStencil);

            depthStencil = Engine.ID3D12Device.CreateCommittedResource(
                new SharpDX.Direct3D12.HeapProperties(SharpDX.Direct3D12.HeapType.Default),
                 SharpDX.Direct3D12.HeapFlags.None, new SharpDX.Direct3D12.ResourceDescription()
                 {
                     Dimension = SharpDX.Direct3D12.ResourceDimension.Texture2D,
                     Alignment = 0,
                     Width = width,
                     Height = height,
                     DepthOrArraySize = 1,
                     MipLevels = 1,
                     Format = SharpDX.DXGI.Format.R24G8_Typeless,
                     SampleDescription = new SharpDX.DXGI.SampleDescription(1, 0),
                     Layout = SharpDX.Direct3D12.TextureLayout.Unknown,
                     Flags = SharpDX.Direct3D12.ResourceFlags.AllowDepthStencil
                 }, SharpDX.Direct3D12.ResourceStates.Common,
                 new SharpDX.Direct3D12.ClearValue()
                 {
                     Format = DepthStencilFormat,
                     DepthStencil = new SharpDX.Direct3D12.DepthStencilValue()
                     {
                         Depth = 1.0f,
                         Stencil = 0
                     }
                 });
        }

        protected virtual void CreateDescriptorHeap(int renderCount)
        {
            ID3D12RenderTargetViewHeap = Engine.ID3D12Device.CreateDescriptorHeap(
                new SharpDX.Direct3D12.DescriptorHeapDescription()
                {
                    DescriptorCount = renderCount,
                    Flags = SharpDX.Direct3D12.DescriptorHeapFlags.None,
                    Type = SharpDX.Direct3D12.DescriptorHeapType.RenderTargetView,
                    NodeMask = 0
                });

            ID3D12DepthStencilViewHeap = Engine.ID3D12Device.CreateDescriptorHeap(
                new SharpDX.Direct3D12.DescriptorHeapDescription()
                {
                    DescriptorCount = 1,
                    Flags = SharpDX.Direct3D12.DescriptorHeapFlags.None,
                    Type = SharpDX.Direct3D12.DescriptorHeapType.DepthStencilView,
                    NodeMask = 0
                });
        }

        protected virtual void CreateResourceView(params SharpDX.Direct3D12.Resource[] RenderTarget)
        {
            var rtvHandle = renderTargetViewHeap.CPUDescriptorHandleForHeapStart;
            var dsvHandle = depthStencilViewHeap.CPUDescriptorHandleForHeapStart;

            foreach (var item in RenderTarget)
            {
                Engine.ID3D12Device.CreateRenderTargetView(item, null, rtvHandle);
                rtvHandle += ResourceHeap.RenderTargetHeapSize;
            }

            Engine.ID3D12Device.CreateDepthStencilView(depthStencil, new SharpDX.Direct3D12.DepthStencilViewDescription()
            {
                Dimension = SharpDX.Direct3D12.DepthStencilViewDimension.Texture2D,
                Flags = SharpDX.Direct3D12.DepthStencilViewFlags.None,
                Format = DepthStencilFormat,
                Texture2D = new SharpDX.Direct3D12.DepthStencilViewDescription.Texture2DResource() { MipSlice = 0 }
            }, dsvHandle);

            using (var CommandList = Engine.ID3D12Device.CreateCommandList(SharpDX.Direct3D12.CommandListType.Direct,
               Engine.ID3D12CommandAllocator, null))
            {
                CommandList.ResourceBarrierTransition(depthStencil, SharpDX.Direct3D12.ResourceStates.Common,
                     SharpDX.Direct3D12.ResourceStates.DepthWrite);

                CommandList.Close();

                Engine.ID3D12CommandQueue.ExecuteCommandList(CommandList);

                Engine.Wait();
            }
        }

        internal virtual void ResetViewport()
        {
            GraphicsPipeline.ID3D12GraphicsCommandList.SetViewport(new SharpDX.Mathematics.Interop.RawViewportF()
            {
                Height = height,
                Width = width,
                MaxDepth = 1.0f,
                MinDepth = 0.0f,
                X = 0f,
                Y = 0f
            });

            GraphicsPipeline.ID3D12GraphicsCommandList.SetScissorRectangles(new SharpDX.Mathematics.Interop.RawRectangle()
            {
                Left = 0,
                Right = width,
                Top = 0,
                Bottom = height
            });
        }

        internal virtual void ResetResourceView()
        {
            GraphicsPipeline.ID3D12GraphicsCommandList.ResourceBarrierTransition(ID3D12RenderTarget,
               SharpDX.Direct3D12.ResourceStates.Common, SharpDX.Direct3D12.ResourceStates.RenderTarget);

            var rtvHandle = ID3D12RenderTargetViewHeap.CPUDescriptorHandleForHeapStart;
            var dsvHandle = ID3D12DepthStencilViewHeap.CPUDescriptorHandleForHeapStart;

            GraphicsPipeline.ID3D12GraphicsCommandList.SetRenderTargets(rtvHandle, dsvHandle);

            GraphicsPipeline.ID3D12GraphicsCommandList.ClearRenderTargetView(rtvHandle, new SharpDX.Mathematics.Interop.RawColor4(
                BackGround.X, BackGround.Y, BackGround.Z, BackGround.W));

            GraphicsPipeline.ID3D12GraphicsCommandList.ClearDepthStencilView(dsvHandle,
                SharpDX.Direct3D12.ClearFlags.FlagsDepth | SharpDX.Direct3D12.ClearFlags.FlagsStencil, 1.0f, 0);
        }

        internal virtual void ClearState()
        {
            GraphicsPipeline.ID3D12GraphicsCommandList.ResourceBarrierTransition(ID3D12RenderTarget,
               SharpDX.Direct3D12.ResourceStates.RenderTarget, SharpDX.Direct3D12.ResourceStates.Common);
        }

        internal virtual void Presented()
        {

        }

        internal Surface() { }

        public Surface(int Width, int Height)
        {
            width = Width;
            height = Height;

            CreateRenderTarget();
            CreateDepthStencil();
            CreateDescriptorHeap(1);
            CreateResourceView(renderTarget);
        }

        internal SharpDX.Direct3D12.DescriptorHeap ID3D12RenderTargetViewHeap
        {
            set => renderTargetViewHeap = value;
            get => renderTargetViewHeap;
        }

        internal SharpDX.Direct3D12.DescriptorHeap ID3D12DepthStencilViewHeap
        {
            set => depthStencilViewHeap = value;
            get => depthStencilViewHeap;
        }

        internal SharpDX.Direct3D12.Resource ID3D12RenderTarget => renderTarget;

        internal static SharpDX.DXGI.Format DepthStencilFormat => SharpDX.DXGI.Format.D24_UNorm_S8_UInt;
        internal static SharpDX.DXGI.Format RenderTargetFormat => SharpDX.DXGI.Format.R8G8B8A8_UNorm;

        public int Width => width;
        public int Height => height;

        public Vector4 BackGround
        {
            set => backGround = value;
            get => backGround;
        }

        ~Surface()
        {
            SharpDX.Utilities.Dispose(ref renderTarget);
            SharpDX.Utilities.Dispose(ref depthStencil);

            SharpDX.Utilities.Dispose(ref renderTargetViewHeap);
            SharpDX.Utilities.Dispose(ref depthStencilViewHeap);
        }
    }

    public static partial class GraphicsPipeline
    {
        private static Surface target;

        public static Surface Target => target;
    }
}
