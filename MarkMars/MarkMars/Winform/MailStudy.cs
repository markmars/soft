using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Mail;
using System.Net.Mime;
using System.ComponentModel;
using System.Windows.Forms;

namespace MarkMars.Winform
{
    public class MailStudy
    {
        static MailMessage m_mailMessage;
        static SmtpClient m_smtpClient;
        static string m_password;//发件人密码  

        /// <summary>  
        /// 审核后类的实例  
        /// </summary>  
        /// <param name="To">收件人地址</param>  
        /// <param name="From">发件人地址</param>  
        /// <param name="Body">邮件正文</param>  
        /// <param name="Title">邮件的主题</param>  
        /// <param name="Password">发件人密码</param>  
        private void SendMail(string strTo, string strFrom, string Body, string Title, string Password)
        {
            m_mailMessage = new MailMessage();
            m_mailMessage.To.Add(strTo);
            m_mailMessage.From = new System.Net.Mail.MailAddress(strFrom);
            m_mailMessage.Subject = Title;
            m_mailMessage.Body = Body;
            m_mailMessage.IsBodyHtml = true;
            m_mailMessage.BodyEncoding = System.Text.Encoding.UTF8;
            m_mailMessage.Priority = System.Net.Mail.MailPriority.Normal;
            m_password = Password;
        }

        /// <summary>
        /// 添加附件
        /// </summary>
        /// <param name="Path">附件路径</param>
        private void Attachments(string Path)
        {
            string[] path = Path.Split(',');
            Attachment data;
            ContentDisposition disposition;
            for (int i = 0; i < path.Length; i++)
            {
                data = new Attachment(path[i], MediaTypeNames.Application.Octet);//实例化附件  
                disposition = data.ContentDisposition;
                disposition.CreationDate = System.IO.File.GetCreationTime(path[i]);//获取附件的创建日期  
                disposition.ModificationDate = System.IO.File.GetLastWriteTime(path[i]);//获取附件的修改日期  
                disposition.ReadDate = System.IO.File.GetLastAccessTime(path[i]);//获取附件的读取日期  
                m_mailMessage.Attachments.Add(data);//添加到附件中  
            }
        }

        /// <summary>  
        /// 异步发送邮件  
        /// </summary>  
        private void SendAsync()
        {
            if (m_mailMessage != null)
            {
                m_smtpClient = new SmtpClient();
                m_smtpClient.Credentials = new System.Net.NetworkCredential(m_mailMessage.From.Address, m_password);//设置发件人身份信息  
                m_smtpClient.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
                m_smtpClient.Host = "smtp." + m_mailMessage.From.Host;
                m_smtpClient.SendCompleted += new SendCompletedEventHandler(SendCompletedCallback);//注册异步发送邮件完成时的事件  
                m_smtpClient.SendAsync(m_mailMessage, m_mailMessage.Body);
            }
        }

        /// <summary>  
        /// 发送邮件  
        /// </summary>  
        private void Send()
        {
            if (m_mailMessage != null)
            {
                m_smtpClient = new SmtpClient();
                m_smtpClient.Credentials = new System.Net.NetworkCredential(m_mailMessage.From.Address, m_password);//设置发件人身份的票据  
                m_smtpClient.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
                m_smtpClient.Host = "smtp." + m_mailMessage.From.Host;
                m_smtpClient.Send(m_mailMessage);
            }
        }

        /// <summary>
        /// 邮件发送回调函数
        /// </summary>
        private void SendCompletedCallback(object sender, AsyncCompletedEventArgs e)
        {
            try
            {
                if (e.Cancelled)
                    MessageBox.Show("发送已取消！");
                if (e.Error != null)
                    MessageBox.Show("邮件发送失败！" + "\n" + "信息:\n" + e.ToString(), "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                else
                    MessageBox.Show("邮件成功发出!", "恭喜!", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception Ex) { MessageBox.Show("邮件发送失败！" + "\n" + "信息:\n" + Ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }
    }
}
