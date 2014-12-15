using System.Collections.Generic;

namespace JIAOFENG.Practices.Logic.AppPush
{
    internal interface IAppPushDal
    {
        ApppushEntity Add(ApppushEntity entity);
        List<ApppushEntity> GetWaitingSendMails(int failInterval, int failMostNum);
        void UpdataMailStatus(int mailID, PushStatus status);
        void UpdateSendCount(int mailID);
    }
}
