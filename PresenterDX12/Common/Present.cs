using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace Presenter
{
    public class Present
    {
        private SharpDX.Direct3D12.DescriptorHeap renderTargetViewHeap;
        private SharpDX.Direct3D12.DescriptorHeap depthStencilViewHeap;

        private SharpDX.Direct3D12.Resource[] renderTarget;
        private SharpDX.Direct3D12.Resource depthStencil;

        protected int width;
        protected int height;

        private Vector4 backGround = Vector4.One;

        private SharpDX.DXGI.SwapChain3 swapChain;

        private IntPtr handle;

        private void CreateDepthStencil()
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

        private void CreateDescriptorHeap(int renderCount)
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

        private void CreateResourceView(params SharpDX.Direct3D12.Resource[] RenderTarget)
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

        internal void ResetViewport()
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

        internal void ResetResourceView()
        {
            GraphicsPipeline.ID3D12GraphicsCommandList.ResourceBarrierTransition(renderTarget[swapChain.CurrentBackBufferIndex],
               SharpDX.Direct3D12.ResourceStates.Present, SharpDX.Direct3D12.ResourceStates.RenderTarget);

            var rtvHandle = ID3D12RenderTargetViewHeap.CPUDescriptorHandleForHeapStart +
                swapChain.CurrentBackBufferIndex * ResourceHeap.RenderTargetHeapSize;
            var dsvHandle = ID3D12DepthStencilViewHeap.CPUDescriptorHandleForHeapStart;

            GraphicsPipeline.ID3D12GraphicsCommandList.SetRenderTargets(rtvHandle, dsvHandle);

            GraphicsPipeline.ID3D12GraphicsCommandList.ClearRenderTargetView(rtvHandle, new SharpDX.Mathematics.Interop.RawColor4(
                backGround.X, backGround.Y, backGround.Z, backGround.W));

            GraphicsPipeline.ID3D12GraphicsCommandList.ClearDepthStencilView(dsvHandle,
                SharpDX.Direct3D12.ClearFlags.FlagsDepth | SharpDX.Direct3D12.ClearFlags.FlagsStencil, 1.0f, 0);
        }

        internal void ClearState()
        {
            GraphicsPipeline.ID3D12GraphicsCommandList.ResourceBarrierTransition(renderTarget[swapChain.CurrentBackBufferIndex],
               SharpDX.Direct3D12.ResourceStates.RenderTarget, SharpDX.Direct3D12.ResourceStates.Present);
        }

        internal void Presented()
        {
            swapChain.Present(0, SharpDX.DXGI.PresentFlags.None);
        }

        public Present(IntPtr Handle, bool Windowed = true)
        {
            handle = Handle;

            APILibrary.Win32.Rect realRect = new APILibrary.Win32.Rect();

            APILibrary.Win32.Internal.GetClientRect(handle, ref realRect);

            using (var factory = new SharpDX.DXGI.Factory4())
            {
                var tempSwapChain = new SharpDX.DXGI.SwapChain(factory, Engine.ID3D12CommandQueue,
                    new SharpDX.DXGI.SwapChainDescription()
                    {
                        BufferCount = BufferCount,
                        ModeDescription = new SharpDX.DXGI.ModeDescription()
                        {
                            Width = width = realRect.right - realRect.left,
                            Height = height = realRect.bottom - realRect.top,
                            Format = RenderTargetFormat,
                            RefreshRate = new SharpDX.DXGI.Rational(60, 1),
                            Scaling = SharpDX.DXGI.DisplayModeScaling.Unspecified,
                            ScanlineOrdering = SharpDX.DXGI.DisplayModeScanlineOrder.Unspecified
                        },
                        Usage = SharpDX.DXGI.Usage.RenderTargetOutput,
                        Flags = SharpDX.DXGI.SwapChainFlags.AllowModeSwitch,
                        SampleDescription = new SharpDX.DXGI.SampleDescription(1, 0),
                        SwapEffect = SharpDX.DXGI.SwapEffect.FlipDiscard,
                        OutputHandle = handle,
                        IsWindowed = Windowed
                    });

                swapChain = tempSwapChain.QueryInterface<SharpDX.DXGI.SwapChain3>();

                SharpDX.Utilities.Dispose(ref tempSwapChain);
            }

            renderTarget = new SharpDX.Direct3D12.Resource[BufferCount];

            for (int i = 0; i < BufferCount; i++)
            {
                renderTarget[i] = swapChain.GetBackBuffer<SharpDX.Direct3D12.Resource>(i);
            }

            CreateDepthStencil();
            CreateDescriptorHeap(2);
            CreateResourceView(renderTarget);
        }

        internal static int BufferCount => 2;

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

        internal static SharpDX.DXGI.Format DepthStencilFormat => SharpDX.DXGI.Format.D24_UNorm_S8_UInt;
        internal static SharpDX.DXGI.Format RenderTargetFormat => SharpDX.DXGI.Format.R8G8B8A8_UNorm;

        public int Width => width;
        public int Height => height;

        public Vector4 BackGround
        {
            set => backGround = value;
            get => backGround;
        }

        public IntPtr Handle => handle;

        ~Present()
        {
            SharpDX.Utilities.Dispose(ref depthStencil);
            SharpDX.Utilities.Dispose(ref renderTargetViewHeap);
            SharpDX.Utilities.Dispose(ref depthStencilViewHeap);
            for (int i = 0; i < renderTarget.Length; i++)
                SharpDX.Utilities.Dispose(ref renderTarget[i]);
            SharpDX.Utilities.Dispose(ref swapChain);
        }
    }

    public static partial class GraphicsPipeline
    {
        private static Present present;
    }

}
