using JIAOFENG.Practices.Database;
using JIAOFENG.Practices.Logic.Mail.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JIAOFENG.Practices.Logic.Mail
{
    internal interface IMailPushDal
    {
        OutMailMessageEntity Add(OutMailMessageEntity entity);
        List<OutMailMessageEntity> GetWaitingSendMails(int failInterval, int failMostNum);
        void UpdataMailStatus(int mailID, OutMailStatus status);
        void UpdateSendCount(int mailID);
    }
}
