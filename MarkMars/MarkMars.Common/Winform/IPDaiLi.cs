using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Web;

namespace MarkMars.Common.Winform
{
    public static class IPDaiLi
    {
        public static bool YanZhengIP(string sIP, int iTimeOut)
        {
            string sUrl = "http://www.baidu.com";
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(sUrl);
            WebProxy webPro = new WebProxy();
            if (sIP != null && sIP.Trim().Length > 0)
                webPro.Address = new Uri("http://" + sIP);
            DateTime beginTime = DateTime.Now;
            string pingTime;
            try
            {
                request.Timeout = iTimeOut;
                request.ReadWriteTimeout = iTimeOut;
                request.ContentType = "text/xml";
                if (sIP != null && sIP.Trim().Length > 0)
                {
                    request.Proxy = webPro;
                }
                request.GetResponse();
                DateTime endTime = DateTime.Now;
                pingTime = (endTime - beginTime).Milliseconds.ToString();
            }
            catch
            {
                request.Abort();
                return false;
            }
            request.Abort();
            return true;
        }
        private static void SetDaiLi(string ip)
        {
            Microsoft.Win32.RegistryKey rk = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(@"Software/Microsoft/Windows/CurrentVersion/Internet Settings", true);
            rk.SetValue("ProxyEnable", 1);
            rk.SetValue("ProxyServer", ip);
            rk.Close();
        }
        private static void DelDaiLi()
        {
            Microsoft.Win32.RegistryKey rk = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(@"Software/Microsoft/Windows/CurrentVersion/Internet Settings", true);
            rk.SetValue("ProxyEnable", 0);
            rk.SetValue("ProxyServer", "");
            rk.Close();
        }
        //获取真实ipview plaincopy to clipboardPRint
        public static string GetRealIP()
        {
            string ip;
            try
            {
                HttpRequest request = HttpContext.Current.Request;
                if (request.ServerVariables["HTTP_VIA"] != null)
                    ip = request.ServerVariables["HTTP_X_FORWARDED_FOR"].ToString().Split(',')[0].Trim();
                else
                    ip = request.UserHostAddress;
            }
            catch (Exception e)
            {
                throw e;
            }
            return ip;
        }
        //获取代理IPview plaincopy to clipboardprint
        public static string GetViaIP()
        {
            string viaIp = null;
            try
            {
                HttpRequest request = HttpContext.Current.Request;
                if (request.ServerVariables["HTTP_VIA"] != null)
                    viaIp = request.UserHostAddress;
            }
            catch (Exception e)
            {
                throw e;
            }
            return viaIp;
        }
    }
}
