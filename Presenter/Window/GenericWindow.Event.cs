using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presenter
{
    public partial class GenericWindow
    {
        private APILibrary.Win32.Internal.WndProc eventProcess;


        private IntPtr Process(IntPtr Hwnd, uint message,
           IntPtr wParam, IntPtr lParam)
        {
            APILibrary.Win32.WinMsg type = (APILibrary.Win32.WinMsg)message;
            switch (type)
            {
                default:
                    return APILibrary.Win32.Internal.DefWindowProc(Hwnd, message, wParam, lParam);
            }
        }
    }
}
