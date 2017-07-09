using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace Presenter
{
    public class Present : Surface
    {
        private SharpDX.DXGI.SwapChain3 swapChain;

        private SharpDX.Direct3D12.Resource[] renderTarget;

        private IntPtr handle;

        internal override void ResetResourceView()
        {
            GraphicsPipeline.ID3D12GraphicsCommandList.ResourceBarrierTransition(renderTarget[swapChain.CurrentBackBufferIndex],
               SharpDX.Direct3D12.ResourceStates.Present, SharpDX.Direct3D12.ResourceStates.RenderTarget);

            var rtvHandle = ID3D12RenderTargetViewHeap.CPUDescriptorHandleForHeapStart +
                swapChain.CurrentBackBufferIndex * ResourceHeap.RenderTargetHeapSize;
            var dsvHandle = ID3D12DepthStencilViewHeap.CPUDescriptorHandleForHeapStart;

            GraphicsPipeline.ID3D12GraphicsCommandList.SetRenderTargets(rtvHandle, dsvHandle);

            GraphicsPipeline.ID3D12GraphicsCommandList.ClearRenderTargetView(rtvHandle, new SharpDX.Mathematics.Interop.RawColor4(
                BackGround.X, BackGround.Y, BackGround.Z, BackGround.W));

            GraphicsPipeline.ID3D12GraphicsCommandList.ClearDepthStencilView(dsvHandle,
                SharpDX.Direct3D12.ClearFlags.FlagsDepth | SharpDX.Direct3D12.ClearFlags.FlagsStencil, 1.0f, 0);
        }

        internal override void ClearState()
        {
            GraphicsPipeline.ID3D12GraphicsCommandList.ResourceBarrierTransition(renderTarget[swapChain.CurrentBackBufferIndex],
               SharpDX.Direct3D12.ResourceStates.RenderTarget, SharpDX.Direct3D12.ResourceStates.Present);
        }

        internal override void Presented()
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

        public IntPtr Handle => handle;

        ~Present()
        {
            for (int i = 0; i < renderTarget.Length; i++)
                SharpDX.Utilities.Dispose(ref renderTarget[i]);
            SharpDX.Utilities.Dispose(ref swapChain);
        }
    }

}
