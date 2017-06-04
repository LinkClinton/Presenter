using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presenter
{
    interface IResource
    {
        void Update<T>(ref T data) where T : struct;

        void Update<T>(T[] data) where T : struct;

        int Size { get; }
    }
}
