using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Mail;
using System.IO;

namespace DashingMailService
{
    public class MailSmtpClient
    {
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

        private string username = "";
        public string UserName
        {
            get { return username; }
            set { username = value; }
        }

        private string password = "";
        public string PassWord
        {
            get { return username; }
            set { username = value; }
        }

        private SmtpClient smtpClient = null;
        public SmtpClient SmtpClient
        {
            get {
                if (smtpClient == null)
                {
                    smtpClient = new SmtpClient(host, port);
                    smtpClient.UseDefaultCredentials = true;
                    smtpClient.Credentials = new System.Net.NetworkCredential(username, password);
                }
                return smtpClient;
            }
        }

        public MailSmtpClient()
        { }
        public MailSmtpClient(string host, string username, string password)
        {
            this.host = host;
            this.username = username;
            this.password = password;
        }

        public void SendMailMessage(string sender, string from, string[] replyTo, string[] to, string[] cc, string[] bcc,
           string subject, string body, Dictionary<string,Stream> dicAttach, bool isBodyHtml)
        {
            MailMessage mailMessage = new MailMessage();
            mailMessage.Sender = new MailAddress(username);
            mailMessage.From = new MailAddress(from);
            if (replyTo != null)
            {
                foreach (string str in replyTo)
                {
                    mailMessage.ReplyToList.Add(str);
                }
            }
            //OutMailDB.WriteLog("SendMailMessage", "ToUser#Legnth:" + to.Length + ";ToUser#Name:" + string.Join(",", to));
            if (to != null)
            {
                for (int i = 0; i < to.Length; i++)
                {
                    mailMessage.To.Add(new MailAddress(to[i]));
                }
            }
            //OutMailDB.WriteLog("SendMailMessage", "SplitToUser: Success");
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
            mailMessage.Attachments.Clear();
            //attchment 
            if (dicAttach.Count()>0)
            {
                foreach (KeyValuePair<string, Stream> att in dicAttach)
                {
                   Attachment attachment = new Attachment(att.Value, att.Key);
                   mailMessage.Attachments.Add(attachment);
                }
            }
            //sendConf out
            //OutMailDB.WriteLog("SendMailMessageReady", "this.SmtpClient.Send");
            this.SmtpClient.Send(mailMessage);
        }
    }
}
