using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presenter
{
    public class PixelShader : Shader, IPixelShader
    {
        public PixelShader(string shaderfile, string entrypoint, bool isCompiled = false)
        {
            System.IO.FileStream file = new System.IO.FileStream(shaderfile, System.IO.FileMode.Open);

            bytecode = new SharpDX.D3DCompiler.ShaderBytecode(file);



            if (isCompiled is true) { file.Close(); return; }

#if DEBUG
            SharpDX.D3DCompiler.CompilationResult result = SharpDX.D3DCompiler.ShaderBytecode.Compile(bytecode, entrypoint, "ps_5_0",
                 SharpDX.D3DCompiler.ShaderFlags.Debug);
#else
            SharpDX.D3DCompiler.CompilationResult result = SharpDX.D3DCompiler.ShaderBytecode.Compile(bytecode, entrypoint, "ps_5_0",
                 SharpDX.D3DCompiler.ShaderFlags.OptimizationLevel2);
#endif
            if (result.HasErrors is true || result.Message != null) throw new Exception(result.Message);

            bytecode = result.Bytecode;

            file.Close();
        }
    }
}
