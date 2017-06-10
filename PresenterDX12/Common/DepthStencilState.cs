using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presenter
{
    public class DepthStencilState : IDepthStencilState
    {
        internal static SharpDX.Direct3D12.DepthStencilStateDescription DepthStencilStateDefault
        {
            get
            {
                return new SharpDX.Direct3D12.DepthStencilStateDescription()
                {
                    IsDepthEnabled = true,
                    DepthWriteMask = SharpDX.Direct3D12.DepthWriteMask.All,
                    DepthComparison = SharpDX.Direct3D12.Comparison.Less,
                    IsStencilEnabled = false,
                    StencilReadMask = 0xff,
                    StencilWriteMask = 0xff,
                    FrontFace = new SharpDX.Direct3D12.DepthStencilOperationDescription()
                    {
                        FailOperation = SharpDX.Direct3D12.StencilOperation.Keep,
                        DepthFailOperation = SharpDX.Direct3D12.StencilOperation.Keep,
                        PassOperation = SharpDX.Direct3D12.StencilOperation.Keep,
                        Comparison = SharpDX.Direct3D12.Comparison.Always
                    },
                    BackFace = new SharpDX.Direct3D12.DepthStencilOperationDescription()
                    {
                        FailOperation = SharpDX.Direct3D12.StencilOperation.Keep,
                        DepthFailOperation = SharpDX.Direct3D12.StencilOperation.Keep,
                        PassOperation = SharpDX.Direct3D12.StencilOperation.Keep,
                        Comparison = SharpDX.Direct3D12.Comparison.Always
                    }
                };
            }
        }

        private SharpDX.Direct3D12.DepthStencilStateDescription depthStencil
            = DepthStencilStateDefault;

        public DepthStencilState(bool depthEnabled = false,
            bool stencilEnabled = false)
        {
            depthStencil.IsDepthEnabled = depthEnabled;
            depthStencil.IsStencilEnabled = stencilEnabled;
        }

        public bool IsDepthEnabled
        {
            get => depthStencil.IsDepthEnabled;
            set => depthStencil.IsDepthEnabled = value;
        }

        public bool IsStencilEnabled
        {
            get => depthStencil.IsStencilEnabled;
            set => depthStencil.IsStencilEnabled = value;
        }

        internal SharpDX.Direct3D12.DepthStencilStateDescription ID3D12DepthStencilState
            => depthStencil;
    }
}
