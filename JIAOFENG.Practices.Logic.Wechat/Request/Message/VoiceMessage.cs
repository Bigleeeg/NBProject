using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace JIAOFENG.Practices.Logic.Wechat.Request.Message
{
    public class VoiceMessage : BaseMessage
    {
        public string MediaId { get; set; }
        public string Format { get; set; }
        public string Recognition { get; set; }
        public static VoiceMessage Parse(XmlDocument doc)
        {
            if (doc == null)
            {
                throw new ArgumentNullException("doc");
            }
            XmlElement rootElement = doc.DocumentElement;
            VoiceMessage message = new VoiceMessage();
            BaseMessage.Parse(doc, message);

            message.MediaId = rootElement.SelectSingleNode("MediaId").InnerText;
            message.Format = rootElement.SelectSingleNode("Format").InnerText;
            message.Recognition = rootElement.SelectSingleNode("Recognition").InnerText;

            return message;
        }
    }
}
