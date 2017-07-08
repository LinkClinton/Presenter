using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace Presenter
{
    public partial class Surface
    {
        private SharpDX.DXGI.SwapChain3 surfaceSwapChain;

        private SharpDX.Direct3D12.DescriptorHeap surfaceRTVHeap;
        private SharpDX.Direct3D12.DescriptorHeap surfaceDSVHeap;

        private SharpDX.Direct3D12.Resource[] surfaceRTV;
        private SharpDX.Direct3D12.Resource surfaceDSV;

        private int surfaceRTVSize;
        private int surfaceDSVSize;

        private Vector4 backGround = Vector4.One;

        private int width;
        private int height;

        private IntPtr surfaceHandle;

        public Surface(IntPtr handle, bool windowed = true)
        {
            surfaceHandle = handle;

            APILibrary.Win32.Rect realRect = new APILibrary.Win32.Rect();

            APILibrary.Win32.Internal.GetClientRect(surfaceHandle, ref realRect);

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
                            Format = SharpDX.DXGI.Format.R8G8B8A8_UNorm,
                            RefreshRate = new SharpDX.DXGI.Rational(60, 1),
                            Scaling = SharpDX.DXGI.DisplayModeScaling.Unspecified,
                            ScanlineOrdering = SharpDX.DXGI.DisplayModeScanlineOrder.Unspecified
                        },
                        Usage = SharpDX.DXGI.Usage.RenderTargetOutput,
                        Flags = SharpDX.DXGI.SwapChainFlags.AllowModeSwitch,
                        SampleDescription = new SharpDX.DXGI.SampleDescription(1, 0),
                        SwapEffect = SharpDX.DXGI.SwapEffect.FlipDiscard,
                        OutputHandle = surfaceHandle,
                        IsWindowed = windowed
                    });

                IDXGISwapChain = tempSwapChain.QueryInterface<SharpDX.DXGI.SwapChain3>();

                SharpDX.Utilities.Dispose(ref tempSwapChain);
            }

            ID3D12RenderTargetViewHeap = Engine.ID3D12Device.CreateDescriptorHeap(
                new SharpDX.Direct3D12.DescriptorHeapDescription()
                {
                    DescriptorCount = BufferCount,
                    Flags = SharpDX.Direct3D12.DescriptorHeapFlags.None,
                    Type = SharpDX.Direct3D12.DescriptorHeapType.RenderTargetView,
                    NodeMask = 0
                });

            ID3D12RenderTargetViewHeapSize = Engine.ID3D12Device.
                GetDescriptorHandleIncrementSize(SharpDX.Direct3D12.DescriptorHeapType.RenderTargetView);

            ID3D12DepthStencilViewHeap = Engine.ID3D12Device.CreateDescriptorHeap(
                new SharpDX.Direct3D12.DescriptorHeapDescription()
                {
                    DescriptorCount = 1,
                    Flags = SharpDX.Direct3D12.DescriptorHeapFlags.None,
                    Type = SharpDX.Direct3D12.DescriptorHeapType.DepthStencilView,
                    NodeMask = 0
                });

            ID3D12DepthStencilViewHeapSize = Engine.ID3D12Device.
                GetDescriptorHandleIncrementSize(SharpDX.Direct3D12.DescriptorHeapType.DepthStencilView);

            surfaceRTV = new SharpDX.Direct3D12.Resource[BufferCount];

            var RTVHandle = ID3D12RenderTargetViewHeap.CPUDescriptorHandleForHeapStart;

            for (int i = 0; i < BufferCount; i++)
            {
                surfaceRTV[i] = IDXGISwapChain.GetBackBuffer<SharpDX.Direct3D12.Resource>(i);

                Engine.ID3D12Device.CreateRenderTargetView(surfaceRTV[i], null, RTVHandle);

                RTVHandle += ID3D12RenderTargetViewHeapSize;
            }

            surfaceDSV = Engine.ID3D12Device.CreateCommittedResource(
                   new SharpDX.Direct3D12.HeapProperties(SharpDX.Direct3D12.HeapType.Default),
                    SharpDX.Direct3D12.HeapFlags.None, new SharpDX.Direct3D12.ResourceDescription()
                    {
                        Dimension = SharpDX.Direct3D12.ResourceDimension.Texture2D,
                        Alignment = 0,
                        Width = width,
                        Height = height,
                        DepthOrArraySize = 1,
                        MipLevels = 1,
                        Format = SharpDX.DXGI.Format.R24G8_Typeless,
                        SampleDescription = new SharpDX.DXGI.SampleDescription(1, 0),
                        Layout = SharpDX.Direct3D12.TextureLayout.Unknown,
                        Flags = SharpDX.Direct3D12.ResourceFlags.AllowDepthStencil
                    },
                     SharpDX.Direct3D12.ResourceStates.Common, new SharpDX.Direct3D12.ClearValue()
                     {
                         Format = DepthStencilFormat,
                         DepthStencil = new SharpDX.Direct3D12.DepthStencilValue() { Depth = 1.0f, Stencil = 0 }
                     });


            Engine.ID3D12Device.CreateDepthStencilView(surfaceDSV, new SharpDX.Direct3D12.DepthStencilViewDescription()
            {
                Flags = SharpDX.Direct3D12.DepthStencilViewFlags.None,
                Dimension = SharpDX.Direct3D12.DepthStencilViewDimension.Texture2D,
                Format = DepthStencilFormat,
                Texture2D = new SharpDX.Direct3D12.DepthStencilViewDescription.Texture2DResource() { MipSlice = 0 }
            }, ID3D12DepthStencilViewHeap.CPUDescriptorHandleForHeapStart);

            using (var CommandList = Engine.ID3D12Device.CreateCommandList(SharpDX.Direct3D12.CommandListType.Direct,
                Engine.ID3D12CommandAllocator, null))
            {
                CommandList.ResourceBarrierTransition(surfaceDSV, SharpDX.Direct3D12.ResourceStates.Common,
                     SharpDX.Direct3D12.ResourceStates.DepthWrite);

                CommandList.Close();

                Engine.ID3D12CommandQueue.ExecuteCommandList(CommandList);

                Engine.Wait();
            }

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

        internal SharpDX.Direct3D12.DescriptorHeap ID3D12DepthStencilViewHeap
        {
            private set => surfaceDSVHeap = value;
            get => surfaceDSVHeap;
        }

        internal SharpDX.Direct3D12.Resource[] RenderTargetView
            => surfaceRTV;

        internal SharpDX.Direct3D12.Resource DepthStencilView
            => surfaceDSV;

        internal int ID3D12RenderTargetViewHeapSize
        {
            private set => surfaceRTVSize = value;
            get => surfaceRTVSize;
        }

        internal int ID3D12DepthStencilViewHeapSize
        {
            private set => surfaceDSVSize = value;
            get => surfaceDSVSize;
        }

        internal static SharpDX.DXGI.Format DepthStencilFormat => SharpDX.DXGI.Format.D24_UNorm_S8_UInt;

        internal static int BufferCount => 2;

        public int Width => width;

        public int Height => height;

        public Vector4 BackGround
        {
            get => backGround;
            set => backGround = value;
        }

        public void Reset(int new_width, int new_height, bool windowed = true)
        {
            throw new NotImplementedException("Wait for support");
        }

        ~Surface()
        {
            SharpDX.Utilities.Dispose(ref surfaceSwapChain);
            SharpDX.Utilities.Dispose(ref surfaceRTVHeap);
            SharpDX.Utilities.Dispose(ref surfaceDSVHeap);

            for (int i = 0; i < surfaceRTV.Length; i++)
                SharpDX.Utilities.Dispose(ref surfaceRTV[i]);
            SharpDX.Utilities.Dispose(ref surfaceDSV);
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
