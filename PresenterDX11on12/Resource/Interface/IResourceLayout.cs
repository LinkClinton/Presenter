using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presenter
{
    interface IResourceLayout
    {
        ResourceLayout.Element[] Elements { get; }

        int SlotCount { get; }

        int StaticSamplerCount { get; }
    }
}
