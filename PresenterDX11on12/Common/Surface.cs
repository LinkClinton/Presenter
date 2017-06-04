using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presenter
{
    public class Surface : ISurface
    {
        private static int surfaceBufferCount = 2;

        private SharpDX.DXGI.SwapChain3 swapchain;

        private int width;
        private int height;

        private SharpDX.Direct3D12.DescriptorHeap surfaceRTVHeap;
        private int surfaceRTVSize;

        private SharpDX.Direct3D12.Resource[] D3D12surfaceRTVBuffer;
        private SharpDX.Direct3D11.Resource[] D3D11surfaceRTVBuffer;

        private SharpDX.Direct3D11.Texture2D surfaceDepthBuffer;

        private SharpDX.Direct3D11.RenderTargetView[] surfaceRTV;
        private SharpDX.Direct3D11.DepthStencilView surfaceDSV;

        private SharpDX.Direct2D1.Bitmap1 surfaceTarget;

        private IntPtr surfaceHandle;

        private (float red, float green, float blue, float alpha) backGround
            = (1, 1, 1, 1);

        public Surface(IntPtr handle, bool windowed = true)
        {
            surfaceHandle = handle;

            APILibrary.Win32.Rect realRect = new APILibrary.Win32.Rect();

            APILibrary.Win32.Internal.GetClientRect(handle, ref realRect);

            width = realRect.right - realRect.left;
            height = realRect.bottom - realRect.top;

            using (var factory = new SharpDX.DXGI.Factory4())
            {
                var tempSwapChain = new SharpDX.DXGI.SwapChain(factory, Manager.ID3D12CommandQueue,
                    new SharpDX.DXGI.SwapChainDescription()
                    {
                        BufferCount = surfaceBufferCount,
                        ModeDescription = new SharpDX.DXGI.ModeDescription()
                        {
                            Width = width = realRect.right - realRect.left,
                            Height = height = realRect.bottom - realRect.top,
                            Format = SharpDX.DXGI.Format.R8G8B8A8_UNorm,
                            RefreshRate = new SharpDX.DXGI.Rational(60, 1)
                        },
                        Usage = SharpDX.DXGI.Usage.RenderTargetOutput,
                        Flags = SharpDX.DXGI.SwapChainFlags.None,
                        SampleDescription = new SharpDX.DXGI.SampleDescription(1, 0),
                        SwapEffect = SharpDX.DXGI.SwapEffect.FlipDiscard,
                        OutputHandle = surfaceHandle,
                        IsWindowed = windowed
                    });

                IDXGISwapChain = tempSwapChain.QueryInterface<SharpDX.DXGI.SwapChain3>();

                SharpDX.Utilities.Dispose(ref tempSwapChain);
            }


            ID3D12RenderTargetViewHeap = Manager.ID3D12Device.CreateDescriptorHeap(
                new SharpDX.Direct3D12.DescriptorHeapDescription()
                {
                    DescriptorCount = surfaceBufferCount,
                    Flags = SharpDX.Direct3D12.DescriptorHeapFlags.None,
                    Type = SharpDX.Direct3D12.DescriptorHeapType.RenderTargetView
                });

            ID3D12RenderTargetViewHeapSize = Manager.ID3D12Device.
                GetDescriptorHandleIncrementSize(SharpDX.Direct3D12.DescriptorHeapType.RenderTargetView);

            D3D12surfaceRTVBuffer = new SharpDX.Direct3D12.Resource[surfaceBufferCount];
            D3D11surfaceRTVBuffer = new SharpDX.Direct3D11.Resource[surfaceBufferCount];
            surfaceRTV = new SharpDX.Direct3D11.RenderTargetView[surfaceBufferCount];

            var RTVHandle = ID3D12RenderTargetViewHeap.CPUDescriptorHandleForHeapStart;

            for (int i = 0; i < surfaceBufferCount; i++)
            {
                D3D12surfaceRTVBuffer[i] = IDXGISwapChain.GetBackBuffer<SharpDX.Direct3D12.Resource>(i);
                Manager.ID3D12Device.CreateRenderTargetView(D3D12surfaceRTVBuffer[i], null, RTVHandle);

                SharpDX.Direct3D11.Texture2D texture = new SharpDX.Direct3D11.Texture2D(Manager.ID3D11Device,
                    new SharpDX.Direct3D11.Texture2DDescription()
                    {
                        Width = 10,
                        Height = 10,
                        ArraySize = 1,
                        BindFlags = SharpDX.Direct3D11.BindFlags.RenderTarget,
                        Format = SharpDX.DXGI.Format.R8G8B8A8_UNorm,
                        MipLevels = 1, SampleDescription = new SharpDX.DXGI.SampleDescription(1, 0)
                    });

                Manager.ID3D11on12Device.CreateWrappedResource(D3D12surfaceRTVBuffer[i],
                    new SharpDX.Direct3D11.D3D11ResourceFlags() { BindFlags = (int)SharpDX.Direct3D11.BindFlags.RenderTarget },
                    (int)SharpDX.Direct3D12.ResourceStates.RenderTarget, (int)SharpDX.Direct3D12.ResourceStates.Present,
                    SharpDX.Utilities.GetGuidFromType(texture.GetType()), out D3D11surfaceRTVBuffer[i]);

                surfaceRTV[i] = new SharpDX.Direct3D11.RenderTargetView(Manager.ID3D11Device, D3D11surfaceRTVBuffer[i]);

                RTVHandle += ID3D12RenderTargetViewHeapSize;

                SharpDX.Utilities.Dispose(ref texture);
            }

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

            /*
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
                });*/

        }

        public void Reset(int new_width, int new_height, bool windowed = true)
        {
            throw new NotImplementedException("Not Support");
        }

        internal SharpDX.DXGI.SwapChain3 IDXGISwapChain
        {
            private set => swapchain = value;
            get => swapchain;
        }

        internal SharpDX.Direct3D12.DescriptorHeap ID3D12RenderTargetViewHeap
        {
            private set => surfaceRTVHeap = value;
            get => surfaceRTVHeap;
        }

        internal SharpDX.Direct3D11.DepthStencilView ID3D11DepthStencilView => surfaceDSV;

        internal SharpDX.Direct3D11.RenderTargetView[] ID3D11RenderTargetView => surfaceRTV;

        internal SharpDX.Direct2D1.Bitmap1 Target => surfaceTarget;

        internal IntPtr Handle => surfaceHandle;

        internal int ID3D12RenderTargetViewHeapSize
        {
            private set => surfaceRTVSize = value;
            get => surfaceRTVSize;
        }

        internal int BufferCount => surfaceBufferCount;



        public int Width => width;
        public int Height => height;

        public (float red, float green, float blue, float alpha) BackGround
        {
            get => backGround;
            set => backGround = value;
        }

        ~Surface()
        {
            SharpDX.Utilities.Dispose(ref surfaceDepthBuffer);
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
            }
        }
    }

}
