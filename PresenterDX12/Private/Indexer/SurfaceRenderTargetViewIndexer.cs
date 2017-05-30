using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presenter
{
    public partial class Surface
    {
        internal class SurfaceRTVIndexer
        {
            Surface surface;

            public SurfaceRTVIndexer(Surface parent)
            {
                surface = parent;
            }

            internal SharpDX.Direct3D12.Resource this[int index] => surface.surfaceRTV[index];
        }
    }
}
