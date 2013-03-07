using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Security;
using System.Management;

[assembly: AllowPartiallyTrustedCallers]
namespace MarkMars.Security
{
    /// <summary>
    /// 系统环境（硬件）侦测类别
    /// 功能： 识别硬盘、网卡、CPU以及主板特征码
    /// </summary>
    public class SystemDetect
    {
        /// <summary>
        /// 硬盘识别
        /// </summary>
        /// <returns>硬盘标识号,注：识别多硬盘</returns>
        public static string[] DetectHardDisk()
        {
            List<string> HDInfo = new List<string>();
            HDInfo.Add(String.Empty);
            //SelectQuery selectQuery = new SelectQuery("select * from Win32_PhysicalMedia");
            //ManagementObjectSearcher searcher = new ManagementObjectSearcher(selectQuery);
            //foreach (ManagementObject disk in searcher.Get())
            //{
            //    HDInfo.Add(disk["SerialNumber"].ToString().Trim());
            //}
            return HDInfo.ToArray();
        }

        /// <summary>
        /// 网卡识别
        /// </summary>
        /// <returns>网卡MAC地址,注：识别多网卡</returns>
        public static string[] DetectNIC()
        {
            List<string> NICInfo = new List<string>();
            NICInfo.Add(String.Empty);
            //SelectQuery selectQuery = new SelectQuery("Win32_NetworkAdapterConfiguration");
            //ManagementObjectSearcher searcher = new ManagementObjectSearcher(selectQuery);
            //string kk = string.Empty;
            //foreach (ManagementObject disk in searcher.Get())
            //{
            //    if (string.Compare(disk["IPEnabled"].ToString(), "True", true) != 0)
            //        continue;
            //    NICInfo.Add(disk["MacAddress"].ToString().Trim().Replace(':','-'));
            //}
            return NICInfo.ToArray();
        }

        /// <summary>
        /// CPU识别
        /// </summary>
        /// <returns>CPU标识号</returns>
        public static string DetectCPU()
        {
            string CPUInfo = string.Empty;
            //ManagementObjectSearcher MySearcher = new ManagementObjectSearcher("SELECT * FROM Win32_Processor");
            //foreach (ManagementObject MyObject in MySearcher.Get())
            //{
            //    CPUInfo = MyObject["ProcessorId"].ToString().Trim();
            //}
            
            return CPUInfo;
        }

        /// <summary>
        /// 主板识别
        /// </summary>
        /// <returns>主板标识号</returns>
        public static string DetectMotherboard()
        {
            string MyInfo = string.Empty;
            //ManagementClass mc = new ManagementClass("WIN32_BaseBoard");
            //ManagementObjectCollection moc = mc.GetInstances();
            //foreach (ManagementObject mo in moc)
            //{
            //    MyInfo = mo["SerialNumber"].ToString().Trim();
            //    break;
            //}
            return MyInfo;
        }
    }
}
