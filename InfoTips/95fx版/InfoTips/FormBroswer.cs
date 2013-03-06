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
    public partial class FormBroswer : BaseForm
    {
        string strUrl = "";
        public FormBroswer(string url, string title, bool b在线行情)
        {
            InitializeComponent();
            this.Text = title;
            webBrowser1.ContextMenu = null;
            webBrowser1.ScriptErrorsSuppressed = false;

            if (b在线行情)
                webBrowser1.Navigate(url);
            else
                webBrowser1.DocumentText = Check_Href(url);
        }
        public FormBroswer(string url, string title)
        {
            InitializeComponent();
            webBrowser1.ContextMenu = null;
            webBrowser1.ScriptErrorsSuppressed = false;

            strUrl = url;
            this.Text = title;
        }
        private void FormBroswer_Load(object sender, EventArgs e)
        {
            Utility.m_listForm.Add(this);
            if (string.IsNullOrEmpty(strUrl))
                return;
            webBrowser1.DocumentText = "正在加载信息，请稍后...";
            Thread thread = new Thread(InitInfo);
            thread.IsBackground = true;
            thread.Start();
        }
        private void FormBroswer_FormClosing(object sender, FormClosingEventArgs e)
        {
            for (int i = 0; i < Utility.m_listForm.Count; i++)
            {
                if (((Form)Utility.m_listForm[i]).Text == this.Text)
                    Utility.m_listForm.Remove(Utility.m_listForm[i]);
            }
        }
        public void InitInfo()
        {
            Utility ut = new Utility();
            StringBuilder str = new StringBuilder();
            str.Append(ut.getUrlSource(strUrl, "gb2312"));//获取采集信息
            Regex r = new Regex("<div id=\"Article\" >" + @"[\s\S]*?</div>[\s\S]*?" + "</div>", RegexOptions.IgnoreCase);
            Match m = r.Match(str.ToString());
            str.Remove(0, str.Length).Append("<link type=\"text/css\" href=\"http://cms.95fx.com/statics/css/reduced4.css\" media=\"all\" rel=\"Stylesheet\"><link href=\"http://www.95fx.com/statics/css/reset.css\" rel=\"stylesheet\" type=\"text/css\" /><link href=\"http://www.95fx.com/statics/css/default_blue.css\" rel=\"stylesheet\" type=\"text/css\" /><div class=\"main\">").Append(m.Value);
            str.Append("</div></div>");
            webBrowser1.DocumentText = Check_Href(str.ToString());
        }
        public string Check_Href(string html)
        {
            System.Text.RegularExpressions.Regex regex = new System.Text.RegularExpressions.Regex(@"<a.*?>|</a>", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            html = regex.Replace(html, "");
            return html;
        }
        private void 刷新_Click(object sender, EventArgs e)
        {
            webBrowser1.Refresh();
        }
        void Window_Error(object sender, HtmlElementErrorEventArgs e)
        {

        }
        private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            if (webBrowser1.Document == null)
                return;
            webBrowser1.Document.Window.Error += Window_Error;
            HtmlDocument hd = webBrowser1.Document;
        }
    }
}
