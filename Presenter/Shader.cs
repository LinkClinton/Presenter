using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presenter
{
    public abstract class Shader
    {
        protected SharpDX.D3DCompiler.ShaderBytecode bytecode;


        public byte[] ShaderByteCode => bytecode;
    }
}
