using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace Presenter
{
    public class BufferLayout
    {
        public enum ElementSize
        {
            eFloat1,
            eFloat2,
            eFloat3,
            eFlaot4
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct Element
        {
            public ElementSize Size;
            public string Tag;
        }

        private SharpDX.Direct3D11.InputLayout layout;

        public BufferLayout(Element[] elements)
        {
            
        }
    }
}
