﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presenter
{
    public class Resource : IResource
    {
        protected int size;

        protected SharpDX.Direct3D12.Resource resource;

        protected IntPtr resourceStart;

        public virtual void Update<T>(ref T data) where T : struct
        {
            SharpDX.Utilities.Write(resourceStart, ref data);
        }

        public virtual void Update<T>(T[] data) where T : struct
        {
            SharpDX.Utilities.Write(resourceStart, data, SharpDX.Utilities.SizeOf<T>(),
                data.Length);
        }

        public int Size => size;

        internal SharpDX.Direct3D12.Resource ID3D12Resource => resource;
    }
}