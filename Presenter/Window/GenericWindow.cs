using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presenter
{
    public partial class GenericWindow : IGenericWindow
    {
        private IntPtr handle;

        private APILibrary.Win32.AppInfo appinfo;

        private int width;
        private int height;

        private string tag;

        private int posx;
        private int posy;


        public GenericWindow(string Tag, int Width, int Height)
        {
            eventProcess += Process;

            tag = Tag;

            width = Width;
            height = Height;

            appinfo = new APILibrary.Win32.AppInfo()
            {
                style = AppStyle,
                lpfnWndProc = eventProcess,
                cbClsExtra = 0,
                cbWndExtra = 0,
                hInstance = APILibrary.Win32.Internal.GetModuleHandle(null),
                hbrBackground = IntPtr.Zero,
                hCursor = APILibrary.Win32.Internal.LoadCursor(IntPtr.Zero, (uint)APILibrary.Win32.CursorType.IDC_ARROW),
                hIcon = IntPtr.Zero,
                lpszClassName = tag,
                lpszMenuName = null
            };

            APILibrary.Win32.Internal.RegisterAppinfo(ref appinfo);

            APILibrary.Win32.Rect realRect = new APILibrary.Win32.Rect()
            {
                left = 0,
                top = 0,
                right = width,
                bottom = height
            };

            APILibrary.Win32.Internal.AdjustWindowRect(ref realRect, AppWindowStyle,
                false);

            handle = APILibrary.Win32.Internal.CreateWindowEx(
               AppExStyle, tag, tag, AppWindowStyle, 0x80000000, 0x80000000,
               realRect.right - realRect.left, realRect.bottom - realRect.top,
               IntPtr.Zero, IntPtr.Zero, appinfo.hInstance, IntPtr.Zero);

            APILibrary.Win32.Internal.SetLayeredWindowAttributes(handle, 0
                , 255, (uint)APILibrary.Win32.UpdateLayeredWindowsFlags.ULW_ALPHA);

            APILibrary.Win32.Internal.GetWindowRect(handle, ref realRect);

            posx = realRect.left;
            posy = realRect.top;
        } 


    }
}
