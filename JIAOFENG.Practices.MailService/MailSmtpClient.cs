using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Mail;
using System.IO;
using Dashinginfo.Practices.Logic.Mail;
using Dashinginfo.Practices.Logic.Mail.Entity;
using Dashinginfo.Practices.Logic.File;

namespace DashingMailService
{
    public class MailSmtpClient
    {
        public MailSmtpClient()
        { }
        public MailSmtpClient(string host, string username, string password)
        {
            this.host = host;
            this.account = username;
            this.password = password;
        }

        private int port=25;
        public int Port
        {
            get { return port; }
        }

        private string host = "";
        public string Host
        {
            get { return host; }
            set { host = value; }
        }

        private string account = "";
        public string Account
        {
            get { return account; }
            set { account = value; }
        }

        private string password = "";
        public string PassWord
        {
            get { return password; }
            set { password = value; }
        }

        private SmtpClient smtpClient = null;
        public SmtpClient SmtpClient
        {
            get 
            {
                if (smtpClient == null)
                {
                    smtpClient = new SmtpClient(this.Host, this.Port);
                    smtpClient.UseDefaultCredentials = true;
                    smtpClient.Credentials = new System.Net.NetworkCredential(this.Account, this.PassWord);
                }
                return smtpClient;
            }
        }
        

        public void SendMailMessage(string sender, string from, string[] replyTo, string[] to, string[] cc, string[] bcc,
           string subject, string body, bool isBodyHtml, FilePackage attachments)
        {
            MailMessage mailMessage = new MailMessage();

            mailMessage.Sender = new MailAddress(account);
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

            if (attachments != null)
            {
                foreach (FileStorage att in attachments.FileStorages)
                {
                    Attachment attachment = new Attachment(new MemoryStream(att.Context), att.FileName);
                    mailMessage.Attachments.Add(attachment);
                }
            }

            this.SmtpClient.Send(mailMessage);
        }
    }
}
