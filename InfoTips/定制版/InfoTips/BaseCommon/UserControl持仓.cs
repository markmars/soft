using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Text.RegularExpressions;

namespace InfoTips
{
    public partial class UserControl持仓 : MyUserControl
    {
        Thread m_thread持仓报告;
        public UserControl持仓()
        {
            InitializeComponent();
        }
        private void UserControl持仓_Load(object sender, EventArgs e)
        {
            m_thread持仓报告 = new Thread(GetData_持仓报告);
            m_thread持仓报告.IsBackground = true;
            m_thread持仓报告.Start("黄金ETF持仓量");
            barButtonItem1.Appearance.ForeColor = Color.Red;
        }
        private void barButtonItem持仓_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (m_thread持仓报告 == null)
            {
                m_thread持仓报告 = new Thread(GetData_持仓报告);
                m_thread持仓报告.IsBackground = true;
                m_thread持仓报告.Start(((DevExpress.XtraBars.BarButtonItem)e.Item).Caption);
            }
            else
            {
                if (!m_thread持仓报告.IsAlive)
                {
                    m_thread持仓报告 = new Thread(GetData_持仓报告);
                    m_thread持仓报告.IsBackground = true;
                    m_thread持仓报告.Start(((DevExpress.XtraBars.BarButtonItem)e.Item).Caption);
                }
            }
            foreach (DevExpress.XtraBars.BarItem c in barManager1.Items)
            {
                if (c == (DevExpress.XtraBars.BarButtonItem)e.Item)
                    ((DevExpress.XtraBars.BarButtonItem)c).Appearance.ForeColor = Color.Red;
                else
                    ((DevExpress.XtraBars.BarButtonItem)c).Appearance.ForeColor = Color.Black;
            }
        }
        private void 刷新ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            webBrowser.Refresh();
        }
        private void GetData_持仓报告(object o)
        {
            Utility ut = new Utility();
            webBrowser.DocumentText = "正在加载信息，请稍后...";
            StringBuilder strGather = new StringBuilder();
            try
            {
                if (o.ToString() == "黄金ETF持仓量")
                {
                    strGather.Append(ut.getUrlSource("http://www.zhijinwang.com/etf/", "gb2312"));
                    string strTemp = strGather.ToString();
                    Regex r = new Regex("<table style=\"BORDER: 1px solid #dddddd;\" border=\"0\" width=\"100%\" id=\"table71\"  cellpadding=\"5\" cellspacing=\"5\">" + @"[\s\S]*?" + "</table>", RegexOptions.IgnoreCase);
                    strTemp = r.Match(strTemp).ToString();
                    r = new Regex("<tr>" + @"[\s\S]*?" + "</tr>", RegexOptions.IgnoreCase);
                    strTemp = strTemp.Replace(r.Matches(strTemp)[1].ToString(), "");
                    r = new Regex("<table style=\"border-collapse: collapse; width: 100%; margin-top: 1px\" id=\"table75\" border=\"1\" bordercolor=\"#000000\" cellspacing=\"0\" cellpadding=\"0\" height=\"61\">" + @"[\s\S]*?" + "</div>", RegexOptions.IgnoreCase);
                    Match m = r.Match(strGather.ToString());
                    strGather.Remove(0, strGather.Length);
                    strGather.Append(strTemp);
                    strGather.Append("<link href=\"http://www.zhijinwang.com/Skins/Css/Css0.Css\" rel=\"stylesheet\" type=\"text/css\" />");
                    strGather.Append("<style>.down {color:#008000;}.up {color:#f00;}body {background:none;}</style>");
                    strGather.Append(m.Value);
                }
                else
                {
                    strGather.Append(ut.getUrlSource("http://www.zhijinwang.com/etf_slv/", "gb2312"));
                    string strTemp = strGather.ToString();
                    Regex r = new Regex("<table style=\"BORDER: 1px solid #dddddd;\" border=\"0\" width=\"100%\" id=\"table71\">" + @"[\s\S]*?" + "</table>", RegexOptions.IgnoreCase);
                    strTemp = r.Match(strTemp).ToString();
                    r = new Regex("<tr>" + @"[\s\S]*?" + "</tr>", RegexOptions.IgnoreCase);
                    strTemp = strTemp.Replace(r.Matches(strTemp)[1].ToString(), "");
                    r = new Regex("<table style=\"border-collapse: collapse; width: 100%; margin-top: 1px\" id=\"table75\" border=\"1\" bordercolor=\"#000000\" cellspacing=\"0\" cellpadding=\"0\" height=\"61\">" + @"[\s\S]*?" + "</div>", RegexOptions.IgnoreCase);
                    Match m = r.Match(strGather.ToString());
                    strGather.Remove(0, strGather.Length);
                    strGather.Append(strTemp);
                    strGather.Append("<link href=\"http://www.zhijinwang.com/Skins/Css/Css0.Css\" rel=\"stylesheet\" type=\"text/css\" />");
                    strGather.Append("<style>.down {color:#008000;}.up {color:#f00;}body {background:none;}</style>");
                    strGather.Append(m.Value);
                }
            }
            catch (Exception exc)
            {
                webBrowser.DocumentText = exc.Message;
            }
            webBrowser.DocumentText = ut.Check_Href(strGather.ToString());
        }
    }
}
