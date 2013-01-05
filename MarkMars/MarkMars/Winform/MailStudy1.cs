using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Mail;
using System.IO;
using System.Net;
using System.Windows.Forms;
using System.ComponentModel;

namespace MarkMars.Winform
{
    class MailStudy1
    {
        SmtpClient m_smtpClient = null;   //设置SMTP协议
        MailAddress m_mailAddress_from = null; //设置发信人地址  当然还需要密码
        MailAddress m_mailAddress_to = null;  //设置收信人地址  不需要密码
        MailMessage m_mailMessage = new MailMessage();
        FileStream m_fileStream = null; //附件文件流

        /// <summary>
        /// 设置Ｓmtp服务器信息
        /// </summary>
        /// <param name="ServerName">SMTP服务名</param>
        /// <param name="Port">端口号</param>
        private void setSmtpClient(string ServerHost, int Port)
        {
            m_smtpClient = new SmtpClient();
            m_smtpClient.Host = ServerHost;//指定SMTP服务名  例如QQ邮箱为 smtp.qq.com 新浪cn邮箱为 smtp.sina.cn等
            m_smtpClient.Port = Port; //指定端口号
            m_smtpClient.Timeout = 0;  //超时时间
        }

        /// <summary>
        /// 验证发件人信息
        /// </summary>
        /// <param name="MailAddress">发件邮箱地址</param>
        /// <param name="MailPwd">邮箱密码</param>
        private void setAddressform(string MailAddress, string MailPwd)
        {
            //创建服务器认证
            NetworkCredential NetworkCredential_my = new NetworkCredential(MailAddress, MailPwd);
            //实例化发件人地址
            m_mailAddress_from = new System.Net.Mail.MailAddress(MailAddress, null);
            //指定发件人信息  包括邮箱地址和邮箱密码
            m_smtpClient.Credentials = new System.Net.NetworkCredential(m_mailAddress_from.Address, MailPwd);
        }

        /// <summary>
        /// 验证附件大小
        /// </summary>
        /// <param name="path">附件路径</param>
        /// <returns>附件是否可用（大小是否超过10M）</returns>
        private bool Attachment_MaiInit(string path)
        {
            try
            {
                m_fileStream = new FileStream(path, FileMode.Open);
                string name = m_fileStream.Name;
                int size = (int)(m_fileStream.Length / 1024 / 1024);
                m_fileStream.Close();
                if (size > 10)//控制文件大小不大于10Ｍ
                {
                    MessageBox.Show("文件长度不能大于10M！你选择的文件大小为" + size.ToString() + "M", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }
                return true;
            }
            catch (IOException E)
            {
                MessageBox.Show(E.Message);
                return false;
            }
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            try
            {
                setSmtpClient("smtp.qq.com", 465);
            }
            catch (Exception Ex)
            {
                MessageBox.Show("邮件发送失败,请确定SMTP服务名是否正确！" + "\n" + "技术信息:\n" + Ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            try
            {
                //验证发件邮箱地址和密码
                setAddressform("2421567821@qq.com", "a13838975978qqq");
            }
            catch (Exception Ex)
            {
                MessageBox.Show("邮件发送失败,请确定发件邮箱地址和密码的正确性！" + "\n" + "技术信息:\n" + Ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            m_mailAddress_to = new MailAddress("2421567821@qq.com");
            m_mailMessage.To.Add(m_mailAddress_to);

            //发件人邮箱
            m_mailMessage.From = m_mailAddress_from;
            //邮件主题
            m_mailMessage.Subject = "主题";
            m_mailMessage.SubjectEncoding = System.Text.Encoding.UTF8;
            //邮件正文
            m_mailMessage.Body = "你好";
            m_mailMessage.BodyEncoding = System.Text.Encoding.UTF8;
            //注册邮件发送完毕后的处理事件
            m_smtpClient.SendCompleted += new SendCompletedEventHandler(SendCompletedCallback);
            //开始发送邮件
            m_smtpClient.SendAsync(m_mailMessage, "000000000");
        }

        /// <summary>
        /// 邮件发送完成的回调函数
        /// </summary>
        private static void SendCompletedCallback(object sender, AsyncCompletedEventArgs e)
        {
            try
            {
                if (e.Cancelled)
                    MessageBox.Show("发送已取消！");
                if (e.Error != null)
                    MessageBox.Show("邮件发送失败！" + "\n" + "技术信息:\n" + e.ToString(), "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                else
                    MessageBox.Show("邮件成功发出!", "恭喜!", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception Ex) { MessageBox.Show("邮件发送失败！" + "\n" + "技术信息:\n" + Ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }
    }
}
