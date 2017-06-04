using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presenter
{
    interface IResourceHeap
    {
        void AddResource<T>(ConstantBuffer<T> resource) where T : struct;

        void AddResource(ShaderResource resource);

        int Count { get; }

        int MaxCount { get; }
    }
}
