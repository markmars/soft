using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace MarkMars.WindowsAPI
{
    public class GetUSBApi
    {
        const int WM_DEVICECHANGE = 0x2190;
        const int DBT_DEVICEARRIVAL = 0x8000;
        const int DBT_DEVICEREMOVECOMPLETE = 0x8004;
        protected void WndProc(ref Message m)
        {
            try
            {
                //if (m.Msg == WM_DEVICECHANGE)
                //{
                switch (m.WParam.ToInt32())
                {
                    case DBT_DEVICEARRIVAL:     // U盘插入
                        DriveInfo[] s = DriveInfo.GetDrives();
                        foreach (DriveInfo drive in s)
                        {
                            if (drive.DriveType == DriveType.Removable)
                            {
                                Console.WriteLine("USB插入");
                                break;
                            }
                        }
                        break;
                    case DBT_DEVICEREMOVECOMPLETE: //U盘卸载
                        //
                        Console.WriteLine("USB卸载");
                        break;
                    default:
                        break;
                }
                //}
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            //base.WndProc(ref m);
        }
    }
}
