using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using JIAOFENG.Practices.Library.Common;
using System.IO;

namespace JIAOFENG.Practices.Logic.Mail.Entity
{
    [Serializable]
    public class OutMailMessageEntity : EntityExtInfo
    {
        #region constructor
        public OutMailMessageEntity()
        {
        }
        public OutMailMessageEntity(OutMailMessageEntity entity)
        {
            CopyFrom(entity);
        }
        #endregion

        #region property

        public int MailID { get; set; }

        public string Sender { get; set; }

        public string MailFrom { get; set; }

        public string MailTo { get; set; }

        public string MailCC { get; set; }

        public string MailBcc { get; set; }

        public string Subject { get; set; }

        public string Body { get; set; }

        public bool IsBodyHtml { get; set; }

        public string BodyEncoding { get; set; }

        public int Priority { get; set; }

        public int Status { get; set; }

        public int SendCount { get; set; }

        public DateTime? LastTrySendTime { get; set; }
        public int? FilePackageID { get; set; }

        public string CreateUserName { get; set; }


        #endregion

        public void CopyFrom(OutMailMessageEntity entity)
        {
            this.MailID = entity.MailID;
            this.Sender = entity.Sender;
            this.MailFrom = entity.MailFrom;
            this.MailTo = entity.MailTo;
            this.MailCC = entity.MailCC;
            this.MailBcc = entity.MailBcc;
            this.Subject = entity.Subject;
            this.Body = entity.Body;
            this.IsBodyHtml = entity.IsBodyHtml;
            this.BodyEncoding = entity.BodyEncoding;
            this.Priority = entity.Priority;
            this.Status = entity.Status;
            this.SendCount = entity.SendCount;
            this.LastTrySendTime = entity.LastTrySendTime;
            this.FilePackageID = entity.FilePackageID;
            this.CreateUserName = entity.CreateUserName;
            this.CreateTime = entity.CreateTime;
            this.CreateBy = entity.CreateBy;
            this.UpdateTime = entity.UpdateTime;
            this.UpdateBy = entity.UpdateBy;
        }
    }
}
