using System.Collections.Generic;

namespace JIAOFENG.Practices.Logic.Wechat
{
    internal interface IWechatPushDal
    {
        WechatpushEntity Add(WechatpushEntity entity);
        List<WechatpushEntity> GetWaitingSendMails(int failInterval, int failMostNum);
        void UpdataMailStatus(int mailID, PushStatus status);
        void UpdateSendCount(int mailID);
    }
}
