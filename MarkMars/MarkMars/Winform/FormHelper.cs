using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace MarkMars.Winform
{
    /// <summary>
    /// 窗口渐时显示隐藏
    /// </summary>
    public static class FormHelper
    {
        #region dwflag的取值
        private const Int32 AW_HOR_POSITIVE = 0x00000001;
        private const Int32 AW_HOR_NEGATIVE = 0x00000002;//从左到右显示
        private const Int32 AW_VER_POSITIVE = 0x00000004;//从右到左显示
        private const Int32 AW_VER_NEGATIVE = 0x00000008;//从上到下显示
        private const Int32 AW_CENTER = 0x00000010;//从下到上显示
        private const Int32 AW_HIDE = 0x00010000;//若使用了AW_HIDE标志，则使窗口向内重叠，即收缩窗口；否则使窗口向外扩展，即展开窗口
        private const Int32 AW_ACTIVATE = 0x00020000;//隐藏窗口，缺省则显示窗口
        private const Int32 AW_SLIDE = 0x00040000;//激活窗口。在使用了AW_HIDE标志后不能使用这个标志
        private const Int32 AW_BLEND = 0x00080000;//使用滑动类型。缺省则为滚动动画类型。当使用AW_CENTER标志时，这个标志就被忽略
        #endregion

        [DllImport("user32")]
        private static extern bool AnimateWindow(IntPtr whnd, int dwtime, int dwflag);

        /// <summary>
        /// 由中心向边界扩展渐进打开窗口
        /// </summary>
        public static void ShowFXCenter(IntPtr wnd, int dwtime)
        {
            AnimateWindow(wnd, dwtime, AW_CENTER | AW_ACTIVATE | AW_SLIDE);
        }

        /// <summary>
        /// 由边界向中心扩展渐进关闭窗口
        /// </summary>
        public static void HideFXCenter(IntPtr wnd, int dwtime)
        {
            AnimateWindow(wnd, dwtime, AW_CENTER | AW_HIDE | AW_SLIDE);
        }
    }
}