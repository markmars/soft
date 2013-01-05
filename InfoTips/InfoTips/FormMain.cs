using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Threading;
using System.Collections;
using System.IO;
using System.Net;
using System.Diagnostics;
using System.Runtime.InteropServices;
using DevExpress.XtraTreeList.Nodes;
using DevExpress.XtraBars;

namespace InfoTips
{
    public partial class FormMain : Form
    {
        [DllImport("winmm.dll")]
        private static extern long sndPlaySound(string lpszSoundName, long uFlags);
        delegate void SetVisibleDelegate();
        List<Thread> m_listThread新闻汇总 = new List<Thread>(), m_listThread评论中心 = new List<Thread>();
        Dictionary<string, 新闻信息> dic_新闻汇总, dic_评论中心;
        List<新闻信息> m_list新闻汇总, m_list评论中心;
        List<财经信息> m_list财经日历;
        Thread m_thread持仓报告 = null;

        public FormMain()
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;
            webBrowser1.IsWebBrowserContextMenuEnabled = false;
            webBrowser3.IsWebBrowserContextMenuEnabled = false;

            InitSkin();
            Init新闻按钮();
        }
        private void FormMain_Load(object sender, EventArgs e)
        {
            dic_新闻汇总 = new Dictionary<string, 新闻信息>();
            dic_评论中心 = new Dictionary<string, 新闻信息>();
            m_list财经日历 = new List<财经信息>();
            m_list新闻汇总 = new List<新闻信息>();
            m_list评论中心 = new List<新闻信息>();

            Start_新闻汇总();
            Start_评论中心();
            Start_财经日历();
        }
        private void FormMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)//当用户点击窗体右上角X按钮或(Alt + F4)时 发生
            {
                e.Cancel = true;
                this.Visible = false;
                notifyIcon.ShowBalloonTip(5000, "外汇信息即时提醒", "外汇信息即时提醒", ToolTipIcon.Info);
                return;
            }
            notifyIcon.Visible = false;
            Environment.Exit(0);
        }
        private void FormMain_SizeChanged(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                this.Visible = false;
                this.WindowState = FormWindowState.Minimized;
            }
            else if (this.WindowState == FormWindowState.Normal)
            {
                this.Visible = true;
                this.WindowState = FormWindowState.Normal;
            }
        }
        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Application.ExitThread();
            notifyIcon.Visible = false;
            Environment.Exit(0);
        }
        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            FormSetting setting = new FormSetting();
            if (setting.ShowDialog() == DialogResult.OK)
            {
                Init新闻按钮();
                Start_新闻汇总();
            }
        }
        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {
            this.Visible = true;
            this.WindowState = FormWindowState.Normal;
        }
        private void notifyIcon_DoubleClick(object sender, EventArgs e)
        {
            if (this.Visible)
            {
                this.Visible = false;
                this.WindowState = FormWindowState.Minimized;
            }
            else
            {
                this.Visible = true;
                this.WindowState = FormWindowState.Normal;
            }
        }
        private void barButtonItem_Setting_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            toolStripMenuItem2_Click(null, null);
        }
        private void barButtonItem_about_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            FormAbout about = new FormAbout();
            about.ShowDialog();
        }
        private void checkButton_Click(object sender, EventArgs e)
        {
            DevExpress.XtraEditors.CheckButton ckBtn = sender as DevExpress.XtraEditors.CheckButton;
            int count = 0;
            foreach (Control c in panelControl1.Controls)
            {
                if (c.GetType().FullName == "DevExpress.XtraEditors.CheckButton")
                    if (((DevExpress.XtraEditors.CheckButton)c).Checked)
                        count++;
            }

            if (!ckBtn.Checked)
            {
                if (count >= 5)
                {
                    MessageBox.Show("最多选择5项");
                    ckBtn.Checked = !ckBtn.Checked;
                    return;
                }
            }
        }
        private void checkButton新闻汇总_CheckedChanged(object sender, EventArgs e)
        {
            筛选数据(panelControl1, treeList新闻汇总);
        }
        private void checkButton评论中心_CheckedChanged(object sender, EventArgs e)
        {
            筛选数据(panelControl2, treeList评论中心);
        }
        private void simpleButton持仓报告_Click(object sender, EventArgs e)
        {
            if (m_thread持仓报告 == null)
            {
                m_thread持仓报告 = new Thread(GetData_持仓报告);
                m_thread持仓报告.IsBackground = true;
                m_thread持仓报告.Start(((DevExpress.XtraEditors.SimpleButton)sender).Text);
            }
            else
            {
                if (!m_thread持仓报告.IsAlive)
                {
                    m_thread持仓报告 = new Thread(GetData_持仓报告);
                    m_thread持仓报告.IsBackground = true;
                    m_thread持仓报告.Start(((DevExpress.XtraEditors.SimpleButton)sender).Text);
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
        private void treeList评论中心_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (treeList评论中心.FocusedNode == null)
                return;
            FormBroswer info = new FormBroswer(treeList评论中心.FocusedNode.Tag.ToString(), treeList评论中心.FocusedNode.GetValue(1).ToString());
            info.Show();
        }
        private void xtraTabControl1_SelectedPageChanged(object sender, DevExpress.XtraTab.TabPageChangedEventArgs e)
        {
            if (e.Page.Text == "持仓报告")
            {
                if (string.IsNullOrEmpty(webBrowser3.DocumentText))
                {
                    Thread thread = new Thread(GetData_持仓报告);
                    thread.IsBackground = true;
                    thread.Start("黄金ETF持仓量");
                }
            }
        }
        private void timer_time_Tick(object sender, EventArgs e)
        {
            GetTime();
        }
        private void simpleButton日期_Click(object sender, EventArgs e)
        {
            List<string> ls = GetWeekDate();
            foreach (TreeListNode tln in treeList财经日历.Nodes)
            {
                if (tln.Tag.ToString() == ls[int.Parse(((DevExpress.XtraEditors.SimpleButton)sender).Tag.ToString())])
                    tln.Visible = true;
                else
                    tln.Visible = false;
            }

            foreach (Control c in panelControl3.Controls)
            {
                if (c == (Control)sender)
                    ((DevExpress.XtraEditors.SimpleButton)c).ForeColor = Color.Red;
                else
                    ((DevExpress.XtraEditors.SimpleButton)c).ForeColor = Color.Black;
            }
        }
        private void simpleButton日期_MouseHover(object sender, EventArgs e)
        {
            List<string> ls = GetWeekDate();
            string str = ls[int.Parse(((DevExpress.XtraEditors.SimpleButton)sender).Tag.ToString())];
            ToolTip tt = new ToolTip();
            tt.AutoPopDelay = 5000;
            tt.InitialDelay = 500;
            tt.ReshowDelay = 500;
            tt.ShowAlways = true;
            tt.SetToolTip((DevExpress.XtraEditors.SimpleButton)sender, str.Substring(0, 4) + "年" + str.Substring(4, 2) + "月" + str.Substring(6, 2) + "日");
        }
        private void treeList财经日历_CustomDrawNodeCell(object sender, DevExpress.XtraTreeList.CustomDrawNodeCellEventArgs e)
        {
            if (e.Column == treeList财经日历.Columns[3])
            {
                if (e.CellValue.ToString() == "高")
                    e.Appearance.ForeColor = Color.Red;
                else if (e.CellValue.ToString() == "中")
                    e.Appearance.ForeColor = Color.Green;
                else if (e.CellValue.ToString() == "低")
                    e.Appearance.ForeColor = Color.Orange;
            }
            if (e.Column == treeList财经日历.Columns[6])
            {
                if (e.CellValue.ToString() != "待公布")
                    e.Appearance.ForeColor = Color.Red;
                else
                    e.Appearance.ForeColor = Color.Green;
            }
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
                        e.Appearance.ForeColor = Color.GreenYellow;
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
                        e.Appearance.ForeColor = Color.PaleTurquoise;
                        break;
                    case "原油":
                        e.Appearance.ForeColor = Color.Lavender;
                        break;
                    case "-DAILYFX-":
                        e.Appearance.ForeColor = Color.LavenderBlush;
                        break;
                    case "-fxstreet-":
                        e.Appearance.ForeColor = Color.LawnGreen;
                        break;
                    case "-reuters-":
                        e.Appearance.ForeColor = Color.MediumSpringGreen;
                        break;
                }
            }
        }
        private void treeList评论中心_CustomDrawNodeCell(object sender, DevExpress.XtraTreeList.CustomDrawNodeCellEventArgs e)
        {
            if (e.Column == treeList评论中心.Columns[2])
            {
                switch (e.CellValue.ToString())
                {
                    case "个人评论":
                        e.Appearance.ForeColor = Color.Red;
                        break;
                    case "机构评论":
                        e.Appearance.ForeColor = Color.Green;
                        break;
                    case "银行评论":
                        e.Appearance.ForeColor = Color.GreenYellow;
                        break;
                    case "其他评论":
                        e.Appearance.ForeColor = Color.LightCoral;
                        break;
                    case "KITCO":
                        e.Appearance.ForeColor = Color.HotPink;
                        break;
                    case "天通金":
                        e.Appearance.ForeColor = Color.IndianRed;
                        break;
                    case "中外银行":
                        e.Appearance.ForeColor = Color.Indigo;
                        break;
                    case "投资机构":
                        e.Appearance.ForeColor = Color.Ivory;
                        break;
                    case "上海黄金交易所":
                        e.Appearance.ForeColor = Color.Yellow;
                        break;
                    case "原油新闻":
                        e.Appearance.ForeColor = Color.Lavender;
                        break;
                    case "原油评论":
                        e.Appearance.ForeColor = Color.LavenderBlush;
                        break;
                }
            }
        }

        public void Start_新闻汇总()
        {
            foreach (Thread t in m_listThread新闻汇总)
            {
                if (t.IsAlive)
                    t.Abort();
            }
            XMLHandle app = new XMLHandle();
            m_listThread新闻汇总.Clear();
            foreach (Control c in panelControl1.Controls)
            {
                if (c.GetType().FullName == "DevExpress.XtraEditors.CheckButton")
                {
                    if (c.Visible)
                    {
                        string url = ((DevExpress.XtraEditors.CheckButton)c).Tag.ToString();
                        string str = ((DevExpress.XtraEditors.CheckButton)c).Text;
                        Thread thread = new Thread(GetData_网站信息);
                        thread.IsBackground = true;
                        thread.Start(str + "|" + url + "|" + "新闻汇总|" + app.GetAppValue(str + "时间"));
                        m_listThread新闻汇总.Add(thread);
                    }
                }
            }
        }
        public void Start_评论中心()
        {
            foreach (Thread t in m_listThread评论中心)
            {
                if (t.IsAlive)
                    t.Abort();
            }
            XMLHandle app = new XMLHandle();
            m_listThread评论中心.Clear();
            foreach (Control c in panelControl2.Controls)
            {
                if (c.GetType().FullName == "DevExpress.XtraEditors.CheckButton")
                {
                    string url = ((DevExpress.XtraEditors.CheckButton)c).Tag.ToString();
                    string str = ((DevExpress.XtraEditors.CheckButton)c).Text;
                    Thread thread = new Thread(GetData_网站信息);
                    thread.IsBackground = true;
                    thread.Start(str + "|" + url + "|" + "评论中心|" + "60");
                }
            }
        }
        private void Start_财经日历()
        {
            Thread thread = new Thread(GetData_财经日历);
            thread.IsBackground = true;
            thread.Start();
        }
        private void Start_SetInfo()
        {
            //while (true)
            //{
            this.Invoke(new SetVisibleDelegate(SetVisible));
            //    Thread.Sleep(2000);
            //}
        }
        private void GetData_网站信息(object o)
        {
            while (true)
            {
                string type = o.ToString().Split(new char[] { '|' })[0];
                string url = o.ToString().Split(new char[] { '|' })[1];
                List<新闻信息> ls = new List<新闻信息>();
                string str = Utility.getUrlSource(url, "utf-8");
                Regex r = new Regex("<ul class=\"list lh24 f14\">" + @"[\s\S]*?" + "</ul>", RegexOptions.IgnoreCase);
                str = r.Match(str).ToString();
                r = new Regex("<li>" + @"[\s\S]*?" + "</li>", RegexOptions.IgnoreCase);
                MatchCollection ms = r.Matches(str);
                foreach (Match m in ms)
                {
                    if (string.IsNullOrEmpty(m.ToString().Replace("<li class=\"bk20 hr\">", "").Replace("</li>", "").Replace(" ", "")))
                        continue;
                    新闻信息 xw = new 新闻信息();
                    r = new Regex("style=\"\" >" + @"[\s\S]*?" + "</a>", RegexOptions.IgnoreCase);
                    xw.title = r.Match(m.ToString()).ToString().Replace("style=\"\" >", "").Replace("</a>", "").Replace("\n\t", "").Replace("&nbsp;", "");
                    r = new Regex("href=\"" + @"[\s\S]*?" + "\"", RegexOptions.IgnoreCase);
                    xw.url = r.Match(m.ToString()).ToString().Replace("href=\"", "").Replace("\"", "");
                    r = new Regex("<span class=\"rt\">" + @"[\s\S]*?" + "</span>", RegexOptions.IgnoreCase);
                    xw.date = r.Match(m.ToString()).ToString().Replace("<span class=\"rt\">", "").Replace("</span>", "").Trim();
                    if (xw.date.StartsWith("12"))
                        xw.date = "2012-" + xw.date;
                    else
                        xw.date = "2013-" + xw.date;
                    xw.type = type;
                    switch (o.ToString().Split(new char[] { '|' })[2])
                    {
                        case "新闻汇总":
                            if (!dic_新闻汇总.ContainsKey(xw.url))
                            {
                                dic_新闻汇总.Add(xw.url, xw);
                                lock (m_list新闻汇总)
                                    m_list新闻汇总.Add(xw);
                            }
                            break;
                        case "评论中心":
                            if (!dic_评论中心.ContainsKey(xw.url))
                            {
                                dic_评论中心.Add(xw.url, xw);
                                lock (m_list评论中心)
                                    m_list评论中心.Add(xw);
                            }
                            break;
                    }
                }
                lock (this)
                    Start_SetInfo();
                Thread.Sleep(Convert.ToInt32(o.ToString().Split(new char[] { '|' })[3]) * 1000);
            }
        }
        private void GetData_持仓报告(object o)
        {
            StringBuilder strGather = new StringBuilder();
            if (o.ToString() == "黄金ETF持仓量")
            {
                strGather.Append(Utility.getUrlSource("http://www.zhijinwang.com/etf/", "gb2312"));
                Regex r = new Regex("<table style=\"border-collapse: collapse; width: 100%; margin-top: 1px\" id=\"table75\" border=\"1\" bordercolor=\"#000000\" cellspacing=\"0\" cellpadding=\"0\" height=\"61\">" + @"[\s\S]*?" + "</div>", RegexOptions.IgnoreCase);
                Match m = r.Match(strGather.ToString());
                strGather.Remove(0, strGather.Length);
                strGather.Append("<link href=\"http://www.zhijinwang.com/Skins/Css/Css0.Css\" rel=\"stylesheet\" type=\"text/css\" />");
                strGather.Append("<style>.down {color:#008000;}.up {color:#f00;}body {background:none;}</style>");
                strGather.Append(m.Value);
            }
            else
            {
                strGather.Append(Utility.getUrlSource("http://www.zhijinwang.com/etf_slv/", "gb2312"));
                Regex r = new Regex("<table style=\"border-collapse: collapse; width: 100%; margin-top: 1px\" id=\"table75\" border=\"1\" bordercolor=\"#000000\" cellspacing=\"0\" cellpadding=\"0\" height=\"61\">" + @"[\s\S]*?" + "</div>", RegexOptions.IgnoreCase);
                Match m = r.Match(strGather.ToString());
                strGather.Remove(0, strGather.Length);
                strGather.Append("<link href=\"http://www.zhijinwang.com/Skins/Css/Css0.Css\" rel=\"stylesheet\" type=\"text/css\" />");
                strGather.Append("<style>.down {color:#008000;}.up {color:#f00;}body {background:none;}</style>");
                strGather.Append(m.Value);
            }
            webBrowser3.DocumentText = Check_Href(strGather.ToString());
        }
        private void GetData_财经日历()
        {
            string url = "http://www.fx678.com/indexs/html/";
            List<string> listDay = GetWeekDate();
            while (true)
            {
                m_list财经日历.Clear();
                for (int i = 0; i < 5; i++)
                {
                    string str = Utility.getUrlSource(url + listDay[i] + ".shtml", "gb2312");
                    Regex r = new Regex("<table width=\"725px\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\" class=\"BlueTable\"" + @"[\s\S]*?" + "<div class=\"dw_tabtitle\">", RegexOptions.IgnoreCase);
                    str = r.Match(str).ToString();
                    r = new Regex("<div" + @"[\s\S]*?" + "</div>", RegexOptions.IgnoreCase);
                    MatchCollection mc = r.Matches(str);
                    foreach (Match ma in mc)
                        str = str.Replace(ma.Value, "");
                    r = new Regex("<tr>" + @"[\s\S]*?" + "</tr>" + @"[\s\S]*?" + "</tr>", RegexOptions.IgnoreCase);
                    MatchCollection mas = r.Matches(str);

                    foreach (Match m in mas)
                    {
                        if (m.Value.Contains("重要性"))
                            continue;
                        财经信息 cjxx = new 财经信息();
                        cjxx.日期 = listDay[i];
                        r = new Regex("<td>" + @"[\s\S]*?" + "</td>", RegexOptions.IgnoreCase);
                        MatchCollection mas1 = r.Matches(m.Value);
                        cjxx.时间 = mas1[0].Value.Replace("<td>", "").Replace("</td>", "");
                        cjxx.国家 = mas1[1].Value.Replace("<td>", "").Replace("</td>", "");
                        r = new Regex("<tbody>" + @"[\s\S]*?" + "</tbody>", RegexOptions.IgnoreCase);
                        string strTemp = r.Match(m.Value).Value;
                        r = new Regex("<td" + @"[\s\S]*?" + "</td>", RegexOptions.IgnoreCase);
                        MatchCollection mas2 = r.Matches(strTemp);
                        cjxx.标题 = mas2[0].Value;
                        if (cjxx.标题.IndexOf("点击查看详细") != -1)
                            cjxx.标题 = cjxx.标题.Substring(cjxx.标题.IndexOf("title='") + 7, cjxx.标题.IndexOf("点击查看详细") - cjxx.标题.IndexOf("title='") - 7).Trim();
                        else
                        {
                            cjxx.标题 = cjxx.标题.Replace("<td width='233px' style='text-align: left; padding: 0 9px;'>", "");
                            cjxx.标题 = cjxx.标题.Substring(cjxx.标题.IndexOf(">") + 1, cjxx.标题.IndexOf("</span>") - cjxx.标题.IndexOf(">") - 1);
                        }
                        cjxx.重要性 = mas2[1].Value.Replace("<td width='52'>", "").Replace("</td>", "").Replace("<strong>", "").Replace("</strong>", "");
                        cjxx.重要性 = cjxx.重要性.Substring(cjxx.重要性.IndexOf(">") + 1, 1);
                        cjxx.前值 = mas2[2].Value;
                        cjxx.前值 = cjxx.前值.Substring(cjxx.前值.IndexOf(">") + 1, cjxx.前值.IndexOf("</td>") - cjxx.前值.IndexOf(">") - 1);
                        cjxx.预测值 = mas2[3].Value;
                        cjxx.预测值 = cjxx.预测值.Substring(cjxx.预测值.IndexOf(">") + 1, cjxx.预测值.IndexOf("</td>") - cjxx.预测值.IndexOf(">") - 1);
                        cjxx.结果 = mas2[4].Value.Replace("<td width='75'>", "").Replace("</td>", "");
                        cjxx.结果 = cjxx.结果.Substring(cjxx.结果.IndexOf(">") + 1, cjxx.结果.IndexOf("</span>") - cjxx.结果.IndexOf(">") - 1);
                        m_list财经日历.Add(cjxx);
                    }
                }
                this.Invoke(new SetVisibleDelegate(SetVisible财经日历));
                Thread.Sleep(600000);
            }
        }
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
                筛选数据(panelControl1, treeList新闻汇总);
            }
            if (m_list评论中心.Count > 0)
            {
                lock (m_list评论中心)
                    foreach (新闻信息 xw in m_list评论中心)
                        this.treeList评论中心.AppendNode(new object[] { xw.date, xw.title, xw.type }, -1, xw.url);
                m_list评论中心.Clear();
                筛选数据(panelControl2, treeList评论中心);
            }
        }
        private void SetVisible财经日历()
        {
            if (m_list财经日历.Count > 0)
            {
                foreach (财经信息 cjxx in m_list财经日历)
                    this.treeList财经日历.AppendNode(new object[] { cjxx.时间, cjxx.国家, cjxx.标题, cjxx.重要性, cjxx.前值, cjxx.预测值, cjxx.结果 }, -1, cjxx.日期);
                m_list财经日历.Clear();
                List<string> ls = GetWeekDate();
                foreach (Control c in panelControl3.Controls)
                {
                    if (ls[int.Parse(((DevExpress.XtraEditors.SimpleButton)c).Tag.ToString())] == DateTime.Now.ToString("yyyy-MM-dd").Replace("-", ""))
                        simpleButton日期_Click(c, null);
                }
            }
        }

        private List<string> GetWeekDate()
        {
            DateTime dt = DateTime.Now;
            List<string> ls = new List<string>();
            DateTime startWeek = dt.AddDays(1 - Convert.ToInt32(dt.DayOfWeek.ToString("d")));
            ls.Add(startWeek.ToString("yyyy-MM-dd").Replace("-", ""));
            for (int i = 1; i <= 6; i++)
                ls.Add(startWeek.AddDays(i).ToString("yyyy-MM-dd").Replace("-", ""));
            return ls;
        }
        private void 筛选数据(DevExpress.XtraEditors.PanelControl panel, DevExpress.XtraTreeList.TreeList treelist)
        {
            foreach (Control c in panel.Controls)
            {
                if (c.GetType().FullName == "DevExpress.XtraEditors.CheckButton")
                {
                    if (!((DevExpress.XtraEditors.CheckButton)c).Checked)
                    {
                        foreach (TreeListNode tln in treelist.Nodes)
                        {
                            if (tln.GetValue(2).ToString() == ((DevExpress.XtraEditors.CheckButton)c).Text)
                                tln.Visible = false;
                        }
                    }
                    else
                    {
                        foreach (TreeListNode tln in treelist.Nodes)
                        {
                            if (tln.GetValue(2).ToString() == ((DevExpress.XtraEditors.CheckButton)c).Text)
                                tln.Visible = true;
                        }
                    }
                }
            }
            if (treelist.Nodes.Count > 0)
                treelist.SetFocusedNode(treelist.Nodes[0]);
        }
        private void 提醒弹窗()
        {
            XMLHandle app = new XMLHandle();
            if (treeList新闻汇总.Nodes.Count == 0)
                return;
            if (app.GetAppValue("自动弹窗") == "1")
            {
                FormInfo ti = new FormInfo(m_list新闻汇总);
                ti.Tag = app.GetAppValue("弹窗时间");
                ti.Show();
            }
            if (app.GetAppValue("语音提醒") == "1")
            {
                string file = Application.StartupPath + "\\Sound\\" + app.GetAppValue("语音文件");
                if (File.Exists(file))
                    sndPlaySound(file, 1);
                else
                {
                    MessageBox.Show("铃声文件缺失，请暂时关闭此功能");
                    return;
                }
            }
        }
        public string Check_Href(string html)
        {
            System.Text.RegularExpressions.Regex regex = new System.Text.RegularExpressions.Regex(@"<a.*?>|</a>", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            html = regex.Replace(html, "");
            return html;
        }
        public void GetTime()
        {
            DateTime dt = DateTime.Now;
            string time = string.Empty;
            time = "    悉尼：" + dt.AddHours(2).ToString("HH:mm:ss");
            time += " 东京：" + dt.AddHours(1).ToString("HH:mm:ss");
            time += " 北京：" + dt.AddHours(0).ToString("HH:mm:ss");
            time += " 法兰克福：" + dt.AddHours(-6).ToString("HH:mm:ss");
            time += " 伦敦：" + dt.AddHours(-7).ToString("HH:mm:ss");
            time += " 纽约：" + dt.AddHours(-12).ToString("HH:mm:ss");
            barStaticItem1.Caption = "外汇信息即时提醒" + time;
        }
        private void Init新闻按钮()
        {
            int count = 0;
            XMLHandle app = new XMLHandle();
            foreach (Control c in panelControl1.Controls)
            {
                if (c.GetType().FullName == "DevExpress.XtraEditors.CheckButton")
                {
                    if (app.GetAppValue(((DevExpress.XtraEditors.CheckButton)c).Text + "采集") == "1")
                    {
                        ((DevExpress.XtraEditors.CheckButton)c).Location = new Point(5 + count * 71, 3);
                        c.Visible = true;
                        count++;
                    }
                    else
                    {
                        c.Visible = false;
                        ((DevExpress.XtraEditors.CheckButton)c).Checked = false;
                    }
                }
            }
        }
        private void InitSkin()
        {
            BarSubItem bar = new BarSubItem();
            bar.Caption = "皮肤设置";
            bar.Name = "皮肤设置";

            //迭代出所有皮肤样式
            foreach (DevExpress.Skins.SkinContainer skin in DevExpress.Skins.SkinManager.Default.Skins)
            {
                BarButtonItem barBI = new BarButtonItem();
                barBI.Tag = skin.SkinName;
                barBI.Name = skin.SkinName;
                barBI.Caption = skin.SkinName;
                barBI.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(ItemClick);

                this.barManager1.Items.AddRange(new DevExpress.XtraBars.BarItem[] { barBI });
                bar.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] { new DevExpress.XtraBars.LinkPersistInfo(barBI) });
            }
            this.barManager1.Items.Add(bar);
            this.bar1.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] { new DevExpress.XtraBars.LinkPersistInfo(bar) });
        }
        private void ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {

            defaultLookAndFeel1.LookAndFeel.SetSkinStyle(e.Item.Tag.ToString());
            e.Item.Hint = e.Item.Tag.ToString();
        }
    }
}