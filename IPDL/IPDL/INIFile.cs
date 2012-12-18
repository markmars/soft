using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.IO;

namespace IPDL
{
    public class INIFile
    {
        /// <summary>
        /// ini文件名（完整路径）
        /// </summary>
        private string m_strFileName;

        /// <summary>
        /// ini文件内容（行），字符串集合
        /// </summary>
        private List<string> m_listLine;

        /// <summary>
        /// 加载ini文件
        /// </summary>
        /// <param name="strFileName">文件名（完整路径）</param>
        public INIFile(string strFileName)
        {
            Debug.Assert(strFileName != null);
            Debug.Assert(strFileName != string.Empty);
            m_strFileName = strFileName;
            Load();
        }

        /// <summary>
        /// 读取ini文件内容
        /// </summary>
        private void Load()
        {
            StreamReader sr = new StreamReader(m_strFileName);
            string[] strsLine = sr.ReadToEnd().Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
            sr.Close();

            m_listLine = new List<string>();
            m_listLine.AddRange(strsLine);
        }

        /// <summary>
        /// 获得ini文件对应键的值,原型：
        /// [strSection]
        /// strName=value
        /// </summary>
        /// <param name="strSection">类别名称</param>
        /// <param name="strName">键名</param>
        /// <returns>值</returns>
        public string GetValue(string strSection, string strName)
        {
            string strSectionName = string.Format("[{0}]", strSection);

            bool bInSection = false;
            foreach (string _strLine in m_listLine)
            {
                string strLine = _strLine.Trim();
                strLine = strLine.Replace(" ", string.Empty);

                if (strLine.StartsWith("[") && strLine.EndsWith("]"))//section
                {
                    if (string.Compare(strLine, strSectionName, true) == 0)
                        bInSection = true;
                    else
                        bInSection = false;
                }
                else if (strLine.Contains("="))//普通内容
                {
                    if (!bInSection)
                        continue;

                    if (strLine.StartsWith(strName + "="))
                        return strLine.Substring(strName.Length + 1);
                }
                else { }//垃圾内容, 忽略
            }
            return null;
        }
    }
}
