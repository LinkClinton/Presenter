using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presenter
{
    public partial class GenericWindow
    {
        private static uint AppStyle = (uint)(APILibrary.Win32.AppInfoStyle.CS_HREDRAW | APILibrary.Win32.AppInfoStyle.CS_VREDRAW);
        private static uint AppExStyle = (uint)(APILibrary.Win32.WindowExStyles.WS_EX_LAYERED);
        private static uint AppWindowStyle = (uint)(APILibrary.Win32.WindowStyles.WS_OVERLAPPEDWINDOW ^ APILibrary.Win32.WindowStyles.WS_SIZEBOX);


    }
}
