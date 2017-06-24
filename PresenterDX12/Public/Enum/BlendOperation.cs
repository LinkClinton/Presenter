using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presenter
{
    public enum BlendOperation
    {
        Add = SharpDX.Direct3D12.BlendOperation.Add,
        Subtract = SharpDX.Direct3D12.BlendOperation.Subtract,
        ReverseSubtract = SharpDX.Direct3D12.BlendOperation.ReverseSubtract,
        Minimum = SharpDX.Direct3D12.BlendOperation.Minimum,
        Maximum = SharpDX.Direct3D12.BlendOperation.Maximum
    }
}
