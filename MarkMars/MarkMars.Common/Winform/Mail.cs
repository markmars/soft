using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Mail;
using System.Net.Mime;
using System.Net;

namespace MarkMars.Common.Winform
{
    /// <summary>
    /// 进行邮件的发送功能
    /// </summary>
    public static class Mail
    {
        /// <summary>
        /// 简单的邮件发送
        /// </summary>
        /// <param name="strSMTP">smtp服务器，原型：smtp.qq.com</param>
        /// <param name="strFrom">发件人地址</param>
        /// <param name="strTO">收件人地址</param>
        /// <param name="strPassword">发件人密码</param>
        /// <param name="strTitle">邮件主题</param>
        /// <param name="strBody">邮件正文</param>
        public static void send_简单发送(string strSMTP, string strFrom, string strTO, string strPassword, string strTitle, string strBody)
        {
            MailMessage message = new MailMessage(strFrom, strTO, strTitle, strBody);//SmtpClient是发送邮件的主体，这个构造函数是告知SmtpClient发送邮件时使用哪个SMTP服务器
            SmtpClient mailClient = new SmtpClient(strSMTP);//将认证实例赋予mailClient,也就是访问SMTP服务器的用户名和密码
            mailClient.Credentials = new NetworkCredential(strFrom, strPassword);
            mailClient.Send(message);//最终的发送方法
        }

        #region 可用研究
        //private static MailMessage mailMessage;
        //private static SmtpClient smtpClient;
        //private static string password;//发件人密码  

        ///// <summary>  
        ///// 处审核后类的实例  
        ///// </summary>  
        ///// <param name="To">收件人地址</param>  
        ///// <param name="From">发件人地址</param>  
        ///// <param name="Body">邮件正文</param>  
        ///// <param name="Title">邮件的主题</param>  
        ///// <param name="Password">发件人密码</param>  
        //private void SendMail(string strTo, string strFrom, string Body, string Title, string Password)
        //{
        //    mailMessage = new MailMessage();
        //    mailMessage.To.Add(strTo);
        //    mailMessage.From = new System.Net.Mail.MailAddress(strFrom);
        //    mailMessage.Subject = Title;
        //    mailMessage.Body = Body;
        //    mailMessage.IsBodyHtml = true;
        //    mailMessage.BodyEncoding = System.Text.Encoding.UTF8;
        //    mailMessage.Priority = System.Net.Mail.MailPriority.Normal;
        //    password = Password;
        //}

        ///// <summary>  
        ///// 添加附件  
        ///// </summary>  
        //private void Attachments(string Path)
        //{
        //    string[] path = Path.Split(',');
        //    Attachment data;
        //    ContentDisposition disposition;
        //    for (int i = 0; i < path.Length; i++)
        //    {
        //        data = new Attachment(path[i], MediaTypeNames.Application.Octet);//实例化附件  
        //        disposition = data.ContentDisposition;
        //        disposition.CreationDate = System.IO.File.GetCreationTime(path[i]);//获取附件的创建日期  
        //        disposition.ModificationDate = System.IO.File.GetLastWriteTime(path[i]);//获取附件的修改日期  
        //        disposition.ReadDate = System.IO.File.GetLastAccessTime(path[i]);//获取附件的读取日期  
        //        mailMessage.Attachments.Add(data);//添加到附件中  
        //    }
        //}

        ///// <summary>  
        ///// 异步发送邮件  
        ///// </summary>  
        ///// <param name="CompletedMethod"></param>  
        //private void SendAsync(SendCompletedEventHandler CompletedMethod)
        //{
        //    if (mailMessage != null)
        //    {
        //        smtpClient = new SmtpClient();
        //        smtpClient.Credentials = new System.Net.NetworkCredential(mailMessage.From.Address, password);//设置发件人身份的票据  
        //        smtpClient.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
        //        smtpClient.Host = "smtp." + mailMessage.From.Host;
        //        smtpClient.SendCompleted += new SendCompletedEventHandler(CompletedMethod);//注册异步发送邮件完成时的事件  
        //        smtpClient.SendAsync(mailMessage, mailMessage.Body);
        //    }
        //}

        ///// <summary>  
        ///// 发送邮件  
        ///// </summary>  
        //private void Send()
        //{
        //    if (mailMessage != null)
        //    {
        //        smtpClient = new SmtpClient();
        //        smtpClient.Credentials = new System.Net.NetworkCredential(mailMessage.From.Address, password);//设置发件人身份的票据  
        //        smtpClient.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
        //        smtpClient.Host = "smtp." + mailMessage.From.Host;
        //        smtpClient.Send(mailMessage);
        //    }
        //}
        #endregion
    }
}
