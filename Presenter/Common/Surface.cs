using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace Presenter
{
    public class Surface 
    {
        private SharpDX.DXGI.SwapChain swapchain;

        private int width;
        private int height;

        private SharpDX.Direct3D11.Texture2D surfaceBackBuffer;
        private SharpDX.Direct3D11.Texture2D surfaceDepthBuffer;

        private SharpDX.Direct3D11.RenderTargetView surfaceRTV;
        private SharpDX.Direct3D11.DepthStencilView surfaceDSV;

        private IntPtr surfaceHandle;

        private Vector4 backGround = new Vector4(1, 1, 1, 1);

        public Surface(IntPtr handle, bool windowed = true)
        {
            surfaceHandle = handle;

            APILibrary.Win32.Rect rect = new APILibrary.Win32.Rect();

            APILibrary.Win32.Internal.GetClientRect(handle, ref rect);

            width = rect.right - rect.left;
            height = rect.bottom - rect.top;

            SharpDX.DXGI.Device dxgidevice = Engine.ID3D11Device.QueryInterface<SharpDX.DXGI.Device>();
            SharpDX.DXGI.Adapter dxgiadapte = dxgidevice.GetParent<SharpDX.DXGI.Adapter>();
            SharpDX.DXGI.Factory dxgifactory = dxgiadapte.GetParent<SharpDX.DXGI.Factory>();

            swapchain = new SharpDX.DXGI.SwapChain(dxgifactory, Engine.ID3D11Device,
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


            surfaceRTV = new SharpDX.Direct3D11.RenderTargetView(Engine.ID3D11Device,
                surfaceBackBuffer = swapchain.GetBackBuffer<SharpDX.Direct3D11.Texture2D>(0));



            surfaceDSV = new SharpDX.Direct3D11.DepthStencilView(Engine.ID3D11Device,
                surfaceDepthBuffer = new SharpDX.Direct3D11.Texture2D(Engine.ID3D11Device, new SharpDX.Direct3D11.Texture2DDescription()
                {
                    Width = width,
                    Height = height,
                    MipLevels = 1,
                    ArraySize = 1,
                    BindFlags = SharpDX.Direct3D11.BindFlags.DepthStencil,
                    Format = SharpDX.DXGI.Format.D24_UNorm_S8_UInt,
                    CpuAccessFlags = SharpDX.Direct3D11.CpuAccessFlags.None,
                    SampleDescription = new SharpDX.DXGI.SampleDescription(1, 0),
                    Usage = SharpDX.Direct3D11.ResourceUsage.Default,
                    OptionFlags = SharpDX.Direct3D11.ResourceOptionFlags.None
                }));


            SharpDX.Utilities.Dispose(ref dxgidevice);
            SharpDX.Utilities.Dispose(ref dxgiadapte);
            SharpDX.Utilities.Dispose(ref dxgifactory);
        }

        public void Reset(int new_width, int new_height, bool windowed = true)
        {
            width = new_width; height = new_height;

            SharpDX.Utilities.Dispose(ref surfaceBackBuffer);
            SharpDX.Utilities.Dispose(ref surfaceDepthBuffer);
            SharpDX.Utilities.Dispose(ref surfaceRTV);
            SharpDX.Utilities.Dispose(ref surfaceDSV);
            SharpDX.Utilities.Dispose(ref swapchain);

            SharpDX.DXGI.Device dxgidevice = Engine.ID3D11Device.QueryInterface<SharpDX.DXGI.Device>();
            SharpDX.DXGI.Adapter dxgiadapte = dxgidevice.GetParent<SharpDX.DXGI.Adapter>();
            SharpDX.DXGI.Factory dxgifactory = dxgiadapte.GetParent<SharpDX.DXGI.Factory>();

            swapchain = new SharpDX.DXGI.SwapChain(dxgifactory, Engine.ID3D11Device,
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


            surfaceRTV = new SharpDX.Direct3D11.RenderTargetView(Engine.ID3D11Device,
              surfaceBackBuffer = swapchain.GetBackBuffer<SharpDX.Direct3D11.Texture2D>(0));

            surfaceDSV = new SharpDX.Direct3D11.DepthStencilView(Engine.ID3D11Device,
                surfaceDepthBuffer = new SharpDX.Direct3D11.Texture2D(Engine.ID3D11Device, new SharpDX.Direct3D11.Texture2DDescription()
                {
                    Width = width,
                    Height = height,
                    MipLevels = 1,
                    ArraySize = 1,
                    BindFlags = SharpDX.Direct3D11.BindFlags.DepthStencil,
                    Format = SharpDX.DXGI.Format.D24_UNorm_S8_UInt,
                    CpuAccessFlags = SharpDX.Direct3D11.CpuAccessFlags.None,
                    SampleDescription = new SharpDX.DXGI.SampleDescription(1, 0),
                    Usage = SharpDX.Direct3D11.ResourceUsage.Default,
                    OptionFlags = SharpDX.Direct3D11.ResourceOptionFlags.None
                }));


            SharpDX.Utilities.Dispose(ref dxgidevice);
            SharpDX.Utilities.Dispose(ref dxgiadapte);
            SharpDX.Utilities.Dispose(ref dxgifactory);

        }

        internal SharpDX.DXGI.SwapChain IDXGISwapChain => swapchain;

        internal SharpDX.Direct3D11.RenderTargetView ID3D11RenderTargetView => surfaceRTV;
        internal SharpDX.Direct3D11.DepthStencilView ID3D11DepthStencilView => surfaceDSV;

        internal IntPtr Handle => surfaceHandle;

        internal static SharpDX.DXGI.Format DepthStencilFormat => SharpDX.DXGI.Format.D24_UNorm_S8_UInt;
        internal static int BufferCount => 1;


        public int Width => width;

        public int Height => height;

        public Vector4 BackGround
        {
            set => backGround = value;
            get => backGround;
        }

        ~Surface()
        {
            SharpDX.Utilities.Dispose(ref surfaceBackBuffer);
            SharpDX.Utilities.Dispose(ref surfaceDepthBuffer);
            SharpDX.Utilities.Dispose(ref surfaceRTV);
            SharpDX.Utilities.Dispose(ref surfaceDSV);
            SharpDX.Utilities.Dispose(ref swapchain);
        }
    }

    public static partial class GraphicsPipeline
    {
        private static Surface surface;

        public static Surface Target
        {
            get => surface;
        }
    }
}
