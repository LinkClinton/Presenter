using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presenter
{
    public class PixelShader : Shader
    {
        private SharpDX.Direct3D11.PixelShader shader;

        private static PixelShaderConstantBufferIndexer constantBuffer = new PixelShaderConstantBufferIndexer();

        public PixelShader(string shaderfile, string entrypoint, bool isCompiled = false)
        {
            bytecode = new SharpDX.D3DCompiler.ShaderBytecode(
                new System.IO.FileStream(shaderfile, System.IO.FileMode.Open));

            if (isCompiled is true)
            {
                shader = new SharpDX.Direct3D11.PixelShader(Manager.ID3D11Device, bytecode);
                return;
            }
            
#if DEBUG
            SharpDX.D3DCompiler.CompilationResult result = SharpDX.D3DCompiler.ShaderBytecode.Compile(bytecode, entrypoint, "ps_5_0",
                 SharpDX.D3DCompiler.ShaderFlags.Debug | SharpDX.D3DCompiler.ShaderFlags.SkipOptimization);
#else
            SharpDX.D3DCompiler.CompilationResult result = SharpDX.D3DCompiler.ShaderBytecode.Compile(bytecode, entrypoint, "ps_5_0",
                 SharpDX.D3DCompiler.ShaderFlags.OptimizationLevel2);
#endif
            if (result.HasErrors is true) throw new Exception(result.Message);

            shader = new SharpDX.Direct3D11.PixelShader(Manager.ID3D11Device, bytecode = result.Bytecode);
        }

        public static implicit operator SharpDX.Direct3D11.PixelShader(PixelShader shader)
            => shader.shader;

        public static PixelShaderConstantBufferIndexer ConstantBuffer
            => constantBuffer;

        ~PixelShader() => shader?.Dispose();
    }

    public static partial class Manager
    {
        private static PixelShader pixelshader;

        public static PixelShader PixelShader
        {
            get => pixelshader;
            set
            {
                pixelshader = value;

                ID3D11DeviceContext.PixelShader.SetShader(pixelshader, null, 0);
            }
        }
    }
}
