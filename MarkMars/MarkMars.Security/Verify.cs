using System;
using System.Collections.Generic;
using System.Text;
using System.Security;
using System.Security.Cryptography;
using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace MarkMars.Security
{
    /// <summary>
    /// 校验类：主要完成加密解密工作
    /// </summary>
    public class Verify
    {
        /// <summary>
        /// 加密算法，使用Base64加密
        /// </summary>
        /// <returns>加密了的识别码</returns>
        public static string Encryption()
        {
            StringBuilder hashStr = new StringBuilder();
            hashStr.Append(SystemDetect.DetectCPU());
            hashStr.Append(SystemDetect.DetectMotherboard());
            hashStr.Append(SystemDetect.DetectHardDisk()[0]);
            hashStr.Append(SystemDetect.DetectNIC()[0]);
            hashStr.Append(GetProduct());
            byte[] hashBytes = Encoding.ASCII.GetBytes(hashStr.ToString());
            return Convert.ToBase64String(hashBytes,
                Base64FormattingOptions.None);
        }

        /// <summary>
        /// 服务端调用，生成验证识别码
        /// </summary>
        /// <param name="computerInfo">计算机硬件信息</param>
        /// <returns>True/False</returns>
        public static string Encryption(Dictionary<string, string> computerInfo)
        {
            try
            {
                StringBuilder hashStr = new StringBuilder();
                hashStr.Append(computerInfo["CPU"]);
                hashStr.Append(computerInfo["Mainboard"]);
                hashStr.Append(computerInfo["HardDisk"]);
                hashStr.Append(computerInfo["MAC"]);
                hashStr.Append(computerInfo["ProductName"]);
                byte[] hashBytes = Encoding.ASCII.GetBytes(hashStr.ToString());
                return Convert.ToBase64String(hashBytes,
                    Base64FormattingOptions.None);
            }
            catch// (Exception e)
            {
                return string.Empty;
            }
        }

        public static string Decryption()
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// 判断是否当日已经授权
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static bool IsCertified(string fileName)
        {
            //第一步，判断是否存在授权文件
            //第二步，判断授权文件是否合法
            string appPath = Environment.CurrentDirectory;
            string filePath = appPath + "\\" + fileName;
            if (!File.Exists(filePath)) return false;
            string certStr = string.Empty;
            using (FileStream fs = new FileStream(filePath, FileMode.Open))
            {
                using (StreamReader sr = new StreamReader(fs))
                {
                    certStr = sr.ReadToEnd();
                }
            }
            string dynamicSign = Encryption();
            byte[] tmp = Encoding.ASCII.GetBytes(certStr);
            byte[] tmpII = new byte[tmp.Length - 3];
            Array.Copy(tmp, 1, tmpII, 0, tmpII.Length);
            certStr = Encoding.ASCII.GetString(tmpII);
            return String.Compare(certStr, dynamicSign, true) == 0 ? true : false;
        }

        /// <summary>
        /// 生成当日授权文件
        /// </summary>
        /// <param name="fileName">文件名称</param>
        public static void SaveCertifiedFile(string fileName)
        {
            string getStr = GetCertifiedStr();
            fileName = Environment.CurrentDirectory + "\\" + fileName;
            if (File.Exists(fileName)) File.Delete(fileName);
            using (FileStream fs = new FileStream(fileName, FileMode.OpenOrCreate))
            {
                using (StreamWriter sw = new StreamWriter(fs))
                {
                    byte[] saveBytes = Encoding.ASCII.GetBytes(getStr);
                    fs.Write(saveBytes, 0, saveBytes.Length);
                }
            }
        }

        private static string GetCertifiedStr()
        {
            string signStr = Encryption();
            byte[] signBytes = Encoding.ASCII.GetBytes(signStr);
            byte[] certBytes = new byte[signBytes.Length + 3];
            certBytes[0] = Convert.ToByte(0);
            Array.Copy(signBytes, 0, certBytes, 1, signBytes.Length);
            certBytes[certBytes.Length - 2] = Convert.ToByte(23);
            certBytes[certBytes.Length - 1] = Convert.ToByte(7);
            return Encoding.ASCII.GetString(certBytes);
        }

        private static string GetProduct()
        {
            AppDomain app = AppDomain.CurrentDomain;
            Assembly[] ass = app.GetAssemblies();
            string assName = string.Empty;
            foreach (Assembly item in ass)
            {
                if (item.GlobalAssemblyCache) continue;
                if (!string.IsNullOrEmpty(item.ManifestModule.Name) && !item.ManifestModule.Name.Contains("vshost") &&
                    String.Compare(item.ManifestModule.Name.Substring(item.ManifestModule.Name.Length - 3), "exe", true) == 0)
                {
                    assName = item.ManifestModule.Name.Trim();
                    break;
                }
            }
            Assembly mainAss = Assembly.Load(assName.Substring(0,assName.Length-4));
            object[] attributes = mainAss.GetCustomAttributes(typeof(AssemblyProductAttribute), false);
            if (attributes != null)
                return (attributes[0] as AssemblyProductAttribute).Product.Trim();
            return string.Empty;
        }
    }
}
