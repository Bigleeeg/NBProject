using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JIAOFENG.Practices.Logic.Wechat.Response.Message
{
    public abstract class BaseMessage
    {
        protected const string CDATAFormat = "<{0}><![CDATA[{{0}}]]></{0}>";
        protected const string NormalFormat = "<{0}>{{0}}</{0}>";
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
        public string ToXML()
        {
            return string.Format("<xml>{0}</xml>", ToXmlNode());
        }
        protected virtual string ToXmlNode()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat(string.Format(CDATAFormat, "ToUserName"), this.ToUserName);
            sb.AppendFormat(string.Format(CDATAFormat, "FromUserName"), this.FromUserName);
            sb.AppendFormat(string.Format(NormalFormat, "CreateTime"), this.CreateTime);
            sb.AppendFormat(string.Format(CDATAFormat, "MsgType"), this.MsgType);
            return sb.ToString();
        }
    }
}
