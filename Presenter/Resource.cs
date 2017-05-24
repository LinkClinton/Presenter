using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presenter
{
    public class Resource
    {
        protected SharpDX.Direct3D11.Resource resource;

        public static implicit operator SharpDX.Direct3D11.Resource(Resource resource)
            => resource.resource;
    }
}
