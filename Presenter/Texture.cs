using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presenter
{
    public class Texture : ShaderResource
    {
        int width;
        int height;

        public Texture(string filename)
        {
        }

        public int Width => width;
        public int Height => height;

        public static implicit operator SharpDX.Direct3D11.Texture2D(Texture texture)
            => (texture.resource as SharpDX.Direct3D11.Texture2D);

        ~Texture() => (resource as SharpDX.Direct3D11.Texture2D).Dispose();
    }


}
