using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;

namespace InfoTips
{
    public partial class UserControl火线 : MyUserControl
    {
        [DllImport("winmm.dll")]
        private static extern long sndPlaySound(string lpszSoundName, long uFlags);
        delegate void SetVisibleDelegate();
        List<Thread> m_listThread火线速递 = new List<Thread>();
        Dictionary<string, 新闻信息> dic_火线速递;
        List<新闻信息> m_list火线速递;

        public UserControl火线()
        {
            InitializeComponent();
            Init菜单();
        }

        private void UserControl火线_Load(object sender, EventArgs e)
        {
            dic_火线速递 = new Dictionary<string, 新闻信息>();
            m_list火线速递 = new List<新闻信息>();

            Start_火线速递();
        }
        private void barCheckItem火线速递_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Utility ut = new Utility();
            ut.筛选数据(barManager1, treeList火线速递);
        }

        public void Start_火线速递()
        {
            foreach (Thread t in m_listThread火线速递)
            {
                if (t.IsAlive)
                    t.Abort();
            }
            XMLHandle app = new XMLHandle();
            m_listThread火线速递.Clear();
            foreach (DevExpress.XtraBars.BarItem c in barManager1.Items)
            {
                if (c.GetType().FullName != "DevExpress.XtraBars.BarCheckItem")
                    continue;
                if (((DevExpress.XtraBars.BarCheckItem)c).Description == "1")
                {
                    if (app.GetAppValue(((DevExpress.XtraBars.BarCheckItem)c).Caption + "采集") == "1")
                    {
                        string url = ((DevExpress.XtraBars.BarCheckItem)c).Tag.ToString();
                        string str = ((DevExpress.XtraBars.BarCheckItem)c).Caption;
                        Thread thread = new Thread(GetData_网站信息);
                        thread.IsBackground = true;
                        thread.Start(str + "|" + url + "|" + "火线速递|" + app.GetAppValue(str + "时间"));
                        m_listThread火线速递.Add(thread);
                    }
                }
            }
        }
        private void GetData_网站信息(object o)
        {
            Utility ut = new Utility();
            while (true)
            {
                string type = o.ToString().Split(new char[] { '|' })[0];
                string url = o.ToString().Split(new char[] { '|' })[1];
                List<新闻信息> ls = new List<新闻信息>();
                string str = "";
                MatchCollection ms = null;
                string[] strs = null;
                switch (url)
                {
                    case "http://www.dailyfx.com.hk/livenews/index.html":
                        str = ut.getUrlSource(url, "utf-8");
                        Regex r = new Regex("<tr class=\"record\" valign=\"top\">" + @"[\s\S]*?" + "</tr>", RegexOptions.IgnoreCase);
                        ms = r.Matches(str);
                        foreach (Match m in ms)
                        {
                            if (m.Value.Contains("查看MT4资讯中心"))
                                continue;
                            新闻信息 xw = new 新闻信息();
                            strs = m.Value.Split(new string[] { "</td>" }, StringSplitOptions.None);
                            xw.title = strs[1].Contains("openDiv") ? strs[1].Substring(strs[1].IndexOf("openDiv\">") + 9, strs[1].IndexOf("</a>") - strs[1].IndexOf("openDiv\">") - 9) : strs[1].Substring(strs[1].IndexOf("stitle\">") + 8);
                            xw.url = "";
                            xw.date = strs[0].Substring(strs[0].IndexOf("width=\"100\">") + 12);
                            if (int.Parse(xw.date.Substring(0, xw.date.IndexOf("月"))) < 10)
                                xw.date = "0" + xw.date;
                            xw.date = xw.date.Replace("月", "/").Replace("日", " ");
                            xw.type = type;

                            if (!dic_火线速递.ContainsKey(xw.title))
                            {
                                dic_火线速递.Add(xw.title, xw);
                                lock (m_list火线速递)
                                    m_list火线速递.Add(xw);
                            }
                        }
                        break;
                    case "http://www.fx678.com/news/flash/default.shtml":
                        str = ut.getUrlSource(url, "gb2312");
                        r = new Regex("<div class=\"list_content01_title\">" + @"[\s\S]*?" + "</div>" + @"[\s\S]*?" + "</div>" + @"[\s\S]*?" + "</div>", RegexOptions.IgnoreCase);
                        ms = r.Matches(str);
                        foreach (Match m in ms)
                        {
                            if (m.Value.Contains("实时喊单分享"))
                                continue;
                            新闻信息 xw = new 新闻信息();
                            strs = m.Value.Split(new string[] { "</div>" }, StringSplitOptions.None);
                            xw.title = strs[0].Substring(strs[0].IndexOf("_blank\">·") + 9, strs[0].IndexOf("</a>") - strs[0].IndexOf("_blank\">·") - 9).Replace("&nbsp;", " ");
                            xw.url = "";
                            xw.date = strs[1].Substring(strs[1].IndexOf("_titler\">") + 9);
                            xw.type = type;

                            if (!dic_火线速递.ContainsKey(xw.title))
                            {
                                dic_火线速递.Add(xw.title, xw);
                                lock (m_list火线速递)
                                    m_list火线速递.Add(xw);
                            }
                        }
                        break;
                    case "http://t.news.fx168.com/indexs.shtml":
                        str = ut.getUrlSource(url, "utf-8");
                        r = new Regex("<div id=\"divNewslist\" class=\"ascout_news_list2\">" + @"[\s\S]*?" + "</div>", RegexOptions.IgnoreCase);
                        str = r.Match(str).Value;
                        r = new Regex("<li>" + @"[\s\S]*?" + "</li>", RegexOptions.IgnoreCase);
                        ms = r.Matches(str);
                        foreach (Match m in ms)
                        {
                            新闻信息 xw = new 新闻信息();
                            str = m.Value;
                            xw.title = str.Substring(str.IndexOf("title=\"") + 7, str.IndexOf("\" id=\"") - str.IndexOf("title=\"") - 7);
                            xw.url = "";
                            xw.date = str.Substring(str.IndexOf("<span>") + 6, str.IndexOf("</span>") - str.IndexOf("<span>") - 6);
                            xw.type = type;

                            if (!dic_火线速递.ContainsKey(xw.title))
                            {
                                dic_火线速递.Add(xw.title, xw);
                                lock (m_list火线速递)
                                    m_list火线速递.Add(xw);
                            }
                        }
                        break;
                }
                lock (this)
                    this.Invoke(new SetVisibleDelegate(SetVisible));
                Thread.Sleep(Convert.ToInt32(o.ToString().Split(new char[] { '|' })[3]) * 1000);
            }
        }
        private void SetVisible()
        {
            if (m_list火线速递.Count > 0)
            {
                lock (m_list火线速递)
                    foreach (新闻信息 xw in m_list火线速递)
                        this.treeList火线速递.AppendNode(new object[] { xw.date, xw.title, xw.type }, -1, xw.url);
                if (m_list火线速递.Count < 10)
                    提醒弹窗();
                m_list火线速递.Clear();
                Utility ut = new Utility();
                ut.筛选数据(barManager1, treeList火线速递);
            }
        }
        private void 提醒弹窗()
        {
            if (treeList火线速递.Nodes.Count == 0)
                return;
            XMLHandle app = new XMLHandle();
            if (app.GetAppValue("语音提醒") == "1")
            {
                string file = Application.StartupPath + "\\Sound\\" + app.GetAppValue("语音文件");
                if (File.Exists(file))
                    sndPlaySound(file, 1);
                else
                    MessageBox.Show("铃声文件缺失");
            }
            if (app.GetAppValue("自动弹窗") == "1")
            {
                FormInfo ti = new FormInfo(m_list火线速递);
                ti.Tag = app.GetAppValue("弹窗时间");
                ti.Show();
            }
        }
        public void Init菜单()
        {
            bool bb = false;
            XMLHandle app = new XMLHandle();
            foreach (DevExpress.XtraBars.BarItem c in barManager1.Items)
            {
                if (c.GetType().FullName != "DevExpress.XtraBars.BarCheckItem")
                    continue;
                if (((DevExpress.XtraBars.BarCheckItem)c).Description == "1")
                {
                    if (app.GetAppValue(((DevExpress.XtraBars.BarCheckItem)c).Caption + "采集") == "1")
                    {
                        c.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
                        ((DevExpress.XtraBars.BarCheckItem)c).Checked = true;

                    }
                    else
                    {
                        c.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
                        ((DevExpress.XtraBars.BarCheckItem)c).Checked = false;
                    }
                }
            }
        }
        private void treeList火线速递_CustomDrawNodeCell(object sender, DevExpress.XtraTreeList.CustomDrawNodeCellEventArgs e)
        {
            if (e.Column == treeList火线速递.Columns[2])
            {
                switch (e.CellValue.ToString())
                {
                    case "dailyfx":
                        e.Appearance.ForeColor = Color.Red;
                        break;
                    case "FX678":
                        e.Appearance.ForeColor = Color.Green;
                        break;
                    case "FX168":
                        e.Appearance.ForeColor = Color.Blue;
                        break;
                }
            }
        }
    }
}
