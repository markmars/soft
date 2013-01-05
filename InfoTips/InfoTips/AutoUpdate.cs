using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace InfoTips
{
    public enum UpdateAction
    {
        需要联网,
        需要更新,
        最新版本
    }
    public class AutoUpdate
    {
        #region 判断是否联网
        [DllImport("wininet.dll")]
        private extern static bool InternetGetConnectedState(out int Description, int ReservedValue);
        private bool IsConnectedToInternet()
        {
            int Desc;
            return InternetGetConnectedState(out Desc, 0);
        }
        #endregion

        int m_BlockLength = 10240;
        string m_TempFile = "Update/InfoTips.exe";
        WebUtility wu = new WebUtility();

        public UpdateAction CheckUpdate(string version)
        {
            if (!IsConnectedToInternet())
                return UpdateAction.需要联网;

            string content = wu.Get(Utility.m_CheckUpdateUrl, string.Empty, false);
            if (string.Compare(version, content, true) != 0)
                return UpdateAction.需要更新;

            return UpdateAction.最新版本;
        }
        public void Update()
        {
            System.IO.Directory.CreateDirectory(Path.GetFileName("Update"));

            using (BinaryReader reader = new BinaryReader(wu.GetResponseStream(Utility.m_UpdateUrl, string.Empty, null, false)))
            {
                using (BinaryWriter writer = new BinaryWriter(new FileStream(m_TempFile, FileMode.Create)))
                {
                    byte[] data = reader.ReadBytes(m_BlockLength);
                    while (data.Length > 0)
                    {
                        writer.Write(data, 0, data.Length);
                        data = reader.ReadBytes(m_BlockLength);
                    }
                }
            }
        }
    }
}