using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presenter
{
    public class ConstantBuffer<T> : Buffer where T : struct
    {
        private SharpDX.Direct3D12.ConstantBufferViewDescription bufferview;

        private IntPtr resourceStart;

        public ConstantBuffer(int dataCount = 1)
        {
            resource = Engine.ID3D12Device.CreateCommittedResource(
                new SharpDX.Direct3D12.HeapProperties(SharpDX.Direct3D12.HeapType.Upload),
                SharpDX.Direct3D12.HeapFlags.None, SharpDX.Direct3D12.ResourceDescription.Buffer(
                    size = dataCount*SharpDX.Utilities.SizeOf<T>()),
                SharpDX.Direct3D12.ResourceStates.GenericRead);

            resourceStart = resource.Map(0);

            bufferview = new SharpDX.Direct3D12.ConstantBufferViewDescription()
            {
                BufferLocation = resource.GPUVirtualAddress,
                SizeInBytes = (Size + 255) & ~255
            };

            count = dataCount;
        }

        public ConstantBuffer(T data)
        {
            resource = Engine.ID3D12Device.CreateCommittedResource(
              new SharpDX.Direct3D12.HeapProperties(SharpDX.Direct3D12.HeapType.Upload),
              SharpDX.Direct3D12.HeapFlags.None, SharpDX.Direct3D12.ResourceDescription.Buffer(size = SharpDX.Utilities.SizeOf<T>()),
              SharpDX.Direct3D12.ResourceStates.GenericRead);

            resourceStart = resource.Map(0);

            Update(ref data);

            bufferview = new SharpDX.Direct3D12.ConstantBufferViewDescription()
            {
                BufferLocation = resource.GPUVirtualAddress,
                SizeInBytes = (Size + 255) & ~255
            };

            count = 1;
        }

        public ConstantBuffer(T[] data)
        {
            resource = Engine.ID3D12Device.CreateCommittedResource(
              new SharpDX.Direct3D12.HeapProperties(SharpDX.Direct3D12.HeapType.Upload),
              SharpDX.Direct3D12.HeapFlags.None, SharpDX.Direct3D12.ResourceDescription.Buffer(size = (SharpDX.Utilities.SizeOf<T>() * data.Length)),
              SharpDX.Direct3D12.ResourceStates.GenericRead);

            resourceStart = resource.Map(0);

            Update(data);

            bufferview = new SharpDX.Direct3D12.ConstantBufferViewDescription()
            {
                BufferLocation = resource.GPUVirtualAddress,
                SizeInBytes = (Size + 255) & ~255
            };

            count = data.Length;
        }

        public override void Update(IntPtr data)
        {
            SharpDX.Utilities.CopyMemory(resourceStart, data, size);
        }

#pragma warning disable CS0693 // Type parameter has the same name as the type parameter from outer type
        public override void Update<T>(T[] data)
#pragma warning restore CS0693 // Type parameter has the same name as the type parameter from outer type
        {
            SharpDX.Utilities.Write(resourceStart, data, 0,
               data.Length);
        }

#pragma warning disable CS0693 // Type parameter has the same name as the type parameter from outer type
        public override void Update<T>(ref T data)
#pragma warning restore CS0693 // Type parameter has the same name as the type parameter from outer type
        {
            SharpDX.Utilities.Write(resourceStart,ref data);
        }



        internal SharpDX.Direct3D12.ConstantBufferViewDescription BufferView => bufferview;
    }
}
