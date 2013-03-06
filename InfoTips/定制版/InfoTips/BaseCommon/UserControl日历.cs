using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraTreeList.Nodes;
using System.Threading;
using System.Text.RegularExpressions;

namespace InfoTips
{
    public partial class UserControl日历 : MyUserControl
    {
        delegate void SetVisibleDelegate();
        List<财经信息> m_list财经日历;
        Thread m_thread日历来源;

        public UserControl日历()
        {
            InitializeComponent();
        }
        private void UserControl日历_Load(object sender, EventArgs e)
        {
            m_list财经日历 = new List<财经信息>();
            Start_财经日历();
        }

        private void barButtonItem日期_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            webBrowser.Visible = false;
            treeList财经日历.Visible = true;
            List<string> ls = GetWeekDate();
            foreach (TreeListNode tln in treeList财经日历.Nodes)
            {
                if (tln.Tag.ToString() == ls[int.Parse(((DevExpress.XtraBars.BarButtonItem)e.Item).Tag.ToString())])
                    tln.Visible = true;
                else
                    tln.Visible = false;
            }
            foreach (DevExpress.XtraBars.BarItem c in barManager1.Items)
            {
                if (c.GetType().FullName != "DevExpress.XtraBars.BarButtonItem")
                    continue;
                if (c == (DevExpress.XtraBars.BarItem)e.Item)
                    ((DevExpress.XtraBars.BarButtonItem)c).Appearance.ForeColor = Color.Red;
                else
                    ((DevExpress.XtraBars.BarButtonItem)c).Appearance.ForeColor = Color.Black;
            }
            if (treeList财经日历.Nodes.Count > 0)
                treeList财经日历.SetFocusedNode(treeList财经日历.Nodes[0]);
        }
        private void barButtonItem财经日历_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Utility ut = new Utility();
            treeList财经日历.Visible = false;
            webBrowser.Visible = true;
            if (m_thread日历来源 != null)
                m_thread日历来源.Abort();

            webBrowser.DocumentText = "正在加载信息，请稍后...";
            string strInfo = "<Center><iframe frameborder=\"0\" scrolling=\"auto\" height=\"666\" width=\"888\" allowtransparency=\"true\" marginwidth=\"0\" marginheight=\"0\" src=\"http://ecal.cn.forexprostools.com/e_cal.php?duration=daily&top_text_color=FFFFFF&header_text_color=333333&bg1=FFFFFF&bg2=F1F5F8&border=CEDBEB\" align=\"center\"></iframe><br /><span style=\"font-size: 11px;color: #333333;text-decoration: none;\">经济日历由Investing.com证券交易门户网 提供技术支持</span></Center>";//http://cn.investing.com/webmaster-tools/经济日历#
            if (((DevExpress.XtraBars.BarButtonItem)e.Item).Caption == "INVESTING")
                webBrowser.DocumentText = ut.Check_Href(strInfo);
            else
            {
                m_thread日历来源 = new Thread(GetData_财经日历来源);
                m_thread日历来源.IsBackground = true;
                m_thread日历来源.Start(((DevExpress.XtraBars.BarButtonItem)e.Item).Caption);
            }
            foreach (DevExpress.XtraBars.BarItem c in barManager1.Items)
            {
                if (c.GetType().FullName != "DevExpress.XtraBars.BarButtonItem")
                    continue;
                if (c == (DevExpress.XtraBars.BarItem)e.Item)
                    ((DevExpress.XtraBars.BarButtonItem)c).Appearance.ForeColor = Color.Red;
                else
                    ((DevExpress.XtraBars.BarButtonItem)c).Appearance.ForeColor = Color.Black;
            }
        }
        private void barButtonItem日历来源_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            foreach (DevExpress.XtraBars.BarItem c in barManager1.Items)
            {
                if (c.GetType().FullName == "DevExpress.XtraBars.BarButtonItem")
                {
                    switch (((DevExpress.XtraBars.BarButtonItem)c).Caption)
                    {
                        case "INVESTING":
                            if (barCheckItem1.Checked)
                                c.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
                            else
                                c.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
                            break;
                        case "DAILYFX中文":
                            if (barCheckItem2.Checked)
                                c.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
                            else
                                c.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
                            break;
                        case "DAILYFX英文":
                            if (barCheckItem5.Checked)
                                c.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
                            else
                                c.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
                            break;
                        case "FX168日历":
                            if (barCheckItem3.Checked)
                                c.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
                            else
                                c.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
                            break;
                        case "FX168周历":
                            if (barCheckItem4.Checked)
                                c.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
                            else
                                c.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
                            break;
                    }
                }
            }
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

        private void Start_财经日历()
        {
            Thread thread = new Thread(GetData_财经日历);
            thread.IsBackground = true;
            thread.Start();
        }
        //private void GetData_财经日历()
        //{
        //    string url = "http://www.licai18.com/Calendar-dailyfx.jsp?date=";
        //    List<string> listDay = GetWeekDate();
        //    Utility ut = new Utility();
        //    while (true)
        //    {
        //        m_list财经日历.Clear();
        //        for (int i = 0; i < 7; i++)
        //        {
        //            string str = ut.getUrlSource(url + listDay[i], "gb2312");
        //            Regex r = new Regex("<ul id=\"data\">" + @"[\s\S]*?" + "</ul>", RegexOptions.IgnoreCase);
        //            str = r.Match(str).ToString();
        //            r = new Regex("<li" + @"[\s\S]*?" + "</li>", RegexOptions.IgnoreCase);
        //            MatchCollection mas = r.Matches(str);

        //            foreach (Match m in mas)
        //            {
        //                财经信息 cjxx = new 财经信息();
        //                cjxx.日期 = listDay[i];
        //                r = new Regex("<div class=\"time\">" + @"[\s\S]*?" + "</div>", RegexOptions.IgnoreCase);
        //                string strTemp = r.Match(m.Value).Value;
        //                cjxx.时间 = strTemp.Replace("<div class=\"time\">", "").Replace("</div>", "").Replace("&nbsp;", " ");
        //                cjxx.国家 = "";
        //                r = new Regex("<div class=\"event\">" + @"[\s\S]*?" + "</div>", RegexOptions.IgnoreCase);
        //                strTemp = r.Match(m.Value).Value.Replace("<div class=\"event\">", "").Replace("</div>", "");
        //                cjxx.标题 = strTemp;
        //                r = new Regex("<div class=\"importance" + @"[\s\S]*?" + "</div>", RegexOptions.IgnoreCase);
        //                strTemp = r.Match(m.Value).Value;
        //                if (strTemp.Contains("high.jpg"))
        //                    cjxx.重要性 = "高";
        //                else if (strTemp.Contains("middle.jpg"))
        //                    cjxx.重要性 = "中";
        //                else if (strTemp.Contains("bend.jpg"))
        //                    cjxx.重要性 = "低";
        //                else
        //                    cjxx.重要性 = "";
        //                r = new Regex("<div class=\"value\">" + @"[\s\S]*?" + "</div>", RegexOptions.IgnoreCase);
        //                strTemp = r.Match(m.Value).Value;
        //                cjxx.前值 = strTemp.Replace("<div class=\"value\">", "").Replace("</div>", "");
        //                r = new Regex("<div class=\"forecast\">" + @"[\s\S]*?" + "</div>", RegexOptions.IgnoreCase);
        //                strTemp = r.Match(m.Value).Value;
        //                cjxx.预测值 = strTemp.Contains("load.gif") ? "正在获取" : strTemp.Replace("<div class=\"forecast\">", "").Replace("</div>", "");
        //                r = new Regex("<span>" + @"[\s\S]*?" + "</span>", RegexOptions.IgnoreCase);
        //                strTemp = r.Match(m.Value).Value;
        //                cjxx.结果 = strTemp.Contains("load.gif") ? "正在获取" : strTemp.Replace("<span>", "").Replace("</span>", "");
        //                m_list财经日历.Add(cjxx);
        //            }
        //        }
        //        this.Invoke(new SetVisibleDelegate(SetVisible财经日历));
        //        Thread.Sleep(600000);
        //    }
        //}
        private void GetData_财经日历()
        {
            string url = "http://www.fx678.com/indexs/html/";
            List<string> listDay = GetWeekDate();
            Utility ut = new Utility();
            while (true)
            {
                m_list财经日历.Clear();
                for (int i = 0; i < 7; i++)
                {
                    string str = ut.getUrlSource(url + listDay[i] + ".shtml", "gb2312");
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
        private void SetVisible财经日历()
        {
            if (m_list财经日历.Count > 0)
            {
                if (treeList财经日历.Nodes.Count > 0)
                    treeList财经日历.ClearNodes();
                foreach (财经信息 cjxx in m_list财经日历)
                    this.treeList财经日历.AppendNode(new object[] { cjxx.时间, cjxx.国家, cjxx.标题, cjxx.重要性, cjxx.前值, cjxx.预测值, cjxx.结果 }, -1, cjxx.日期);
                m_list财经日历.Clear();
                List<string> ls = GetWeekDate();
                foreach (DevExpress.XtraBars.BarItem c in barManager1.Items)
                {
                    if (c.GetType().FullName != "DevExpress.XtraBars.BarButtonItem")
                        continue;
                    if (((DevExpress.XtraBars.BarButtonItem)c).Tag != null && !string.IsNullOrEmpty(((DevExpress.XtraBars.BarButtonItem)c).Tag.ToString()))
                        if (ls[int.Parse(((DevExpress.XtraBars.BarButtonItem)c).Tag.ToString())] == DateTime.Now.ToString("yyyy-MM-dd").Replace("-", ""))
                        {
                            DevExpress.XtraBars.ItemClickEventArgs ie = new DevExpress.XtraBars.ItemClickEventArgs(c, null);
                            barButtonItem日期_ItemClick(null, ie);
                        }
                }
                if (treeList财经日历.Nodes.Count > 0)
                    treeList财经日历.SetFocusedNode(treeList财经日历.Nodes[0]);
            }
        }
        private void GetData_财经日历来源(object o)
        {
            switch (o.ToString())
            {
                case "DAILYFX中文":
                    webBrowser.Navigate("http://www.dailyfx.com.hk/calendar/index.html");
                    break;
                case "DAILYFX英文":
                    webBrowser.Navigate("http://www.dailyfx.com/calendar?tz=8&sort=date&week=2013%2F0224&eur=true&usd=true&jpy=true&gbp=true&chf=true&aud=true&cad=true&nzd=true&cny=true&high=true&medium=true&low=true");
                    break;
                case "FX168日历":
                    webBrowser.Navigate("http://www.95fx.com/tools/flash/FX168/TIME-D.php");
                    break;
                case "FX168周历":
                    webBrowser.Navigate("http://www.95fx.com/tools/flash/FX168/TIME-w.php");
                    break;
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
        private void 刷新ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            webBrowser.Refresh();
        }
    }
}
