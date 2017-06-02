using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presenter
{
    public class Buffer : Resource, IBuffer
    {
        protected int count;

        public int Count => count;
    }

    public static partial class Manager
    {
        public static void DrawObject(int vertexCount, int startLocation = 0,
           PrimitiveType type = PrimitiveType.TriangleList)
        {
            ID3D12GraphicsCommandList.PrimitiveTopology = (SharpDX.Direct3D.PrimitiveTopology)type;

            ID3D12GraphicsCommandList.DrawInstanced(vertexCount, 1, startLocation, 0);
        }

        public static void DrawObjectIndexed(int indexCount, int startLocation = 0,
            int baseVertexLocation = 0, PrimitiveType type = PrimitiveType.TriangleList)
        {
            ID3D12GraphicsCommandList.PrimitiveTopology = (SharpDX.Direct3D.PrimitiveTopology)type;

            ID3D12GraphicsCommandList.DrawIndexedInstanced(indexCount, 1, startLocation, baseVertexLocation, 0);
        }
    }
}
