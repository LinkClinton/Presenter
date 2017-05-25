using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presenter
{
    public class Surface
    {
        private SharpDX.DXGI.SwapChain swapchain;

        private SharpDX.Direct3D11.RenderTargetView surfaceRTV;
        private SharpDX.Direct3D11.DepthStencilView surfaceDSV;

        private SharpDX.Direct2D1.Bitmap1 surfaceTarget;

        private IntPtr surfaceHandle;

        private Brush background = Brush.Context[(1, 1, 1, 1)];

        public Surface(IntPtr handle, bool windowed = true)
        {
            surfaceHandle = handle;

            APILibrary.Win32.Rect rect = new APILibrary.Win32.Rect();

            APILibrary.Win32.Internal.GetClientRect(handle, ref rect);

            int width = rect.right - rect.left;
            int height = rect.bottom - rect.top;

            SharpDX.DXGI.Device dxgidevice = Manager.ID3D11Device.QueryInterface<SharpDX.DXGI.Device>();
            SharpDX.DXGI.Adapter dxgiadapte = dxgidevice.GetParent<SharpDX.DXGI.Adapter>();
            SharpDX.DXGI.Factory dxgifactory = dxgiadapte.GetParent<SharpDX.DXGI.Factory>();

            swapchain = new SharpDX.DXGI.SwapChain(dxgifactory, Manager.ID3D11Device,
                new SharpDX.DXGI.SwapChainDescription()
                {
                    ModeDescription = new SharpDX.DXGI.ModeDescription()
                    {
                        Width = width,
                        Height = height,
                        RefreshRate = new SharpDX.DXGI.Rational(60, 1),
                        Scaling = SharpDX.DXGI.DisplayModeScaling.Unspecified,
                        ScanlineOrdering = SharpDX.DXGI.DisplayModeScanlineOrder.Unspecified,
                        Format = SharpDX.DXGI.Format.R8G8B8A8_UNorm,
                    },
                    BufferCount = 1,
                    Usage = SharpDX.DXGI.Usage.RenderTargetOutput,
                    Flags = SharpDX.DXGI.SwapChainFlags.None,
                    OutputHandle = handle,
                    SampleDescription = new SharpDX.DXGI.SampleDescription(4, Manager.Msaa4xQuality - 1),
                    SwapEffect = SharpDX.DXGI.SwapEffect.Discard,
                    IsWindowed = windowed
                });


            surfaceRTV = new SharpDX.Direct3D11.RenderTargetView(Manager.ID3D11Device,
                swapchain.GetBackBuffer<SharpDX.Direct3D11.Texture2D>(0));



            surfaceDSV = new SharpDX.Direct3D11.DepthStencilView(Manager.ID3D11Device,
                new SharpDX.Direct3D11.Texture2D(Manager.ID3D11Device, new SharpDX.Direct3D11.Texture2DDescription()
                {
                    Width = width,
                    Height = height,
                    MipLevels = 1,
                    ArraySize = 1,
                    BindFlags = SharpDX.Direct3D11.BindFlags.DepthStencil,
                    Format = SharpDX.DXGI.Format.D24_UNorm_S8_UInt,
                    CpuAccessFlags = SharpDX.Direct3D11.CpuAccessFlags.None,
                    SampleDescription = new SharpDX.DXGI.SampleDescription(4, Manager.Msaa4xQuality - 1),
                    Usage = SharpDX.Direct3D11.ResourceUsage.Default,
                    OptionFlags = SharpDX.Direct3D11.ResourceOptionFlags.None
                }));


            surfaceTarget = new SharpDX.Direct2D1.Bitmap1(Manager.ID2D1DeviceContext,
                swapchain.GetBackBuffer<SharpDX.DXGI.Surface>(0), new SharpDX.Direct2D1.BitmapProperties1()
                {
                    BitmapOptions = SharpDX.Direct2D1.BitmapOptions.Target | SharpDX.Direct2D1.BitmapOptions.CannotDraw,
                    DpiX = Manager.DpiX,
                    DpiY = Manager.DpiY,
                    PixelFormat = new SharpDX.Direct2D1.PixelFormat()
                    {
                        Format = SharpDX.DXGI.Format.R8G8B8A8_UNorm,
                        AlphaMode = SharpDX.Direct2D1.AlphaMode.Premultiplied
                    }
                });
            
        }

        internal SharpDX.DXGI.SwapChain IDXGISwapChain => swapchain;

        internal SharpDX.Direct3D11.RenderTargetView ID3D11RenderTargetView => surfaceRTV;
        internal SharpDX.Direct3D11.DepthStencilView ID3D11DepthStencilView => surfaceDSV;

        internal SharpDX.Direct2D1.Bitmap1 Target => surfaceTarget;

        internal IntPtr Handle => surfaceHandle;

        public Brush BackGround
        {
            set => background = value;
            get => background;
        }

        ~Surface()
        {
            swapchain?.Dispose();
            surfaceRTV?.Dispose();
            surfaceDSV?.Dispose();
            surfaceTarget?.Dispose();
        }
    }

    public static partial class Manager
    {
        private static Surface surface = null;

        public static Surface Surface
        {
            get => surface;
            set
            {
                surface = value;

                APILibrary.Win32.Rect rect = new APILibrary.Win32.Rect();

                APILibrary.Win32.Internal.GetClientRect(surface.Handle, ref rect);

                int width = rect.right - rect.left;
                int height = rect.bottom - rect.top;

                ID3D11DeviceContext.Rasterizer.SetViewport(new SharpDX.Mathematics.Interop.RawViewportF()
                {
                    Width = width,
                    Height = height,
                    MinDepth = 0f,
                    MaxDepth = 1f,
                    X = 0,
                    Y = 0
                });

                ID3D11DeviceContext.OutputMerger.SetTargets(surface.ID3D11DepthStencilView,
                    surface.ID3D11RenderTargetView);

                context2d.Target = surface.Target;
            }
        }
    }

}
