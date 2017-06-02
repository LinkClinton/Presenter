using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presenter
{
    public partial class Surface : ISurface
    {
        private static int surfaceBufferCount = 2;

        private SharpDX.DXGI.SwapChain3 surfaceSwapChain;

        private SharpDX.Direct3D12.Resource[] surfaceRTV;
        private SharpDX.Direct3D12.DescriptorHeap surfaceRTVHeap;

        private SurfaceRTVIndexer surfaceRTVIndexer;

        private int surfaceRTVSize;

        private (float red, float green, float blue, float alpha) backGround
            = (1, 1, 1, 1);

        private int width;
        private int height;

        private IntPtr surfaceHandle;

        public Surface(IntPtr handle,bool windowed = true)
        {
            surfaceHandle = handle;

            APILibrary.Win32.Rect realRect = new APILibrary.Win32.Rect();

            APILibrary.Win32.Internal.GetClientRect(surfaceHandle, ref realRect);

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

            surfaceRTV = new SharpDX.Direct3D12.Resource[surfaceBufferCount];

            var RTVHandle = ID3D12RenderTargetViewHeap.CPUDescriptorHandleForHeapStart;

            for (int i = 0; i < surfaceBufferCount; i++)
            {
                surfaceRTV[i] = IDXGISwapChain.GetBackBuffer<SharpDX.Direct3D12.Resource>(i);
                Manager.ID3D12Device.CreateRenderTargetView(surfaceRTV[i], null, RTVHandle);
                RTVHandle += ID3D12RenderTargetViewHeapSize;
            }


            surfaceRTVIndexer = new SurfaceRTVIndexer(this);
        }

        internal SharpDX.DXGI.SwapChain3 IDXGISwapChain
        {
            private set => surfaceSwapChain = value;
            get => surfaceSwapChain;
        }

        internal SharpDX.Direct3D12.DescriptorHeap ID3D12RenderTargetViewHeap
        {
            private set => surfaceRTVHeap = value;
            get => surfaceRTVHeap;
        }

        internal SurfaceRTVIndexer RenderTargetView => surfaceRTVIndexer;

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

        public void Reset(int new_width, int new_height, bool windowed = true)
        {
            throw new NotImplementedException();
        }
    }

    public static partial class Manager
    {
        private static Surface surface;

        public static Surface Surface
        {
            set => surface = value;
            get => surface;
        }
    }

}
