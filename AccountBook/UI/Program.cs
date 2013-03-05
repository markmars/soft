using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace AccountBook.UI
{
    /// <summary>  
    /// 作者：MarkMars  
    /// 时间：2013-2-19 16:42:52  
    /// 公司:  
    /// 版权：2013-2013  
    /// CLR版本：4.0.30319.17929  
    /// 博客地址：http://www.wakealone.com  
    /// Program说明：本代码版权归MarkMars所有，使用时必须带上MarkMars博客地址  
    /// 唯一标识：739887af-b3d2-4915-8882-d48abd9389f7  
    /// </summary>  
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}
