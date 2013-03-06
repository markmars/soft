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
    public partial class UserControl浏览 : MyUserControl
    {
        public string strUrl = "", strTitle = "";
        public bool b浏览地址 = false;

        public UserControl浏览()
        {
            InitializeComponent();
            this.Text = strTitle;
        }
        private void UserControl浏览_Load(object sender, EventArgs e)
        {
            RefreshSelf();
        }
        private void 刷新ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            webBrowser.Refresh();
        }
        private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            if (webBrowser.Document == null)
                return;
            webBrowser.Document.Window.Error += Window_Error;
            HtmlDocument hd = webBrowser.Document;
        }
        public string Check_Href(string html)
        {
            System.Text.RegularExpressions.Regex regex = new System.Text.RegularExpressions.Regex(@"<a.*?>|</a>", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            html = regex.Replace(html, "");
            return html;
        }
        public void RefreshSelf()
        {
            if (b浏览地址)
            {
                webBrowser.Navigate(strUrl);
            }
            else
                webBrowser.DocumentText = strUrl;
        }
        void Window_Error(object sender, HtmlElementErrorEventArgs e)
        {

        }
    }
}
