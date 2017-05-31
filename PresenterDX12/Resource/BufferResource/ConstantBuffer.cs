﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presenter
{
    public class ConstantBuffer<T> : Buffer, IConstantBuffer where T : struct
    {
        public ConstantBuffer(int dataSize, int dataCount = 1)
        {
            resource = Manager.ID3D12Device.CreateCommittedResource(
                new SharpDX.Direct3D12.HeapProperties(SharpDX.Direct3D12.HeapType.Upload),
                SharpDX.Direct3D12.HeapFlags.None, SharpDX.Direct3D12.ResourceDescription.Buffer(size = dataSize),
                SharpDX.Direct3D12.ResourceStates.GenericRead);

            resourceStart = resource.Map(0);

            count = dataCount;
        }

        public ConstantBuffer(T data)
        {
            resource = Manager.ID3D12Device.CreateCommittedResource(
              new SharpDX.Direct3D12.HeapProperties(SharpDX.Direct3D12.HeapType.Upload),
              SharpDX.Direct3D12.HeapFlags.None, SharpDX.Direct3D12.ResourceDescription.Buffer(size = SharpDX.Utilities.SizeOf<T>()),
              SharpDX.Direct3D12.ResourceStates.GenericRead);

            resourceStart = resource.Map(0);

            Update(ref data);

            count = 1;
        }

        public ConstantBuffer(T[] data)
        {
            resource = Manager.ID3D12Device.CreateCommittedResource(
              new SharpDX.Direct3D12.HeapProperties(SharpDX.Direct3D12.HeapType.Upload),
              SharpDX.Direct3D12.HeapFlags.None, SharpDX.Direct3D12.ResourceDescription.Buffer(size = (SharpDX.Utilities.SizeOf<T>() * data.Length)),
              SharpDX.Direct3D12.ResourceStates.GenericRead);

            resourceStart = resource.Map(0);

            Update(data);

            count = data.Length;
        }
    }
}
