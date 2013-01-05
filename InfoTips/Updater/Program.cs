using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using System.Text;

namespace Updater
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length > 0 && string.Compare("Update", args[0], true) == 0 && File.Exists("Update/InfoTips.exe"))
            {
                Process[] procList = Process.GetProcessesByName("InfoTips");
                foreach (Process proc in procList)
                {
                    proc.Kill();
                }

                int times = 1;
                while (times++ > 0)
                {
                    Thread.Sleep(1000);
                    try
                    {
                        File.Delete("InfoTips.exe");
                        break;
                    }
                    catch (Exception e)
                    {
                        if (times >= 10)
                        {
                            MessageBox.Show("无法完成更新,错误信息:\"" + e.Message + "\"", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return;
                        }
                    }
                }

                File.Copy("Update/InfoTips.exe", "InfoTips.exe", true);

                try
                {
                    File.Delete("Update/InfoTips.exe");
                }
                catch (Exception e)
                {
                    MessageBox.Show("更新错误："+e.Message);
                }

                MessageBox.Show("更新完成", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Process.Start("InfoTips.exe", "NoCheck");
            }
        }
    }
}