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
        static void Main()
        {
            if (CheckUpdate())
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                bool flag;
                System.Threading.Mutex AppMutex = new System.Threading.Mutex(true, "InfoTips.exe", out flag);
                if (flag)
                {
                    DevExpress.UserSkins.BonusSkins.Register();
                    if (new FormLogin().ShowDialog() == DialogResult.OK)
                    {
                        Application.Run(new FrmMain());
                        AppMutex.ReleaseMutex();
                    }
                }
                else
                    MessageBox.Show("程序已经运行!", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
                Application.Exit();
        }

        static bool CheckUpdate()
        {
            bool bb = false;
            AutoUpdate autoUp = new AutoUpdate();
            switch (autoUp.CheckUpdate())
            {
                case UpdateAction.需要联网:
                    MessageBox.Show("连接不上服务器，请检查网络连接!", "友情提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    break;
                case UpdateAction.需要更新:
                    MessageBox.Show("您的版本已过期,请确定后会自动更新!", "重要提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Process.Start("AutoUpdate.exe");
                    break;
                case UpdateAction.更新错误:
                    MessageBox.Show("更新出现未知错误，请联系作者!", "友情提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    break;
                case UpdateAction.程序损坏:
                    MessageBox.Show("程序损坏，请到官方下载最新版安装使用!", "友情提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    break;
                case UpdateAction.最新版本:
                    bb = true;
                    break;
            }
            return bb;
        }
    }
}