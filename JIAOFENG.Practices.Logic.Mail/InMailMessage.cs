using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Mail;
using JIAOFENG.Practices.Library.Common;
using JIAOFENG.Practices.Logic.Mail.Entity;
using JIAOFENG.Practices.Logic.File;

namespace JIAOFENG.Practices.Logic.Mail
{
    public class InMailMessage : InMailMessageEntity
    {
        public InMailMessage()
        {
            this.Attachments = new FilePackage();
        }
        public FilePackage Attachments { get; set; }
    }
}
