using AutoUpdate;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Xml;

namespace InfoTips
{
    public partial class FrmMain : BaseForm
    {
        private MyUserControl m_formActive, m_form火线, m_form新闻, m_form评论, m_form日历, m_form持仓, m_form群聊;
        private MyUserControl m_form浏览网址, m_form浏览代码;
        public FrmMain()
        {
            InitializeComponent();
            InitNaviBars();
            InitSkin();
        }
        private void FrmMain_Load(object sender, EventArgs e)
        {
            notifyIcon.Visible = true;
            DevExpress.XtraBars.ItemClickEventArgs eee = new DevExpress.XtraBars.ItemClickEventArgs(barButtonItem新闻汇总, null);
            barButtonItem主菜单_ItemClick(null, eee);
            eee = new DevExpress.XtraBars.ItemClickEventArgs(barButtonItem火线速递, null);
            barButtonItem主菜单_ItemClick(null, eee);
        }
        private void Frm主窗体_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                this.Visible = false;
                notifyIcon.ShowBalloonTip(5000, "外汇信息即时提醒", "外汇信息即时提醒", ToolTipIcon.Info);
                return;
            }
            for (int i = 100; i < 112; i++)
                HotKey.UnregisterHotKey(this.Handle, i);
            Application.ExitThread();
            notifyIcon.Visible = false;
            Environment.Exit(0);
        }
        private void Frm主窗体_SizeChanged(object sender, EventArgs e)
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

        private void barButtonItem主菜单_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            switch (((DevExpress.XtraBars.BarButtonItem)e.Item).Caption)
            {
                case "火线速递":
                    if (m_form火线 == null)
                        m_form火线 = new UserControl火线();
                    SetActiveForm(m_form火线);
                    break;
                case "新闻汇总":
                    if (m_form新闻 == null)
                        m_form新闻 = new UserControl新闻();
                    SetActiveForm(m_form新闻);
                    break;
                case "评论中心":
                    if (m_form评论 == null)
                        m_form评论 = new UserControl评论();
                    SetActiveForm(m_form评论);
                    break;
                case "财经日历":
                    if (m_form日历 == null)
                        m_form日历 = new UserControl日历();
                    SetActiveForm(m_form日历);
                    break;
                case "持仓报告":
                    if (m_form持仓 == null)
                        m_form持仓 = new UserControl持仓();
                    SetActiveForm(m_form持仓);
                    break;
                case "行情群聊":
                    if (m_form群聊 == null)
                        m_form群聊 = new UserControl群聊();
                    SetActiveForm(m_form群聊);
                    break;
                case "参数":
                    FormSetting setting = new FormSetting();
                    if (setting.ShowDialog() == DialogResult.OK)
                    {
                        if (m_form新闻 != null)
                            if (m_form新闻.GetType().Name == "UserControl新闻")
                                ((UserControl新闻)m_form新闻).Init菜单();
                        if (m_form评论 != null)
                            if (m_form评论.GetType().Name == "UserControl评论")
                                ((UserControl评论)m_form评论).Init菜单();
                        if (m_form火线 != null)
                            if (m_form火线.GetType().Name == "UserControl火线")
                                ((UserControl火线)m_form火线).Init菜单();
                    }
                    break;
                case "全屏":
                    if (m_formActive != null && m_formActive.GetType().Name == "UserControl浏览" && ((UserControl浏览)m_formActive).strTitle != "群聊")
                    {
                        FormBroswer fb = new FormBroswer(((UserControl浏览)m_formActive).strUrl, ((UserControl浏览)m_formActive).strTitle, true);
                        fb.WindowState = FormWindowState.Maximized;
                        fb.Show();
                    }
                    break;
                case "老板键设置":
                    new Form快捷键(this.Handle).ShowDialog();
                    break;
            }
        }
        private void 浏览代码listToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (m_form浏览代码 == null)
                m_form浏览代码 = new UserControl浏览();
            ((UserControl浏览)m_form浏览代码).strUrl = ((ToolStripMenuItem)sender).Tag.ToString();
            ((UserControl浏览)m_form浏览代码).strTitle = ((ToolStripMenuItem)sender).Text;
            ((UserControl浏览)m_form浏览代码).b浏览地址 = false;
            SetActiveForm(m_form浏览代码);
        }
        private void 任务栏菜单ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            switch (((ToolStripMenuItem)sender).Text)
            {
                case "显示主面板":
                    this.Visible = true;
                    this.WindowState = FormWindowState.Normal;
                    break;
                case "参数设置":
                    FormSetting setting = new FormSetting();
                    if (setting.ShowDialog() == DialogResult.OK)
                    {
                        if (m_form新闻 != null)
                            if (m_form新闻.GetType().Name == "UserControl新闻")
                                ((UserControl新闻)m_form新闻).Init菜单();
                        if (m_form评论 != null)
                            if (m_form评论.GetType().Name == "UserControl评论")
                                ((UserControl评论)m_form评论).Init菜单();
                        if (m_form火线 != null)
                            if (m_form火线.GetType().Name == "UserControl火线")
                                ((UserControl火线)m_form火线).Init菜单();
                    }
                    break;
                case "退出程序":
                    Application.ExitThread();
                    notifyIcon.Visible = false;
                    Environment.Exit(0);
                    break;
            }
        }
        private void notifyIcon_MouseDoubleClick(object sender, MouseEventArgs e)
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
        private void timer当前时间_Tick(object sender, EventArgs e)
        {
            DateTime dt = DateTime.Now;
            string time = string.Empty;
            time = "    悉尼：" + dt.AddHours(2).ToString("HH:mm:ss");
            time += " 东京：" + dt.AddHours(1).ToString("HH:mm:ss");
            time += " 北京：" + dt.AddHours(0).ToString("HH:mm:ss");
            time += " 法兰克福：" + dt.AddHours(-6).ToString("HH:mm:ss");
            time += " 伦敦：" + dt.AddHours(-7).ToString("HH:mm:ss");
            time += " 纽约：" + dt.AddHours(-12).ToString("HH:mm:ss");
            barStaticItem当前时间.Caption = time;
        }
        private void barButtonItem关于_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            new FormAbout().ShowDialog();
        }
        private void barButtonItem快捷窗口_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (m_form浏览网址 == null)
                m_form浏览网址 = new UserControl浏览();
            ((UserControl浏览)m_form浏览网址).strUrl = ((DevExpress.XtraBars.BarButtonItem)e.Item).Tag.ToString();
            ((UserControl浏览)m_form浏览网址).strTitle = ((DevExpress.XtraBars.BarButtonItem)e.Item).Caption;
            ((UserControl浏览)m_form浏览网址).b浏览地址 = true;
            SetActiveForm(m_form浏览网址);
        }
        private void barStaticItem2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Process.Start("http://www.fx168.com");
        }
        private void barStaticItem3_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Process.Start("http://www.95fx.com/t/linktext.php");
        }

        private void InitNaviBars()
        {
            string strXmlFiles = Application.StartupPath + "\\NaviBar.xml";
            if (!File.Exists(strXmlFiles))
            {
                MessageBox.Show("程序损坏,请到官网下载完整版本！");
                Application.ExitThread();
                notifyIcon.Visible = false;
                Environment.Exit(0);
            }

            navBarControl功能导航.Items.Clear();
            navBarControl功能导航.Groups.Clear();
            XmlFiles localXmlFiles = new XmlFiles(strXmlFiles);
            XmlNodeList listNode = localXmlFiles.GetNodeList("NaviBars");
            foreach (XmlNode xn in listNode)
            {
                DevExpress.XtraNavBar.NavBarGroup navGroup = new DevExpress.XtraNavBar.NavBarGroup();
                navGroup.Caption = xn.Attributes["Name"].Value.Trim();
                foreach (XmlNode xnxn in xn)
                {
                    DevExpress.XtraNavBar.NavBarItem navLink = new DevExpress.XtraNavBar.NavBarItem();
                    navLink.Caption = xnxn.Attributes["Name"].Value.Trim();
                    navLink.Tag = xnxn.Attributes["Url"].Value.Trim();
                    navLink.LinkClicked += navLink_LinkClicked;
                    navGroup.ItemLinks.Add(navLink);
                }
                navGroup.Expanded = true;
                navBarControl功能导航.Groups.Add(navGroup);
            }
        }
        void navLink_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            if (m_form浏览网址 == null)
                m_form浏览网址 = new UserControl浏览();
            ((UserControl浏览)m_form浏览网址).strUrl = ((DevExpress.XtraNavBar.NavBarItem)sender).Tag.ToString();
            ((UserControl浏览)m_form浏览网址).strTitle = ((DevExpress.XtraNavBar.NavBarItem)sender).Caption;
            ((UserControl浏览)m_form浏览网址).b浏览地址 = true;
            SetActiveForm(m_form浏览网址);
        }
        private void SetActiveForm(MyUserControl f)
        {
            if (m_formActive != null)
                m_formActive.Hide();

            f.Parent = this.panel主区域;
            f.Dock = DockStyle.Fill;
            f.Show();
            if (f.GetType().Name == "UserControl浏览" && ((UserControl浏览)f).strTitle != "群聊")
                ((UserControl浏览)f).RefreshSelf();
            m_formActive = f;
        }
        protected override void WndProc(ref Message m)
        {
            const int WM_HOTKEY = 0x0312;
            //按快捷键 
            switch (m.Msg)
            {
                case WM_HOTKEY:
                    switch (m.WParam.ToInt32())
                    {
                        case 100:
                            判断快捷键("F1");
                            break;
                        case 101:
                            判断快捷键("F2");
                            break;
                        case 102:
                            判断快捷键("F3");
                            break;
                        case 103:
                            判断快捷键("F4");
                            break;
                        case 104:
                            判断快捷键("F5");
                            break;
                        case 105:
                            判断快捷键("F6");
                            break;
                        case 106:
                            判断快捷键("F7");
                            break;
                        case 107:
                            判断快捷键("F8");
                            break;
                        case 108:
                            判断快捷键("F9");
                            break;
                        case 109:
                            判断快捷键("F10");
                            break;
                        case 110:
                            判断快捷键("F11");
                            break;
                        case 111:
                            判断快捷键("F12");
                            break;
                    }
                    break;
            }
            base.WndProc(ref m);
        }
        private void 判断快捷键(string key)
        {
            XMLHandle app = new XMLHandle();
            app.LoadAppConfig();
            string str1 = app.GetAppValue("贵金属快捷开");
            string str2 = app.GetAppValue("贵金属快捷关");
            string str3 = app.GetAppValue("市场报价快捷开");
            string str4 = app.GetAppValue("市场报价快捷关");
            string str5 = app.GetAppValue("外汇行情快捷开");
            string str6 = app.GetAppValue("外汇行情快捷关");
            string str7 = app.GetAppValue("全球股指快捷开");
            string str8 = app.GetAppValue("全球股指快捷关");
            string str9 = app.GetAppValue("金价恒信贵快捷开");
            string str10 = app.GetAppValue("金价恒信贵快捷关");
            if (key == str1)
            {
                FormBroswer fb = new FormBroswer("http://www.95fx.com/tools/flash/stools/bar-1.php", "贵金属", true);
                fb.Show();
            }
            if (key == str2)
            {
                for (int i = 0; i < Utility.m_listForm.Count; i++)
                {
                    if (((Form)Utility.m_listForm[i]).Text == "贵金属")
                        ((Form)Utility.m_listForm[i]).Close();
                }
            }
            if (key == str3)
            {
                FormBroswer fb = new FormBroswer("http://www.95fx.com/tools/flash/stools/hy.php", "市场报价", true);
                fb.Show();
            }
            if (key == str4)
            {
                for (int i = 0; i < Utility.m_listForm.Count; i++)
                {
                    if (((Form)Utility.m_listForm[i]).Text == "市场报价")
                        ((Form)Utility.m_listForm[i]).Close();
                }
            }
            if (key == str5)
            {
                FormBroswer fb = new FormBroswer("http://www.95fx.com/tools/flash/stools/bar-2.php", "外汇行情", true);
                fb.Show();
            }
            if (key == str6)
            {
                for (int i = 0; i < Utility.m_listForm.Count; i++)
                {
                    if (((Form)Utility.m_listForm[i]).Text == "外汇行情")
                        ((Form)Utility.m_listForm[i]).Close();
                }
            }
            if (key == str7)
            {
                FormBroswer fb = new FormBroswer("http://www.95fx.com/tools/flash/stools/bar-3.php", "全球股指", true);
                fb.Show();
            }
            if (key == str8)
            {
                for (int i = 0; i < Utility.m_listForm.Count; i++)
                {
                    if (((Form)Utility.m_listForm[i]).Text == "全球股指")
                        ((Form)Utility.m_listForm[i]).Close();
                }
            }
            if (key == str9)
            {
                FormBroswer fb = new FormBroswer("http://www.365huangjin.com/images/gold.php", "金价-恒信贵", true);
                fb.Show();
            }
            if (key == str10)
            {
                for (int i = 0; i < Utility.m_listForm.Count; i++)
                {
                    if (((Form)Utility.m_listForm[i]).Text == "金价-恒信贵")
                        ((Form)Utility.m_listForm[i]).Close();
                }
            }
        }
        #region 风格设置
        private void InitSkin()
        {
            //迭代出所有皮肤样式
            foreach (DevExpress.Skins.SkinContainer skin in DevExpress.Skins.SkinManager.Default.Skins)
            {
                DevExpress.XtraBars.BarButtonItem bbi = new DevExpress.XtraBars.BarButtonItem();
                bbi.Tag = skin.SkinName;
                bbi.Name = skin.SkinName;
                bbi.Caption = skin.SkinName;
                bbi.ItemClick += bbi_ItemClick;

                barSubItem风格.ItemLinks.Add(bbi);
            }
        }
        void bbi_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            defaultLookAndFeel1.LookAndFeel.SetSkinStyle(((DevExpress.XtraBars.BarButtonItem)e.Item).Tag.ToString());
            Utility.m_skinName = ((DevExpress.XtraBars.BarButtonItem)e.Item).Tag.ToString();
        }
        #endregion

        private void 刷新ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            webBrowser下广告.Refresh();
        }
        private void barButtonItem15_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Process.Start("http://www.95fx.com");
        }
    }
}
