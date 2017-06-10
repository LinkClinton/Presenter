using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presenter
{
    public class DepthStencilState : IDepthStencilState
    {
        private SharpDX.Direct3D11.DepthStencilStateDescription depthStencilDesc
            = SharpDX.Direct3D11.DepthStencilStateDescription.Default();

        private SharpDX.Direct3D11.DepthStencilState depthStencilState;

        private void Update()
        {
            depthStencilState = new SharpDX.Direct3D11.DepthStencilState(Manager.ID3D11Device,
                depthStencilDesc);
        }

        public DepthStencilState(bool depthEnabled = false,
            bool stencilEnabled = false)
        {
            depthStencilDesc.IsDepthEnabled = depthEnabled;
            depthStencilDesc.IsStencilEnabled = stencilEnabled;

            Update();
        }

        public bool IsDepthEnabled
        {
            get => depthStencilDesc.IsDepthEnabled;
            set
            {
                depthStencilDesc.IsDepthEnabled = value;
                Update();
            }
        }

        public bool IsStencilEnabled
        {
            get => depthStencilDesc.IsStencilEnabled;
            set
            {
                depthStencilDesc.IsStencilEnabled = value;
                Update();
            }
        }

        internal SharpDX.Direct3D11.DepthStencilState ID3D11DepthStencilState
            => depthStencilState;
    }
}
