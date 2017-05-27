using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presenter
{
    interface ITexture : IShaderResource
    {


        int Width { get; }
        int Height { get; }
    }
}
