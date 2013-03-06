using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Text.RegularExpressions;
using System.IO;
using System.Runtime.InteropServices;

namespace InfoTips
{
    public partial class UserControl评论 : MyUserControl
    {
        [DllImport("winmm.dll")]
        private static extern long sndPlaySound(string lpszSoundName, long uFlags);
        delegate void SetVisibleDelegate();
        List<Thread> m_listThread评论中心 = new List<Thread>();
        Dictionary<string, 新闻信息> dic_评论中心;
        List<新闻信息> m_list评论中心;

        public UserControl评论()
        {
            InitializeComponent();
            Init菜单();
        }
        private void UserControl评论_Load(object sender, EventArgs e)
        {
            dic_评论中心 = new Dictionary<string, 新闻信息>();
            m_list评论中心 = new List<新闻信息>();

            Start_评论中心();
        }
        private void barCheckItem评论中心_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Utility ut = new Utility();
            ut.筛选数据(barManager1, treeList评论中心);
        }

        //public void Start_评论中心()
        //{
        //    foreach (Thread t in m_listThread评论中心)
        //    {
        //        if (t.IsAlive)
        //            t.Abort();
        //    }
        //    XMLHandle app = new XMLHandle();
        //    m_listThread评论中心.Clear();
        //    foreach (DevExpress.XtraBars.BarItem c in barManager1.Items)
        //    {
        //        if (c.GetType().FullName != "DevExpress.XtraBars.BarCheckItem")
        //            continue;
        //        if (((DevExpress.XtraBars.BarCheckItem)c).Description == "1")
        //        {
        //            if (app.GetAppValue(((DevExpress.XtraBars.BarCheckItem)c).Caption + "采集") == "1")
        //            {
        //                string url = ((DevExpress.XtraBars.BarCheckItem)c).Tag.ToString();
        //                string str = ((DevExpress.XtraBars.BarCheckItem)c).Caption;
        //                Thread thread = new Thread(GetData_网站信息);
        //                thread.IsBackground = true;
        //                thread.Start(str + "|" + url + "|" + "评论中心|" + app.GetAppValue(str + "时间"));
        //                m_listThread评论中心.Add(thread);
        //            }
        //        }
        //    }
        //}
        public void Start_评论中心()
        {
            foreach (Thread t in m_listThread评论中心)
            {
                if (t.IsAlive)
                    t.Abort();
            }
            XMLHandle app = new XMLHandle();
            m_listThread评论中心.Clear();
            Thread thread1 = new Thread(GetData_网站信息);
            thread1.IsBackground = true;
            thread1.Start("http://cms.95fx.com/index.php?m=content&c=index&a=lists&catid=45");
            Thread thread2 = new Thread(GetData_网站信息);
            thread2.IsBackground = true;
            thread2.Start("http://cms.95fx.com/index.php?m=content&c=index&a=lists&catid=46");
            m_listThread评论中心.Add(thread1);
            m_listThread评论中心.Add(thread2);
        }
        private void GetData_网站信息(object o)
        {
            Utility ut = new Utility();
            while (true)
            {
                string url = o.ToString();
                List<新闻信息> ls = new List<新闻信息>();
                string str = ut.getUrlSource(url, "utf-8");
                Regex r = new Regex("<LI><SPAN>" + @"[\s\S]*?" + "</LI>", RegexOptions.IgnoreCase);
                MatchCollection ms = r.Matches(str);
                foreach (Match m in ms)
                {
                    新闻信息 xw = new 新闻信息();
                    r = new Regex("\">[" + @"[\s\S]*?" + "]</A>", RegexOptions.IgnoreCase);
                    xw.type = r.Match(m.ToString()).ToString().Replace("\">[", "").Replace("]</A>", "").Replace(" ", "");
                    r = new Regex("target=_blank>" + @"[\s\S]*?" + "</A>", RegexOptions.IgnoreCase);
                    xw.title = r.Match(m.ToString()).ToString().Replace("target=_blank>", "").Replace("</A>", "");
                    r = new Regex("</SPAN>" + @"[\s\S]*?" + "\" target=_blank>", RegexOptions.IgnoreCase);
                    xw.url = r.Match(m.ToString()).ToString().Replace("\" target=_blank>", "").Substring(30);
                    xw.date = r.Match(m.ToString()).ToString().Substring(0, 19).Replace("</SPAN> ", "");

                    if (!dic_评论中心.ContainsKey(xw.url))
                    {
                        dic_评论中心.Add(xw.url, xw);
                        lock (m_list评论中心)
                            m_list评论中心.Add(xw);
                    }
                }
                lock (this)
                    this.Invoke(new SetVisibleDelegate(SetVisible));
                Thread.Sleep(15000);
            }
        }
        //private void GetData_网站信息(object o)
        //{
        //    Utility ut = new Utility();
        //    while (true)
        //    {
        //        string type = o.ToString().Split(new char[] { '|' })[0];
        //        string url = o.ToString().Split(new char[] { '|' })[1];
        //        List<新闻信息> ls = new List<新闻信息>();
        //        string str = ut.getUrlSource(url, "utf-8");
        //        Regex r = new Regex("<ul class=\"list lh24 f14\">" + @"[\s\S]*?" + "</ul>", RegexOptions.IgnoreCase);
        //        str = r.Match(str).ToString();
        //        r = new Regex("<li>" + @"[\s\S]*?" + "</li>", RegexOptions.IgnoreCase);
        //        MatchCollection ms = r.Matches(str);
        //        foreach (Match m in ms)
        //        {
        //            if (string.IsNullOrEmpty(m.ToString().Replace("<li class=\"bk20 hr\">", "").Replace("</li>", "").Replace(" ", "")))
        //                continue;
        //            新闻信息 xw = new 新闻信息();
        //            r = new Regex("style=\"\" >" + @"[\s\S]*?" + "</a>", RegexOptions.IgnoreCase);
        //            xw.title = r.Match(m.ToString()).ToString().Replace("style=\"\" >", "").Replace("</a>", "").Replace("\n\t", "").Replace("&nbsp;", "");
        //            r = new Regex("href=\"" + @"[\s\S]*?" + "\"", RegexOptions.IgnoreCase);
        //            xw.url = r.Match(m.ToString()).ToString().Replace("href=\"", "").Replace("\"", "");
        //            r = new Regex("<span class=\"rt\">" + @"[\s\S]*?" + "</span>", RegexOptions.IgnoreCase);
        //            xw.date = r.Match(m.ToString()).ToString().Replace("<span class=\"rt\">", "").Replace("</span>", "").Trim();
        //            if (xw.date.StartsWith("11") || xw.date.StartsWith("12"))
        //                xw.date = "2012-" + xw.date;
        //            else
        //                xw.date = "2013-" + xw.date;
        //            xw.type = type;

        //            if (!dic_评论中心.ContainsKey(xw.url))
        //            {
        //                dic_评论中心.Add(xw.url, xw);
        //                lock (m_list评论中心)
        //                    m_list评论中心.Add(xw);
        //            }
        //        }
        //        lock (this)
        //            this.Invoke(new SetVisibleDelegate(SetVisible));
        //        Thread.Sleep(Convert.ToInt32(o.ToString().Split(new char[] { '|' })[3]) * 1000);
        //    }
        //}
        private void SetVisible()
        {
            if (m_list评论中心.Count > 0)
            {
                lock (m_list评论中心)
                    foreach (新闻信息 xw in m_list评论中心)
                        this.treeList评论中心.AppendNode(new object[] { xw.date, xw.title, xw.type }, -1, xw.url);
                if (m_list评论中心.Count < 10)
                    提醒弹窗();
                m_list评论中心.Clear();
                Utility ut = new Utility();
                ut.筛选数据(barManager1, treeList评论中心);
            }
        }
        private void 提醒弹窗()
        {
            if (treeList评论中心.Nodes.Count == 0)
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
                FormInfo ti = new FormInfo(m_list评论中心);
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
        private void treeList评论中心_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (treeList评论中心.FocusedNode == null)
                return;
            FormBroswer info = new FormBroswer(treeList评论中心.FocusedNode.Tag.ToString(), treeList评论中心.FocusedNode.GetValue(1).ToString());
            info.Show();
        }
        private void treeList评论中心_CustomDrawNodeCell(object sender, DevExpress.XtraTreeList.CustomDrawNodeCellEventArgs e)
        {
            if (e.Column == treeList评论中心.Columns[2])
            {
                switch (e.CellValue.ToString())
                {
                    case "个人汇评":
                        e.Appearance.ForeColor = Color.Red;
                        break;
                    case "机构汇评":
                        e.Appearance.ForeColor = Color.Green;
                        break;
                    case "银行汇评":
                        e.Appearance.ForeColor = Color.Blue;
                        break;
                    case "其他汇评":
                        e.Appearance.ForeColor = Color.LightCoral;
                        break;
                    case "KITCO":
                        e.Appearance.ForeColor = Color.HotPink;
                        break;
                    case "天交所":
                        e.Appearance.ForeColor = Color.IndianRed;
                        break;
                    case "中外银行":
                        e.Appearance.ForeColor = Color.Indigo;
                        break;
                    case "投资机构":
                        e.Appearance.ForeColor = Color.Brown;
                        break;
                    case "上海黄金交易所":
                        e.Appearance.ForeColor = Color.DarkSlateBlue;
                        break;
                    case "原油评论":
                        e.Appearance.ForeColor = Color.DarkRed;
                        break;
                }
            }
        }
    }
}