using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;

namespace InfoTips
{
    public static class Utility
    {
        //static PerformanceCounter pc = new PerformanceCounter("Processor", "% Processor Time", "_Total");
        //public static float m_CPU//CPU
        //{
        //    get { return pc.NextValue(); }
        //}
        //public static long m_MemoryAvailable//可用内存
        //{
        //    get
        //    {
        //        long availablebytes = 0;
        //        ManagementClass mos = new ManagementClass("Win32_OperatingSystem");
        //        foreach (ManagementObject mo in mos.GetInstances())
        //        {
        //            if (mo["Freem_PhysicalMemory"] != null)
        //            {
        //                availablebytes = 1024 * long.Parse(mo["Freem_PhysicalMemory"].ToString());
        //            }
        //        }
        //        return availablebytes;
        //    }
        //}
        //public static long m_PhysicalMemory//物理内存
        //{
        //    get
        //    {

        //        long m_m_PhysicalMemory = 0;
        //        ManagementClass mc = new ManagementClass("Win32_ComputerSystem");
        //        ManagementObjectCollection moc = mc.GetInstances();
        //        foreach (ManagementObject mo in moc)
        //        {
        //            if (mo["Totalm_PhysicalMemory"] != null)
        //            {
        //                m_m_PhysicalMemory = long.Parse(mo["Totalm_PhysicalMemory"].ToString());
        //            }
        //        }
        //        return m_m_PhysicalMemory;
        //    }
        //}
        public static readonly string m_CheckUpdateUrl = "";
        public static readonly string m_UpdateUrl = "";
        public static string getUrlSource(string strUrl, string strEncoding)
        {
            string lsResult;
            try
            {
                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(strUrl);
                req.Timeout = 10000;
                HttpWebResponse rep = (HttpWebResponse)req.GetResponse();
                StreamReader sr = new StreamReader(rep.GetResponseStream(), Encoding.GetEncoding(strEncoding));
                lsResult = sr.ReadToEnd();
            }
            catch
            {
                lsResult = "";
                return lsResult;
            }
            return lsResult;
        }
    }
}
