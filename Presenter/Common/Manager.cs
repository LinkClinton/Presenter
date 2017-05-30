using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        private static Matrix3x2 transform = Matrix3x2.Identity;

        static Manager()
        {
            ID3D11Device = new SharpDX.Direct3D11.Device(SharpDX.Direct3D.DriverType.Hardware,
                 SharpDX.Direct3D11.DeviceCreationFlags.BgraSupport);
            
            ID3D11DeviceContext = ID3D11Device.ImmediateContext; 

            ID2D1Factory = new SharpDX.Direct2D1.Factory1(SharpDX.Direct2D1.FactoryType.SingleThreaded);

            IDWriteFactory = new SharpDX.DirectWrite.Factory(SharpDX.DirectWrite.FactoryType.Shared);

            ImagingFactory = new SharpDX.WIC.ImagingFactory();

            ID2D1Device = new SharpDX.Direct2D1.Device(ID2D1Factory, ID3D11Device.QueryInterface<SharpDX.DXGI.Device>());

            ID2D1DeviceContext = new SharpDX.Direct2D1.DeviceContext(ID2D1Device, SharpDX.Direct2D1.DeviceContextOptions.None);

            SharpDX.Direct3D11.RasterizerStateDescription raster_desc = new SharpDX.Direct3D11.RasterizerStateDescription()
            {
                FillMode = SharpDX.Direct3D11.FillMode.Solid,
                CullMode = SharpDX.Direct3D11.CullMode.None,
                IsFrontCounterClockwise = false,
                DepthBias = 0,
                SlopeScaledDepthBias = 0,
                DepthBiasClamp = 0,
                IsDepthClipEnabled = true,
                IsScissorEnabled = false,
                IsMultisampleEnabled = false,
                IsAntialiasedLineEnabled = false
            };

            context3d.Rasterizer.State = new SharpDX.Direct3D11.RasterizerState(ID3D11Device, raster_desc);
        }

        public static void ClearObject()
        {
            ID2D1DeviceContext.BeginDraw();

            ID3D11DeviceContext.ClearRenderTargetView(Surface.ID3D11RenderTargetView,
                new SharpDX.Mathematics.Interop.RawColor4(Surface.BackGround.red,
                Surface.BackGround.green, Surface.BackGround.blue, Surface.BackGround.alpha));

            ID3D11DeviceContext.ClearDepthStencilView(Surface.ID3D11DepthStencilView,
                SharpDX.Direct3D11.DepthStencilClearFlags.Depth | SharpDX.Direct3D11.DepthStencilClearFlags.Stencil, 1f, 0);
        }

        public static void FlushObject()
        {
            ID2D1DeviceContext.EndDraw();

            Surface.IDXGISwapChain.Present(0, SharpDX.DXGI.PresentFlags.None);
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

        public static CullMode CullMode
        {
            get => (CullMode)ID3D11DeviceContext.Rasterizer.State.Description.CullMode;
            set
            {
                SharpDX.Direct3D11.RasterizerStateDescription desc = ID3D11DeviceContext.Rasterizer.State.Description;

                desc.CullMode = (SharpDX.Direct3D11.CullMode)value;

                ID3D11DeviceContext.Rasterizer.State = new SharpDX.Direct3D11.RasterizerState(ID3D11Device, desc);
            }
        }

        public static FillMode FillMode
        {
            get => (FillMode)ID3D11DeviceContext.Rasterizer.State.Description.FillMode;
            set
            {
                SharpDX.Direct3D11.RasterizerStateDescription desc = ID3D11DeviceContext.Rasterizer.State.Description;

                desc.FillMode = (SharpDX.Direct3D11.FillMode)value;

                ID3D11DeviceContext.Rasterizer.State = new SharpDX.Direct3D11.RasterizerState(ID3D11Device, desc);
            }
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
