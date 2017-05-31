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
}
