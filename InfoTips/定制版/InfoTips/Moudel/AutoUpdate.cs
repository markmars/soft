using AutoUpdate;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using System.Xml;

namespace InfoTips
{
    public class AutoUpdate
    {
        WebUtility wu = new WebUtility();

        [DllImport("wininet.dll")]
        private extern static bool InternetGetConnectedState(out int Description, int ReservedValue);

        private bool IsConnectedToInternet()
        {
            int Desc;
            return InternetGetConnectedState(out Desc, 0);
        }
        public UpdateAction CheckUpdate()
        {
            if (!IsConnectedToInternet())
                return UpdateAction.需要联网;

            string strXmlPath = Application.StartupPath + "\\UpdateList.xml", serverXmlFile = string.Empty, strTempPath;
            XmlFiles updaterXmlFiles;
            try
            {
                updaterXmlFiles = new XmlFiles(strXmlPath);
            }
            catch (Exception exc)
            {
                MessageBox.Show("读取配置文件出错!" + exc.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return UpdateAction.更新错误;
            }
            string strUrl = updaterXmlFiles.GetNodeValue("//Url");

            AppUpdater appUpdater = new AppUpdater();
            appUpdater.UpdaterUrl = strUrl + "/UpdateList.xml";
            try
            {
                strTempPath = Environment.GetEnvironmentVariable("Temp") + "\\" + "_" + updaterXmlFiles.FindNode("//Application").Attributes["applicationId"].Value + "_" + "l" + "_" + "i" + "_" + "s" + "_";
                appUpdater.DownAutoUpdateFile(strTempPath);
            }
            catch (Exception exc)
            {
                MessageBox.Show("与服务器连接失败,操作超时!" + exc.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return UpdateAction.更新错误;
            }

            serverXmlFile = strTempPath + "\\UpdateList.xml";
            if (!File.Exists(serverXmlFile))
                return UpdateAction.更新错误;
            Hashtable htUpdateFile;
            int nCount = appUpdater.CheckForUpdate(serverXmlFile, strXmlPath, out htUpdateFile);
            if (nCount > 0)
                return UpdateAction.需要更新;
            else if (nCount == -1)
                return UpdateAction.程序损坏;
            else
                return UpdateAction.最新版本;
        }
    }
}