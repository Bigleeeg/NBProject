using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace JIAOFENG.Practices.Logic.Wechat.Request.Message
{
    public class LocationMessage : BaseMessage
    {
        /// <summary>
        /// 地理位置纬度
        /// </summary>
        public string Location_X {get;set;}

        /// <summary>
        /// 地理位置经度
        /// </summary>
        public string Location_Y {get;set;}

        /// <summary>
        /// 地图缩放大小
        /// </summary>
        public string Scale {get;set;}

        /// <summary>
        /// 地理位置信息
        /// </summary>
        public string Label {get;set;}
        public static LocationMessage Parse(XmlDocument doc)
        {
            if (doc == null)
            {
                throw new ArgumentNullException("doc");
            }
            XmlElement rootElement = doc.DocumentElement;
            LocationMessage message = new LocationMessage();
            BaseMessage.Parse(doc, message);

            message.Location_X = rootElement.SelectSingleNode("Location_X").InnerText;
            message.Location_Y = rootElement.SelectSingleNode("Location_Y").InnerText;
            message.Scale = rootElement.SelectSingleNode("Scale").InnerText;
            message.Label = rootElement.SelectSingleNode("Label").InnerText;
          
            return message;
        }
    }
}
