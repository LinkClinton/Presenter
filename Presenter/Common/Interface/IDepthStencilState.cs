using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presenter
{
    interface IDepthStencilState
    {
        bool IsDepthEnabled { get; set; }

        bool IsStencilEnabled { get; set; }
    }
}
