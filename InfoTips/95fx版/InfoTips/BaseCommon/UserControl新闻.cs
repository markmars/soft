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
    public partial class UserControl新闻 : MyUserControl
    {
        [DllImport("winmm.dll")]
        private static extern long sndPlaySound(string lpszSoundName, long uFlags);
        List<Thread> m_listThread新闻汇总 = new List<Thread>();
        Dictionary<string, 新闻信息> dic_新闻汇总;
        delegate void SetVisibleDelegate();
        List<新闻信息> m_list新闻汇总;

        public UserControl新闻()
        {
            InitializeComponent();
            Init菜单();
        }
        private void UserControl新闻_Load(object sender, EventArgs e)
        {
            dic_新闻汇总 = new Dictionary<string, 新闻信息>();
            m_list新闻汇总 = new List<新闻信息>();

            Start_新闻汇总();
        }
        private void barCheckItem新闻_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Utility ut = new Utility();
            ut.筛选数据(barManager1, treeList新闻汇总);
        }
        //public void Start_新闻汇总()
        //{
        //    foreach (Thread t in m_listThread新闻汇总)
        //    {
        //        if (t.IsAlive)
        //            t.Abort();
        //    }
        //    XMLHandle app = new XMLHandle();
        //    m_listThread新闻汇总.Clear();
        //    foreach (DevExpress.XtraBars.BarItem c in barManager1.Items)
        //    {
        //        if (((DevExpress.XtraBars.BarCheckItem)c).Description == "1")
        //        {
        //            if (app.GetAppValue(((DevExpress.XtraBars.BarCheckItem)c).Caption + "采集") == "1")
        //            {
        //                string url = ((DevExpress.XtraBars.BarCheckItem)c).Tag.ToString();
        //                string str = ((DevExpress.XtraBars.BarCheckItem)c).Caption;
        //                Thread thread = new Thread(GetData_网站信息);
        //                thread.IsBackground = true;
        //                thread.Start(str + "|" + url + "|" + "新闻汇总|" + app.GetAppValue(str + "时间"));
        //                m_listThread新闻汇总.Add(thread);
        //            }
        //        }
        //    }
        //}
        public void Start_新闻汇总()
        {
            foreach (Thread t in m_listThread新闻汇总)
            {
                if (t.IsAlive)
                    t.Abort();
            }
            XMLHandle app = new XMLHandle();
            m_listThread新闻汇总.Clear();
            Thread thread1 = new Thread(GetData_网站信息);
            thread1.IsBackground = true;
            thread1.Start("http://cms.95fx.com/index.php?m=content&c=index&a=lists&catid=47");
            Thread thread2 = new Thread(GetData_网站信息);
            thread2.IsBackground = true;
            thread2.Start("http://cms.95fx.com/index.php?m=content&c=index&a=lists&catid=48");
            Thread thread3 = new Thread(GetData_网站信息);
            thread3.IsBackground = true;
            thread3.Start("http://cms.95fx.com/index.php?m=content&c=index&a=lists&catid=49");
            m_listThread新闻汇总.Add(thread1);
            m_listThread新闻汇总.Add(thread2);
            m_listThread新闻汇总.Add(thread3);
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
                    xw.type = r.Match(m.ToString()).ToString().Replace("\">[", "").Replace("]</A>", "");
                    r = new Regex("target=_blank>" + @"[\s\S]*?" + "</A>", RegexOptions.IgnoreCase);
                    xw.title = r.Match(m.ToString()).ToString().Replace("target=_blank>", "").Replace("</A>", "");
                    r = new Regex("</SPAN>" + @"[\s\S]*?" + "\" target=_blank>", RegexOptions.IgnoreCase);
                    xw.url = r.Match(m.ToString()).ToString().Replace("\" target=_blank>", "").Substring(30);
                    xw.date = r.Match(m.ToString()).ToString().Substring(0, 19).Replace("</SPAN> ", "");

                    if (!dic_新闻汇总.ContainsKey(xw.url))
                    {
                        dic_新闻汇总.Add(xw.url, xw);
                        lock (m_list新闻汇总)
                            m_list新闻汇总.Add(xw);
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
        //            if (!dic_新闻汇总.ContainsKey(xw.url))
        //            {
        //                dic_新闻汇总.Add(xw.url, xw);
        //                lock (m_list新闻汇总)
        //                    m_list新闻汇总.Add(xw);
        //            }
        //        }
        //        lock (this)
        //            this.Invoke(new SetVisibleDelegate(SetVisible));
        //        Thread.Sleep(Convert.ToInt32(o.ToString().Split(new char[] { '|' })[3]) * 1000);
        //    }
        //}
        private void SetVisible()
        {
            if (m_list新闻汇总.Count > 0)
            {
                lock (m_list新闻汇总)
                    foreach (新闻信息 xw in m_list新闻汇总)
                        this.treeList新闻汇总.AppendNode(new object[] { xw.date, xw.title, xw.type }, -1, xw.url);
                if (m_list新闻汇总.Count < 10)
                    提醒弹窗();
                m_list新闻汇总.Clear();
                Utility ut = new Utility();
                ut.筛选数据(barManager1, treeList新闻汇总);
            }
        }
        private void 提醒弹窗()
        {
            if (treeList新闻汇总.Nodes.Count == 0)
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
                FormInfo ti = new FormInfo(m_list新闻汇总);
                ti.Tag = app.GetAppValue("弹窗时间");
                ti.Show();
            }
        }
        public void Init菜单()
        {
            XMLHandle app = new XMLHandle();

            foreach (DevExpress.XtraBars.BarItem c in barManager1.Items)
            {
                if (((DevExpress.XtraBars.BarCheckItem)c).Description == "1")
                {
                    if (app.GetAppValue(((DevExpress.XtraBars.BarCheckItem)c).Caption + "采集") == "1")
                    {
                        c.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
                    }
                    else
                    {
                        c.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
                        ((DevExpress.XtraBars.BarCheckItem)c).Checked = false;
                    }
                }
            }
        }
        private void treeList新闻汇总_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (treeList新闻汇总.FocusedNode == null)
                return;
            FormBroswer info = new FormBroswer(treeList新闻汇总.FocusedNode.Tag.ToString(), treeList新闻汇总.FocusedNode.GetValue(1).ToString());
            info.Show();
        }
        private void treeList新闻汇总_CustomDrawNodeCell(object sender, DevExpress.XtraTreeList.CustomDrawNodeCellEventArgs e)
        {
            if (e.Column == treeList新闻汇总.Columns[2])
            {
                switch (e.CellValue.ToString())
                {
                    case "头条":
                        e.Appearance.ForeColor = Color.Red;
                        break;
                    case "指标":
                        e.Appearance.ForeColor = Color.Green;
                        break;
                    case "讲话":
                        e.Appearance.ForeColor = Color.DarkGray;
                        break;
                    case "播报":
                        e.Appearance.ForeColor = Color.LightCoral;
                        break;
                    case "国际争端":
                        e.Appearance.ForeColor = Color.HotPink;
                        break;
                    case "国家政治":
                        e.Appearance.ForeColor = Color.IndianRed;
                        break;
                    case "经济争端":
                        e.Appearance.ForeColor = Color.Indigo;
                        break;
                    case "世界灾难":
                        e.Appearance.ForeColor = Color.Ivory;
                        break;
                    case "黄金":
                        e.Appearance.ForeColor = Color.Blue;
                        break;
                    case "原油":
                        e.Appearance.ForeColor = Color.DodgerBlue;
                        break;
                    case "-DAILYFX-":
                        e.Appearance.ForeColor = Color.LavenderBlush;
                        break;
                    case "-fxstreet-":
                        e.Appearance.ForeColor = Color.LawnGreen;
                        break;
                    case "-reuters-":
                        e.Appearance.ForeColor = Color.Brown;
                        break;
                }
            }
        }
    }
}
