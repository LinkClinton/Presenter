using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presenter
{
    public abstract class Buffer : Resource, IBuffer
    {
        protected int count;

        public int Count => count;

        internal SharpDX.Direct3D11.Buffer ID3D11Buffer => resource as SharpDX.Direct3D11.Buffer;
    }

    public static partial class Manager
    {
        public static void DrawObject(int vertexCount, int startLocation = 0,
             PrimitiveType type = PrimitiveType.TriangleList)
        {
            ID3D11DeviceContext.InputAssembler.PrimitiveTopology = (SharpDX.Direct3D.PrimitiveTopology)type;

            ID3D11DeviceContext.Draw(vertexCount, startLocation);
        }

        public static void DrawObjectIndexed(int indexCount, int startLocation = 0,
            int baseVertexLocation = 0, PrimitiveType type = PrimitiveType.TriangleList)
        {
            ID3D11DeviceContext.InputAssembler.PrimitiveTopology = (SharpDX.Direct3D.PrimitiveTopology)type;

            ID3D11DeviceContext.DrawIndexed(indexCount, startLocation, baseVertexLocation);
        }
    }
}
