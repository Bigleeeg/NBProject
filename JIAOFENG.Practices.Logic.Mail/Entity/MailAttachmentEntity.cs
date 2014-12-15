using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using Dashinginfo.Practices.Library.Common;

namespace Dashinginfo.Practices.Logic.Mail.Entity
{
    [Serializable]
    public class MailAttachmentEntity
    {
        #region constructor
        public MailAttachmentEntity()
        {

        }
        public MailAttachmentEntity(MailAttachmentEntity entity)
        {
            CopyFrom(entity);
        }
        #endregion

        #region property

        public int MailAttachmentID { get; set; }

        public int AttachmentType { get; set; }

        public string FileName { get; set; }

        public string NameEncoding { get; set; }

        public byte[] Attachment { get; set; }

        public int MailID { get; set; }
        public int CreateBy { get; set; }
        public DateTime CreateTime { get; set; }

        #endregion

        public void CopyFrom(MailAttachmentEntity entity)
        {
            this.MailAttachmentID = entity.MailAttachmentID;
            this.AttachmentType = entity.AttachmentType;
            this.FileName = entity.FileName;
            this.NameEncoding = entity.NameEncoding;
            this.Attachment = entity.Attachment;
            this.MailID = entity.MailID;
            this.CreateTime = entity.CreateTime;
            this.CreateBy = entity.CreateBy;
        }
    }
}
