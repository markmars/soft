using System;
using System.Runtime.InteropServices;
using System.Text;

namespace MarkMars.Common
{
    public class WinAPI
    {
        public delegate Boolean EnumWindowsCallBack(IntPtr hwnd, String param);

        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public Int32 Left;
            public Int32 Top;
            public Int32 Right;
            public Int32 Bottom;
        }

        [DllImport("user32")]
        public static extern IntPtr SetParent(IntPtr wndChild, IntPtr wndNewParent);

        [DllImport("user32")]
        public static extern IntPtr GetParent(IntPtr hwnd);

        [DllImport("user32")]
        public static extern IntPtr FindWindow(String ipClassName, String ipWindowName);

        [DllImport("user32")]
        public static extern IntPtr SetWindowPos(IntPtr hwnd, IntPtr wndInsertAfter, Int32 x, Int32 y, Int32 cx, Int32 cy, Int32 flags);

        [DllImport("user32")]
        public static extern IntPtr GetClientRect(IntPtr hwnd, out RECT rect);

        [DllImport("user32")]
        public static extern Int32 EnumWindows(EnumWindowsCallBack enumWindowsCallBack, String param);

        [DllImport("user32")]
        public static extern Int32 GetWindowText(IntPtr hwnd, StringBuilder lpString, Int32 nMaxCount);

        [DllImport("user32.dll")]
        public static extern IntPtr SendMessage(IntPtr hwnd, Int32 msg, Int32 wParam, Int32 lParam);

        #region 窗体显示特效
        [DllImport("user32")]
        private static extern bool AnimateWindow(IntPtr hwnd, int dwTime, int dwFlags);
        //下面是可用的常量，按照不合的动画结果声明本身须要的
        private const int AW_HOR_POSITIVE = 0x0001;//自左向右显示窗口，该标记可以在迁移转变动画和滑动动画中应用。应用AW_CENTER标记时忽视该标记
        private const int AW_HOR_NEGATIVE = 0x0002;//自右向左显示窗口，该标记可以在迁移转变动画和滑动动画中应用。应用AW_CENTER标记时忽视该标记
        private const int AW_VER_POSITIVE = 0x0004;//自顶向下显示窗口，该标记可以在迁移转变动画和滑动动画中应用。应用AW_CENTER标记时忽视该标记
        private const int AW_VER_NEGATIVE = 0x0008;//自下向上显示窗口，该标记可以在迁移转变动画和滑动动画中应用。应用AW_CENTER标记时忽视该标记该标记
        private const int AW_HIDE = 0x10000;//隐蔽窗口
        private const int AW_ACTIVE = 0x20000;//激活窗口，在应用了AW_HIDE标记后不要应用这个标记
        private const int AW_CENTER = 0x0010;//若应用了AW_HIDE标记，则使窗口向内重叠；不然向外扩大
        private const int AW_SLIDE = 0x40000;//应用滑动类型动画结果，默认为迁移转变动画类型，当应用AW_CENTER标记时，这个标记就被忽视
        private const int AW_BLEND = 0x80000;//应用淡入淡出结果
        /*调用方法
            int x = Screen.PrimaryScreen.WorkingArea.Right - this.Width;
            int y = Screen.PrimaryScreen.WorkingArea.Bottom - this.Height;
            this.Location = new Point(x, y);//设置窗体在屏幕右下角显示
            AnimateWindow(this.Handle, 500, AW_CENTER | AW_ACTIVE | AW_VER_NEGATIVE);//打开
            AnimateWindow(this.Handle, 500, AW_BLEND | AW_HIDE);//关闭
             * */
        #endregion
    }
}