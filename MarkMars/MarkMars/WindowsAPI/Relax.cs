using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace MarkMars.WindowsAPI
{
    /// <summary>
    /// 娱乐函数集
    /// </summary>
    public static class Relax
    {
        [DllImport("User32.DLL")]
        static extern int SendMessage(IntPtr hWnd, uint Msg, int wParam, int lParam);
        static bool monitorOpen = true;

        public static void CloseScreen(IntPtr Handle)
        {
            const int WM_SYSCOMMAND = 0x0112;
            const int SC_MONITORPOWER = 61808;
            monitorOpen = !monitorOpen;
            SendMessage(Handle, WM_SYSCOMMAND, SC_MONITORPOWER, monitorOpen ? 0 : 1);
        }
    }

}
