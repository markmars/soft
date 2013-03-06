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
    public partial class UserControl群聊 : MyUserControl
    {
        public UserControl群聊()
        {
            InitializeComponent();
        }
        private void UserControl浏览_Load(object sender, EventArgs e)
        {
            webBrowser.Navigate("http://bbs.95fx.com/chat/call.html");
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
        void Window_Error(object sender, HtmlElementErrorEventArgs e)
        {

        }

        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            webBrowser.Navigate(e.Item.Tag.ToString());
        }
    }
}
