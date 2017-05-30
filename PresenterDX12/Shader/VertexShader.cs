using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presenter
{
    public class VertexShader : Shader, IVertexShader
    {
        public VertexShader(string shaderfile, string entrypoint, bool isCompiled = false)
        {
            bytecode = new SharpDX.D3DCompiler.ShaderBytecode(
                new System.IO.FileStream(shaderfile, System.IO.FileMode.Open));

            if (isCompiled is true) return;

#if DEBUG
            SharpDX.D3DCompiler.CompilationResult result = SharpDX.D3DCompiler.ShaderBytecode.Compile(bytecode, entrypoint, "vs_5_0",
                 SharpDX.D3DCompiler.ShaderFlags.Debug | SharpDX.D3DCompiler.ShaderFlags.SkipOptimization);
#else
            SharpDX.D3DCompiler.CompilationResult result = SharpDX.D3DCompiler.ShaderBytecode.Compile(bytecode, entrypoint, "vs_5_0",
                 SharpDX.D3DCompiler.ShaderFlags.OptimizationLevel2);
#endif
            if (result.HasErrors is true) throw new Exception(result.Message);

            bytecode = result.Bytecode;
        }
    }
}
