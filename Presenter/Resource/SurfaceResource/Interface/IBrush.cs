using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presenter
{
    interface IBrush
    {
        float Red { get; }
        float Green { get; }
        float Blue { get; }
        float Alpha { get; }
    }
}
