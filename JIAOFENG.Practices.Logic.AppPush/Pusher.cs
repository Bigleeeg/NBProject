using com.igetui.api.openservice;
using com.igetui.api.openservice.igetui;
using com.igetui.api.openservice.igetui.template;
using JIAOFENG.Practices.Library.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JIAOFENG.Practices.Logic.AppPush
{
    public class Pusher
    {
        public Pusher(string host, string appKey, string masterSecret, string appID)
        {
            this.Host = host;
            this.AppKey = appKey;
            this.MasterSecret = masterSecret;
            this.AppID = appID;

            this.IGtPush = new IGtPush(this.Host, this.AppKey, this.MasterSecret);
        }
        public string Host { get; private set; }
        public string AppKey { get; private set; }
        public string MasterSecret { get; private set; }
        public string AppID { get; private set; }

        // 推送主类
        public IGtPush IGtPush { get; private set; }
        public AppPushResult PushMessageToSingle(string clientID, ITemplate template)
        {
            /*消息模版：
                1.TransmissionTemplate:透传模板
                2.LinkTemplate:通知链接模板
                3.NotificationTemplate：通知透传模板
                4.NotyPopLoadTemplate：通知弹框下载模板
             */

            // 单推消息模型
            SingleMessage message = new SingleMessage();
            message.IsOffline = true;                         // 用户当前不在线时，是否离线存储,可选
            message.OfflineExpireTime = 1000 * 3600 * 12;            // 离线有效时间，单位为毫秒，可选
            message.Data = template;
            //message.PushNetWorkType = 0;        //判断是否客户端是否wifi环境下推送，1为在WIFI环境下，0为非WIFI环境

            com.igetui.api.openservice.igetui.Target target = new com.igetui.api.openservice.igetui.Target();
            target.appId = this.AppID;
            target.clientId = clientID;
            target.alias = "hft";


            String strResult = this.IGtPush.pushMessageToSingle(message, target);
            AppPushResult result = JsonHelper.ToObjectFromJSON<AppPushResult>(strResult);
            //if (error != null)
            //{
            //    if (error.errcode == 0)
            //    {
            //        result = true;
            //    }
            //}
            return result;
            //System.Console.WriteLine("-----------------------------------------------");
            //System.Console.WriteLine("----------------服务端返回结果：" + pushResult);
        }
        public AppPushResult PushMessageToList(List<string> clientIDs, ITemplate template)
        {
            /*消息模版：
                 1.TransmissionTemplate:透传功能模板
                 2.LinkTemplate:通知打开链接功能模板
                 3.NotificationTemplate：通知透传功能模板
                 4.NotyPopLoadTemplate：通知弹框下载功能模板
             */
            ListMessage message = new ListMessage();
            message.IsOffline = true;                         // 用户当前不在线时，是否离线存储,可选
            message.OfflineExpireTime = 1000 * 3600 * 12;            // 离线有效时间，单位为毫秒，可选
            message.Data = template;
            //message.PushNetWorkType = 0;             //判断是否客户端是否wifi环境下推送，1为在WIFI环境下，0为非WIFI环境

            //设置接收者
            List<com.igetui.api.openservice.igetui.Target> targetList = new List<com.igetui.api.openservice.igetui.Target>();
            foreach(string clientID in clientIDs)
            {
                com.igetui.api.openservice.igetui.Target target = new com.igetui.api.openservice.igetui.Target();
                target.appId = this.AppID;
                target.clientId = clientID;
                targetList.Add(target);
            }

            String contentId = this.IGtPush.getContentId(message, null);
            String strResult = this.IGtPush.pushMessageToList(contentId, targetList);
            AppPushResult result = JsonHelper.ToObjectFromJSON<AppPushResult>(strResult);
            return result;
        }

        //pushMessageToApp接口测试代码
        public AppPushResult PushMessageToApp(ITemplate template)
        {
            AppMessage message = new AppMessage();
            message.IsOffline = true;                         // 用户当前不在线时，是否离线存储,可选
            message.OfflineExpireTime = 1000 * 3600 * 12;            // 离线有效时间，单位为毫秒，可选
            message.Data = template;
            //message.PushNetWorkType = 0;            //判断是否客户端是否wifi环境下推送，1为在WIFI环境下，0为非WIFI环境

            List<String> appIdList = new List<string>();
            appIdList.Add(this.AppID);

            List<String> phoneTypeList = new List<string>();    //通知接收者的手机操作系统类型
            //phoneTypeList.Add("ANDROID");
            //phoneTypeList.Add("IOS");

            List<String> provinceList = new List<string>();     //通知接收者所在省份
            //provinceList.Add("浙江");
            //provinceList.Add("上海");
            //provinceList.Add("北京");

            List<String> tagList = new List<string>();
            //tagList.Add("456");

            message.AppIdList = appIdList;
            message.PhoneTypeList = phoneTypeList;
            message.ProvinceList = provinceList;
            message.TagList = tagList;


            String strResult = this.IGtPush.pushMessageToApp(message, "Test");
            AppPushResult result = JsonHelper.ToObjectFromJSON<AppPushResult>(strResult);
            return result;
        }
    }
}
