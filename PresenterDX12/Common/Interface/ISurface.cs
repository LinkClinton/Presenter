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

        (float red, float green, float blue, float alpha) BackGround { get; set; }

        int Width { get; }
        int Height { get; }
    }
}
