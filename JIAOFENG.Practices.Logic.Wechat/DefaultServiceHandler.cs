using JIAOFENG.Practices.Logic.Wechat.Request.Event;
using JIAOFENG.Practices.Logic.Wechat.Request.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace JIAOFENG.Practices.Logic.Wechat
{
    public class DefaultServiceHandler : ServiceHandler
    {
        public override string HandleService(string postStr)
        {
            //封装请求类
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(postStr);
            XmlElement rootElement = doc.DocumentElement;

            XmlNode msgType = rootElement.SelectSingleNode("MsgType");
            switch (msgType.InnerText.ToLower())
            {
                case Const.MessageLocation:
                    LocationMessage locationMessage = LocationMessage.Parse(doc);
                    return HandleService(locationMessage);
                case Const.MessageText:
                    TextMessage textMessage = TextMessage.Parse(doc);
                    return HandleService(textMessage);
                    //return HandleService(textMessage);
                case Const.MessageImage:
                    ImageMessage imageMessage = ImageMessage.Parse(doc);
                    return HandleService(imageMessage);
                case Const.MessageLink:
                    LinkMessage linkMessage = LinkMessage.Parse(doc);
                    return HandleService(linkMessage);
                case Const.MessageVoice:
                    VoiceMessage voiceMessage = VoiceMessage.Parse(doc);
                    return HandleService(voiceMessage);
                case Const.MessageVideo:
                    VideoMessage videoMessage = VideoMessage.Parse(doc);
                    return HandleService(videoMessage);
                case Const.MessageEvent:
                    XmlNode eventType = rootElement.SelectSingleNode("Event");
                    switch (eventType.InnerText.ToLower())
                    {
                        case Const.EventSubscribe:
                            SubscribeEvent subscribeEvent = SubscribeEvent.Parse(doc);
                            return HandleSubscribeEvent(subscribeEvent);
                        case Const.EventUnsubscribe:
                            SubscribeEvent unsubscribeEvent = SubscribeEvent.Parse(doc);
                            return HandleUnsubscribeEvent(unsubscribeEvent);
                        case Const.EventLocation:
                            LocationEvent locationEvent = LocationEvent.Parse(doc);
                            return HandleEvent(locationEvent);
                        case Const.EventClick:
                            MenuEvent menuEvent = MenuEvent.Parse(doc);
                            return HandleEvent(menuEvent);
                        case Const.EventScan:
                            QRCodeEvent qrCodeEvent = QRCodeEvent.Parse(doc);
                            return HandleEvent(qrCodeEvent);
                    }
                    break;
                default:
                    break;
            }
            return string.Empty;
        }

        #region protected method
        protected virtual string HandleService(TextMessage objReceived)
        {
            JIAOFENG.Practices.Logic.Wechat.Response.Message.TextMessage message = new JIAOFENG.Practices.Logic.Wechat.Response.Message.TextMessage();
            message.FromUserName = objReceived.ToUserName;
            message.ToUserName = objReceived.FromUserName;
            message.CreateTime = ConvertDateTimeToInt(DateTime.Now);
            message.MsgType = Const.ResponseText;
            message.Content = "收到文本消息";
            return message.ToXML();
        }
        protected virtual string HandleService(LocationMessage objReceived)
        {
            JIAOFENG.Practices.Logic.Wechat.Response.Message.TextMessage message = new JIAOFENG.Practices.Logic.Wechat.Response.Message.TextMessage();
            message.FromUserName = objReceived.ToUserName;
            message.ToUserName = objReceived.FromUserName;
            message.CreateTime = ConvertDateTimeToInt(DateTime.Now);
            message.MsgType = Const.ResponseText;
            message.Content = "收到地理位置消息";
            return message.ToXML();
        }
        protected virtual string HandleService(ImageMessage objReceived)
        {
            JIAOFENG.Practices.Logic.Wechat.Response.Message.TextMessage message = new JIAOFENG.Practices.Logic.Wechat.Response.Message.TextMessage();
            message.FromUserName = objReceived.ToUserName;
            message.ToUserName = objReceived.FromUserName;
            message.CreateTime = ConvertDateTimeToInt(DateTime.Now);
            message.MsgType = Const.ResponseText;
            message.Content = "收到图片消息";
            return message.ToXML();
        }
        protected virtual string HandleService(LinkMessage objReceived)
        {
            JIAOFENG.Practices.Logic.Wechat.Response.Message.TextMessage message = new JIAOFENG.Practices.Logic.Wechat.Response.Message.TextMessage();
            message.FromUserName = objReceived.ToUserName;
            message.ToUserName = objReceived.FromUserName;
            message.CreateTime = ConvertDateTimeToInt(DateTime.Now);
            message.MsgType = Const.ResponseText;
            message.Content = "收到链接消息";
            return message.ToXML();
        }
        protected virtual string HandleService(VoiceMessage objReceived)
        {
            JIAOFENG.Practices.Logic.Wechat.Response.Message.TextMessage message = new JIAOFENG.Practices.Logic.Wechat.Response.Message.TextMessage();
            message.FromUserName = objReceived.ToUserName;
            message.ToUserName = objReceived.FromUserName;
            message.CreateTime = ConvertDateTimeToInt(DateTime.Now);
            message.MsgType = Const.ResponseText;
            message.Content = "收到声音消息：" + objReceived.Recognition; ;
            return message.ToXML();
        }
        protected virtual string HandleService(VideoMessage objReceived)
        {
            JIAOFENG.Practices.Logic.Wechat.Response.Message.TextMessage message = new JIAOFENG.Practices.Logic.Wechat.Response.Message.TextMessage();
            message.FromUserName = objReceived.ToUserName;
            message.ToUserName = objReceived.FromUserName;
            message.CreateTime = ConvertDateTimeToInt(DateTime.Now);
            message.MsgType = Const.ResponseText;
            message.Content = "收到视频消息";
            return message.ToXML();
        }
        protected virtual string HandleSubscribeEvent(SubscribeEvent eventReceived)
        {
            JIAOFENG.Practices.Logic.Wechat.Response.Message.TextMessage message = new JIAOFENG.Practices.Logic.Wechat.Response.Message.TextMessage();
            message.FromUserName = eventReceived.ToUserName;
            message.ToUserName = eventReceived.FromUserName;
            message.CreateTime = ConvertDateTimeToInt(DateTime.Now);
            message.MsgType = Const.ResponseText;
            message.Content = "谢谢您的关注！";
            return message.ToXML();
        }
        protected virtual string HandleUnsubscribeEvent(SubscribeEvent eventReceived)
        {
            JIAOFENG.Practices.Logic.Wechat.Response.Message.TextMessage message = new JIAOFENG.Practices.Logic.Wechat.Response.Message.TextMessage();
            message.FromUserName = eventReceived.ToUserName;
            message.ToUserName = eventReceived.FromUserName;
            message.CreateTime = ConvertDateTimeToInt(DateTime.Now);
            message.MsgType = Const.ResponseText;
            message.Content = "欢迎再来！";
            return message.ToXML();
        }
        protected virtual string HandleEvent(LocationEvent eventReceived)
        {
            JIAOFENG.Practices.Logic.Wechat.Response.Message.TextMessage message = new JIAOFENG.Practices.Logic.Wechat.Response.Message.TextMessage();
            message.FromUserName = eventReceived.ToUserName;
            message.ToUserName = eventReceived.FromUserName;
            message.CreateTime = ConvertDateTimeToInt(DateTime.Now);
            message.MsgType = Const.ResponseText;
            message.Content = "位置！";
            return message.ToXML();
        }
        protected virtual string HandleEvent(MenuEvent eventReceived)
        {
            JIAOFENG.Practices.Logic.Wechat.Response.Message.TextMessage message = new JIAOFENG.Practices.Logic.Wechat.Response.Message.TextMessage();
            message.FromUserName = eventReceived.ToUserName;
            message.ToUserName = eventReceived.FromUserName;
            message.CreateTime = ConvertDateTimeToInt(DateTime.Now);
            message.MsgType = Const.ResponseText;
            message.Content = "菜单！";
            return message.ToXML();
        }
        protected virtual string HandleEvent(QRCodeEvent eventReceived)
        {
            JIAOFENG.Practices.Logic.Wechat.Response.Message.TextMessage message = new JIAOFENG.Practices.Logic.Wechat.Response.Message.TextMessage();
            message.FromUserName = eventReceived.ToUserName;
            message.ToUserName = eventReceived.FromUserName;
            message.CreateTime = ConvertDateTimeToInt(DateTime.Now);
            message.MsgType = Const.ResponseText;
            message.Content = "扫描！";
            return message.ToXML();
        }
        
        #endregion
    }
}
