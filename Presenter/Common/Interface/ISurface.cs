using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presenter
{
    interface ISurface
    {
        void Reset(int new_width, int new_height, bool windowed = true);

        Brush BackGround { get; set; }

        float Width { get; }
        float Height { get; }
    }
}
