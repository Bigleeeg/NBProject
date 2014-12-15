using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace JIAOFENG.Practices.Logic.Wechat.Request.Message
{
    public class ImageMessage : BaseMessage
    {
        /// <summary>
        /// 图片链接，开发者可以用HTTP GET获取
        /// </summary>
        public string PicUrl {get;set;}
        public static ImageMessage Parse(XmlDocument doc)
        {
            if (doc == null)
            {
                throw new ArgumentNullException("doc");
            }
            XmlElement rootElement = doc.DocumentElement;
            ImageMessage message = new ImageMessage();
            BaseMessage.Parse(doc, message);

            message.PicUrl = rootElement.SelectSingleNode("PicUrl").InnerText;

            return message;
        }
    }
}
