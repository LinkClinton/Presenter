using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace Presenter
{
    public class Present
    {
        private SharpDX.DXGI.SwapChain3 swapChain;

        private Surface[] surface;

        private int width;
        private int height;

        private IntPtr handle;

        private Vector4 backGround = Vector4.One;

        public Present(IntPtr Handle, bool Windowed = true)
        {
            handle = Handle;

            APILibrary.Win32.Rect realRect = new APILibrary.Win32.Rect();

            APILibrary.Win32.Internal.GetClientRect(handle, ref realRect);

            using (var factory = new SharpDX.DXGI.Factory4())
            {
                var tempSwapChain = new SharpDX.DXGI.SwapChain(factory, Engine.ID3D12CommandQueue,
                    new SharpDX.DXGI.SwapChainDescription()
                    {
                        BufferCount = BufferCount,
                        ModeDescription = new SharpDX.DXGI.ModeDescription()
                        {
                            Width = width = realRect.right - realRect.left,
                            Height = height = realRect.bottom - realRect.top,
                            Format = DefaultFormat,
                            RefreshRate = new SharpDX.DXGI.Rational(60, 1),
                            Scaling = SharpDX.DXGI.DisplayModeScaling.Unspecified,
                            ScanlineOrdering = SharpDX.DXGI.DisplayModeScanlineOrder.Unspecified
                        },
                        Usage = SharpDX.DXGI.Usage.RenderTargetOutput,
                        Flags = SharpDX.DXGI.SwapChainFlags.AllowModeSwitch,
                        SampleDescription = new SharpDX.DXGI.SampleDescription(1, 0),
                        SwapEffect = SharpDX.DXGI.SwapEffect.FlipDiscard,
                        OutputHandle = handle,
                        IsWindowed = Windowed
                    });

                IDXGISwapChain = tempSwapChain.QueryInterface<SharpDX.DXGI.SwapChain3>();

                SharpDX.Utilities.Dispose(ref tempSwapChain);
            }

            surface = new Surface[BufferCount];

            for (int i = 0; i < BufferCount; i++)
            {
                surface[i] = new Surface(swapChain.GetBackBuffer<SharpDX.Direct3D12.Resource>(i),
                    width, height);
            }
        }

        public void Swap()
        {
            IDXGISwapChain.Present(0, SharpDX.DXGI.PresentFlags.None);
        }

        internal SharpDX.DXGI.SwapChain3 IDXGISwapChain
        {
            set => swapChain = value;
            get => swapChain;
        }

        internal static int BufferCount => 2;

        internal static SharpDX.DXGI.Format DefaultFormat => SharpDX.DXGI.Format.R8G8B8A8_UNorm;

        public Surface CurrentSurface => surface[IDXGISwapChain.CurrentBackBufferIndex];

        public Vector4 BackGround
        {
            get => backGround;
            set
            {
                backGround = value;

                foreach (var item in surface)
                {
                    item.BackGround = value;
                }
            }
        }

        ~Present()
        {
            SharpDX.Utilities.Dispose(ref swapChain);
        }

    }
}
