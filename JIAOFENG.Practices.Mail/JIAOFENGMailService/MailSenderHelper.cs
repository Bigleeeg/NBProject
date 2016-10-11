using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.IO;
using System.Net.Mail;
using Dashinginfo.Practices.Logical.Log;

namespace DashingMailService
{
    public class MailSenderHelper
    {
        #region Property
        public static string host = System.Configuration.ConfigurationManager.AppSettings["SmtpServer"];
        public static string userName = System.Configuration.ConfigurationManager.AppSettings["MailUserName"];
        public static string password = System.Configuration.ConfigurationManager.AppSettings["MailPassWord"];

        private static int port = 25;

        private static SmtpClient smtpClient = null;
        /// <summary>
        /// 发送邮件用的SmtpClient
        /// </summary>
        private static SmtpClient SmtpClient
        {
            get
            {
                if (smtpClient == null)
                {
                    smtpClient = new SmtpClient(host, port);
                    smtpClient.UseDefaultCredentials = false;
                    smtpClient.Credentials = new System.Net.NetworkCredential(userName, password);
                    //smtpClient.DeliveryMethod = SmtpDeliveryMethod.PickupDirectoryFromIis;
                }
                return smtpClient;
            }
        }
        #endregion

        public static void SendMailMessage(string sender, string from, string[] replyTo, string[] to, string[] cc, string[] bcc,
            string subject, string body, string[] attach, bool isBodyHtml)
        {
            MailMessage mailMessage = new MailMessage();
            mailMessage.Sender = new MailAddress(userName);
            mailMessage.From = new MailAddress(from);
            if (replyTo != null)
            {
                foreach (string str in replyTo)
                {
                    mailMessage.ReplyToList.Add(str);
                }
            }
            if (to != null)
            {
                for (int i = 0; i < to.Length; i++)
                {
                    mailMessage.To.Add(new MailAddress(to[i]));
                }
            }

            if (cc != null)
            {
                for (int i = 0; i < cc.Length; i++)
                {
                    mailMessage.CC.Add(new MailAddress(cc[i]));
                }
            }

            if (bcc != null)
            {
                for (int i = 0; i < bcc.Length; i++)
                {
                    mailMessage.Bcc.Add(new MailAddress(bcc[i]));
                }
            }

            mailMessage.Subject = subject;
            mailMessage.Body = body;
            mailMessage.IsBodyHtml = isBodyHtml;
            //attchment
            if (attach != null)
            {
                foreach (string path in attach)
                {
                    if (!string.IsNullOrEmpty(path))
                    {
                        Attachment attachment = new Attachment(path);
                        mailMessage.Attachments.Add(attachment);
                    }
                }
            }
            //send out
            SmtpClient.Send(mailMessage);
        }
    }
}