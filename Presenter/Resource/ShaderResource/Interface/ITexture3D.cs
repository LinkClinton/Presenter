using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presenter
{
    interface ITexture3D
    {
        int Width { get; }
        int Height { get; }
        int Depth { get; }

        int MipLevels { get; }
    }
}
