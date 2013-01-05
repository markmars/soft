using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;

namespace InfoTips
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            //if (args.Length == 0 || string.Compare("NoCheck", args[0], true) != 0)
            //{
            //    switch (AutoUpdate.CheckUpdate(Application.ProductVersion))
            //    {
            //        case UpdateAction.需要联网:
            //            MessageBox.Show("连接不上服务器，请检查网络连接!", "友情提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //            Application.Exit();
            //            break;
            //        case UpdateAction.需要更新:
            //            MessageBox.Show("您的版本已过期,请确定后会自动更新!", "重要提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //            AutoUpdate.Update();
            //            Process.Start("Updater.exe", "Update");
            //            Application.Exit();
            //            return;
            //    }
            //}

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //bool flag;
            //System.Threading.Mutex AppMutex = new System.Threading.Mutex(true, "InfoTips.exe", out flag);
            //if (flag)
            //{
            //    FormLogin login = new FormLogin();
            //    if (login.ShowDialog() == DialogResult.OK)
            //    {
            DevExpress.UserSkins.BonusSkins.Register();
            Application.Run(new FormMain());
            //        AppMutex.ReleaseMutex();
            //    }
            //}
            //else
            //    MessageBox.Show("程序已经运行!", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}