using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;


namespace Presenter
{
    public class ConstantBuffer<T> : Buffer where T : struct
    {
        public ConstantBuffer(T data)
        {
            buffer = new SharpDX.Direct3D11.Buffer(Manager.ID3D11Device,
                size = Marshal.SizeOf<T>(), SharpDX.Direct3D11.ResourceUsage.Default,
                SharpDX.Direct3D11.BindFlags.ConstantBuffer, SharpDX.Direct3D11.CpuAccessFlags.None,
                SharpDX.Direct3D11.ResourceOptionFlags.None, 0);

            Update(ref data);

        }

        public ConstantBuffer(T[] data)
        {
            buffer = new SharpDX.Direct3D11.Buffer(Manager.ID3D11Device,
                size = Marshal.SizeOf<T>() * data.Length, SharpDX.Direct3D11.ResourceUsage.Default,
                SharpDX.Direct3D11.BindFlags.ConstantBuffer, SharpDX.Direct3D11.CpuAccessFlags.None,
                SharpDX.Direct3D11.ResourceOptionFlags.None, 0);

            Update(data);
        }
    }
}
