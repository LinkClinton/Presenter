using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace Presenter
{
    public static partial class Manager
    {
        private static SharpDX.Direct2D1.Factory1 d2d1factory;
        
        private static SharpDX.WIC.ImagingFactory imagefactory;

        private static SharpDX.Direct3D11.Device device3d;
        private static SharpDX.Direct3D11.DeviceContext context3d;
        
        static Manager()
        {
#if DEBUG
            ID3D11Device = new SharpDX.Direct3D11.Device(SharpDX.Direct3D.DriverType.Hardware,
                 SharpDX.Direct3D11.DeviceCreationFlags.Debug);
#else
            ID3D11Device = new SharpDX.Direct3D11.Device(SharpDX.Direct3D.DriverType.Hardware);
#endif
            ID3D11DeviceContext = ID3D11Device.ImmediateContext; 

            ID2D1Factory = new SharpDX.Direct2D1.Factory1(SharpDX.Direct2D1.FactoryType.SingleThreaded);

            ImagingFactory = new SharpDX.WIC.ImagingFactory();
        }

        public static void ClearObject()
        {
            ResourceLayout.InputSlot.Reset();

            ID3D11DeviceContext.Rasterizer.SetViewport(new SharpDX.Mathematics.Interop.RawViewportF()
            {
                Width = surface.Width,
                Height = surface.Height,
                MinDepth = 0f,
                MaxDepth = 1f,
                X = 0,
                Y = 0
            });
            
            ID3D11DeviceContext.OutputMerger.SetTargets(surface.ID3D11DepthStencilView,
                surface.ID3D11RenderTargetView);

            ID3D11DeviceContext.ClearRenderTargetView(Surface.ID3D11RenderTargetView,
                new SharpDX.Mathematics.Interop.RawColor4(Surface.BackGround.red,
                Surface.BackGround.green, Surface.BackGround.blue, Surface.BackGround.alpha));

            ID3D11DeviceContext.ClearDepthStencilView(Surface.ID3D11DepthStencilView,
                SharpDX.Direct3D11.DepthStencilClearFlags.Depth | SharpDX.Direct3D11.DepthStencilClearFlags.Stencil, 1f, 0);
        }

        public static void FlushObject()
        {
            Surface.IDXGISwapChain.Present(0, SharpDX.DXGI.PresentFlags.None);
        }

        internal static SharpDX.Direct2D1.Factory1 ID2D1Factory
        {
            private set => d2d1factory = value;
            get => d2d1factory;
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

        public static float AppScale => (DpiX + DpiY) / 192;

        public static float DpiX => d2d1factory.DesktopDpi.Width;
        public static float DpiY => d2d1factory.DesktopDpi.Height;
    }
}
