using JIAOFENG.Practices.Logic.Log;
using JIAOFENG.Practices.Logic.Wechat;
using JIAOFENG.Practices.Logic.Wechat.Response.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace JIAOFENG.Practices.PushService
{  
    public class WechatPushService : PushService
    {
        public static Token ClientAccessToken {get;set;}
        public WechatPushService(int failRetryIntervalPow, int failMostNum, ClientConfig cc)
            : base(failRetryIntervalPow, failMostNum, cc)
        {

        }
        public override void Push()
        {
            if (ClientAccessToken == null || !ClientAccessToken.IsValid)
            {
                string appId = this.Client.Wechat.WechatAppId;
                string appSecret = this.Client.Wechat.AppSecret;
                string cientAccessTokenUrl = this.Client.Wechat.ClientAccessTokenUrl;
                ClientAccessToken = Token.GetClientAccessToken(appId, appSecret, cientAccessTokenUrl);
            }

            WechatManager.Database = JIAOFENG.Practices.Database.DatabaseFactory.CreateDatabase(this.Client.Provider.ProviderType, this.Client.Provider.ConnectionString);

            List<WechatpushEntity> list = WechatManager.GetWaitingSendMails(this.FailRetryIntervalPow, this.FailMostNum);
            foreach(WechatpushEntity entity in list)
            {
                TemplateMessage message = JIAOFENG.Practices.Library.Utility.XmlHelper.SerializeToObject<TemplateMessage>(entity.BodyXml);
                
                if (JIAOFENG.Practices.Logic.Wechat.CommonUtil.SendTemplateMessage(message, this.Client.Wechat.TemplateMessageHost, WechatPushService.ClientAccessToken.access_token))
                {
                    //更新发送状态
                    WechatManager.UpdataMailStatus(entity.WechatPushID, PushStatus.Success);
                }
                else
                {
                    //更新失败次数
                    WechatManager.UpdateSendCount(entity.WechatPushID);

                    if (entity.SendCount >= this.FailMostNum - 1)
                    {
                        //失败，根据this.Client.ConnectionString来修改
                        WechatManager.UpdataMailStatus(entity.WechatPushID, PushStatus.Fail);
                    }

                    //应该根据rc.ConnectionString写Log。。todo
                    LogManager.LogError("微信通知发送失败", "", entity.TemplateId, entity.ToUser, Constant.LogCategoryID, Constant.LogCategoryName);
                }
            }
        }
    }
}
