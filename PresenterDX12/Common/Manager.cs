using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Numerics;

namespace Presenter
{
    public enum CullMode
    {
        CullNone = 1,
        CullFront = 2,
        CullBack = 3
    }

    public enum FillMode
    {
        Wireframe = 2,
        Solid = 3
    }

    public static partial class Manager
    {
        private static SharpDX.Direct2D1.Factory1 d2d1Factory;

        private static SharpDX.WIC.ImagingFactory imagingFactory;

        private static SharpDX.Direct3D12.Device device;
        private static SharpDX.Direct3D12.CommandQueue commandQueue;
        private static SharpDX.Direct3D12.CommandAllocator commandAllocator;
        private static SharpDX.Direct3D12.GraphicsCommandList graphicsCommandList;

        private static SharpDX.Direct3D12.RootSignature rootSignature;

        private static SharpDX.Direct3D12.Fence fence;

        private static long fenceValue;
        private static AutoResetEvent fenceEvent;

        static Manager()
        {
            ID3D12Device = new SharpDX.Direct3D12.Device(null, SharpDX.Direct3D.FeatureLevel.Level_11_0);

            ID3D12CommandQueue = ID3D12Device.CreateCommandQueue(
                new SharpDX.Direct3D12.CommandQueueDescription(SharpDX.Direct3D12.CommandListType.Direct));

            ID3D12CommandAllocator = ID3D12Device.CreateCommandAllocator(SharpDX.Direct3D12.CommandListType.Direct);

            ID3D12RootSignature = ID3D12Device.CreateRootSignature(
                new SharpDX.Direct3D12.RootSignatureDescription(SharpDX.Direct3D12.RootSignatureFlags.AllowInputAssemblerInputLayout)
                .Serialize());

            ID3D12GraphicsCommandList = ID3D12Device.CreateCommandList(SharpDX.Direct3D12.CommandListType.Direct,
                ID3D12CommandAllocator, null);

            ID3D12GraphicsCommandList.Close();

            ID3D12Fence = ID3D12Device.CreateFence(1, SharpDX.Direct3D12.FenceFlags.None);
            fenceValue = 1;

            fenceEvent = new AutoResetEvent(false);

            ID2D1Factory = new SharpDX.Direct2D1.Factory1(SharpDX.Direct2D1.FactoryType.SingleThreaded);

            ImagingFactory = new SharpDX.WIC.ImagingFactory();
        }

        public static void ClearObject()
        {
            ID3D12CommandAllocator.Reset();

            ID3D12GraphicsCommandList.Reset(ID3D12CommandAllocator, null);


            ID3D12GraphicsCommandList.SetViewport(new SharpDX.Mathematics.Interop.RawViewportF()
            {
                Height = surface.Height,
                Width = surface.Width,
                MaxDepth = 1f,
                MinDepth = 0f,
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

            ID3D12GraphicsCommandList.SetGraphicsRootSignature(ID3D12RootSignature);

            ID3D12GraphicsCommandList.ResourceBarrierTransition(surface.RenderTargetView[surface.IDXGISwapChain.CurrentBackBufferIndex],
                 SharpDX.Direct3D12.ResourceStates.Present, SharpDX.Direct3D12.ResourceStates.RenderTarget);

            var RTVHandle = surface.ID3D12RenderTargetViewHeap.CPUDescriptorHandleForHeapStart;
            RTVHandle += surface.IDXGISwapChain.CurrentBackBufferIndex * surface.ID3D12RenderTargetViewHeapSize;
            ID3D12GraphicsCommandList.SetRenderTargets(RTVHandle, null);

            ID3D12GraphicsCommandList.ClearRenderTargetView(RTVHandle, new SharpDX.Mathematics.Interop.RawColor4(
                surface.BackGround.red, surface.BackGround.green, surface.BackGround.blue, surface.BackGround.alpha));
        }

        private static void WaitForFrame()
        {
            long localFence = fenceValue;
            ID3D12CommandQueue.Signal(ID3D12Fence, localFence);
            fenceValue++;

            if (ID3D12Fence.CompletedValue < localFence)
            {
                ID3D12Fence.SetEventOnCompletion(localFence, fenceEvent.SafeWaitHandle.DangerousGetHandle());
                fenceEvent.WaitOne();
            }

        }

        public static void FlushObject()
        {
            ID3D12GraphicsCommandList.ResourceBarrierTransition(surface.RenderTargetView[surface.IDXGISwapChain.CurrentBackBufferIndex],
                 SharpDX.Direct3D12.ResourceStates.RenderTarget, SharpDX.Direct3D12.ResourceStates.Present);

            ID3D12GraphicsCommandList.Close();

            ID3D12CommandQueue.ExecuteCommandList(ID3D12GraphicsCommandList);

            surface.IDXGISwapChain.Present(1, 0);

            WaitForFrame();
        }

     

        internal static SharpDX.Direct3D12.Device ID3D12Device
        {
            private set => device = value;
            get => device;
        }

        internal static SharpDX.Direct3D12.CommandQueue ID3D12CommandQueue
        {
            private set => commandQueue = value;
            get => commandQueue;
        }

        internal static SharpDX.Direct3D12.CommandAllocator ID3D12CommandAllocator
        {
            private set => commandAllocator = value;
            get => commandAllocator;
        }

        internal static SharpDX.Direct3D12.GraphicsCommandList ID3D12GraphicsCommandList
        {
            private set => graphicsCommandList = value;
            get => graphicsCommandList;
        }

        internal static SharpDX.Direct3D12.RootSignature ID3D12RootSignature
        {
            private set => rootSignature = value;
            get => rootSignature;
        }

        internal static SharpDX.Direct3D12.Fence ID3D12Fence
        {
            private set => fence = value;
            get => fence;
        }

        internal static SharpDX.Direct2D1.Factory1 ID2D1Factory
        {
            private set => d2d1Factory = value;
            get => d2d1Factory;
        }

        internal static SharpDX.WIC.ImagingFactory ImagingFactory
        {
            private set => imagingFactory = value;
            get => imagingFactory;
        }

        public static float AppScale => (DpiX + DpiY) / 192;

        public static float DpiX => d2d1Factory.DesktopDpi.Width;
        public static float DpiY => d2d1Factory.DesktopDpi.Height;
    }
}
