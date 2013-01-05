using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Collections.Specialized;
using System.Diagnostics;

namespace InfoTips
{
    public partial class FormLogin : Form
    {
        readonly string _AuthCookieName = "F3Ce_2132_auth";
        readonly string _PostString = "username={0}&password={1}&quickforward=yes&handlekey=ls&questionid=0&answer=";
        readonly string _LoginUrl = "http://bbs.95fx.net/member.php?mod=logging&action=login&loginsubmit=yes&infloat=yes&inajax=1";
        CookieCollection cookies = null;

        public FormLogin()
        {
            InitializeComponent();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            bool loginSign = false;
            string name = textBox1.Text, password = textBox2.Text, responseString;

            if (string.IsNullOrEmpty(name))
            {
                MessageBox.Show("请输入会员名！");
                return;
            }
            if (string.IsNullOrEmpty(password))
            {
                MessageBox.Show("请输入密码！");
                return;
            }

            try
            {
                if (!WebUtility.HttpPost(_LoginUrl, string.Format(_PostString, name, password), null, Encoding.GetEncoding("GBK"), false, out responseString, out cookies))
                {
                    MessageBox.Show("无法连接到服务器,请重试!");
                    return;
                }
                foreach (Cookie cookie in cookies)
                {
                    if (string.Compare(_AuthCookieName, cookie.Name, true) == 0 && cookie.Value.Length > 0)
                    {
                        loginSign = true;
                        break;
                    }
                }
                if (!loginSign)
                {
                    MessageBox.Show("会员名或密码输入错误,请重试!");
                    return;
                }

                this.DialogResult = DialogResult.OK;
            }
            catch
            {
                System.Windows.Forms.MessageBox.Show("登录失败，请重新登录！", "友情提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            Process.Start("iexplore.exe", "http://bbs.95fx.net/member.php?mod=register");
        }
    }
}