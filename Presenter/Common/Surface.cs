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
        private SharpDX.Direct3D11.RenderTargetView renderTargetView;
        private SharpDX.Direct3D11.DepthStencilView depthStencilView;

        protected SharpDX.Direct3D11.Texture2D renderTarget;
        protected SharpDX.Direct3D11.Texture2D depthStencil;

        protected int width;
        protected int height;

        private Vector4 backGround = Vector4.One;

        protected virtual void CreateResource(ref SharpDX.Direct3D11.Texture2D resource,
            SharpDX.Direct3D11.BindFlags bindFlags, SharpDX.DXGI.Format format)
        {
            SharpDX.Utilities.Dispose(ref resource);

            resource = new SharpDX.Direct3D11.Texture2D(Engine.ID3D11Device,
                new SharpDX.Direct3D11.Texture2DDescription()
                {
                    ArraySize = 1,
                    BindFlags = bindFlags,
                    CpuAccessFlags = SharpDX.Direct3D11.CpuAccessFlags.None,
                    Format = format,
                    Height = height,
                    MipLevels = 1,
                    OptionFlags = SharpDX.Direct3D11.ResourceOptionFlags.None,
                    SampleDescription = new SharpDX.DXGI.SampleDescription(1, 0),
                    Usage = SharpDX.Direct3D11.ResourceUsage.Default,
                    Width = width
                });
        }

        protected virtual void CreateResourceView()
        {
            renderTargetView = new SharpDX.Direct3D11.RenderTargetView(Engine.ID3D11Device, renderTarget,
                new SharpDX.Direct3D11.RenderTargetViewDescription()
                {
                    Dimension = SharpDX.Direct3D11.RenderTargetViewDimension.Texture2D,
                    Format = RenderTargetFormat,
                    Texture2D = new SharpDX.Direct3D11.RenderTargetViewDescription.Texture2DResource()
                    {
                        MipSlice = 0
                    }
                });

            depthStencilView = new SharpDX.Direct3D11.DepthStencilView(Engine.ID3D11Device, depthStencil,
                new SharpDX.Direct3D11.DepthStencilViewDescription()
                {
                    Dimension = SharpDX.Direct3D11.DepthStencilViewDimension.Texture2D,
                    Format = DepthStencilFormat,
                    Flags = SharpDX.Direct3D11.DepthStencilViewFlags.None,
                    Texture2D = new SharpDX.Direct3D11.DepthStencilViewDescription.Texture2DResource()
                    {
                        MipSlice = 0
                    }
                });
        }

        internal virtual void ResetViewPort()
        {
            Engine.ID3D11DeviceContext.Rasterizer.SetViewport(new SharpDX.Mathematics.Interop.RawViewportF()
            {
                Height = height,
                Width = width,
                MaxDepth = 1.0f,
                MinDepth = 0.0f,
                X = 0f,
                Y = 0f
            });

            Engine.ID3D11DeviceContext.Rasterizer.SetScissorRectangle(0, 0, width, height);
        }

        internal virtual void ResetResourceView()
        {
            Engine.ID3D11DeviceContext.OutputMerger.SetTargets(depthStencilView, renderTargetView);

            Engine.ID3D11DeviceContext.ClearRenderTargetView(renderTargetView,
                new SharpDX.Mathematics.Interop.RawColor4(backGround.X, backGround.Y, backGround.Z, backGround.W));

            Engine.ID3D11DeviceContext.ClearDepthStencilView(depthStencilView,
                 SharpDX.Direct3D11.DepthStencilClearFlags.Depth | SharpDX.Direct3D11.DepthStencilClearFlags.Stencil, 1f, 0);
        }

        internal virtual void ClearState()
        {

        }

        internal virtual void Presented()
        {

        }

        internal Surface() { }
        
        public Surface(int Width, int Height)
        {
            width = Width;
            height = Height;

            CreateResource(ref renderTarget, SharpDX.Direct3D11.BindFlags.RenderTarget |
                SharpDX.Direct3D11.BindFlags.ShaderResource, RenderTargetFormat);
            CreateResource(ref depthStencil, SharpDX.Direct3D11.BindFlags.DepthStencil, DepthStencilFormat);

            CreateResourceView();
        }

        internal SharpDX.Direct3D11.Texture2D RenderTarget => renderTarget;
        internal SharpDX.Direct3D11.Texture2D DepthStencil => depthStencil;

        internal static SharpDX.DXGI.Format DepthStencilFormat => SharpDX.DXGI.Format.D24_UNorm_S8_UInt;
        internal static SharpDX.DXGI.Format RenderTargetFormat => SharpDX.DXGI.Format.R8G8B8A8_UNorm;

        public int Width => width;
        public int Height => height;

        public Vector4 BackGround
        {
            set => backGround = value;
            get => backGround;
        }

        ~Surface()
        {
            SharpDX.Utilities.Dispose(ref renderTarget);
            SharpDX.Utilities.Dispose(ref depthStencil);
            SharpDX.Utilities.Dispose(ref renderTargetView);
            SharpDX.Utilities.Dispose(ref depthStencilView);
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
