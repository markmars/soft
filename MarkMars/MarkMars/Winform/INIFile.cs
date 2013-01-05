using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;

namespace MarkMars.Winform
{
    public class INIFile
    {
        private string strPath;
        [DllImport("kernel32")]
        private static extern bool WritePrivateProfileString(string section, string key, string val, string filePath);
        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section, string key, string defVal, Byte[] retVal, int size, string filePath);

        public INIFile(string strPath)
        {
            FileInfo fileInfo = new FileInfo(strPath);
            if ((!fileInfo.Exists))
                throw (new ApplicationException("Ini文件不存在"));
            this.strPath = strPath;
        }
        public bool KeyExists(string section, string key)
        {
            string strValue = string.Empty;
            strValue = GetValue(section, key);
            return strValue.IndexOf(key) > -1;
        }
        public string GetValue(string section, string key)
        {
            Byte[] Buffer = new Byte[65535];
            int bufLen = GetPrivateProfileString(section, key, "", Buffer, Buffer.GetUpperBound(0), strPath);
            string str = Encoding.GetEncoding(0).GetString(Buffer);
            str = str.Substring(0, bufLen);
            return str.Trim();
        }
        public void SetValue(string section, string key, string value)
        {
            if (!WritePrivateProfileString(section, key, value, strPath))
                throw (new ApplicationException("写Ini文件出错"));
        }
        public List<String> GetSectionKeys(string section)
        {
            Byte[] Buffer = new Byte[16384];
            int bufLen = GetPrivateProfileString(section, null, null, Buffer, Buffer.GetUpperBound(0), strPath);
            List<string> ls = new List<string>();
            GetStringsFromBuffer(Buffer, bufLen, ls);
            return ls;
        }
        public List<string> GetSections(List<string> list_Section)
        {
            byte[] Buffer = new byte[65535];
            int bufLen = 0;
            bufLen = GetPrivateProfileString(null, null, null, Buffer, Buffer.GetUpperBound(0), strPath);
            GetStringsFromBuffer(Buffer, bufLen, list_Section);
            return list_Section;
        }
        public List<string> GetSectionValues(string section)
        {
            List<string> ls = new List<string>();
            List<string> list = new List<string>();
            list = GetSectionKeys(section);
            ls.Clear();
            foreach (string key in list)
            {
                ls.Add(GetValue(section, key));
            }
            return ls;
        }
        public void DelSection(string section)
        {
            if (!WritePrivateProfileString(section, null, null, strPath))
            {
                throw (new ApplicationException("无法删除Ini文件中的section"));
            }
        }
        public void DeleteKey(string section, string key)
        {
            WritePrivateProfileString(section, key, null, strPath);
        }
        private void GetStringsFromBuffer(Byte[] buffer, int bufLen, List<String> ls)
        {
            ls.Clear();
            if (bufLen != 0)
            {
                int start = 0;
                for (int i = 0; i < bufLen; i++)
                {
                    if ((buffer[i] == 0) && ((i - start) > 0))
                    {
                        String s = Encoding.GetEncoding(0).GetString(buffer, start, i - start);
                        ls.Add(s);
                        start = i + 1;
                    }
                }
            }
        }
        ~INIFile()
        {
            WritePrivateProfileString(null, null, null, strPath);
        }
    }
}
