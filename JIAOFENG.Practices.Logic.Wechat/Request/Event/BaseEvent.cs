using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace JIAOFENG.Practices.Logic.Wechat.Request.Event
{
    public abstract class BaseEvent
    {
        /// <summary>
        /// 消息接收方微信号，一般为公众平台账号微信号
        /// </summary>
        public string ToUserName { get; set; }

        /// <summary>
        /// 消息发送方微信号
        /// </summary>
        public string FromUserName { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public long CreateTime { get; set; }

        /// <summary>
        /// 信息类型 地理位置:location,文本消息:text,图片:image, link,video,voice
        /// </summary>
        public string MsgType { get; set; }

        /// <summary>
        /// 事件类型
        /// </summary>
        public string Event { get; set; }

        protected static void Parse(XmlDocument doc, BaseEvent baseEvent)
        {
            if (doc == null)
            {
                throw new ArgumentNullException("doc");
            }
            XmlElement rootElement = doc.DocumentElement;

            baseEvent.ToUserName = rootElement.SelectSingleNode("ToUserName").InnerText;
            baseEvent.FromUserName = rootElement.SelectSingleNode("FromUserName").InnerText;
            baseEvent.CreateTime = long.Parse(rootElement.SelectSingleNode("CreateTime").InnerText);
            baseEvent.MsgType = rootElement.SelectSingleNode("MsgType").InnerText;
            baseEvent.Event = rootElement.SelectSingleNode("Event").InnerText;
        }
    }
}
