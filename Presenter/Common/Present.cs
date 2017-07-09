using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presenter
{
    public class Present : Surface
    {
        private SharpDX.DXGI.SwapChain swapChain;

        private IntPtr handle;

        internal override void Presented()
        {
            swapChain.Present(0, SharpDX.DXGI.PresentFlags.None);
        }

        public Present(IntPtr Handle, bool Windowed = true)
        {
            handle = Handle;

            APILibrary.Win32.Rect realRect = new APILibrary.Win32.Rect();

            APILibrary.Win32.Internal.GetClientRect(handle, ref realRect);

            SharpDX.DXGI.Device dxgidevice = Engine.ID3D11Device.QueryInterface<SharpDX.DXGI.Device>();
            SharpDX.DXGI.Adapter dxgiadapte = dxgidevice.GetParent<SharpDX.DXGI.Adapter>();
            SharpDX.DXGI.Factory dxgifactory = dxgiadapte.GetParent<SharpDX.DXGI.Factory>();

            swapChain = new SharpDX.DXGI.SwapChain(dxgifactory, Engine.ID3D11Device,
                new SharpDX.DXGI.SwapChainDescription()
                {
                    ModeDescription = new SharpDX.DXGI.ModeDescription()
                    {
                        Width = width = realRect.right - realRect.left,
                        Height = height = realRect.bottom - realRect.top,
                        RefreshRate = new SharpDX.DXGI.Rational(60, 1),
                        Scaling = SharpDX.DXGI.DisplayModeScaling.Unspecified,
                        ScanlineOrdering = SharpDX.DXGI.DisplayModeScanlineOrder.Unspecified,
                        Format = RenderTargetFormat,
                    },
                    BufferCount = 1,
                    Usage = SharpDX.DXGI.Usage.RenderTargetOutput,
                    Flags = SharpDX.DXGI.SwapChainFlags.AllowModeSwitch,
                    OutputHandle = handle,
                    SampleDescription = new SharpDX.DXGI.SampleDescription(1, 0),
                    SwapEffect = SharpDX.DXGI.SwapEffect.Discard,
                    IsWindowed = Windowed
                });

            renderTarget = swapChain.GetBackBuffer<SharpDX.Direct3D11.Texture2D>(0);

            CreateResource(ref depthStencil, SharpDX.Direct3D11.BindFlags.DepthStencil, DepthStencilFormat);

            CreateResourceView();

            SharpDX.Utilities.Dispose(ref dxgidevice);
            SharpDX.Utilities.Dispose(ref dxgiadapte);
            SharpDX.Utilities.Dispose(ref dxgifactory);
        }

        public IntPtr Handle => handle;

        ~Present()
        {
            SharpDX.Utilities.Dispose(ref swapChain);
        }

    }
}
