using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.Text.RegularExpressions;
using System.Web;
using System.IO;
using System.IO.Compression;
using System.Threading;
 
namespace login3Dezu
{
    public partial class Form1 : Form
    {
        CookieContainer cc = new CookieContainer();
        string formhash = null;
        BackgroundWorker bw = new BackgroundWorker();
        public Form1()
        {
            InitializeComponent();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            bw.WorkerReportsProgress = true;
            bw.WorkerSupportsCancellation = true;
            bw.DoWork += new DoWorkEventHandler(bw_DoWork);
            bw.ProgressChanged += new ProgressChangedEventHandler(bw_ProgressChanged);
            bw.RunWorkerAsync();
        }
 
        void bw_DoWork(object sender, DoWorkEventArgs e)
        {
            while (!bw.CancellationPending)
            {
                StringBuilder sb = new StringBuilder();
                Encoding code = Encoding.ASCII;
 
                bw.ReportProgress(1,"当前日期："+DateTime.Now.ToString()+"\r\n\r\n");
                // 登录页面
                bw.ReportProgress(1, "登录页面\r\n");
 
                string p = "fastloginfield=username&username=XXXXX&password=XXXXX&quickforward=yes&handlekey=ls";
                byte[] postData = code.GetBytes(p);
                string s = Encoding.UTF8.GetString(getBytes("http://www.3dezu.com/member.php?mod=logging&action=login&loginsubmit=yes&infloat=yes&lssubmit=yes&inajax=1", cc, postData));
 
 
                // 查看积分
                bw.ReportProgress(1, "查看积分\r\n");
                s = Encoding.UTF8.GetString(getBytes("http://www.3dezu.com/home.php?mod=spacecp&ac=credit&op=base", cc, null));
                MatchCollection mc = Regex.Matches(s, @"<li[^<]*><em>(?<i>[^<]+)</em>(?<m>\d+)");
                foreach (Match m in mc)
                {
                    sb.AppendLine(string.Format("{0,20} {1}", m.Groups["i"].Value, m.Groups["m"].Value));
                }
                bw.ReportProgress(1,sb.ToString()+"\r\n\r\n");
 
                // 获取 formhash
 
                formhash = Regex.Match(s, @"(?<=formhash=)\w+").Value;
 
                // 获取留言信息，自动从163取新闻标题
                bw.ReportProgress(1, "自动获取留言\r\n");
                s = Encoding.Default.GetString(getBytes("http://news.163.com/special/0001220O/news_json.js", null, null));
                string ly = Regex.Match(s, @"[\u4e00-\u9fa5]{20,}").Value;
                if (string.IsNullOrEmpty(ly)) ly = "快乐又一天~~~~，继续Happy~~~~~~";
                bw.ReportProgress(1, "　　"+ly+"\r\n");
 
 
                // 提交签到数据
 
                p = string.Format("formhash={0}&qdxq=kx&qdmode=1&todaysay={1}&fastreply=1", formhash, HttpUtility.UrlEncode(ly));
                postData = code.GetBytes(p);
                s = Encoding.UTF8.GetString(getBytes("http://www.3dezu.com/plugin.php?id=dsu_paulsign:sign&operation=qiandao&infloat=1&sign_as=1&inajax=1", cc, postData));
                //bw.ReportProgress(1, s);
 
                // 查看积分
                bw.ReportProgress(1, "再次查看积分\r\n");
                sb.Length = 0;
                s = Encoding.UTF8.GetString(getBytes("http://www.3dezu.com/home.php?mod=spacecp&ac=credit&op=base", cc, null));
                mc = Regex.Matches(s, @"<li[^<]*><em>(?<i>[^<]+)</em>(?<m>\d+)");
                foreach (Match m in mc)
                {
                    sb.AppendLine(string.Format("{0,20} {1}", m.Groups["i"].Value, m.Groups["m"].Value));
                }
                bw.ReportProgress(1, sb.ToString() + "\r\n\r\n--------------------------------------------------------------------------------------------------------\r\n");
 
 
 
 
                DateTime c=DateTime.Now;
                DateTime dt = c.Date.AddHours(27.5);
 
                Thread.Sleep(dt.Subtract(c));
            }
        }
 
        void bw_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            richTextBox1.AppendText(e.UserState.ToString());
        }
 
 
        // 读取网络资源，返回字节数组
        public byte[] getBytes(string url, CookieContainer cookie, byte[] postData)
        {
            byte[] data = null;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Timeout = 30000;
            request.AllowAutoRedirect = true;
            if (cookie != null) request.CookieContainer = cookie;
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
 
            if (response == null) return new byte[0];
 
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
 
        private byte[] Rar(ref byte[] data)     // 压缩数据
        {
            MemoryStream ys = new MemoryStream();                       // 压缩后的流   
            GZipStream g = new GZipStream(ys, CompressionMode.Compress);
            g.Write(data, 0, data.Length);
            g.Close();
            data = ys.ToArray();
            ys.Close();
            return data;
        }
    }
}