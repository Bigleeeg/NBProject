using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Mail;
using JIAOFENG.Practices.Library.Common;

namespace JIAOFENG.Practices.Logic.Mail.Entity
{
    [Serializable]
    public class InMailMessageEntity
    {
        public int MailID { get; set; }
        public string MailReceiver { get; set; }
        public string MailSendTime { get; set; }
        public string MailSenderAddress { get; set; }
        public string MailTo { get; set; }
        public string MailCC { get; set; }
        public string MailBCC { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public int CreateBy { get; set; }
        public DateTime CreateTime { get; set; }
    }
}
