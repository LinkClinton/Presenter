using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Presenter
{
    public static class Engine
    {
        private static SharpDX.Direct2D1.Factory1 d2d1Factory;

        private static SharpDX.WIC.ImagingFactory imagingFactory;

        private static SharpDX.Direct3D12.Device device;
        private static SharpDX.Direct3D12.CommandQueue commandQueue;
        private static SharpDX.Direct3D12.CommandAllocator commandAllocator;
        
        //Resource
        private static SharpDX.Direct3D12.CommandAllocator resourceCommandAllocator;

        private static SharpDX.Direct3D12.Fence fence;

        private static long fenceValue;
        private static AutoResetEvent fenceEvent;

        static Engine()
        {
#if DEBUG
            SharpDX.Direct3D12.DebugInterface.Get().EnableDebugLayer();
#endif
            ID3D12Device = new SharpDX.Direct3D12.Device(null, SharpDX.Direct3D.FeatureLevel.Level_11_0);

            ID3D12CommandQueue = ID3D12Device.CreateCommandQueue(
                new SharpDX.Direct3D12.CommandQueueDescription(SharpDX.Direct3D12.CommandListType.Direct));
            
            ID3D12CommandAllocator = ID3D12Device.CreateCommandAllocator(SharpDX.Direct3D12.CommandListType.Direct);

            resourceCommandAllocator = ID3D12Device.CreateCommandAllocator(SharpDX.Direct3D12.CommandListType.Direct);

            ID3D12Fence = ID3D12Device.CreateFence(1, SharpDX.Direct3D12.FenceFlags.None);
            fenceValue = 1;

            fenceEvent = new AutoResetEvent(false);

            ID2D1Factory = new SharpDX.Direct2D1.Factory1(SharpDX.Direct2D1.FactoryType.SingleThreaded);

            ImagingFactory = new SharpDX.WIC.ImagingFactory();
        }

        internal static void Wait(SharpDX.Direct3D12.CommandQueue commandQueue)
        {
            long localFence = fenceValue;
            commandQueue.Signal(ID3D12Fence, localFence);
            fenceValue++;

            if (ID3D12Fence.CompletedValue < localFence)
            {
                ID3D12Fence.SetEventOnCompletion(localFence, fenceEvent.SafeWaitHandle.DangerousGetHandle());
                fenceEvent.WaitOne();
            }
        }

        internal static void Wait()
        {
            Wait(ID3D12CommandQueue);
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

        internal static SharpDX.Direct3D12.CommandAllocator ResourceCommandAllocator
            => resourceCommandAllocator;

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

        public static float DpiX => d2d1Factory.DesktopDpi.Width;
        public static float DpiY => d2d1Factory.DesktopDpi.Height;

        public static float AppScale => (DpiX + DpiY) / 192;

    }
}
