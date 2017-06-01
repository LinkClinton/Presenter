using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presenter
{
    public class ObjectPasser
    {
        private Buffer vertexBuffer;
        private Buffer indexBuffer;
        private Shader vertexShader;
        private Shader pixelShader;
        private BufferLayout bufferLayout;
        

        public Buffer VertexBuffer
        {
            set => vertexBuffer = value;
            get => vertexBuffer;
        }

        public Buffer IndexBuffer
        {
            set => indexBuffer = value;
            get => indexBuffer;
        }

        public Shader VertexShader
        {
            set => vertexShader = value;
            get => vertexShader;
        }

        public Shader PixelShader
        {
            set => pixelShader = value;
            get => pixelShader;
        }

        public BufferLayout BufferLayout
        {
            set => bufferLayout = value;
            get => bufferLayout;
        }

    }
}
