using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Data;

namespace InfoTips
{
    public partial class FormLogin : BaseForm
    {
        readonly string _AuthCookieName = "EVNc_2132_auth";
        readonly string _PostString = "username={0}&password={1}&quickforward=yes&handlekey=ls&questionid=0&answer=";
        readonly string _LoginUrl = "http://bbs.95fx.com/member.php?mod=logging&action=login&loginsubmit=yes&infloat=yes&inajax=1";
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
                        if (检查是否已登录())
                        {
                            MessageBox.Show("已经登陆，请不要重复登陆!");
                            return;
                        }
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
        private bool 检查是否已登录()
        {
            MySqlDB ms = new MySqlDB("210.209.125.197", "db20450304", "db20450304", "tonnyguang");
            string strSQL = string.Format("select status from pre_common_member where username='{0}'", textBox1.Text);
            if (ms.GetTableBySQL(strSQL).Rows[0][0].ToString() != "0")
                return true;
            else
            {
                strSQL = string.Format("update pre_common_member set status=1 where username='{0}'", textBox1.Text);
                ms.ExecuteSQL(strSQL);
                Utility.m_userName = textBox1.Text;
                return false;
            }
        }

        private void textBox2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 13)
                button1_Click(null, null);
        }
        public static string MD5ForPHP(string s)
        {
            System.Security.Cryptography.MD5CryptoServiceProvider HashMD5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
            return BitConverter.ToString(HashMD5.ComputeHash(Encoding.UTF8.GetBytes(s))).Replace("-", "").ToLower();
        }
        public static string GetUCenterPassword(string Password, string Salt)
        {
            return MD5ForPHP(MD5ForPHP(Password) + Salt);
        }
    }
}