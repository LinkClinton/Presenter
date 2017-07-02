using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presenter
{
    public class RasterizerState
    {
        private SharpDX.Direct3D12.RasterizerStateDescription rasterizerState
            = SharpDX.Direct3D12.RasterizerStateDescription.Default();

        public RasterizerState()
        {
            
        }

        public CullMode CullMode
        {
            get => (CullMode)rasterizerState.CullMode;
            set => rasterizerState.CullMode = (SharpDX.Direct3D12.CullMode)value;
        }

        public FillMode FillMode
        {
            get => (FillMode)rasterizerState.FillMode;
            set => rasterizerState.FillMode = (SharpDX.Direct3D12.FillMode)value;
        }

        internal SharpDX.Direct3D12.RasterizerStateDescription ID3D12RasterizerState
            => rasterizerState;

    }
}
