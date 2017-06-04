using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presenter
{
    interface IObjectPasser
    {
        void Pass();

        PrimitiveType PrimitiveType { set; get; }

        int PassBaseVertexLocation { set; get; }

        int PassBaseIndexLocation { set; get; }

        int PassIndexCount { set; get; }
    }
}
