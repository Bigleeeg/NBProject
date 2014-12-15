using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace JIAOFENG.Practices.Logic.Wechat.Request.Message
{
    public class TextMessage : BaseMessage
    {
        /// <summary>
        /// 信息内容
        /// </summary>
        public string Content {get;set;}
        public static TextMessage Parse(XmlDocument doc)
        {
            if (doc == null)
            {
                throw new ArgumentNullException("doc");
            }
            XmlElement rootElement = doc.DocumentElement;
            TextMessage message = new TextMessage();
            BaseMessage.Parse(doc, message);

            message.Content = rootElement.SelectSingleNode("Content").InnerText;

            return message;
        }
    }
}
