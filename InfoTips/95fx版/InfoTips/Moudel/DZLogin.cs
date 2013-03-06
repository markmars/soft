using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.Text.RegularExpressions;
using System.Web;
using System.IO;
using System.IO.Compression;
using System.Threading;

namespace InfoTips
{
    public class DZLogin
    {
        public DZLogin()
        {
            DoWork(null, null);
        }

        void DoWork(object sender, DoWorkEventArgs e)
        {
            CookieContainer cc = new CookieContainer();
            StringBuilder sb = new StringBuilder();
            Encoding code = Encoding.ASCII;
            string p = "fastloginfield=username&username=a789456123&password=123456789&quickforward=yes&handlekey=ls";
            byte[] postData = code.GetBytes(p);
            string s = Encoding.Default.GetString(getBytes("http://www.zjghsy.cn/member.php?mod=logging&action=login&loginsubmit=yes&infloat=yes&lssubmit=yes&inajax=1", cc, postData));
        }

        public byte[] getBytes(string url, CookieContainer cookie, byte[] postData)
        {
            byte[] data = null;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Timeout = 30000;
            request.AllowAutoRedirect = true;
            if (cookie != null)
                request.CookieContainer = cookie;
            request.UserAgent = "Mozilla/4.0 (compatible; MSIE 8.0; Windows NT 6.1; Trident/4.0; SLCC2; .NET CLR 2.0.50727; .NET CLR 3.5.30729; .NET CLR 3.0.30729; Media Center PC 6.0)";
            request.Headers[HttpRequestHeader.AcceptEncoding] = "gzip, deflate";

            if (postData != null)                                           // 需要 Post 数据
            {
                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = postData.Length;
                try
                {
                    Stream requestStream = request.GetRequestStream();
                    requestStream.Write(postData, 0, postData.Length);
                    requestStream.Close();
                }
                catch
                {
                    return new byte[0];
                }
            }
            else
            {
                request.Method = "GET";
            }
            HttpWebResponse response = null;
            try
            {
                response = (HttpWebResponse)request.GetResponse();
            }
            catch (WebException ex)
            {
                response = (HttpWebResponse)ex.Response;
            }

            if (response == null)
                return new byte[0];

            string ce = response.Headers[HttpResponseHeader.ContentEncoding];
            int ContentLength = (int)response.ContentLength;
            Stream s = response.GetResponseStream();
            int c = 1024 * 10;
            if (ContentLength < 0)                                          // 不能获取数据的长度
            {
                data = new byte[c];
                MemoryStream ms = new MemoryStream();
                int l = s.Read(data, 0, c);
                while (l > 0)
                {
                    ms.Write(data, 0, l);
                    l = s.Read(data, 0, c);
                }
                data = ms.ToArray();
                ms.Close();
            }
            else                                                            // 数据长度已知
            {
                data = new byte[ContentLength];
                int pos = 0;
                while (ContentLength > 0)
                {
                    int l = s.Read(data, pos, ContentLength);
                    pos += l;
                    ContentLength -= l;
                }
            }
            s.Close();
            response.Close();

            if (ce == "gzip")                                               // 若数据是压缩格式，则要进行解压
            {
                unRar(ref data);
            }
            return data;                                                    // 返回字节数组
        }

        private byte[] unRar(ref  byte[] data)     // 解压数据
        {
            try
            {
                MemoryStream js = new MemoryStream();                       // 解压后的流   
                MemoryStream ms = new MemoryStream(data);                   // 用于解压的流   
                GZipStream g = new GZipStream(ms, CompressionMode.Decompress);
                byte[] buffer = new byte[10240];                                // 读数据缓冲区      
                int l = g.Read(buffer, 0, 10240);                               // 一次读 10K      
                while (l > 0)
                {
                    js.Write(buffer, 0, l);
                    l = g.Read(buffer, 0, 10240);
                }
                g.Close();
                ms.Close();
                data = js.ToArray();
                js.Close();
                return data;
            }
            catch
            {
                return data;
            }
        }
    }
}