using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        private static int msaa4xQuality;

        static Manager()
        {
            ID3D11Device = new SharpDX.Direct3D11.Device(SharpDX.Direct3D.DriverType.Hardware,
                 SharpDX.Direct3D11.DeviceCreationFlags.BgraSupport);

            ID3D11DeviceContext = ID3D11Device.ImmediateContext; 

            Msaa4xQuality = ID3D11Device.CheckMultisampleQualityLevels(SharpDX.DXGI.Format.R8G8B8A8_UNorm, 4);

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
                new SharpDX.Mathematics.Interop.RawColor4(1, 1, 1, 1));

            ID3D11DeviceContext.ClearDepthStencilView(Surface.ID3D11DepthStencilView,
                SharpDX.Direct3D11.DepthStencilClearFlags.Depth | SharpDX.Direct3D11.DepthStencilClearFlags.Stencil, 1f, 0);
        }

        public static void FlushObject()
        {
            ID2D1DeviceContext.EndDraw();

            Surface.IDXGISwapChain.Present(0, SharpDX.DXGI.PresentFlags.None);
        }

        public static void PutObject((float x, float y) start, (float x, float y) end,
            Brush brush, float width = 1.0f)
        {
            ID2D1DeviceContext.DrawLine(new SharpDX.Mathematics.Interop.RawVector2(start.x, start.y),
                new SharpDX.Mathematics.Interop.RawVector2(end.x, end.y), brush,
                width);
        }

        public static void PutObject((float left, float top, float right, float bottom) rect,
            Brush brush, float width = 1.0f)
        {
            ID2D1DeviceContext.DrawRectangle(new SharpDX.Mathematics.Interop.RawRectangleF(rect.left, rect.top,
                rect.right, rect.bottom), brush, width);
        }

        public static void PutObject((float x, float y) center, float radiusx, float radiusy,
            Brush brush, float width = 1.0f)
        {
            ID2D1DeviceContext.DrawEllipse(new SharpDX.Direct2D1.Ellipse(
                new SharpDX.Mathematics.Interop.RawVector2(center.x, center.y), radiusx, radiusy),
                brush, width);
        }

        public static void PutObject(string text, (float x, float y) pos,
            Brush brush, Fontface fontface)
        {
            SharpDX.DirectWrite.TextLayout layout = new SharpDX.DirectWrite.TextLayout(
                IDWriteFactory, text, fontface, float.MaxValue, float.MaxValue);

            ID2D1DeviceContext.DrawTextLayout(new SharpDX.Mathematics.Interop.RawVector2(pos.x, pos.y),
                layout, brush);

            layout.Dispose();
        }

        public static void PutObject((float left, float top, float right, float bottom) rect,
            Bitmap bitmap)
        {
            ID2D1DeviceContext.DrawBitmap(bitmap, new SharpDX.Mathematics.Interop.RawRectangleF(
                rect.left, rect.top, rect.right, rect.bottom), 1f, SharpDX.Direct2D1.BitmapInterpolationMode.Linear);
        }

        public static void FillObject((float left, float top, float right, float bottom) rect,
            Brush brush)
        {
            ID2D1DeviceContext.FillRectangle(new SharpDX.Mathematics.Interop.RawRectangleF(rect.left, rect.top,
             rect.right, rect.bottom), brush);
        }

        public static void FillObject((float x, float y) center, float radiusx, float radiusy,
            Brush brush)
        {
            ID2D1DeviceContext.FillEllipse(new SharpDX.Direct2D1.Ellipse(
                new SharpDX.Mathematics.Interop.RawVector2(center.x, center.y), radiusx, radiusy),
                brush);
        }

        public static void DrawObject(int vertexCount, int startLocation = 0,
            PrimitiveType type = PrimitiveType.TriangleList)
        {
            ID3D11DeviceContext.InputAssembler.PrimitiveTopology = (SharpDX.Direct3D.PrimitiveTopology)type;

            ID3D11DeviceContext.Draw(vertexCount, startLocation);
        }

        public static void DrawObjectIndexed(int indexCount, int startLocation = 0,
            int baseVertexLocation = 0, PrimitiveType type = PrimitiveType.TriangleList)
        {
            ID3D11DeviceContext.InputAssembler.PrimitiveTopology = (SharpDX.Direct3D.PrimitiveTopology)type;

            ID3D11DeviceContext.DrawIndexed(indexCount, startLocation, baseVertexLocation);
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

        public static int Msaa4xQuality
        {
            private set => msaa4xQuality = value;
            get => msaa4xQuality;
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
        
        public static float AppScale => (DpiX + DpiY) / 192;

        public static float DpiX => d2d1factory.DesktopDpi.Width;
        public static float DpiY => d2d1factory.DesktopDpi.Height;
    }
}
