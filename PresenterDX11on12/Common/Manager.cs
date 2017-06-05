using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        private static SharpDX.Direct2D1.Device device2d;
        private static SharpDX.Direct2D1.Factory1 d2d1factory;
        private static SharpDX.Direct2D1.DeviceContext context2d;

        private static SharpDX.DirectWrite.Factory writefactory;

        private static SharpDX.WIC.ImagingFactory imagefactory;

        private static SharpDX.Direct3D11.Device device3d;
        private static SharpDX.Direct3D11.DeviceContext context3d;

        private static SharpDX.Direct3D11.Device11On12 device11on12;

        private static SharpDX.Direct3D12.Device d3d12device;
        private static SharpDX.Direct3D12.CommandQueue commandQueue;

        private static SharpDX.Direct3D12.Fence fence;

        private static long fenceValue;
        private static AutoResetEvent fenceEvent;

        private static Matrix3x2 transform = Matrix3x2.Identity;

        static Manager()
        {
#if DEBUG
            SharpDX.Direct3D12.DebugInterface.Get().EnableDebugLayer();
#endif
            ID3D12Device = new SharpDX.Direct3D12.Device(null, SharpDX.Direct3D.FeatureLevel.Level_11_0);

            ID3D12CommandQueue = ID3D12Device.CreateCommandQueue(new SharpDX.Direct3D12.CommandQueueDescription(SharpDX.Direct3D12.CommandListType.Direct));

            ID3D11Device = SharpDX.Direct3D11.Device.CreateFromDirect3D12(ID3D12Device,
                 SharpDX.Direct3D11.DeviceCreationFlags.BgraSupport, new SharpDX.Direct3D.FeatureLevel[] {
                  SharpDX.Direct3D.FeatureLevel.Level_11_0, SharpDX.Direct3D.FeatureLevel.Level_11_1, SharpDX.Direct3D.FeatureLevel.Level_12_0},
                 null, ID3D12CommandQueue);

            ID3D11on12Device = ID3D11Device.QueryInterface<SharpDX.Direct3D11.Device11On12>();
            
            ID3D11DeviceContext = ID3D11Device.ImmediateContext; 

            ID2D1Factory = new SharpDX.Direct2D1.Factory1(SharpDX.Direct2D1.FactoryType.SingleThreaded);

            IDWriteFactory = new SharpDX.DirectWrite.Factory(SharpDX.DirectWrite.FactoryType.Shared);

            ImagingFactory = new SharpDX.WIC.ImagingFactory();

            ID3D12Fence = ID3D12Device.CreateFence(1, SharpDX.Direct3D12.FenceFlags.None);
            fenceValue = 1;

            fenceEvent = new AutoResetEvent(false);

            ID2D1Device = new SharpDX.Direct2D1.Device(ID2D1Factory, ID3D11Device.QueryInterface<SharpDX.DXGI.Device>());

            ID2D1DeviceContext = new SharpDX.Direct2D1.DeviceContext(ID2D1Device, SharpDX.Direct2D1.DeviceContextOptions.None);
        }

        public static void ClearObject()
        {
            resouceInput.Reset();

            ID3D11DeviceContext.Rasterizer.SetViewport(new SharpDX.Mathematics.Interop.RawViewportF()
            {
                Width = surface.Width,
                Height = surface.Height,
                MinDepth = 0f,
                MaxDepth = 1f,
                X = 0,
                Y = 0
            });

            ID3D11DeviceContext.Rasterizer.SetScissorRectangle(0, 0, surface.Width, surface.Height);

            int index = surface.IDXGISwapChain.CurrentBackBufferIndex;

            ID3D11DeviceContext.OutputMerger.SetTargets(surface.ID3D11DepthStencilView,
                surface.ID3D11RenderTargetView[index]);
            
            ID2D1DeviceContext.Target = surface.Target;

            ID2D1DeviceContext.BeginDraw();

            ID3D11DeviceContext.ClearRenderTargetView(Surface.ID3D11RenderTargetView[index],
                new SharpDX.Mathematics.Interop.RawColor4(Surface.BackGround.red,
                Surface.BackGround.green, Surface.BackGround.blue, Surface.BackGround.alpha));
            
            ID3D11DeviceContext.ClearDepthStencilView(Surface.ID3D11DepthStencilView,
                SharpDX.Direct3D11.DepthStencilClearFlags.Depth | SharpDX.Direct3D11.DepthStencilClearFlags.Stencil, 1f, 0);
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
            ID2D1DeviceContext.EndDraw();

            ID3D11DeviceContext.Flush();

            Surface.IDXGISwapChain.Present(0, SharpDX.DXGI.PresentFlags.None);

            WaitForFrame();
        }

        internal static SharpDX.Direct2D1.Device ID2D1Device
        {
            private set => device2d = value;
            get => device2d;
        }

        internal static SharpDX.Direct2D1.DeviceContext ID2D1DeviceContext
        {
            private set => context2d = value;
            get => context2d;
        }

        internal static SharpDX.Direct2D1.Factory1 ID2D1Factory
        {
            private set => d2d1factory = value;
            get => d2d1factory;
        }

        internal static SharpDX.DirectWrite.Factory IDWriteFactory
        {
            private set => writefactory = value;
            get => writefactory;
        }

        internal static SharpDX.WIC.ImagingFactory ImagingFactory
        {
            private set => imagefactory = value;
            get => imagefactory;
        }

        internal static SharpDX.Direct3D11.Device ID3D11Device
        {
            private set => device3d = value;
            get => device3d;
        }

        internal static SharpDX.Direct3D11.DeviceContext ID3D11DeviceContext
        {
            private set => context3d = value;
            get => context3d;
        }

        internal static SharpDX.Direct3D12.Device ID3D12Device
        {
            private set => d3d12device = value;
            get => d3d12device;
        }

        internal static SharpDX.Direct3D12.CommandQueue ID3D12CommandQueue
        {
            private set => commandQueue = value;
            get => commandQueue;
        }

        internal static SharpDX.Direct3D11.Device11On12 ID3D11on12Device
        {
            private set => device11on12 = value;
            get => device11on12;
        }

        internal static SharpDX.Direct3D12.Fence ID3D12Fence
        {
            private set => fence = value;
            get => fence;
        }

        public static Matrix3x2 Transform
        {
            set
            {
                transform = value;
                
                ID2D1DeviceContext.Transform = new SharpDX.Mathematics.Interop.RawMatrix3x2()
                {
                    M11 = transform.M11,
                    M12 = transform.M12,
                    M21 = transform.M21,
                    M22 = transform.M22,
                    M31 = transform.M31,
                    M32 = transform.M32
                };
            }
            get => transform;
        }

        public static float AppScale => (DpiX + DpiY) / 192;

        public static float DpiX => d2d1factory.DesktopDpi.Width;
        public static float DpiY => d2d1factory.DesktopDpi.Height;
    }
}
