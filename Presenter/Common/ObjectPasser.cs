using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presenter
{
    public class ObjectPasser : IObjectPasser
    {
        private Buffer passVertexBuffer;
        private Buffer passIndexBuffer;
        private Shader passVertexShader;
        private Shader passPixelShader;
        private BufferLayout PassbufferLayout;

        private PrimitiveType passPrimitiveType = PrimitiveType.TriangleList;
        private int passBaseIndexLocation = 0;
        private int passBaseVertexLocation = 0;
        private int passIndexCount = 0;

        public ObjectPasser(Buffer vertexBuffer,
            Buffer indexBuffer, Shader vertexShader,
            Shader pixelShader, BufferLayout.Element[] inputElement)
        {
            passVertexBuffer = vertexBuffer;
            passIndexBuffer = indexBuffer;
            passVertexShader = vertexShader;
            passPixelShader = pixelShader;

            PassbufferLayout = new BufferLayout(inputElement, vertexShader);
        }

        public ObjectPasser(Buffer vertexBuffer,
            Buffer indexBuffer, Shader vertexShader,
            Shader pixelShader, BufferLayout bufferLayout)
        {
            passVertexBuffer = vertexBuffer;
            passIndexBuffer = indexBuffer;
            passVertexShader = vertexShader;
            passPixelShader = pixelShader;
            PassbufferLayout = bufferLayout;
        }

        public void Pass()
        {
            Manager.VertexBuffer = passVertexBuffer;
            Manager.IndexBuffer = passIndexBuffer;
            Manager.VertexShader = passVertexShader as VertexShader;
            Manager.PixelShader = passPixelShader as PixelShader;
            Manager.BufferLayout = PassbufferLayout;

            Manager.DrawObjectIndexed(passIndexCount == 0 ? passIndexBuffer.Count : passIndexCount, passBaseIndexLocation,
                passBaseVertexLocation, passPrimitiveType);
        }

        public PrimitiveType PrimitiveType
        {
            set => passPrimitiveType = value;
            get => passPrimitiveType;
        }

        public int PassBaseVertexLocation
        {
            set => passBaseVertexLocation = value;
            get => passBaseVertexLocation;
        }

        public int PassBaseIndexLocation
        {
            set => passBaseIndexLocation = value;
            get => passBaseIndexLocation;
        }

        public int PassIndexCount
        {
            set => passIndexCount = value;
            get => passIndexCount;
        }

    }
}
