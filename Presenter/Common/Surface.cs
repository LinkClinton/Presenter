using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presenter
{
    public class Surface : ISurface
    {
        private SharpDX.DXGI.SwapChain swapchain;

        private int width;
        private int height;

        private SharpDX.Direct3D11.Texture2D surfaceBackBuffer;
        private SharpDX.Direct3D11.Texture2D surfaceDepthBuffer;

        private SharpDX.Direct3D11.RenderTargetView surfaceRTV;
        private SharpDX.Direct3D11.DepthStencilView surfaceDSV;

        private SharpDX.Direct2D1.Bitmap1 surfaceTarget;

        private IntPtr surfaceHandle;

        private (float red, float green, float blue, float alpha) backGround
            = (1, 1, 1, 1);

        public Surface(IntPtr handle, bool windowed = true)
        {
            surfaceHandle = handle;

            APILibrary.Win32.Rect rect = new APILibrary.Win32.Rect();

            APILibrary.Win32.Internal.GetClientRect(handle, ref rect);

            width = rect.right - rect.left;
            height = rect.bottom - rect.top;

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
                    SampleDescription = new SharpDX.DXGI.SampleDescription(1, 0),
                    SwapEffect = SharpDX.DXGI.SwapEffect.Discard,
                    IsWindowed = windowed
                });


            surfaceRTV = new SharpDX.Direct3D11.RenderTargetView(Manager.ID3D11Device,
                surfaceBackBuffer = swapchain.GetBackBuffer<SharpDX.Direct3D11.Texture2D>(0));



            surfaceDSV = new SharpDX.Direct3D11.DepthStencilView(Manager.ID3D11Device,
                surfaceDepthBuffer = new SharpDX.Direct3D11.Texture2D(Manager.ID3D11Device, new SharpDX.Direct3D11.Texture2DDescription()
                {
                    Width = width,
                    Height = height,
                    MipLevels = 1,
                    ArraySize = 1,
                    BindFlags = SharpDX.Direct3D11.BindFlags.DepthStencil,
                    Format = SharpDX.DXGI.Format.D32_Float_S8X24_UInt,
                    CpuAccessFlags = SharpDX.Direct3D11.CpuAccessFlags.None,
                    SampleDescription = new SharpDX.DXGI.SampleDescription(1, 0),
                    Usage = SharpDX.Direct3D11.ResourceUsage.Default,
                    OptionFlags = SharpDX.Direct3D11.ResourceOptionFlags.None
                }));

            SharpDX.DXGI.Surface surface;

            surfaceTarget = new SharpDX.Direct2D1.Bitmap1(Manager.ID2D1DeviceContext,
                surface = swapchain.GetBackBuffer<SharpDX.DXGI.Surface>(0), new SharpDX.Direct2D1.BitmapProperties1()
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


            SharpDX.Utilities.Dispose(ref dxgidevice);
            SharpDX.Utilities.Dispose(ref dxgiadapte);
            SharpDX.Utilities.Dispose(ref dxgifactory);
            SharpDX.Utilities.Dispose(ref surface);
        }

        public void Reset(int new_width, int new_height, bool windowed = true)
        {
            width = new_width; height = new_height;

            SharpDX.Utilities.Dispose(ref surfaceBackBuffer);
            SharpDX.Utilities.Dispose(ref surfaceDepthBuffer);
            SharpDX.Utilities.Dispose(ref surfaceRTV);
            SharpDX.Utilities.Dispose(ref surfaceDSV);
            SharpDX.Utilities.Dispose(ref surfaceTarget);
            SharpDX.Utilities.Dispose(ref swapchain);

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
                    OutputHandle = surfaceHandle,
                    SampleDescription = new SharpDX.DXGI.SampleDescription(1, 0),
                    SwapEffect = SharpDX.DXGI.SwapEffect.Discard,
                    IsWindowed = windowed
                });


            surfaceRTV = new SharpDX.Direct3D11.RenderTargetView(Manager.ID3D11Device,
              surfaceBackBuffer = swapchain.GetBackBuffer<SharpDX.Direct3D11.Texture2D>(0));

            surfaceDSV = new SharpDX.Direct3D11.DepthStencilView(Manager.ID3D11Device,
                surfaceDepthBuffer = new SharpDX.Direct3D11.Texture2D(Manager.ID3D11Device, new SharpDX.Direct3D11.Texture2DDescription()
                {
                    Width = width,
                    Height = height,
                    MipLevels = 1,
                    ArraySize = 1,
                    BindFlags = SharpDX.Direct3D11.BindFlags.DepthStencil,
                    Format = SharpDX.DXGI.Format.D32_Float_S8X24_UInt,
                    CpuAccessFlags = SharpDX.Direct3D11.CpuAccessFlags.None,
                    SampleDescription = new SharpDX.DXGI.SampleDescription(1, 0),
                    Usage = SharpDX.Direct3D11.ResourceUsage.Default,
                    OptionFlags = SharpDX.Direct3D11.ResourceOptionFlags.None
                }));

            SharpDX.DXGI.Surface surface;

            surfaceTarget = new SharpDX.Direct2D1.Bitmap1(Manager.ID2D1DeviceContext,
                surface = swapchain.GetBackBuffer<SharpDX.DXGI.Surface>(0), new SharpDX.Direct2D1.BitmapProperties1()
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


            SharpDX.Utilities.Dispose(ref dxgidevice);
            SharpDX.Utilities.Dispose(ref dxgiadapte);
            SharpDX.Utilities.Dispose(ref dxgifactory);
            SharpDX.Utilities.Dispose(ref surface);

            if (Manager.Surface == this)
                Manager.Surface = this;
        }

        internal SharpDX.DXGI.SwapChain IDXGISwapChain => swapchain;

        internal SharpDX.Direct3D11.RenderTargetView ID3D11RenderTargetView => surfaceRTV;
        internal SharpDX.Direct3D11.DepthStencilView ID3D11DepthStencilView => surfaceDSV;

        internal SharpDX.Direct2D1.Bitmap1 Target => surfaceTarget;

        internal IntPtr Handle => surfaceHandle;

       

        public int Width => width;
        public int Height => height;

        public (float red, float green, float blue, float alpha) BackGround
        {
            get => backGround;
            set => backGround = value;
        }

        ~Surface()
        {
            SharpDX.Utilities.Dispose(ref surfaceBackBuffer);
            SharpDX.Utilities.Dispose(ref surfaceDepthBuffer);
            SharpDX.Utilities.Dispose(ref surfaceRTV);
            SharpDX.Utilities.Dispose(ref surfaceDSV);
            SharpDX.Utilities.Dispose(ref surfaceTarget);
            SharpDX.Utilities.Dispose(ref swapchain);
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

                context2d.Target = surface.Target;
            }
        }
    }

}
