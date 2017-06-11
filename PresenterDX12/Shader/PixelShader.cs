using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presenter
{
    public class PixelShader : Shader, IPixelShader
    {
        private void CreatePixelShader(SharpDX.D3DCompiler.ShaderBytecode byteCode,
            string entrypoint, bool isCompiled = false)
        {
            bytecode = byteCode;

            if (isCompiled is true) return; 

#if DEBUG
            SharpDX.D3DCompiler.CompilationResult result = SharpDX.D3DCompiler.ShaderBytecode.Compile(bytecode, entrypoint, "ps_5_0",
                 SharpDX.D3DCompiler.ShaderFlags.Debug);
#else
            SharpDX.D3DCompiler.CompilationResult result = SharpDX.D3DCompiler.ShaderBytecode.Compile(bytecode, entrypoint, "ps_5_0",
                 SharpDX.D3DCompiler.ShaderFlags.OptimizationLevel2);
#endif
            if (result.HasErrors is true || result.Message != null) throw new Exception(result.Message);

            bytecode = result.Bytecode;
        }

        public PixelShader(byte[] shaderCode,string entrypoint,bool isCompiled = false)
        {
            CreatePixelShader(new SharpDX.D3DCompiler.ShaderBytecode(shaderCode), entrypoint, isCompiled);
        }

        public PixelShader(string shaderfile, string entrypoint, bool isCompiled = false)
        {
            System.IO.FileStream file = new System.IO.FileStream(shaderfile, System.IO.FileMode.Open);

            CreatePixelShader(new SharpDX.D3DCompiler.ShaderBytecode(file), entrypoint, isCompiled);

            file.Close();
        }
    }
}
