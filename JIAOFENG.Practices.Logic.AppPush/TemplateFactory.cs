using com.igetui.api.openservice;
using com.igetui.api.openservice.igetui.template;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JIAOFENG.Practices.Logic.AppPush
{
    public class TemplateFactory
    {
        public TemplateFactory(string appID, string appKey)
        {
            this.AppID = appID;
            this.AppKey = appKey;
        }
        public string AppID { get; private set; }
        public string AppKey { get; private set; }
        public ITemplate CreateTransmissionTemplate(string content)
        {
            TransmissionTemplate template = new TransmissionTemplate();
            template.AppId = this.AppID;
            template.AppKey = this.AppKey;
            template.TransmissionType = "1";            //应用启动类型，1：强制应用启动 2：等待应用启动
            template.TransmissionContent = content;  //透传内容
            //iOS推送需要的pushInfo字段
            //template.setPushInfo(actionLocKey, badge, message, sound, payload, locKey, locArgs, launchImage);
            //template.setPushInfo("1", 4, "2", "", "", "", "", "");
            return template;
        }
        public ITemplate CreateNotificationTemplate(string title, string text, string context, string logo = "", string logoURL = "")
        {
            NotificationTemplate template = new NotificationTemplate();
            template.AppId = this.AppID;
            template.AppKey = this.AppKey;
            template.Title = title;         //通知栏标题
            template.Text = text;          //通知栏内容
            template.Logo = logo;               //通知栏显示本地图片
            template.LogoURL = logoURL;                    //通知栏显示网络图标

            template.TransmissionType = "1";          //应用启动类型，1：强制应用启动  2：等待应用启动
            template.TransmissionContent = context;   //透传内容
            //iOS推送需要的pushInfo字段
            //template.setPushInfo(actionLocKey, badge, message, sound, payload, locKey, locArgs, launchImage);

            template.IsRing = true;                //接收到消息是否响铃，true：响铃 false：不响铃
            template.IsVibrate = true;               //接收到消息是否震动，true：震动 false：不震动
            template.IsClearable = true;             //接收到消息是否可清除，true：可清除 false：不可清除
            return template;
        }

        //通知链接动作内容
        public ITemplate CreateLinkTemplate(string title, string text, string context, string url, string logo = "", string logoURL = "")
        {
            LinkTemplate template = new LinkTemplate();
            template.AppId = this.AppID;
            template.AppKey = this.AppKey;
            template.Title = "请填写通知标题";         //通知栏标题
            template.Text = "请填写通知内容";          //通知栏内容
            template.Logo = "";               //通知栏显示本地图片
            template.LogoURL = "";  //通知栏显示网络图标，如无法读取，则显示本地默认图标，可为空
            template.Url = url;      //打开的链接地址

            //iOS推送需要的pushInfo字段
            //template.setPushInfo(actionLocKey, badge, message, sound, payload, locKey, locArgs, launchImage);

            template.IsRing = true;                 //接收到消息是否响铃，true：响铃 false：不响铃
            template.IsVibrate = true;               //接收到消息是否震动，true：震动 false：不震动
            template.IsClearable = true;             //接收到消息是否可清除，true：可清除 false：不可清除

            return template;
        }
    }
}
