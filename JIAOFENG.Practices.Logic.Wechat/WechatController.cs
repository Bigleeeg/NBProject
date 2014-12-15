using JIAOFENG.Practices.Library.Utility;
using JIAOFENG.Practices.Logic.Log;
using JIAOFENG.Practices.Logic.Wechat.Request.Event;
using JIAOFENG.Practices.Logic.Wechat.Request.Message;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Security;
using System.Xml;

namespace JIAOFENG.Practices.Logic.Wechat
{
    public class WechatController : WechatBaseController
    {
        public static IServiceHandler ServiceHandler = new DefaultServiceHandler();

        [HttpGet]
        public ContentResult Default(string signature, string timestamp, string nonce, string echoStr)
        {
            //LogManager.LogDebug("Auth", "", "start", "start");
            //string signature = System.Web.HttpContext.Current.Request.QueryString["signature"];
            //string timestamp = System.Web.HttpContext.Current.Request.QueryString["timestamp"];
            //string nonce = System.Web.HttpContext.Current.Request.QueryString["nonce"];
            if (CheckSignature(signature, timestamp, nonce))
            {
                //string echoStr = System.Web.HttpContext.Current.Request.QueryString["echoStr"];
                if (!string.IsNullOrEmpty(echoStr))
                {
                    return Content(echoStr);
                }
            }
            return Content(string.Empty);
        }

        [HttpPost]
        public ContentResult Default(string signature, string timestamp, string nonce)
        {
            //LogManager.LogDebug("Post", "", "start", "start");
            if (CheckSignature(signature, timestamp, nonce))
            {
                Stream s = System.Web.HttpContext.Current.Request.InputStream;
                byte[] b = new byte[s.Length];
                s.Read(b, 0, (int)s.Length);
                string postStr = Encoding.UTF8.GetString(b);
                //LogManager.LogDebug("Post", postStr, "postStr", "postStr");

                string respXML = ServiceHandler.HandleService(postStr);
                //LogManager.LogDebug("Post", respXML, "respXML", "respXML");
                return Content(respXML);
            }
            return Content("Service Error"); 
        }

        public void OAuth(WechatOauthCode oauthCode)
        {
            //LogManager.LogDebug("OAuth", oauthCode.code, "oauthCode", "oauthCode");
            if (oauthCode.IsValid)
            {
                WechatOauth2Token token = WechatOauth2Token.GetWechatOauth2Token(oauthCode.code);
                this.BaseAccessToken = token;
                LogManager.LogDebug("OAuth", oauthCode.state, "Redirect", "Redirect");
            }
            //Server.Transfer(oauthCode.state, true);
            Response.Redirect(oauthCode.state, true);
        }

        [HttpGet]
        public ContentResult CreateMenu()
        {
            XmlDocument doc = new XmlDocument();
            string localPath = this.ControllerContext.HttpContext.Server.MapPath("~/Configs/Menu.xml");
            doc.Load(localPath);
            Menu menu = MenuUtil.GetMenu(doc);
            MenuUtil.CreateMenu(menu, this.ClientAccessToken.access_token);

            string requestUrl = ConfigurationManager.AppSettings["MenuCreateUrl"];
            requestUrl = string.Format(requestUrl, this.ClientAccessToken.access_token);
            string strResult = CommonUtil.HttpRequest(requestUrl, "Post", menu.ToJson());
            WechatResult error = JsonHelper.ToObjectFromJSON<WechatResult>(strResult);
            if (error.errcode == 0)
            {
                return Content("Successfully"); 
            }
            else
            {
                return Content("Failed, " + error.errmsg); 
            }
        }      

        #region private/protected method
        private bool CheckSignature(string signature, string timestamp, string nonce)
        {
            string[] arrTmp = { WechatToken, timestamp, nonce };
            Array.Sort(arrTmp);     //字典排序          
            string tmpStr = string.Join("", arrTmp);
            //LogManager.LogDebug("Auth", tmpStr, "tmpStr", "tmpStr");
            tmpStr = JIAOFENG.Practices.Library.Cryptography.SHA1Cryptography.ProtectString(tmpStr);                     
            tmpStr = tmpStr.ToLower();
            //LogManager.LogDebug("Auth", tmpStr, "sha1", "sha1");
            //LogManager.LogDebug("Auth", signature, "signature", "signature");
            if (tmpStr == signature)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public static void UpdateServiceHandler(IServiceHandler handler)
        {
            ServiceHandler = handler;
        }
        //protected static int ConvertDateTimeToInt(System.DateTime time)
        //{
        //    System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
        //    return (int)(time - startTime).TotalSeconds;
        //}

        //public string Handle(string postStr)
        //{
        //    //封装请求类
        //    XmlDocument doc = new XmlDocument();
        //    doc.LoadXml(postStr);
        //    XmlElement rootElement = doc.DocumentElement;

        //    XmlNode msgType = rootElement.SelectSingleNode("MsgType");
        //    switch (msgType.InnerText.ToLower())
        //    {
        //        case Const.MessageLocation:
        //            LocationMessage locationMessage = LocationMessage.Parse(doc);
        //            return HandleService(locationMessage);
        //        case Const.MessageText:
        //            TextMessage textMessage = TextMessage.Parse(doc);
        //            return HandleService(textMessage);
        //        case Const.MessageImage:
        //            ImageMessage imageMessage = ImageMessage.Parse(doc);
        //            return HandleService(imageMessage);
        //        case Const.MessageLink:
        //            LinkMessage linkMessage = LinkMessage.Parse(doc);
        //            return HandleService(linkMessage);
        //        case Const.MessageVoice:
        //            VoiceMessage voiceMessage = VoiceMessage.Parse(doc);
        //            return HandleService(voiceMessage);
        //        case Const.MessageVideo:
        //            VideoMessage videoMessage = VideoMessage.Parse(doc);
        //            return HandleService(videoMessage);
        //        case Const.MessageEvent:
        //            XmlNode eventType = rootElement.SelectSingleNode("Event");
        //            switch (eventType.InnerText.ToLower())
        //            {
        //                case Const.EventSubscribe:
        //                    SubscribeEvent subscribeEvent = SubscribeEvent.Parse(doc);
        //                    return HandleSubscribeEvent(subscribeEvent);
        //                case Const.EventUnsubscribe:
        //                    SubscribeEvent unsubscribeEvent = SubscribeEvent.Parse(doc);
        //                    return HandleUnsubscribeEvent(unsubscribeEvent);
        //                case Const.EventLocation:
        //                    LocationEvent locationEvent = LocationEvent.Parse(doc);
        //                    return HandleEvent(locationEvent);
        //                case Const.EventClick:
        //                    MenuEvent menuEvent = MenuEvent.Parse(doc);
        //                    return HandleEvent(menuEvent);
        //                case Const.EventScan:
        //                    QRCodeEvent qrCodeEvent = QRCodeEvent.Parse(doc);
        //                    return HandleEvent(qrCodeEvent);
        //            }
        //            break;
        //        default:
        //            break;
        //    }
        //    return string.Empty;
        //}
        //protected virtual string HandleService(TextMessage objReceived)
        //{
        //    JIAOFENG.Practices.Logic.Wechat.Response.Message.TextMessage message = new JIAOFENG.Practices.Logic.Wechat.Response.Message.TextMessage();
        //    message.FromUserName = objReceived.ToUserName;
        //    message.ToUserName = objReceived.FromUserName;
        //    message.CreateTime = ConvertDateTimeToInt(DateTime.Now);
        //    message.MsgType = Const.ResponseText;
        //    message.Content = "收到文本消息";
        //    return message.ToXML();
        //}
        //protected virtual string HandleService(LocationMessage objReceived)
        //{
        //    JIAOFENG.Practices.Logic.Wechat.Response.Message.TextMessage message = new JIAOFENG.Practices.Logic.Wechat.Response.Message.TextMessage();
        //    message.FromUserName = objReceived.ToUserName;
        //    message.ToUserName = objReceived.FromUserName;
        //    message.CreateTime = ConvertDateTimeToInt(DateTime.Now);
        //    message.MsgType = Const.ResponseText;
        //    message.Content = "收到地理位置消息";
        //    return message.ToXML();
        //}
        //protected virtual string HandleService(ImageMessage objReceived)
        //{
        //    JIAOFENG.Practices.Logic.Wechat.Response.Message.TextMessage message = new JIAOFENG.Practices.Logic.Wechat.Response.Message.TextMessage();
        //    message.FromUserName = objReceived.ToUserName;
        //    message.ToUserName = objReceived.FromUserName;
        //    message.CreateTime = ConvertDateTimeToInt(DateTime.Now);
        //    message.MsgType = Const.ResponseText;
        //    message.Content = "收到图片消息";
        //    return message.ToXML();
        //}
        //protected virtual string HandleService(LinkMessage objReceived)
        //{
        //    JIAOFENG.Practices.Logic.Wechat.Response.Message.TextMessage message = new JIAOFENG.Practices.Logic.Wechat.Response.Message.TextMessage();
        //    message.FromUserName = objReceived.ToUserName;
        //    message.ToUserName = objReceived.FromUserName;
        //    message.CreateTime = ConvertDateTimeToInt(DateTime.Now);
        //    message.MsgType = Const.ResponseText;
        //    message.Content = "收到链接消息";
        //    return message.ToXML();
        //}
        //protected virtual string HandleService(VoiceMessage objReceived)
        //{
        //    JIAOFENG.Practices.Logic.Wechat.Response.Message.TextMessage message = new JIAOFENG.Practices.Logic.Wechat.Response.Message.TextMessage();
        //    message.FromUserName = objReceived.ToUserName;
        //    message.ToUserName = objReceived.FromUserName;
        //    message.CreateTime = ConvertDateTimeToInt(DateTime.Now);
        //    message.MsgType = Const.ResponseText;
        //    message.Content = "收到声音消息：" + objReceived.Recognition; ;
        //    return message.ToXML();
        //}
        //protected virtual string HandleService(VideoMessage objReceived)
        //{
        //    JIAOFENG.Practices.Logic.Wechat.Response.Message.TextMessage message = new JIAOFENG.Practices.Logic.Wechat.Response.Message.TextMessage();
        //    message.FromUserName = objReceived.ToUserName;
        //    message.ToUserName = objReceived.FromUserName;
        //    message.CreateTime = ConvertDateTimeToInt(DateTime.Now);
        //    message.MsgType = Const.ResponseText;
        //    message.Content = "收到视频消息";
        //    return message.ToXML();
        //}
        //protected virtual string HandleSubscribeEvent(SubscribeEvent eventReceived)
        //{
        //    JIAOFENG.Practices.Logic.Wechat.Response.Message.TextMessage message = new JIAOFENG.Practices.Logic.Wechat.Response.Message.TextMessage();
        //    message.FromUserName = eventReceived.ToUserName;
        //    message.ToUserName = eventReceived.FromUserName;
        //    message.CreateTime = ConvertDateTimeToInt(DateTime.Now);
        //    message.MsgType = Const.ResponseText;
        //    message.Content = "谢谢您的关注！";
        //    return message.ToXML();
        //}
        //protected virtual string HandleUnsubscribeEvent(SubscribeEvent eventReceived)
        //{
        //    JIAOFENG.Practices.Logic.Wechat.Response.Message.TextMessage message = new JIAOFENG.Practices.Logic.Wechat.Response.Message.TextMessage();
        //    message.FromUserName = eventReceived.ToUserName;
        //    message.ToUserName = eventReceived.FromUserName;
        //    message.CreateTime = ConvertDateTimeToInt(DateTime.Now);
        //    message.MsgType = Const.ResponseText;
        //    message.Content = "欢迎再来！";
        //    return message.ToXML();
        //}
        //protected virtual string HandleEvent(LocationEvent eventReceived)
        //{
        //    JIAOFENG.Practices.Logic.Wechat.Response.Message.TextMessage message = new JIAOFENG.Practices.Logic.Wechat.Response.Message.TextMessage();
        //    message.FromUserName = eventReceived.ToUserName;
        //    message.ToUserName = eventReceived.FromUserName;
        //    message.CreateTime = ConvertDateTimeToInt(DateTime.Now);
        //    message.MsgType = Const.ResponseText;
        //    message.Content = "位置！";
        //    return message.ToXML();
        //}
        //protected virtual string HandleEvent(MenuEvent eventReceived)
        //{
        //    JIAOFENG.Practices.Logic.Wechat.Response.Message.TextMessage message = new JIAOFENG.Practices.Logic.Wechat.Response.Message.TextMessage();
        //    message.FromUserName = eventReceived.ToUserName;
        //    message.ToUserName = eventReceived.FromUserName;
        //    message.CreateTime = ConvertDateTimeToInt(DateTime.Now);
        //    message.MsgType = Const.ResponseText;
        //    message.Content = "菜单！";
        //    return message.ToXML();
        //}
        //protected virtual string HandleEvent(QRCodeEvent eventReceived)
        //{
        //    JIAOFENG.Practices.Logic.Wechat.Response.Message.TextMessage message = new JIAOFENG.Practices.Logic.Wechat.Response.Message.TextMessage();
        //    message.FromUserName = eventReceived.ToUserName;
        //    message.ToUserName = eventReceived.FromUserName;
        //    message.CreateTime = ConvertDateTimeToInt(DateTime.Now);
        //    message.MsgType = Const.ResponseText;
        //    message.Content = "扫描！";
        //    return message.ToXML();
        //}
        #endregion
    }
}
