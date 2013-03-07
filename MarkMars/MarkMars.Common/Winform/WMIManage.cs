using System;
using System.Collections.Generic;
using System.Text;
using System.Management;
using System.Net;
using System.IO;

namespace MarkMars.Common.Winform
{
    /// <summary>
    /// 系统环境（硬件）侦测类别
    /// 功能： 识别硬盘、网卡、CPU等信息
    /// </summary>
    public class WMIManage
    {
        /// <summary>
        /// 通用查询函数
        /// </summary>
        /// <param name="Querystring">查询语句，原型:SELECT * FROM Win32_Processor(查询cpu信息)</param>
        /// <param name="Item_Name">列名</param>
        /// <returns>信息</returns>
        public string WMI_Searcher(string Querystring, string Item_Name)
        {
            try
            {
                string Result = string.Empty;
                ManagementObjectSearcher MOS = new ManagementObjectSearcher(Querystring);
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
        public string Get计算机名()
        {
            try
            {
                string Result = string.Empty;
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
        public string Get操作系统()
        {
            try
            {
                string Result = string.Empty;
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
        public string Get内存信息_MB()
        {
            try
            {
                string Result = string.Empty;
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
        public string GetCPU信息()
        {
            try
            {
                string Result = string.Empty;
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
        public string Get计算机登录名()
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
        public string Get网卡硬件地址()
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
        public string GetPC类型()
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
        public string Get内网ip()
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
        public string Get外网ip()
        {
            try
            {
                string Result = string.Empty;
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
        public string Get外网ip归属地()
        {
            try
            {
                string Result = string.Empty;
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

        /// <summary>
        /// 获取所有共享资源
        /// </summary>
        /// <returns>共享资源</returns>
        public string Get共享资源()
        {
            try
            {
                string Result = string.Empty;
                ManagementObjectSearcher MOS = new ManagementObjectSearcher("SELECT * FROM Win32_share");
                ManagementObjectCollection MOC = MOS.Get();
                foreach (ManagementObject MOB in MOC)
                {
                    Result = MOB.GetText(TextFormat.Mof);
                    break;
                }
                MOC.Dispose();
                MOS.Dispose();
                return Result;
            }
            catch { return "unknow"; }
        }

        /// <summary>
        /// 设置共享文件夹
        /// 首先，这需要以有相应权限的用户登录系统才行
        /// 将
        /// object[] obj = {"C:\\Temp","我的共享",0,10,"Dot Net 实现的共享",""};
        /// 改为
        /// object[] obj = {"C:\\Temp","我的共享",0,null,"Dot Net 实现的共享",""};
        /// 就可以实现授权给最多用户了。
        /// </summary>
        /// <param name="strFloderPath">要共享的文件夹路径</param>
        /// <param name="strShareName">共享名称</param>
        /// <param name="strDescription">描述</param>
        public void Set共享资源(string strFloderPath, string strShareName, string strDescription)
        {

            ManagementClass MC = new ManagementClass(new ManagementPath("Win32_Share"));
            object[] obj = { strFloderPath, strShareName, 0, 10, strDescription, "" };
            MC.InvokeMethod("create", obj);
        }

        /// <summary>
        /// 获取系统服务的运行状态
        /// </summary>
        /// <returns></returns>
        public List<string> Get服务运行状态()
        {
            List<string> ls = new List<string>();
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_Service");
            foreach (ManagementObject mo in searcher.Get())
            {
                ls.Add(mo["Name"].ToString());
                ls.Add(mo["StartMode"].ToString());
                if (mo["Started"].Equals(true))

                    ls.Add("Started");
                else
                    ls.Add("Stop");
                ls.Add(mo["StartName"].ToString());
            }
            return ls;
        }

        /// <summary>
        /// 获取全部ip信息
        /// </summary>
        /// <returns></returns>
        public List<string> Get全部IP信息()
        {
            List<string> ls = new List<string>();
            ManagementClass mc = new ManagementClass("Win32_NetworkAdapterConfiguration");
            ManagementObjectCollection moc = mc.GetInstances();
            foreach (ManagementObject mo in moc)
            {
                if (!(bool)mo["IPEnabled"])
                    continue;
                ls.Add(mo["Caption"].ToString());
                ls.Add(mo["ServiceName"].ToString());
                ls.Add(mo["MACAddress"].ToString());
                ls.Add(mo["IPAddress"].ToString());
                ls.Add(mo["IPSubnet"].ToString());
            }
            return ls;
        }

        #region 设置ip
        public void SwitchTopublic()
        {
            ManagementBaseObject inPar = null;
            ManagementBaseObject outPar = null;
            ManagementClass mc = new ManagementClass("Win32_NetworkAdapterConfiguration");
            ManagementObjectCollection moc = mc.GetInstances();
            foreach (ManagementObject mo in moc)
            {
                if (!(bool)mo["IPEnabled"])
                    continue;
                inPar = mo.GetMethodParameters("Enablepublic");
                inPar["IPAddress"] = new string[] { "192.168.1.1" };
                inPar["SubnetMask"] = new string[] { "255.255.255.0" };
                outPar = mo.InvokeMethod("Enablepublic", inPar, null);
                break;
            }
        }
        public void SwitchToDHCP()
        {
            ManagementBaseObject inPar = null;
            ManagementBaseObject outPar = null;
            ManagementClass mc = new ManagementClass("Win32_NetworkAdapterConfiguration");
            ManagementObjectCollection moc = mc.GetInstances();
            foreach (ManagementObject mo in moc)
            {
                if (!(bool)mo["IPEnabled"])
                    continue;
                inPar = mo.GetMethodParameters("EnableDHCP");
                outPar = mo.InvokeMethod("EnableDHCP", inPar, null);
                break;
            }
        }
        #endregion
        #region 远程关闭计算机
        public void StartCom()
        {
            //连接远程计算机
            ConnectionOptions co = new ConnectionOptions();
            co.Username = "john";
            co.Password = "john";
            System.Management.ManagementScope ms = new System.Management.ManagementScope("\\\\192.168.1.2\\root\\cimv2", co);
            //查询远程计算机
            System.Management.ObjectQuery oq = new System.Management.ObjectQuery("SELECT * FROM Win32_OperatingSystem");
            ManagementObjectSearcher query1 = new ManagementObjectSearcher(ms, oq);
            ManagementObjectCollection queryCollection1 = query1.Get();
            foreach (ManagementObject mo in queryCollection1)
            {
                string[] ss = { "" };
                mo.InvokeMethod("Reboot", ss);
                string str = mo.ToString();
            }
        }
        #endregion
        #region 利用WMI创建一个新的进程
        public void Create进程()
        {
            //Get the object on which the method will be invoked
            ManagementClass processClass = new ManagementClass("Win32_Process");
            //Get an input parameters object for this method
            ManagementBaseObject inParams = processClass.GetMethodParameters("Create");
            //Fill in input parameter values
            inParams["CommandLine"] = "calc.exe";
            //Execute the method
            ManagementBaseObject outParams = processClass.InvokeMethod("Create", inParams, null);
            //Display results
            //Note: The return code of the method is provided in the "returnvalue" property of the outParams object
            string str = "Creation of calculator process returned: " + outParams["returnvalue"];
            str = "Process ID: " + outParams["processId"];
        }
        #endregion
        #region 如何通过WMI终止一个进程
        public void Kill进程()
        {
            ManagementObject service = new ManagementObject("win32_service=\"winmgmt\"");
            InvokeMethodOptions options = new InvokeMethodOptions();
            options.Timeout = new TimeSpan(0, 0, 0, 5);
            ManagementBaseObject outParams = service.InvokeMethod("StopService", null, options);
            string str = "Return Status = " + outParams["Returnvalue"];
        }
        #endregion
        #region 如何用WMI 来获取远程机器的目录以及文件
        public void Get远程机器目录和文件()
        {
            ManagementObject disk = new ManagementObject("win32_logicaldisk.deviceid=\"c:\"");
            disk.Get();
            string str="Logical Disk Size = " + disk["Size"] + " bytes";
        }
        #endregion
        #region 网卡的MAC地址
        public void Get网卡MAC地址()
        {
            ManagementClass mc = new ManagementClass("Win32_NetworkAdapterConfiguration");
            ManagementObjectCollection moc = mc.GetInstances();
            string str;
            foreach (ManagementObject mo in moc)
            {
                if ((bool)mo["IPEnabled"] == true)
                    str = "MAC address" + mo["MacAddress"].ToString() + "<br>";
                mo.Dispose();
            }
        }
        #endregion
        #region CPU的系列号
        public void GetCPU序列号()
        {
            string cpuInfo = "";
            ManagementClass cimobject = new ManagementClass("Win32_Processor");
            ManagementObjectCollection moc = cimobject.GetInstances();
            foreach (ManagementObject mo in moc)
            {
                cpuInfo = mo.Properties["ProcessorId"].Value.ToString();
            }
        }
        #endregion
        #region 主板的系列号
        public void Get主板序列号()
        {
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_BaseBoard");
            foreach (ManagementObject share in searcher.Get())
            {
                string str = "主板制造商:" + share["Manufacturer"].ToString();
                str = "型号:" + share["Product"].ToString();
                str = "序列号:" + share["SerialNumber"].ToString();
            }
        }
        #endregion
        #region 获取硬盘ID
        public void Get硬盘ID()
        {
            string HDid;
            ManagementClass cimobject = new ManagementClass("Win32_DiskDrive");
            ManagementObjectCollection moc = cimobject.GetInstances();
            foreach (ManagementObject mo in moc)
            {
                HDid = (string)mo.Properties["Model"].Value;
            }
        }
        #endregion
        #region 获取本机的用户列表
        public void Get本机用户列表()
        {
            SelectQuery query = new SelectQuery("SELECT * FROM Win32_UserAccount");
            ManagementObjectSearcher searcher = new ManagementObjectSearcher(query);
            foreach (ManagementObject os in searcher.Get())
            {
                string str = os["Name"].ToString();
            }
        }
        #endregion
        #region 操作技巧
        //网卡的MAC地址
        //SELECT MACAddress FROM Win32_NetworkAdapter WHERE ((MACAddress Is Not NULL) AND (Manufacturer <> 'Microsoft'))
        //结果：08:00:46:63:FF:8C
        //CPU的系列号
        //Select ProcessorId From Win32_Processor
        //结果：3FEBF9FF00000F24
        //主板的系列号
        //Select SerialNumber From Win32_BIOS
        //结果：28362630-3700521
        #endregion
        #region WMI异常处理
        public void 异常处理()
        {
            ManagementObjectCollection queryCollection1;
            ConnectionOptions co = new ConnectionOptions();
            co.Username = "administrator";
            co.Password = "111";
            try
            {
                System.Management.ManagementScope ms = new System.Management.ManagementScope(@"\\csnt3\root\cimv2", co);
                System.Management.ObjectQuery oq = new System.Management.ObjectQuery("SELECT * FROM Win32_OperatingSystem");
                ManagementObjectSearcher query1 = new ManagementObjectSearcher(ms, oq);

                queryCollection1 = query1.Get();
                foreach (ManagementObject mo in queryCollection1)
                {
                    string[] ss = { "" };
                    mo.InvokeMethod("Reboot", ss);
                    Console.WriteLine(mo.ToString());
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine("error");
            }
        }
        #endregion
        #region 机器码算法一
        /// <summary>
        /// 机器码算法一
        /// </summary>
        /// <param name="HardDisk">硬盘序号</param>
        /// <param name="sn">cpu序列号</param>
        /// <returns></returns>
        private string GetRegPassword(string HardDisk, char[] sn)
        {
            long Num1 = 0, Num2 = 0, Num3 = 0;
            char[] DirName = HardDisk.ToCharArray();
            int len = HardDisk.Length, i;

            if (len != 0)
            {
                for (i = 1; i <= len; i++)
                {
                    int temp = DirName[i - 1];
                    Num1 = (long)((Num1 + temp * i) * (i * Math.Sqrt(temp + 5))) % 100000;
                    Num2 = (long)(Num2 * i + ((Math.Pow(temp, 3) * i))) % 100000;
                    Num3 = (long)(Num2 + (Math.Sqrt(Num1) * (i + 2))) % 100000;
                }
                //以下把三个算法结果分别生成5个字符，共有15个
                for (i = 0; i < 5; i++)
                    sn[i] = (char)((Num1 + 31 + i * i * i) % 128);
                for (i = 5; i < 10; i++)
                    sn[i] = (char)((Num2 + 31 + i * i * i) % 128);
                for (i = 10; i < 15; i++)
                    sn[i] = (char)((Num3 + 31 + i * i * i) % 128);
                sn[15] = '8';
                //以下循环把所有生成的字符转换为0---9，A---Z，a----z
                string str = "";
                for (i = 0; i < 15; i++)
                {
                    while ((sn[i] < '0' || sn[i] > '9') && (sn[i] < 'A' || sn[i] > 'Z') && (sn[i] < 'a' || sn[i] > 'z'))
                    {
                        sn[i] = (char)((sn[i] + 31 + 7 * i) % 128);
                        str += sn[i];
                    }
                }
                return str;
            }
            else
                return "";
        }
        #endregion
    }
}
