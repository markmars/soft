using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.IO;

namespace MarkMars.Common
{
    public class INIFile
    {
        private string m_strFileName;

        private List<string> m_listLine;

        public INIFile(string strFileName)
        {
            m_strFileName = strFileName;

            Load();
        }

        private void Load()
        {
            StreamReader sr = new StreamReader(m_strFileName, Encoding.GetEncoding("gb2312"));
            string[] strsLine = sr.ReadToEnd().Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
            sr.Close();

            m_listLine = new List<string>();
            m_listLine.AddRange(strsLine);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="strName"></param>
        /// <returns>如果找不到, 返回null</returns>
        public string GetValue(string strSection, string strName)
        {
            string strSectionName = string.Format("[{0}]", strSection);

            bool bInSection = false;
            foreach (string _strLine in m_listLine)
            {
                string strLine = _strLine.Trim();
                //strLine = strLine.Replace(" ", string.Empty);

                if (strLine.StartsWith(";"))
                    continue;//注释
                else if (strLine.StartsWith("[") && strLine.EndsWith("]"))
                {//section
                    if (string.Compare(strLine, strSectionName, true) == 0)
                        bInSection = true;
                    else
                        bInSection = false;
                }
                else if (strLine.Contains("="))
                {//普通内容
                    if (!bInSection)
                        continue;

                    if (strLine.StartsWith(strName + "="))
                        return strLine.Substring(strName.Length + 1);
                }
                else
                {//垃圾内容, 忽略
                }
            }

            return null;
        }
    }
}
