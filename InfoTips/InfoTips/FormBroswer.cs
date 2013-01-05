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

namespace InfoTips
{
    public partial class FormBroswer : Form
    {
        string strUrl = "";
        public FormBroswer()
        {
            InitializeComponent();
        }
        public FormBroswer(string url, string title)
        {
            InitializeComponent();
            webBrowser1.ContextMenuStrip = null;
            strUrl = url;
            this.Text = title;
        }
        private void FormBroswer_Load(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(strUrl))
                return;
            Thread thread = new Thread(InitInfo);
            thread.IsBackground = true;
            thread.Start();
        }
        public void InitInfo()
        {
            StringBuilder str = new StringBuilder();
            str.Append(Utility.getUrlSource(strUrl, "gb2312"));//获取采集信息
            Regex r = new Regex("<div id=\"Article\" >" + @"[\s\S]*?</div>[\s\S]*?" + "</div>", RegexOptions.IgnoreCase);
            Match m = r.Match(str.ToString());
            str.Remove(0, str.Length).Append("<link type=\"text/css\" href=\"http://www.95fx.com/statics/css/reduced4.css\" media=\"all\" rel=\"Stylesheet\"><link href=\"http://www.95fx.com/statics/css/reset.css\" rel=\"stylesheet\" type=\"text/css\" /><link href=\"http://www.95fx.com/statics/css/default_blue.css\" rel=\"stylesheet\" type=\"text/css\" /><div class=\"main\">").Append(m.Value);
            str.Append("</div></div>");
            webBrowser1.DocumentText = Check_Href(str.ToString());
        }
        public string Check_Href(string html)
        {
            System.Text.RegularExpressions.Regex regex = new System.Text.RegularExpressions.Regex(@"<a.*?>|</a>", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            html = regex.Replace(html, "");
            return html;
        }
    }
}
