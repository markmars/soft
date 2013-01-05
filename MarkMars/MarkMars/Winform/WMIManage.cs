using System;
using System.Collections.Generic;
using System.Text;
using System.Management;
using System.Net;
using System.IO;

namespace MarkMars.Winform
{
    /// <summary>
    /// 系统环境（硬件）侦测类别
    /// 功能： 识别硬盘、网卡、CPU等信息
    /// </summary>
    public static class WMIManage
    {
        public static string m_计算机名 = Get计算机名();
        public static string m_操作系统 = Get操作系统();
        public static string m_内存大小 = Get内存信息_MB();
        public static string m_CPU信息 = GetCPU信息();
        public static string m_内网IP = Get内网ip();
        public static string m_外网IP = Get外网ip();
        public static string m_IP归属地 = Get外网ip归属地();
        public static string m_计算机登录名 = Get计算机登录名();
        public static string m_网卡硬件地址 = Get网卡硬件地址();
        public static string m_PC类型 = GetPC类型();

        /// <summary>
        /// 通用查询函数
        /// </summary>
        /// <param name="QueryString">查询语句，原型:SELECT * FROM Win32_Processor(查询cpu信息)</param>
        /// <param name="Item_Name">列名</param>
        /// <returns>信息</returns>
        public static String WMI_Searcher(String QueryString, String Item_Name)
        {
            try
            {
                String Result = string.Empty;
                ManagementObjectSearcher MOS = new ManagementObjectSearcher(QueryString);
                ManagementObjectCollection MOC = MOS.Get();
                foreach (ManagementObject MOB in MOC)
                {
                    Result = MOB[Item_Name].ToString();
                    break;
                }
                MOC.Dispose();
                MOS.Dispose();
                return Result;
            }
            catch { return "unknow"; }
        }

        /// <summary>
        /// 获取计算机名
        /// </summary>
        /// <returns>计算机名</returns>
        private static String Get计算机名()
        {
            try
            {
                String Result = string.Empty;
                ManagementObjectSearcher MOS = new ManagementObjectSearcher("SELECT * FROM Win32_ComputerSystem");
                ManagementObjectCollection MOC = MOS.Get();
                foreach (ManagementObject MOB in MOC)
                {
                    Result = MOB["Caption"].ToString();
                    break;
                }
                MOC.Dispose();
                MOS.Dispose();
                return Result;
            }
            catch { return "unknow"; }
        }

        /// <summary>
        /// 获取操作系统信息
        /// </summary>
        /// <returns>操作系统信息</returns>
        private static String Get操作系统()
        {
            try
            {
                String Result = string.Empty;
                ManagementObjectSearcher MOS = new ManagementObjectSearcher("SELECT * FROM Win32_OperatingSystem");
                ManagementObjectCollection MOC = MOS.Get();
                foreach (ManagementObject MOB in MOC)
                {
                    Result = MOB["Caption"].ToString();
                    break;
                }
                MOC.Dispose();
                MOS.Dispose();
                return Result;
            }
            catch { return "unknow"; }
        }

        /// <summary>
        /// 获取内存大小（MB）
        /// </summary>
        /// <returns>内存大小</returns>
        private static String Get内存信息_MB()
        {
            try
            {
                String Result = string.Empty;
                ManagementObjectSearcher MOS = new ManagementObjectSearcher("SELECT * FROM Win32_OperatingSystem");
                ManagementObjectCollection MOC = MOS.Get();
                foreach (ManagementObject MOB in MOC)
                {
                    Result = MOB["TotalVisibleMemorySize"].ToString();
                }
                MOC.Dispose();
                MOS.Dispose();
                return (Convert.ToInt32(Result) / 1024).ToString();
            }
            catch { return "unknow"; }
        }

        /// <summary>
        /// 获取CPU信息
        /// </summary>
        /// <returns>CPU信息</returns>
        private static String GetCPU信息()
        {
            try
            {
                String Result = string.Empty;
                ManagementObjectSearcher MOS = new ManagementObjectSearcher("SELECT * FROM Win32_Processor");
                ManagementObjectCollection MOC = MOS.Get();
                foreach (ManagementObject MOB in MOC)
                {
                    Result = MOB["Caption"].ToString();
                    break;
                }
                MOC.Dispose();
                MOS.Dispose();
                return Result;
            }
            catch { return "unknow"; }
        }

        /// <summary>
        /// 获取计算机登录名
        /// </summary>
        /// <returns>计算机登录名</returns>
        private static string Get计算机登录名()
        {
            try
            {
                string Result = string.Empty;
                ManagementClass MC = new ManagementClass("Win32_ComputerSystem");
                ManagementObjectCollection MOC = MC.GetInstances();
                foreach (ManagementObject MOB in MOC)
                {

                    Result = MOB["UserName"].ToString();

                }
                MOC.Dispose();
                MC.Dispose();
                return Result;
            }
            catch { return "unknow"; }
        }

        /// <summary>
        /// 获取网卡硬件地址
        /// </summary>
        /// <returns>网卡硬件地址</returns>
        private static string Get网卡硬件地址()
        {
            try
            {
                string Result = string.Empty;
                ManagementClass MC = new ManagementClass("Win32_NetworkAdapterConfiguration");
                ManagementObjectCollection MOC = MC.GetInstances();
                foreach (ManagementObject MOB in MOC)
                {
                    if ((bool)MOB["IPEnabled"] == true)
                    {
                        Result = MOB["MacAddress"].ToString();
                        break;
                    }
                }
                MOC.Dispose();
                MC.Dispose();
                return Result;
            }
            catch { return "unknow"; }
        }

        /// <summary>
        /// 获取PC类型
        /// </summary>
        /// <returns>PC类型</returns>
        private static string GetPC类型()
        {
            try
            {
                string Result = string.Empty;
                ManagementClass MC = new ManagementClass("Win32_ComputerSystem");
                ManagementObjectCollection MOC = MC.GetInstances();
                foreach (ManagementObject MOB in MOC)
                {
                    Result = MOB["SystemType"].ToString();
                    break;
                }
                MOC.Dispose();
                MC.Dispose();
                return Result;
            }
            catch { return "unknow"; }
        }

        /// <summary>
        /// 获取内网ip地址
        /// </summary>
        /// <returns>内网ip地址</returns>
        private static String Get内网ip()
        {
            try
            {
                string hostname = Dns.GetHostName();
                IPHostEntry localhost = Dns.GetHostByName(hostname);
                IPAddress localaddr = localhost.AddressList[0];
                return localaddr.ToString();
            }
            catch { return "unknow"; }
        }

        /// <summary>
        /// 获取外网ip
        /// </summary>
        /// <returns>外网ip地址</returns>
        private static String Get外网ip()
        {
            try
            {
                String Result = string.Empty;
                Uri uri = new Uri("http://city.ip138.com/city1.asp");//获得IP的原网址http://www.ip138.com/ip2city.asp。(新浪接口：http://counter.sina.com.cn/ip?ip=127.0.0.1)
                WebRequest webreq = WebRequest.Create(uri);
                Stream s = webreq.GetResponse().GetResponseStream();
                StreamReader sr = new StreamReader(s, Encoding.Default);
                string all = sr.ReadToEnd();         //读取网站返回的数据  格式：您的IP地址是：[x.x.x.x]
                string tempip = all.Substring(all.IndexOf("[") + 1, 15);
                Result = tempip.Replace("]", "").Replace(" ", "").Replace("<", "");     //去除杂项找出ip
                return Result;
            }
            catch { return "unknow"; }
        }
        /// <summary>
        /// 获取外网ip归属地
        /// </summary>
        /// <returns>归属地</returns>
        private static String Get外网ip归属地()
        {
            try
            {
                String Result = string.Empty;
                Uri uri = new Uri("http://city.ip138.com/city1.asp");//获得IP的原网址http://www.ip138.com/ip2city.asp。(新浪接口：http://counter.sina.com.cn/ip?ip=127.0.0.1)
                WebRequest webreq = WebRequest.Create(uri);
                Stream s = webreq.GetResponse().GetResponseStream();
                StreamReader sr = new StreamReader(s, Encoding.Default);
                string all = sr.ReadToEnd();         //读取网站返回的数据  格式：您的IP地址是：[x.x.x.x]
                string tempip = all.Substring(all.IndexOf("来自：") + 3);
                Result = tempip.Replace("</center></body></html>", "").Replace("]", "").Replace(" ", "").Replace("<", "");     //去除杂项找出ip
                return Result;
            }
            catch { return "unknow"; }
        }
    }
}
