using com.igetui.api.openservice;
using JIAOFENG.Practices.Logic.AppPush;
using JIAOFENG.Practices.Logic.Log;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JIAOFENG.Practices.PushService
{
    public class AppPushService : PushService
    {
        public AppPushService(int failRetryIntervalPow, int failMostNum, ClientConfig cc)
            : base(failRetryIntervalPow, failMostNum, cc)
        {

        }
        public override void Push()
        {
            JIAOFENG.Practices.Database.Database db = JIAOFENG.Practices.Database.DatabaseFactory.CreateDatabase(this.Client.Provider.ProviderType, this.Client.Provider.ConnectionString);
            AppManager.Database = db;
            TemplateFactory factory = new TemplateFactory(this.Client.AppPush.AppId, this.Client.AppPush.AppKey);
            Pusher pusher = new Pusher(this.Client.AppPush.Host, this.Client.AppPush.AppKey, this.Client.AppPush.MasterSecret, this.Client.AppPush.AppId);
            List<ApppushEntity> list = AppManager.GetWaitingSendMails(this.FailRetryIntervalPow, this.FailMostNum);
            foreach (ApppushEntity entity in list)
            {
                AppPushResult r;
                if (!string.IsNullOrWhiteSpace(entity.ClientID))
                {
                    ITemplate t = factory.CreateNotificationTemplate(entity.Subject, entity.Summary, entity.Body);
                    r = pusher.PushMessageToSingle(entity.ClientID, t);
                }
                else
                {
                    ITemplate t = factory.CreateNotificationTemplate(entity.Subject, entity.Summary, entity.Body);
                    r = pusher.PushMessageToApp(t);
                }

                if (r.IsOk)
                {
                    //更新发送状态
                    AppManager.UpdataMailStatus(entity.AppPushID, PushStatus.Success);
                }
                else
                {
                    //更新失败次数
                    AppManager.UpdateSendCount(entity.AppPushID);

                    if (entity.SendCount >= this.FailMostNum - 1)
                    {
                        //失败，根据this.Client.ConnectionString来修改
                        AppManager.UpdataMailStatus(entity.AppPushID, PushStatus.Fail);
                    }

                    //应该根据rc.ConnectionString写Log。。todo
                    LogManager.LogError("App通知发送失败", r.result, entity.Client, entity.ClientID, Constant.LogCategoryID, Constant.LogCategoryName);
                }
            }
        }
    }
}
