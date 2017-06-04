using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presenter
{
    interface IGraphicsPipelineState
    {

        VertexShader VertexShader { get; }
        PixelShader PixelShader { get; }

        BufferLayout BufferLayout { get; }
        ResourceLayout ResourceLayout { get; }
    }
}
