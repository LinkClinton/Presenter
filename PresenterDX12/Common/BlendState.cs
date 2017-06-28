using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presenter
{
    public class BlendState
    {
        private SharpDX.Direct3D12.BlendStateDescription blendState;
        private SharpDX.Direct3D12.RenderTargetBlendDescription rtvBlend;

        public BlendState(bool isEnable = false)
        {
            rtvBlend = new SharpDX.Direct3D12.RenderTargetBlendDescription()
            {
                IsBlendEnabled = isEnable,
                LogicOpEnable = false,
                SourceBlend = SharpDX.Direct3D12.BlendOption.One,
                DestinationBlend = SharpDX.Direct3D12.BlendOption.Zero,
                BlendOperation = SharpDX.Direct3D12.BlendOperation.Add,
                SourceAlphaBlend = SharpDX.Direct3D12.BlendOption.One,
                DestinationAlphaBlend = SharpDX.Direct3D12.BlendOption.Zero,
                AlphaBlendOperation = SharpDX.Direct3D12.BlendOperation.Add,
                LogicOp = SharpDX.Direct3D12.LogicOperation.Noop,
                RenderTargetWriteMask = SharpDX.Direct3D12.ColorWriteMaskFlags.All
            };



            blendState = SharpDX.Direct3D12.BlendStateDescription.Default();

            blendState.RenderTarget[0] = rtvBlend;
        }

        public BlendOperation AlphaBlendOperation
        {
            get => (BlendOperation)rtvBlend.AlphaBlendOperation;
            set => rtvBlend.AlphaBlendOperation = (SharpDX.Direct3D12.BlendOperation)value;
        }

        public BlendOperation BlendOperation
        {
            get => (BlendOperation)rtvBlend.BlendOperation;
            set => rtvBlend.BlendOperation = (SharpDX.Direct3D12.BlendOperation)value;
        }

        public BlendOption DestinationAlphaBlend
        {
            get => (BlendOption)rtvBlend.DestinationAlphaBlend;
            set => rtvBlend.DestinationAlphaBlend = (SharpDX.Direct3D12.BlendOption)value;
        }

        public BlendOption DestinationBlend
        {
            get => (BlendOption)rtvBlend.DestinationBlend;
            set => rtvBlend.DestinationBlend = (SharpDX.Direct3D12.BlendOption)value;
        }

        public bool IsBlendEnabled
        {
            get => rtvBlend.IsBlendEnabled;
            set => rtvBlend.IsBlendEnabled = value;
        }

        public ColorMask RenderTargetWriteMask
        {
            get => (ColorMask)rtvBlend.RenderTargetWriteMask;
            set => rtvBlend.RenderTargetWriteMask = (SharpDX.Direct3D12.ColorWriteMaskFlags)value;
        }

        public BlendOption SourceAlphaBlend
        {
            get => (BlendOption)rtvBlend.SourceAlphaBlend;
            set => rtvBlend.SourceAlphaBlend = (SharpDX.Direct3D12.BlendOption)value;
        }

        public BlendOption SourceBlend
        {
            get => (BlendOption)rtvBlend.SourceBlend;
            set => rtvBlend.SourceBlend = (SharpDX.Direct3D12.BlendOption)value;
        }

        internal SharpDX.Direct3D12.BlendStateDescription ID3D12BlendState => blendState;
    }
}
