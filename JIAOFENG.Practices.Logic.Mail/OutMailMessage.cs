using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using JIAOFENG.Practices.Library.Common;
using System.IO;
using JIAOFENG.Practices.Logic.Mail.Entity;
using JIAOFENG.Practices.Logic.File;

namespace JIAOFENG.Practices.Logic.Mail
{
    [Serializable]
    public class OutMailMessage : OutMailMessageEntity
    {
        #region constructor
        public OutMailMessage()
        {
        }
        public OutMailMessage(OutMailMessage entity)
        {
            CopyFrom(entity);
        }
        #endregion

        #region property
        public FilePackage Attachments { get; set; }

        #endregion

        public void CopyFrom(OutMailMessage entity)
        {
            base.CopyFrom(entity);
            this.Attachments = entity.Attachments;
        }
    }
}
