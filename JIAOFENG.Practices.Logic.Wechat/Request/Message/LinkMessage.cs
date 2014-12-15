using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace JIAOFENG.Practices.Logic.Wechat.Request.Message
{
    public class LinkMessage : BaseMessage
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Url { get; set; }
        public static LinkMessage Parse(XmlDocument doc)
        {
            if (doc == null)
            {
                throw new ArgumentNullException("doc");
            }
            XmlElement rootElement = doc.DocumentElement;
            LinkMessage message = new LinkMessage();
            BaseMessage.Parse(doc, message);

            message.Title = rootElement.SelectSingleNode("Title").InnerText;
            message.Description = rootElement.SelectSingleNode("Description").InnerText;
            message.Url = rootElement.SelectSingleNode("Url").InnerText;

            return message;
        }
    }
}
